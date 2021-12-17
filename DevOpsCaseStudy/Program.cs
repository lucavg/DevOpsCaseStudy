using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace DevOpsCaseStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();

            options.PageLoadStrategy = PageLoadStrategy.Normal;

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-crash-reporter");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-in-process-stack-traces");
            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--output=/dev/null");

            string decision = "0";

            Console.Clear();
            Console.WriteLine("Which site would you like to search?");
            Console.WriteLine("Youtube = 1");
            Console.WriteLine("Indeed = 2");
            Console.WriteLine("YgoProDeck = 3");
            Console.WriteLine();
            decision = Console.ReadLine();
            switch (decision)
            {
                case "1":
                    Console.WriteLine("You chose: Youtube");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine("Please enter a search term to scrape:");
                    string ytSearchTerm = Console.ReadLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    if (string.IsNullOrWhiteSpace(ytSearchTerm) == false)
                    {
                        using (var driver = new ChromeDriver(service, options))
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
                            Console.WriteLine("Scraping is done!");
                            Console.WriteLine("Results were sent to C:/DevOpsOutput/YoutubeOutput.csv!");
                            Console.WriteLine("Press any key to exit the program.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter a valid search term.");
                        Console.WriteLine("Program will now close");
                        break;
                    }
                    break;
                case "2":
                    Console.WriteLine("You chose: Indeed");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine("Please enter a job advertisement to scrape:");
                    string indSearchTerm = Console.ReadLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    if (string.IsNullOrWhiteSpace(indSearchTerm) == false)
                    {
                        using (var driver = new ChromeDriver(service, options))
                        {
                            driver.Navigate().GoToUrl("https://be.indeed.com/advanced_search?%22");

                            driver.Manage().Window.Maximize();

                            var indeedSearchBar = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[1]/div[1]/div[2]/input"));
                            indeedSearchBar.Click();
                            indeedSearchBar.SendKeys(indSearchTerm);

                            var indSortByDate = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[3]/div/div[3]/select/option[2]"));
                            indSortByDate.Click();

                            var indPeriod = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[2]/div[2]/select"));
                            indPeriod.Click();

                            indeedSearchBar.Submit();

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
                        Console.WriteLine("Scraping is done!");
                        Console.WriteLine("Results were sent to C:/DevOpsOutput/IndeedOutput.csv!");
                        Console.WriteLine("Press any key to exit the program.");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter a valid search term.");
                        Console.WriteLine("Program will now close");
                        break;
                    }
                    break;
                case "3":
                    Console.WriteLine("You chose: YgoProDeck");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine("Enter a deck you'd like to search for:");
                    string yugiohSearchTerm= Console.ReadLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    if (string.IsNullOrWhiteSpace(yugiohSearchTerm) == false)
                    {
                        using (var driver = new ChromeDriver(service, options))
                        {
                            Actions builder = new Actions(driver);
                            driver.Navigate().GoToUrl("https://ygoprodeck.com/deck-search");
                            driver.Manage().Window.Maximize();

                            var searchbox = driver.FindElement(By.XPath("//*[@id='DSPdeckName']"));
                            var searchButton = driver.FindElement(By.XPath("//*[@id='DSPsearchDecks']"));

                            searchbox.Click();
                            searchbox.SendKeys(yugiohSearchTerm);
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
                            Console.WriteLine("Scraping is done!");
                            Console.WriteLine("Results were sent to C:/DevOpsOutput/YugiohOutput.csv!");
                            Console.WriteLine("Press any key to exit the program.");
                            Console.ReadLine();
                        }
                    } else
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter a valid search term.");
                        Console.WriteLine("Program will now close");
                        break;
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("Thank you for using our webscraping service!");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }
}