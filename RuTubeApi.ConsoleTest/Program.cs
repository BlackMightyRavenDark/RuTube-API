using System;
using static RuTubeApi.RuTubeAPI;

namespace RuTubeApi.ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string videoUrl = $"{RUTUBE_BASE_URL}/video/cef16f69b228bd7d1019cc6479ca92e2/";
            string videoId = Utils.ExtractVideoIdFromUrl(videoUrl);
            if (!string.IsNullOrEmpty(videoId))
            {
                RuTubeAPI.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:104.0) Gecko/20100101 Firefox/104.0";
                RuTubeAPI api = new RuTubeAPI();
                RuTubeVideo ruTubeVideo = api.GetRuTubeVideo(videoId);
                if (ruTubeVideo != null)
                {
                    Console.WriteLine(ruTubeVideo);
                    if (ruTubeVideo.ChannelOwned != null)
                    {
                        Console.WriteLine($"Channel info:\n{ruTubeVideo.ChannelOwned}");
                    }
                    else
                    {
                        Console.WriteLine("Channel info: NULL");
                    }
                }
                else
                {
                    Console.WriteLine($"Video ID {videoId} error!");
                }
            }
            else
            {
                Console.WriteLine("Failed to extract video ID!");
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }
}
