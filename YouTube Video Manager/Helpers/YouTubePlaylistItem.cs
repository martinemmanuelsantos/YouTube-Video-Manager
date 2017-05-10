using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTube_Video_Manager.Helpers
{
    public class YouTubePaylistItem
    {
        public string PlaylistItemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime PublishedAt { get; set; }
        public string ChannelID { get; set; }
        public string ChannelTitle { get; set; }
        public string ThumbnailURL { get; set; }
        public string PlaylistID { get; set; }
        public uint Position { get; set; }

        public string Kind { get; set; }

        public string VideoID { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public string Note { get; set; }
        public DateTime VideoPublishedAt { get; set; }

        public string PrivacyStatus { get; set; }


        public YouTubePaylistItem(string ID)
        {
            this.PlaylistItemID = ID;
        }

        public void GetInfo()
        {
            //YouTubeAPI.GetPlaylistItemInfo(this);
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
    }
}
