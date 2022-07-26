using System.Collections.Generic;

namespace RuTubeApi
{
    public class UrlList
    {
        private List<string> _urls = new List<string>();
        public string[] Urls => _urls.ToArray();
        public int Count => _urls.Count;

        public UrlList()
        {
            
        }

        public int Add(string item)
        {
            _urls.Add(item);
            return Count - 1;
        }

        public void Clear()
        {
            _urls.Clear();
        }

        public override string ToString()
        {
            string t = string.Empty;
            foreach (string url in _urls)
            {
                t += "\n" + url;
            }

            return t;
        }
    }
}
