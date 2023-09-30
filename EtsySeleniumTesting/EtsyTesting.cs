using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Linq;
using System.Xml.Linq;
using SeleniumExtras.WaitHelpers;



namespace ConsoleApp11
{
    [TestFixture]
    public class EtsyTesting
    {
        IWebDriver driver;
        WebDriverWait wait;


        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));


        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();

        }

        [Test]
        public void Register()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/");

            IWebElement signinbutton = driver.FindElement(By.ClassName("signin-header-action"));
            signinbutton.Click();

            IWebElement signupbutton = driver.FindElement(By.ClassName("select-register"));
            signupbutton.Click();

            string randomEmail = GenerateRandomEmail(); 
            IWebElement emailfield = wait.Until(ElementToBeClickable(By.Id("join_neu_email_field")));
            emailfield.SendKeys(randomEmail);

            IWebElement firstName = driver.FindElement(By.Id("join_neu_first_name_field"));
            firstName.SendKeys("Aba");

            IWebElement passwordfield = driver.FindElement(By.Id("join_neu_password_field"));
            passwordfield.SendKeys("asdasd123123");

            IWebElement registerButton = driver.FindElement(By.Name("submit_attempt"));
            registerButton.Click();

            //The wait is for the user to manually solve the recaptcha. 
            wait.Until(UrlToBe("https://www.etsy.com/?"));

            string expectedUrl = "https://www.etsy.com/?";
            string actualUrl = driver.Url;

            Assert.AreEqual(expectedUrl, actualUrl);


        }
        private string GenerateRandomEmail()
        {
            string baseEmail = "abaandpreach";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomEmail = $"{baseEmail}{timestamp}@gmail.com";

            return randomEmail;
        }



        [Test]
        public void Login()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/");

            IWebElement signinbutton = driver.FindElement(By.ClassName("signin-header-action"));
            signinbutton.Click();

            IWebElement emailfield = driver.FindElement(By.Id("join_neu_email_field"));
            emailfield.SendKeys("dimitarnikolov769@gmail.com");

            IWebElement passwordfield = driver.FindElement(By.Id("join_neu_password_field"));
            passwordfield.SendKeys("ddiimmiittaarr12345");

            IWebElement singinbutton = driver.FindElement(By.Name("submit_attempt"));
            singinbutton.Click();

            //The wait is for the user to manually solve the recaptcha. 
            wait.Until(UrlToBe("https://www.etsy.com/?"));

            string expectedurl = "https://www.etsy.com/?";
            string actualurl = driver.Url;
            Assert.AreEqual(expectedurl, actualurl);

        }



        [Test]
        public void SearchResults()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/");

            IWebElement searchBar = driver.FindElement(By.Name("search_query"));
            searchBar.SendKeys("Casio Watch");

            IWebElement searchButton = driver.FindElement(By.CssSelector("button[aria-label='Search']"));
            searchButton.Click();

            string expectedURL = "https://www.etsy.com/search?q=Casio%20Watch&ref=search_bar";
            string actualURL = driver.Url;

            Assert.AreEqual(expectedURL, actualURL);


        }



        [Test]
        public void ShoppingCart()
        {
            
            driver.Navigate().GoToUrl("https://www.etsy.com/listing/1109747524/casio-gold-a168wg-original-digital?click_key=ca4ba66e932cebdb97109f4fcf17dcc78adfa2c2%3A1109747524&click_sum=a87c8fb9&ref=hp_rv-1&pro=1&frs=1");

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement addToCartButton = driver.FindElement(By.CssSelector("div[data-selector='add-to-cart-button']"));
            addToCartButton.Click();

            IWebElement viewCartButton = driver.FindElement(By.CssSelector("a.wt-btn.wt-btn--primary.wt-width-full"));
            viewCartButton.Click();

            IWebElement itemInCart = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[@class='wt-text-title-01']/a")));

            Assert.IsTrue(itemInCart.Displayed, "Item was not added to the cart.");
        }

    }
}