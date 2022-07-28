using Newtonsoft.Json.Linq;

namespace RuTubeApi
{
    public class RuTubeChannelInfoResult
    {
        public JObject ChannelInfo { get; private set; }
        public int ErrorCode { get; private set; }

        public RuTubeChannelInfoResult(JObject channelInfo, int errorCode)
        {
            ChannelInfo = channelInfo;
            ErrorCode = errorCode;
        }
    }
}
