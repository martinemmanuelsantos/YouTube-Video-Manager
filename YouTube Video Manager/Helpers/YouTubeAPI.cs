using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.IO;
using System.Threading;
using System.Net.Http;

namespace YouTube_Video_Manager.Helpers
{
    public static class YouTubeAPI
    {

        public static YouTubeService YTservice;
        private static UserCredential credentials;
        public static string channelID = "";
        public static string channelTitle = "";

        private static YouTubeService Auth()
        {
            using (var stream = new FileStream("youtube_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets,
                            new[] { YouTubeService.Scope.Youtube,
                                YouTubeService.Scope.Youtube,
                                YouTubeService.Scope.YoutubeForceSsl,
                                YouTubeService.Scope.Youtubepartner,
                                YouTubeService.Scope.YoutubepartnerChannelAudit,
                                YouTubeService.Scope.YoutubeReadonly,
                                YouTubeService.Scope.YoutubeUpload
                            },
                            "user",
                            CancellationToken.None
                            //new FileDataStore("YouTubeAPI")
                        ).Result;
            }
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "YouTubeAPI"
            });

            return service;
        }

        public static async Task<string> Authenticate()
        {
            YTservice = Auth();

            var request = YTservice.Channels.List("snippet");
            request.Mine = true;

            try
            {
                var response = await request.ExecuteAsync();
                channelID = response.Items[0].Id;
                channelTitle = response.Items[0].Snippet.Title;

                return "Authentication successful";
            } catch (Exception ex)
            {
                Console.WriteLine("Authentication was not successful, trying again...");
            }

            YTservice = Auth();

            try
            {
                var response = await request.ExecuteAsync();
                channelID = response.Items[0].Id;
                channelTitle = response.Items[0].Snippet.Title;

                return "Authentication successful";
            }
            catch (Exception ex)
            {
                return "Could not authenticate";
            }

        }

        public static async void Logout()
        {

            string response_string = "";

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "token", credentials.Token.AccessToken}
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://accounts.google.com/o/oauth2/revoke", content);

                response_string = await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine(response_string);

            System.Environment.Exit(1);
        }

        public static void GetVideoInfo(YouTubeVideo video)
        {
            var request = YTservice.Videos.List("snippet, contentDetails, status, statistics, player, fileDetails, liveStreamingDetails");
            request.Id = video.VideoID;

            try
            {
                var response = request.Execute();
                if (response.Items.Count > 0)
                {
                    video.Title = response.Items[0].Snippet.Title;
                    video.Description = response.Items[0].Snippet.Description;
                    video.Tags = response.Items[0].Snippet.Tags;
                    video.PublishedAt = response.Items[0].Snippet.PublishedAt.Value;

                    video.ChannelID = response.Items[0].Snippet.ChannelId;
                    video.ChannelTitle = response.Items[0].Snippet.ChannelTitle;
                    video.ThumbnailURL = response.Items[0].Snippet.Thumbnails.Default__.Url;
                    video.CategoryID = response.Items[0].Snippet.CategoryId;
                    video.LiveBroadcastContent = response.Items[0].Snippet.LiveBroadcastContent;
                    video.DefaultLanguage = response.Items[0].Snippet.DefaultLanguage;
                    video.DefaultAudioLanguage = response.Items[0].Snippet.DefaultAudioLanguage;

                    video.Duration = response.Items[0].ContentDetails.Duration;
                    video.Dimension = response.Items[0].ContentDetails.Dimension;
                    video.Definition = response.Items[0].ContentDetails.Definition;
                    video.Caption = response.Items[0].ContentDetails.Caption;
                    video.LicensedContent = response.Items[0].ContentDetails.LicensedContent;

                    video.UploadStatus = response.Items[0].Status.UploadStatus;
                    video.FailureReason = response.Items[0].Status.FailureReason;
                    video.RejectionReason = response.Items[0].Status.RejectionReason;
                    video.PrivacyStatus = response.Items[0].Status.PrivacyStatus;
                    video.PublishAt = response.Items[0].Status.PublishAt;
                    video.License = response.Items[0].Status.License;
                    video.Embeddable = response.Items[0].Status.Embeddable;
                    video.PublicStatsViewable = response.Items[0].Status.PublicStatsViewable;

                    video.ViewCount = response.Items[0].Statistics.ViewCount;
                    video.LikeCount = response.Items[0].Statistics.LikeCount;
                    video.DislikeCount = response.Items[0].Statistics.DislikeCount;
                    video.FavoriteCount = response.Items[0].Statistics.FavoriteCount;
                    video.CommentCount = response.Items[0].Statistics.CommentCount;

                    video.EmbedHTML = response.Items[0].Player.EmbedHtml;
                    video.EmbedHeight = response.Items[0].Player.EmbedHeight;
                    video.EmbedWidth = response.Items[0].Player.EmbedWidth;

                    video.FileName = response.Items[0].FileDetails.FileName;
                    video.FileSize = response.Items[0].FileDetails.FileSize;
                    video.FileType = response.Items[0].FileDetails.FileType;

                    if (response.Items[0].LiveStreamingDetails != null)
                    {
                        video.ActualStartTime = response.Items[0].LiveStreamingDetails.ActualStartTime;
                        video.ActualEndTime = response.Items[0].LiveStreamingDetails.ActualEndTime;
                        video.ScheduledStartTime = response.Items[0].LiveStreamingDetails.ScheduledStartTime;
                        video.ScheduledEndTime = response.Items[0].LiveStreamingDetails.ScheduledEndTime;
                        video.ConcurrentViewers = response.Items[0].LiveStreamingDetails.ConcurrentViewers;
                        video.ActiveLiveChatID = response.Items[0].LiveStreamingDetails.ActiveLiveChatId;
                    }

                    Console.WriteLine(video.Title);
                }
                else
                {
                    // Video ID not found
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void GetPlaylistInfo(YouTubePlaylist playlist)
        {
            var request = YTservice.Playlists.List("snippet, contentDetails, status, player");
            request.Id = playlist.PlaylistID;

            var response = request.Execute();
            if (response.Items.Count > 0)
            {
                playlist.Title = response.Items[0].Snippet.Title;
                playlist.Description = response.Items[0].Snippet.Description;
                playlist.Tags = response.Items[0].Snippet.Tags;
                playlist.PublishedDate = response.Items[0].Snippet.PublishedAt.Value;

                playlist.ChannelID = response.Items[0].Snippet.ChannelId;
                playlist.ChannelTitle = response.Items[0].Snippet.ChannelTitle;
                playlist.ThumbnailURL = response.Items[0].Snippet.Thumbnails.Default__.Url;
                playlist.DefaultLanguage = response.Items[0].Snippet.DefaultLanguage;
                playlist.PrivacyStatus = response.Items[0].Status.PrivacyStatus;
                playlist.ItemCount = response.Items[0].ContentDetails.ItemCount;
                playlist.EmbedHTML = response.Items[0].Player.EmbedHtml;

                Console.WriteLine(playlist.Title);
            }
            else
            {
                // Playlist ID not found
            }

        }

        public static List<YouTubeVideo> GetPlaylistVideos(YouTubePlaylist playlist)
        {

            List<YouTubeVideo> videos = new List<YouTubeVideo>();

            var request = YTservice.PlaylistItems.List("snippet");
            request.PlaylistId = playlist.PlaylistID;
            request.MaxResults = 50;
            
            var nextPageToken = "";
            while (nextPageToken != null)
            {

                request.PageToken = nextPageToken;
                var response = request.Execute();
                if (response.Items.Count > 0)
                {
                    for (int i = 0; i < response.Items.Count; i++)
                    {
                        string videoID = response.Items[i].Snippet.ResourceId.VideoId;
                        YouTubeVideo video = new YouTubeVideo(videoID);
                        
                        //video.GetInfo();
                        video.Title = response.Items[i].Snippet.Title;
                        video.Description = response.Items[i].Snippet.Description;
                        video.PublishedAt = response.Items[i].Snippet.PublishedAt.Value;
                        if (response.Items[0].Snippet.Thumbnails != null)
                            video.ThumbnailURL = response.Items[0].Snippet.Thumbnails.Default__.Url;

                        videos.Add(video);
                    }
                }
                else
                {
                    // Playlists not found
                }

                nextPageToken = response.NextPageToken;

            }

            return videos;

        }

        public static List<YouTubeVideo> GetListOfVideos()
        {

            List<YouTubeVideo> videos = new List<YouTubeVideo>();

            var request = YTservice.Channels.List("contentDetails");
            request.Mine = true;

            var response = request.Execute();
            if (response.Items.Count > 0)
            {
                string playlistID = response.Items[0].ContentDetails.RelatedPlaylists.Uploads;
                YouTubePlaylist playlist = new YouTubePlaylist(playlistID);

                videos.AddRange(GetPlaylistVideos(playlist));

            }
            else
            {
                // Channel not found
            }

            return videos;

        }

        public static List<YouTubePlaylist> GetListOfPlaylists()
        {

            List<YouTubePlaylist> playlists = new List<YouTubePlaylist>();

            var request = YTservice.Playlists.List("snippet");
            request.Mine = true;
            request.MaxResults = 50;

            var nextPageToken = "";
            while (nextPageToken != null)
            {

                request.PageToken = nextPageToken;
                var response = request.Execute();
                if (response.Items.Count > 0)
                {
                    for (int i = 0; i < response.Items.Count; i++)
                    {
                        string playlistID = response.Items[i].Id;
                        YouTubePlaylist playlist = new YouTubePlaylist(playlistID);

                        //playlist.GetInfo();
                        playlist.Title = response.Items[i].Snippet.Title;
                        playlist.Description = response.Items[i].Snippet.Description;
                        playlist.Tags = response.Items[i].Snippet.Tags;
                        playlist.PublishedDate = response.Items[i].Snippet.PublishedAt.Value;
                        playlist.ChannelID = response.Items[i].Snippet.ChannelId;
                        playlist.ChannelTitle = response.Items[i].Snippet.ChannelTitle;
                        playlist.ThumbnailURL = response.Items[0].Snippet.Thumbnails.Default__.Url;

                        if (playlist.ChannelID == channelID)
                            playlists.Add(playlist);
                    }
                }
                else
                {
                    // Playlists not found
                }

                nextPageToken = response.NextPageToken;

            }

            return playlists;

        }

    }
}
