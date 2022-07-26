using Newtonsoft.Json.Linq;

namespace RuTubeApi
{
    public class RuTubeVideoInfoResult
    {
        public JObject VideoInfo { get; private set; }
        public int ErrorCode { get; private set; }

        public RuTubeVideoInfoResult(JObject videoInfo, int errorCode)
        {
            VideoInfo = videoInfo;
            ErrorCode = errorCode;
        }
    }
}
