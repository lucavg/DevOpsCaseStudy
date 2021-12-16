using System.Text.RegularExpressions;

namespace DevOpsCaseStudy
{
    internal class IndeedObject
    {
        string title;
        string url;
        string location;
        string company;

        public IndeedObject()
        {
            this.title = "";
            this.url = "";
            this.location = "";
            this.company = "";
        }

        public IndeedObject(string title, string url, string location, string company)
        {
            this.title = title;
            this.url = url;
            this.location = location;
            this.company = company;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public string getTitle()
        {
            return this.title.Replace(",", "");
        }

        public void setUrl(string url)
        {
            this.url = url;
        }

        public string getUrl()
        {
            return this.url;
        }

        public void setLocation(string location)
        {
            this.location = location;
        }

        public string getLocation()
        {
            return this.location.Replace(",", "");
        }

        public void setCompany(string company)
        {
            this.company = company;
        }

        public string getCompany()
        {
            return this.company;
        }

        public override string ToString()
        {
            return this.getTitle() + "," +
                   this.getCompany() + "," +
                   this.getLocation() + "," +
                   this.getUrl(); ;
        }
    }
}
