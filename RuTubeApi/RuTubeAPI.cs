using Newtonsoft.Json.Linq;
using MultiThreadedDownloaderLib;
using static RuTubeApi.Utils;

namespace RuTubeApi
{
    public class RuTubeAPI
    {
        public const string RUTUBE_BASE_URL = "https://rutube.ru";
        public const string RUTUBE_ENDPOINT_PLAY_OPTIONS_URL = "https://rutube.ru/api/play/options";
        public const string RUTUBE_ENDPOINT_PROFILE_USER_URL = "https://rutube.ru/api/profile/user";

        public static string UserAgent = null;

        public RuTubeVideo GetRuTubeVideo(string videoId)
        {
            RuTubeVideoInfoResult infoApi = GetVideoInfoFromApi(videoId);
            if (infoApi.ErrorCode == 200)
            {
                RuTubeWebPage ruTubeWebPage = GetVideoWebPage(videoId);
                return ParseRuTubeVideoInfo(infoApi, ruTubeWebPage);
            }
            return null;
        }

        public RuTubeChannel GetRuTubeChannel(string channelId)
        {
            return GetRuTubeChannelInfo(channelId);
        }

        public RuTubeVideoInfoResult GetVideoInfoFromApi(string videoId)
        {
            FileDownloader d = new FileDownloader();
            string url = GetVideoInfoRequestUrl(videoId);
            d.Url = url;
            if (!string.IsNullOrEmpty(UserAgent) && !string.IsNullOrWhiteSpace(UserAgent))
            {
                d.Headers.Add("User-Agent", UserAgent);
            }
            int errorCode = d.DownloadString(out string jsonString);
            JObject json = errorCode == 200 ? JObject.Parse(jsonString) : null;
            return new RuTubeVideoInfoResult(json, errorCode);
        }

        public RuTubeChannelInfoResult GetChannelInfo(string channelId)
        {
            return Utils.GetChannelInfo(channelId);
        }
    }
}
