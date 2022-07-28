using System;
using System.IO;

namespace RuTubeApi
{
    public class RuTubeChannel : IDisposable
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string Description { get; private set; }
        public bool IsOfficial { get; private set; }
        public uint VideoCount { get; private set; }
        public ulong ViewCount { get; private set; }
        public uint SubscriberCount { get; private set; }
        public string AvatarImageUrl { get; private set; }
        public Stream AvatarImageData { get; private set; }
        public DateTime DateCreated { get; private set; }

        public RuTubeChannel(string name, string id, string description,
            bool isOfficial, uint videoCount, ulong viewCount, uint subscriberCount,
            string avatarImageUrl, Stream avatarImageData, DateTime dateCreated)
        {
            Name = name;
            Id = id;
            Description = description;
            IsOfficial = isOfficial;
            VideoCount = videoCount;
            ViewCount = viewCount;
            SubscriberCount = subscriberCount;
            AvatarImageUrl = avatarImageUrl;
            AvatarImageData = avatarImageData;
            DateCreated = dateCreated;
        }

        public void Dispose()
        {
            if (AvatarImageData != null)
            {
                AvatarImageData.Dispose();
                AvatarImageData = null;
            }
        }

        public override string ToString()
        {
            string t = $"Name: {Name}\nID: {Id}\n" +
                $"URL: {RuTubeAPI.RUTUBE_BASE_URL}/video/person/{Id}/\nDescription: ";
            if (!string.IsNullOrEmpty(Description))
            {
                t += Description;
            }
            t += $"\nIs official: {IsOfficial}\nVideo count: {VideoCount}\nView count: {ViewCount}\n" +
                $"Subscriber count: {SubscriberCount}\n" +
                $"Avatar image URL: {AvatarImageUrl}\nAvatar image size: ";
            t += AvatarImageData != null ? $"{AvatarImageData.Length} bytes" : "NULL";

            return t;
        }
    }
}
