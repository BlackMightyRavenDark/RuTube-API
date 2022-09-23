using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using MultiThreadedDownloaderLib;

namespace RuTubeApi
{
    public class ManifestParser
    {
        private readonly string Manifest;

        public ManifestParser(string manifest)
        {
            Manifest = manifest;
        }

        public List<RuTubeVideoFormat> Parse()
        {
            string manifest = Manifest;
            Regex regex = new Regex("CODECS=\"(.+?)\"");
            MatchCollection matches = regex.Matches(manifest);
            int diff = 0;
            for (int i = 0; i < matches.Count; ++i)
            {
                Match match = matches[i];

                int len = 7;
                string tmp = match.Value.Substring(len);
                int actualPos = match.Index + len;
                int actualLength = match.Length - len;
                string encoded = HttpUtility.UrlEncode(tmp);
                if (i > 0)
                {
                    diff += encoded.Length - actualLength;
                }
                manifest = manifest.Remove(actualPos + diff, actualLength).Insert(actualPos + diff, encoded);
            }

            List<RuTubeVideoFormat> resultList = new List<RuTubeVideoFormat>();
            string[] strings = manifest.Split('\n');
            int max = strings.Length - 2;
            for (int i = 1; i < max; i += 2)
            {
                string str = strings[i].Substring(strings[i].IndexOf(':') + 1);
                Dictionary<string, string> dict = Utils.SplitStringToKeyValues(str, ", ", '=');
                int bandwidth = dict.ContainsKey("BANDWIDTH") ? int.Parse(dict["BANDWIDTH"]) : 0;
                int videoWidth = 0;
                int videoHeight = 0;
                if (dict.ContainsKey("RESOLUTION"))
                {
                    string res = dict["RESOLUTION"];
                    string[] resSplitted = res.Split('x');
                    videoWidth = int.Parse(resSplitted[0]);
                    videoHeight = int.Parse(resSplitted[1]);
                }
                string codecs = dict.ContainsKey("CODECS") ? HttpUtility.UrlDecode(dict["CODECS"]) : null;

                string url = strings[i + 1];
                UrlList chunkUrls = null;
                if (GetPlaylist(url, out string playlist) == 200)
                {
                    string urlBase = url.Substring(0, url.LastIndexOf('/') + 1);
                    chunkUrls = ParsePlaylist(playlist, urlBase);
                }
                RuTubeVideoFormat videoFormat = new RuTubeVideoFormat(videoWidth, videoHeight, bandwidth, codecs, chunkUrls);
                resultList.Add(videoFormat);
            }

            return resultList;
        }

        private static int GetPlaylist(string playlistUrl, out string responce)
        {
            FileDownloader d = new FileDownloader();
            d.Url = playlistUrl;
            return d.DownloadString(out responce);
        }

        private UrlList ParsePlaylist(string playlist, string urlBase)
        {
            string[] strings = playlist.Split('\n');
            UrlList resultList = new UrlList();
            foreach (string str in strings)
            {
                if (!string.IsNullOrEmpty(str) && !str.StartsWith("#"))
                {
                    resultList.Add(urlBase + str);
                }
            }
            return resultList;
        }
    }
}
