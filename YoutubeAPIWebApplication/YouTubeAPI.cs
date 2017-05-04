using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeAPIWebApplication
{
    public class YouTubeAPI
    {
        public static List<YoutubeVideo> youtubeVideos;

        private static YouTubeService ytService = Auth();
        //Google.Apis.YouTube.v3.SearchResource.ListRequest;
        private static YouTubeService Auth()
        {
            UserCredential creds;
            
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\youtube_client_secret.json";
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                creds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("YouTubeAPI")
                    ).Result;
            }
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "YouTubeAPI"
            });
            return service;
        }

        public static void KeywordSearch(string keyword, int monthsAgo, int maxItems = 300, int maxSubscribers =1000)
        {
            int maxPages = 50;
            youtubeVideos = new List<YoutubeVideo>();

            var searchListRequest = ytService.Search.List("snippet");
            searchListRequest.Q = keyword;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
            searchListRequest.PublishedAfter = DateTime.Today.AddMonths(-1 * monthsAgo);

            searchListRequest.MaxResults = 50;
            if (maxItems < 50)
                searchListRequest.MaxResults = maxItems; 
        
            SearchListResponse searchListResponse;

            int page = 0;
            while (youtubeVideos.Count < maxItems && page < maxPages)
            {
                //var searchListResponse = await searchListRequest.ExecuteAsync();
                searchListResponse = searchListRequest.Execute();

                var nextToken = searchListResponse.NextPageToken;
                Debug.WriteLine("NextPageToken = " + nextToken);

                if (searchListResponse.Items.Count < 0)
                {
                    // video ID not found
                    Debug.WriteLine(string.Format("No video with keyword '{0}' found", keyword));
                    break;
                }
                parseSearchItems(searchListResponse.Items,maxSubscribers);
                page++;
            }
        

            var videos = from video in youtubeVideos
                         where video.type == "video"
                         select new { video.title };
            var channels = from video in youtubeVideos
                         where video.type == "channel"
                         select new { video.title };
            var playlists = from video in youtubeVideos
                         where video.type == "playlist"
                         select new { video.title };
            var unknowns = from video in youtubeVideos
                            where video.type == "unknowns"
                            select new { video.title };

            Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
            Console.WriteLine(String.Format("Unknowns:\n{0}\n", string.Join("\n", unknowns)));
        }

        /// <summary>
        /// Parses the returned API data and places it into the items static class
        /// </summary>
        /// <param name="items"></param>
        private static void parseSearchItems(IList<SearchResult> items,int maxSubscribers)
        {
            ulong viewCount    = 0;
            ulong likeCount    = 0;
            ulong dislikeCount = 0;
            ulong commentCount = 0;
            ulong subscriberCount = 0;

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in items)
            {
                string id = null;
                string chanId = null;
                string type = "unknown";
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        id = searchResult.Id.VideoId;
                        type = "video";
                        chanId = searchResult.Snippet.ChannelId;
                        break;

                    case "youtube#channel":
                        id = searchResult.Id.ChannelId;
                        type = "channel";
                        break;

                    case "youtube#playlist":
                        id = searchResult.Id.PlaylistId;
                        type = "playlist";
                        break;
                }
                if (id != null && chanId != null)
                {
                    var chanRequest = ytService.Channels.List("statistics");
                    chanRequest.Id = chanId;
                    var chanResponse = chanRequest.Execute();
                    if (chanResponse.Items.Count > 0)
                    {
                        //response.Items[0].Snippet.
                        subscriberCount    = chanResponse.Items[0].Statistics.SubscriberCount.Value;
                    }
                    else
                    {
                        // channel ID not found
                        Debug.WriteLine(string.Format("video ID {0} not found", chanId));
                    }
                    //if ((int) subscriberCount > m  (int) subscriberCount < maxSubscribers)
                    if ((int) subscriberCount < maxSubscribers)
                    {
                        var videoRequest = ytService.Videos.List("statistics");
                        videoRequest.Id = id;
                        var response = videoRequest.Execute();
                        if (response.Items.Count > 0)
                        {
                            //response.Items[0].Snippet.
                            viewCount = response.Items[0].Statistics.ViewCount.Value;
                            likeCount = response.Items[0].Statistics.LikeCount.Value;
                            dislikeCount = response.Items[0].Statistics.DislikeCount.Value;
                            commentCount = response.Items[0].Statistics.CommentCount.Value;
                        }
                        else
                        {
                            // video ID not found
                            Debug.WriteLine(string.Format("video ID {0} statistics not found", id));
                        }
                    }
                }
                if ((int) subscriberCount < maxSubscribers)
                    youtubeVideos.Add(new YoutubeVideo() {
                        title = searchResult.Snippet.Title,
                        id = id,
                        type = type,
                        publishedDate = searchResult.Snippet.PublishedAt.Value,
                        viewCount = viewCount,
                        likeCount = likeCount,
                        dislikeCount = dislikeCount,
                        commentCount = commentCount
                    });
            }
        }

    //public static void KeywordSearch(YoutubeVideoList videoList)
    //{
    //    var searchListRequest = ytService.Search.List("snippet");
    //    searchListRequest.Q = videoList.keyword;
    //    searchListRequest.MaxResults = 50;
    //    //var searchListResponse = await searchListRequest.ExecuteAsync();
    //    var searchListResponse = searchListRequest.Execute();

    //    if (searchListResponse.Items.Count < 0)
    //    {
    //        // video ID not found
    //        Debug.WriteLine(string.Format("No video with keyword '{0}' found", videoList.keyword));
    //    }


    //    // Add each result to the appropriate list, and then display the lists of
    //    // matching videos, channels, and playlists.
    //    foreach (var searchResult in searchListResponse.Items)
    //    {
    //        switch (searchResult.Id.Kind)
    //        {
    //            case "youtube#video":
    //                videoList.videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
    //                break;

    //            case "youtube#channel":
    //                videoList.channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
    //                break;

    //            case "youtube#playlist":
    //                videoList.playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
    //                break;
    //        }
    //    }

    //    Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videoList.videos)));
    //    Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", videoList.channels)));
    //    Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", videoList.playlists)));

    //}


    public static void GetVideoInfo(YouTubeVideo video)
        {
            var videoRequest = ytService.Videos.List("snippet");
            videoRequest.Id = video.id;

            var response = videoRequest.Execute();
            if (response.Items.Count > 0)
            {
                //response.Items[0].Snippet.
                video.title = response.Items[0].Snippet.Title;
                video.description = response.Items[0].Snippet.Description;
                video.publishedDate = response.Items[0].Snippet.PublishedAt.Value;
            } else
            {
                // video ID not found
                Debug.WriteLine(string.Format("video ID {0} not found", video.id));
            }
        }
    }
}