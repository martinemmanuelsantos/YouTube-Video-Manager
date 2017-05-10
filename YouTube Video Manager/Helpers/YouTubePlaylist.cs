using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTube_Video_Manager.Helpers
{
    public class YouTubePlaylist
    {
        public string PlaylistID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> Tags { get; set; }
        public DateTime PublishedDate { get; set; }

        public string ChannelID { get; set; }
        public string ChannelTitle { get; set; }
        public string ThumbnailURL { get; set; }
        public string DefaultLanguage { get; set; }
        public string PrivacyStatus { get; set; }
        public long? ItemCount { get; set; }
        public string EmbedHTML { get; set; }

        public YouTubePlaylist(string ID)
        {
            this.PlaylistID = ID;
        }

        public void GetInfo()
        {
            YouTubeAPI.GetPlaylistInfo(this);
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
