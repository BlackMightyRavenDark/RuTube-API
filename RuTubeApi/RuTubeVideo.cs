using System;
using System.Collections.Generic;
using System.IO;

namespace RuTubeApi
{
    public class RuTubeVideo
    {
        public string Title { get; private set; }
        public string Id { get; private set; }
        public string Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateTime DateUploaded { get; private set; }
        public DateTime DatePublished { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public List<RuTubeVideoFormat> Formats { get; private set; }
        public Stream ImageData { get; private set; }
        public RuTubeChannel ChannelOwned { get; private set; }

        public RuTubeVideo(string title, string id, string description,
            TimeSpan duration, DateTime dateUploaded, DateTime datePublished,
            string thumbnailUrl, List<RuTubeVideoFormat> videoFormats, Stream imageData, RuTubeChannel channelOwned)
        {
            Title = title;
            Id = id;
            Description = description;
            Duration = duration;
            DateUploaded = dateUploaded;
            DatePublished = datePublished;
            ThumbnailUrl = thumbnailUrl;
            Formats = videoFormats;
            ImageData = imageData;
            ChannelOwned = channelOwned;
        }

        public void Dispose()
        {
            if (ImageData != null)
            {
                ImageData.Dispose();
                ImageData = null;
            }
        }

        public override string ToString()
        {
            string videoUrl = $"{RuTubeAPI.RUTUBE_BASE_URL}/video/{Id}/";
            string t = $"Title: {Title}\nID: {Id}\nURL: {videoUrl}\nDescription: ";
            if (!string.IsNullOrEmpty(Description))
            {
                t += Description;
            }
            t += $"\nDuration: {Duration}\n" +
                $"Upload date: {DateUploaded:yyyy.MM.dd HH:mm:ss}\n" +
                $"Publishing date: {DatePublished:yyyy.MM.dd HH:mm:ss}";
            t += $"\nThumbnail: {ThumbnailUrl}\nThumbnail size: ";
            t += (ImageData != null ? $"{ImageData.Length} bytes" : "NULL") + "\nFormats: ";
            if (Formats != null)
            {
                int formatCount = Formats.Count;
                t += formatCount.ToString();
                for (int i = formatCount - 1; i >= 0; --i)
                {
                    RuTubeVideoFormat videoFormat = Formats[i];
                    t += $"\n{videoFormat}";
                }
            }
            else
            {
                t += "None\n";
            }

            return t;
        }
    }
}
