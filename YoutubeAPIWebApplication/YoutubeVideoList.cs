using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeAPIWebApplication
{
    public class YoutubeVideoList
    {
        public string keyword { get; set; }
        public DateTime publishedDate { get; set; }
        public List<string> videos = new List<string>();
        public List<string> channels = new List<string>();
        public List<string> playlists = new List<string>();

        public YoutubeVideoList(string keyword)
        {
            this.keyword = keyword;
            //YouTubeAPI.KeywordSearch(this);
        }

    }

    public class YoutubeVideo
    {
        //public string keyword { get; set; }
        public string subscriberCount { get; set; }
        public string watchCount { get; set; }
        public DateTime publishedDate { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string chanId { get; set; }
        public ulong viewCount { get; set; }
        public ulong likeCount { get; set; }
        public ulong dislikeCount { get; set; }
        public ulong commentCount { get; set; }

        public YoutubeVideo()
        {
        }

    }

}