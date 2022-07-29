using System;
using System.IO;
using Multi_threaded_downloader;

namespace RuTubeApi.DownloadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string videoId = "cef16f69b228bd7d1019cc6479ca92e2";
            RuTubeAPI api = new RuTubeAPI();
            RuTubeVideo video = api.GetRuTubeVideo(videoId);
            if (video != null)
            {
                Console.WriteLine($"Video title: {video.Title}");
                string outputFileName = $"{FixFileName(video.Title)}.ts";
                if (File.Exists(outputFileName))
                {
                    File.Delete(outputFileName);
                }
                try
                {
                    FileDownloader d = new FileDownloader();
                    using (Stream outputStream = File.OpenWrite(outputFileName))
                    {
                        RuTubeVideoFormat format = video.Formats[video.Formats.Count - 1];
                        UrlList chunks = format.ChunkUrls;
                        Console.WriteLine($"Selected format: {format}");
                        for (int i = 0; i < chunks.Count; ++i)
                        {
                            string url = chunks.Urls[i];
                            d.Url = url;
                            Console.WriteLine($"Downloading chunk №{i + 1}...");
                            using (MemoryStream mem = new MemoryStream())
                            {
                                int errorCode = d.Download(mem);
                                if (errorCode != 200)
                                {
                                    Console.WriteLine("Download failed!");
                                    break;
                                }
                                mem.Position = 0;
                                if (!MultiThreadedDownloader.AppendStream(mem, outputStream))
                                {
                                    Console.WriteLine("Appending failed!");
                                    break;
                                }
                            }

                            if (i == chunks.Count - 1)
                            {
                                Console.WriteLine("Download success!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Download failed!");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Video ID {videoId} error!");
            }

            Console.ReadLine();
        }

        public static string FixFileName(string fn)
        {
            return fn.Replace("\\", "\u29F9").Replace("|", "\u2758").Replace("/", "\u2044")
                .Replace("?", "\u2753").Replace(":", "\uFE55").Replace("<", "\u227A").Replace(">", "\u227B")
                .Replace("\"", "\u201C").Replace("*", "\uFE61").Replace("^", "\u2303").Replace("\n", " ");
        }
    }
}
