using Multi_threaded_downloader;

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
            ErrorCode = d.DownloadString(out string webPage);
            if (ErrorCode == 200)
            {
                VideoWebPage = webPage;
            }
            return ErrorCode;
        }
    }
}
