
namespace RuTubeApi
{
    public class RuTubeVideoFormat
    {
        public int VideoWidth { get; private set; }
        public int VideoHeight { get; private set; }
        public int Bitrate { get; private set; }
        public string Codecs { get; private set; }
        public UrlList ChunkUrls { get; private set; }

        public RuTubeVideoFormat(int videoWidth, int videoHeight, int bitrate, string codecs, UrlList chunkUrls)
        {
            VideoWidth = videoWidth;
            VideoHeight = videoHeight;
            Bitrate = bitrate;
            Codecs = codecs;
            ChunkUrls = chunkUrls;
        }

        public string GetShortInfo()
        {
            string t = $"{VideoWidth}x{VideoHeight}, ~{Bitrate / 1024} kbps";
            return t;
        }

        public override string ToString()
        {
            string resolutionString = $"{VideoWidth}x{VideoHeight}";
            string bitrateString = $"~{Bitrate / 1024} kbps";
            string urls = ChunkUrls != null ? ChunkUrls.ToString() : "Empty!";
            string t = $"{resolutionString}, {bitrateString}, {Codecs}\nDownload URLs: {urls}" ;

            return t;
        }
    }
}
