using MultiThreadedDownloaderLib;

namespace RuTubeApi
{
    public class RuTubeWebPage
    {
        public string VideoWebPage { get; private set; } = null;
        public int ErrorCode { get; private set; }

        public int GetVideoWebPage(string videoId)
        {
            string url = Utils.GetVideoUrl(videoId);
            FileDownloader d = new FileDownloader();
            d.Url = url;
            if (!string.IsNullOrEmpty(RuTubeAPI.UserAgent) && !string.IsNullOrWhiteSpace(RuTubeAPI.UserAgent))
            {
                d.Headers.Add("User-Agent", RuTubeAPI.UserAgent);
            }
            ErrorCode = d.DownloadString(out string webPage);
            if (ErrorCode == 200)
            {
                VideoWebPage = webPage;
            }
            return ErrorCode;
        }
    }
}
