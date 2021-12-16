using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace DevOpsCaseStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            string decision = "0";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Which site would you like to search?");
                Console.WriteLine("Youtube = 1");
                Console.WriteLine("Indeed = 2");
                Console.WriteLine("YgoProDeck = 3");
                Console.WriteLine("Exit application = 4");
                Console.WriteLine();
                decision = Console.ReadLine();
                Console.WriteLine("You chose: " + decision);
                Thread.Sleep(1500);
                Console.Clear();
                if (decision == "1")
                {
                    Console.WriteLine("Please enter a search term to scrape:");
                    string ytSearchTerm = Console.ReadLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    if (ytSearchTerm != null || ytSearchTerm != "")
                    {
                        using (var driver = new ChromeDriver())
                        {
                            Console.WriteLine("You searched for: " + ytSearchTerm);
                            driver.Navigate().GoToUrl("https://www.youtube.com");
                            driver.Manage().Window.Maximize();

                            var confirm = driver.FindElement(By.XPath("//*[@id=\"content\"]/div[2]/div[5]/div[2]/ytd-button-renderer[2]/a"));
                            confirm.Click();

                            var searchbox = driver.FindElement(By.XPath("/html/body/ytd-app/div/div/ytd-masthead/div[3]/div[2]/ytd-searchbox/form/div[1]/div[1]/input"));

                            searchbox.Click();
                            searchbox.SendKeys(ytSearchTerm);
                            searchbox.Submit();
                            Thread.Sleep(1000);

                            string filterPath = "/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]";
                            var filter = driver.FindElement(By.XPath(filterPath + "/div/ytd-toggle-button-renderer/a/tp-yt-paper-button"));
                            filter.Click();
                            Thread.Sleep(1000);

                            var sortLastHour = driver.FindElement(By.XPath(filterPath + "/iron-collapse/div/ytd-search-filter-group-renderer[1]/ytd-search-filter-renderer[1]/a"));
                            sortLastHour.Click();
                            Thread.Sleep(1000);

                            filter.Click();
                            Thread.Sleep(1000);

                            var sortByDate = driver.FindElement(By.XPath(filterPath + "/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a"));
                            sortByDate.Click();
                            Console.Clear();
                            Thread.Sleep(200);

                            var videos = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div/ytd-item-section-renderer/div/ytd-video-renderer/div/div/div/div/h3/a/yt-formatted-string"));
                            Console.Clear();
                            using (var writer = new StreamWriter("c:/DevOpsOutput/YoutubeOutput.csv"))
                            {
                                for (int i = 1; i < 6; i++)
                                {
                                    string videoPath = "/html/body/ytd-app/div/ytd-page-manager/ytd-search/div/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div/ytd-item-section-renderer/div/ytd-video-renderer[" + i + "]";
                                    var videoTitle = driver.FindElements(By.XPath(videoPath + "/div/div/div/div/h3/a/yt-formatted-string"));
                                    var videoAuthor = driver.FindElements(By.XPath(videoPath + "/div/div/div/ytd-channel-name/div/div/yt-formatted-string/a"));
                                    var videoLink = driver.FindElements(By.XPath(videoPath + "/div/div/div/div/h3/a"));
                                    var videoViews = driver.FindElements(By.XPath(videoPath + "/div/div/div/ytd-video-meta-block/div/div/span"));
                                    YoutubeObject obj = new YoutubeObject();
                                    obj.setTitle(videoTitle.First().Text);
                                    obj.setAuthor(videoAuthor.First().Text);
                                    obj.setUrl(videoLink.First().GetDomProperty("href"));
                                    obj.setViews(videoViews.First().Text);
                                    writer.WriteLine(obj.ToString());
                                }
                            }
                            Console.WriteLine("Results were sent to C:/DevOpsOutput/YoutubeOutput.csv!");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                else if (decision == "2")
                {
                    Console.WriteLine("Please enter a job advertisement to scrape:");
                    string indSearchTerm = Console.ReadLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    if (indSearchTerm != null || indSearchTerm != "")
                    {
                        using (var driver = new ChromeDriver())
                        {
                            driver.Navigate().GoToUrl("https://be.indeed.com/advanced_search?%22");

                            driver.Manage().Window.Maximize();

                            var input = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[1]/div[1]/div[2]/input"));
                            input.Click();
                            input.SendKeys(indSearchTerm);

                            var indSortByDate = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[3]/div/div[3]/select/option[2]"));
                            indSortByDate.Click();

                            var indPeriod = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[2]/div[2]/select"));
                            indPeriod.Click();

                            input.Submit();

                            Thread.Sleep(2000);

                            string jobPath = "//*[@id=\"mosaic-provider-jobcards\"]/a[";

                            Directory.CreateDirectory("c:/DevOpsOutput");
                            using (var writer = new StreamWriter("c:/DevOpsOutput/IndeedOutput.csv"))
                            {
                                for (int i = 1; i < 11; i++)
                                {
                                    IndeedObject obj = new IndeedObject();
                                    string titlePath = jobPath + i + "]/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[1]/h2/span";
                                    obj.setTitle(driver.FindElement(By.XPath(titlePath)).Text);

                                    string companyPath = jobPath + i + "]/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/span";
                                    obj.setCompany(driver.FindElement(By.XPath(companyPath)).Text);

                                    string locationPath = jobPath + i + "]/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/div";
                                    string location = driver.FindElement(By.XPath(locationPath)).GetAttribute("innerHTML");
                                    if (location.IndexOf('<') != -1)
                                    {
                                        location = location.Substring(0, location.IndexOf('<'));
                                    }
                                    obj.setLocation(location);
                                    string urlPath = jobPath + i + "]";
                                    obj.setUrl(driver.FindElement(By.XPath(urlPath)).GetAttribute("href"));

                                    writer.WriteLine(obj.ToString());
                                }
                            }
                        }
                        Console.WriteLine("Results were sent to C:/DevOpsOutput/IndeedOutput.csv");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else if (decision == "3")
                {
                    using (var driver = new ChromeDriver())
                    {
                        Actions builder = new Actions(driver);
                        driver.Navigate().GoToUrl("https://ygoprodeck.com/deck-search");
                        driver.Manage().Window.Maximize();

                        var searchbox = driver.FindElement(By.XPath("//*[@id='DSPdeckName']"));
                        var searchButton = driver.FindElement(By.XPath("//*[@id='DSPsearchDecks']"));

                        Console.WriteLine("Enter a deck you'd like to search for:");
                        string input = Console.ReadLine();
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                        searchbox.Click();
                        searchbox.SendKeys(input);
                        searchButton.Click();
                        Thread.Sleep(2000);

                        string deckPath = "/html/body/div/div/div/div/div/div/div[";
                        using (var writer = new StreamWriter("c:/DevOpsOutput/YugiohOutput.csv"))
                        {
                            for (int i = 1; i < 11; i++)
                            {
                                YugiohObject obj = new YugiohObject();
                                var deckTitle = driver.FindElement(By.XPath(deckPath + i + "]/div/h2/a"));
                                obj.setTitle(deckTitle.Text);

                                var deckDescription = driver.FindElement(By.XPath(deckPath + i + "]/div/p[3]"));
                                obj.setDescription(deckDescription.Text);

                                var deckUrl = driver.FindElement(By.XPath(deckPath + i + "]/a"));
                                obj.setUrl(deckUrl.GetDomProperty("href"));

                                writer.WriteLine(obj.ToString());
                            }
                        }
                        Console.WriteLine("Results were sent to C:/DevOpsOutput/YugiohOutput.csv");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else if (decision != "1" || decision != "2" || decision != "3" || decision != "4")
                {
                    Console.WriteLine("Please enter a valid option from the choices given.");
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (decision == "4")
                {
                    Console.Clear();
                    Console.WriteLine("Thank you for using our webscraping service!");
                    Console.ReadLine();
                    break;
                }
            }
            Environment.Exit(0);
        }
    }
}