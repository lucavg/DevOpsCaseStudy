using System.Text.RegularExpressions;

namespace DevOpsCaseStudy
{
    internal class YoutubeObject
    {
        string title;
        string url;
        string views;
        string author;

        public YoutubeObject()
        {
            this.title = "";
            this.url = "";
            this.views = "";
            this.author = "";
        }

        public YoutubeObject(string title, string url, string description, string author)
        {
            this.title = title;
            this.url = url;
            this.views = description;
            this.author = author;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public string getTitle()
        {
            return Regex.Replace(this.title, @"(\s*,\s*)+", ",").TrimEnd(',');
        }

        public void setUrl(string url)
        {
            this.url = url;
        }

        public string getUrl()
        {
            return this.url;
        }

        public void setViews(string views)
        {
            this.views = views;
        }

        public string getViews()
        {
            return this.views;
        }

        public void setAuthor(string author)
        {
            this.author = author;
        }

        public string getAuthor()
        {
            return this.author;
        }

        public override string ToString()
        {
            return this.getTitle() + "," +
                   this.getAuthor() + "," +
                   this.getViews() + "," +
                   this.getUrl(); ;
        }
    }
}
