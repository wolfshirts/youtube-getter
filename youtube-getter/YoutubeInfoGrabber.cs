using System.Net;
using System.Text.RegularExpressions;

namespace youtubegetter
{
    public class YoutubeInfoGrabber
    {
        public string Title { get; private set; }
        string url;
        public YoutubeInfoGrabber(string url)
        {
            this.url = url;
            getTitle();
        }

        void getTitle(){
            var html = new WebClient().DownloadString(url);
            var pattern = "^\"title\":\".+\"";
            Match match = Regex.Match(html, pattern);
            Title = match.ToString();
        }
    }
}
