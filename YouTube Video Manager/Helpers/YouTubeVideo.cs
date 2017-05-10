using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTube_Video_Manager.Helpers
{
    public class YouTubeVideo
    {
        public string VideoID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> Tags { get; set; }
        public DateTime PublishedAt { get; set; }

        public string ChannelID { get; set; }
        public string ChannelTitle { get; set; }
        public string ThumbnailURL { get; set; }
        public string CategoryID { get; set; }
        public string LiveBroadcastContent { get; set; }
        public string DefaultLanguage { get; set; }
        public string DefaultAudioLanguage { get; set; }

        public string Duration { get; set; }
        public string Dimension { get; set; }
        public string Definition { get; set; }
        public string Caption { get; set; }
        public bool? LicensedContent { get; set; }

        //public string RegionRestriction { get; set; }

        public string UploadStatus { get; set; }
        public string FailureReason { get; set; }
        public string RejectionReason { get; set; }
        public string PrivacyStatus { get; set; }
        public DateTime? PublishAt { get; set; }
        public string License { get; set; }
        public bool? Embeddable { get; set; }
        public bool? PublicStatsViewable { get; set; }

        public ulong? ViewCount { get; set; }
        public ulong? LikeCount { get; set; }
        public ulong? DislikeCount { get; set; }
        public ulong? FavoriteCount { get; set; }
        public ulong? CommentCount { get; set; }

        public string EmbedHTML { get; set; }
        public long? EmbedHeight { get; set; }
        public long? EmbedWidth { get; set; }

        //public string TopicDetails { get; set; }

        //public string RecordingDetails { get; set; }

        public string FileName { get; set; }
        public ulong? FileSize { get; set; }
        public string FileType { get; set; }
        //public string VideoStreams { get; set; }
        //public string AudioStreams { get; set; }

        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public DateTime? ScheduledStartTime { get; set; }
        public DateTime? ScheduledEndTime { get; set; }
        public ulong? ConcurrentViewers { get; set; }
        public string ActiveLiveChatID { get; set; }


        public YouTubeVideo(string ID)
        {
            this.VideoID = ID;
        }

        public void GetInfo()
        {
            YouTubeAPI.GetVideoInfo(this);
        }

        public string ShortDescription
        {
            get
            {
                if (Description.Length > 150)
                {
                    int index = (Description.Substring(0, 150) + "...").LastIndexOf(' ');

                    return Description.Substring(0, index) + "...";
                }
                else
                    return Description;
            }
        }

        public string TagsString
        {
            get
            {
                return (Tags != null) ? string.Join(",", Tags.ToArray()) : "";
            }
        }
    }
}
