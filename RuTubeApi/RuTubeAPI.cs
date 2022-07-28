using Newtonsoft.Json.Linq;
using Multi_threaded_downloader;

namespace RuTubeApi
{
    public class RuTubeAPI
    {
        public const string RUTUBE_BASE_URL = "https://rutube.ru";
        public const string RUTUBE_ENDPOINT_PLAY_OPTIONS_URL = "https://rutube.ru/api/play/options";
        public const string RUTUBE_ENDPOINT_PROFILE_USER_URL = "http://rutube.ru/api/profile/user";

        public RuTubeVideo GetRuTubeVideo(string videoId)
        {
            RuTubeVideoInfoResult infoApi = GetVideoInfoFromApi(videoId);
            if (infoApi.ErrorCode == 200)
            {
                RuTubeWebPage ruTubeWebPage = Utils.GetVideoWebPage(videoId);
                return Utils.ParseRuTubeVideoInfo(infoApi, ruTubeWebPage);
            }
            return null;
        }

        public RuTubeChannel GetRuTubeChannel(string channelId)
        {
            return Utils.GetRuTubeChannelInfo(channelId);
        }

        public RuTubeVideoInfoResult GetVideoInfoFromApi(string videoId)
        {
            FileDownloader d = new FileDownloader();
            string url = Utils.GetVideoInfoRequestUrl(videoId);
            d.Url = url;
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
