using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Multi_threaded_downloader;
using static RuTubeApi.RuTubeAPI;

namespace RuTubeApi
{
    public static class Utils
    {
        public static string ExtractVideoIdFromUrl(string url)
        {
            string expression = @"rutube\.ru/video/(\w*)";
            Regex regex = new Regex(expression, RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regex.Matches(url);
            if (matchCollection.Count > 0)
            {
                return matchCollection[0].Groups[1].Value;
            }
            return null;
        }

        internal static RuTubeVideoInfoResult GetVideoInfoFromWebPage(string videoId)
        {
            RuTubeWebPage ruTubeWebPage = GetVideoWebPage(videoId);
            if (ruTubeWebPage != null)
            {
                if (ruTubeWebPage.ErrorCode == 200)
                {
                    string t = ExtractInfoFromWebPage(ruTubeWebPage.VideoWebPage);
                    if (!string.IsNullOrEmpty(t))
                    {
                        //TODO: Fix crashing here
                        return new RuTubeVideoInfoResult(JObject.Parse(t), ruTubeWebPage.ErrorCode);
                    }
                }
            }
            return new RuTubeVideoInfoResult(null, 404);
        }

        internal static string ExtractInfoFromWebPage(string webPage)
        {
            string subString = "window.reduxState = {";
            int n = webPage.IndexOf(subString);
            if (n >= 0)
            {
                string t = webPage.Substring(n + subString.Length - 1);
                n = t.IndexOf("};\n");
                if (n > 0)
                {
                    return t.Substring(0, n + 1);
                }
            }
            return null;
        }

        internal static DateTime ExtractUploadDateFromWebPage(string webPage)
        {
            string subString = "\"uploadDate\" : \"";
            string t = ExtractValue(webPage, subString);
            if (!string.IsNullOrEmpty(t))
            {
                return StringToDateTime(t);
            }
            return DateTime.MaxValue;
        }

        internal static DateTime ExtractPublishedDateFromWebPage(string webPage)
        {
            string subString = "\"datePublished\": \"";
            string t = ExtractValue(webPage, subString);
            if (!string.IsNullOrEmpty(t))
            {
                return StringToDateTime(t);
            }
            return DateTime.MaxValue;
        }

        internal static DateTime ExtractVideoUploadDateTime(string videoId)
        {
            RuTubeWebPage webPage = new RuTubeWebPage();
            webPage.GetVideoWebPage(videoId);
            if (webPage.ErrorCode == 200)
            {
                return ExtractVideoUploadDateTime(webPage);
            }
            return DateTime.MaxValue;
        }

        internal static DateTime ExtractVideoUploadDateTime(RuTubeWebPage webPage)
        {
            return ExtractUploadDateFromWebPage(webPage.VideoWebPage);
        }

        private static string ExtractValue(string text, string paramId)
        {
            int n = text.IndexOf(paramId);
            if (n >= 0)
            {
                string t = text.Substring(n + paramId.Length);
                string s = t.Substring(0, 19);
                return s;
            }

            return null;
        }

        internal static DateTime StringToDateTime(string t)
        {
            return DateTime.ParseExact(t, "yyyy-MM-ddTHH:mm:ss",
                CultureInfo.CurrentCulture);
        }

        public static RuTubeVideo ParseRuTubeVideoInfo(
            RuTubeVideoInfoResult videoInfo, RuTubeWebPage videoWebPage)
        {
            if (videoInfo == null || videoInfo.ErrorCode != 200)
            {
                return null;
            }
            string videoTitle = videoInfo.VideoInfo.Value<string>("title");
            string videoId = videoInfo.VideoInfo.Value<string>("video_id");
            string videoDescription = null;
            JToken jt = videoInfo.VideoInfo.Value<JToken>("description");
            if (jt != null)
            {
                videoDescription = jt.Value<string>();
            }
            string thumbnailUrl = videoInfo.VideoInfo.Value<string>("thumbnail_url");
            int dur = int.Parse(videoInfo.VideoInfo.Value<string>("duration"));
            TimeSpan duration = TimeSpan.FromMilliseconds(dur);
            DateTime uploadDate = ExtractUploadDateFromWebPage(videoWebPage.VideoWebPage);
            DateTime publishedDate = ExtractPublishedDateFromWebPage(videoWebPage.VideoWebPage);

            string playlistManifestUrl = videoInfo.VideoInfo.Value<JObject>("video_balancer").Value<string>("m3u8");
            List<RuTubeVideoFormat> videoFormats = null;
            if (GetPlaylistManifest(playlistManifestUrl, out string manifest) == 200)
            {
                videoFormats = ParsePlaylistManifest(manifest);
            }

            MemoryStream imageData = GetImageData(thumbnailUrl);

            return new RuTubeVideo(videoTitle, videoId, videoDescription, duration,
                uploadDate, publishedDate, thumbnailUrl, videoFormats, imageData);
        }

        private static MemoryStream GetImageData(string imageUrl)
        {
            MemoryStream stream = new MemoryStream();
            FileDownloader d = new FileDownloader();
            d.Url = imageUrl;
            int errorCode = d.Download(stream);
            if (errorCode != 200)
            {
                stream.Dispose();
                stream = null;
            }
            return stream;
        }

        private static int GetPlaylistManifest(string manifestUrl, out string response)
        {
            FileDownloader d = new FileDownloader();
            d.Url = manifestUrl;
            int errorCode = d.DownloadString(out response);
            return errorCode;
        }

        private static List<RuTubeVideoFormat> ParsePlaylistManifest(string manifest)
        {
            ManifestParser parser = new ManifestParser(manifest);
            return parser.Parse();
        }

        public static RuTubeWebPage GetVideoWebPage(string videoId)
        {
            RuTubeWebPage ruTubeWebPage = new RuTubeWebPage();
            ruTubeWebPage.GetVideoWebPage(videoId);
            return ruTubeWebPage;
        }

        public static string GetVideoUrl(string videoId)
        {
            string url = $"{RUTUBE_BASE_URL}/video/{videoId}/";
            return url;
        }

        public static string GetVideoInfoRequestUrl(string videoId)
        {
            string url = $"{RUTUBE_ENDPOINT_PLAY_OPTIONS_URL}/{videoId}";
            return url;
        }

        internal static Dictionary<string, string> SplitStringToKeyValues(
            string inputString, string keySeparaor, char valueSeparator)
        {
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrWhiteSpace(inputString))
            {
                return null;
            }
            string[] keyValues = inputString.Split(new string[] { keySeparaor }, StringSplitOptions.None);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < keyValues.Length; i++)
            {
                string[] t = keyValues[i].Split(valueSeparator);
                dict.Add(t[0], t[1]);
            }
            return dict;
        }
    }
}
