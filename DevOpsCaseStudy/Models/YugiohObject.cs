using System.Text.RegularExpressions;

namespace DevOpsCaseStudy
{
    internal class YugiohObject
    {
        string title;
        string description;
        string url;

        public YugiohObject()
        {
            this.title = "";
            this.description = "";
            this.url = "";
        }

        public YugiohObject(string title, string description, string url)
        {
            this.title = title;
            this.description = description;
            this.url = url;
        }

        public void setTitle(string title) 
        { 
            this.title = title; 
        }

        public string getTitle()
        {
            return this.title;
        }

        public void setDescription(string description)
        {
            this.description = description;
        }

        public string getDescription()
        {
            return this.description.Replace(",", "");
        }

        public void setUrl(string url)
        {
            this.url = url;
        }

        public string getUrl()
        {
            return this.url;
        }
        
        public override string ToString()
        {
            return this.getTitle() + "," + 
                   this.getDescription() + "," +
                   this.getUrl();
        }
    }
}
