using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Linq;
using System.Xml.Linq;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System.Reflection.Emit;

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

            IWebElement welcomeMessage = driver.FindElement(By.CssSelector("div[data-appears-component-name='Homepage_Vesta_View_WelcomeMessage']"));
            string expectedText = "Welcome";
            string actualText = welcomeMessage.Text;

            Assert.IsTrue(actualText.Contains(expectedText));

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
        public void AddingToShoppingCart()
        {
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.Navigate().GoToUrl("https://www.etsy.com/listing/1109747524/casio-gold-a168wg-original-digital?click_key=ca4ba66e932cebdb97109f4fcf17dcc78adfa2c2%3A1109747524&click_sum=a87c8fb9&ref=hp_rv-1&pro=1&frs=1");

            IWebElement addToCartButton = driver.FindElement(By.CssSelector("div[data-selector='add-to-cart-button']"));
            addToCartButton.Click();

            IWebElement viewCartButton = driver.FindElement(By.CssSelector("a[data-selector='post-atc-overlay-go-to-cart-button']"));
            viewCartButton.Click();

            IWebElement itemInCart = wait2.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[@class='wt-text-title-01']/a")));

            Assert.IsTrue(itemInCart.Displayed, "Item was not added to the cart.");

        }

        [Test]
        public void ChangingProfileInformation()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/");

            WebDriverWait wait2;
            wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement signinButton = driver.FindElement(By.ClassName("signin-header-action"));
            signinButton.Click();

            IWebElement emailField = driver.FindElement(By.Id("join_neu_email_field"));
            emailField.SendKeys("dimitarnikolov769@gmail.com");

            IWebElement passwordField = driver.FindElement(By.Id("join_neu_password_field"));
            passwordField.SendKeys("ddiimmiittaarr12345");

            IWebElement singinButton = driver.FindElement(By.Name("submit_attempt"));
            singinButton.Click();

            // Wait for the user to manually solve the CAPTCHA
            wait.Until(UrlToBe("https://www.etsy.com/?"));

            IWebElement accountDropdownButton = driver.FindElement(By.CssSelector("button[aria-label='You']"));
            accountDropdownButton.Click();

            IWebElement accountSettingsButton = driver.FindElement(By.CssSelector("a[role='menuitem'][href='https://www.etsy.com/your/account?ref=hdr_user_menu-settings']"));
            accountSettingsButton.Click();

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            IWebElement editProfileButton = driver.FindElement(By.CssSelector("button[class='wt-btn wt-btn--secondary wt-btn--small']"));
            editProfileButton.Click();

            IWebElement nameChangeLink = driver.FindElement(By.CssSelector("a[class='request-name-change overlay-trigger']"));
            nameChangeLink.Click();

            IWebElement iframeElement = driver.FindElement(By.Id("namechange-overlay"));

            IWebElement firstNameInput = driver.FindElement(By.Name("new-first-name"));
            firstNameInput.Clear();
            firstNameInput.SendKeys(GenerateRandomFirstName());

            IWebElement lastNameInput = driver.FindElement(By.Name("new-last-name"));
            lastNameInput.Clear();
            lastNameInput.SendKeys(GenerateRandomLastName());

            IWebElement saveChangesNameAndSurnameButton = driver.FindElement(By.Name("save"));
            saveChangesNameAndSurnameButton.Click();

            IWebElement genderButton = driver.FindElement(By.Id("male"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView();", genderButton);
            genderButton.Click();

            IWebElement cityField = driver.FindElement(By.Id("city3"));
            cityField.Clear();
            cityField.SendKeys("Skopje, North Macedonia");

            IWebElement birthMonthDropdown = driver.FindElement(By.Id("birth-month"));

            SelectElement selectMonth = new SelectElement(birthMonthDropdown);

            selectMonth.SelectByText("September");

            IWebElement birthDayDropdown = driver.FindElement(By.Id("birth-day"));

            SelectElement selectDay = new SelectElement(birthDayDropdown);

            selectDay.SelectByIndex(14);

            IWebElement aboutField = driver.FindElement(By.Id("bio"));
            aboutField.Clear();
            aboutField.SendKeys("I am a 22 year old junior tester looking for a job");

            IWebElement materialsField = driver.FindElement(By.Id("materials"));
            materialsField.Clear();
            materialsField.SendKeys("Meditations by Marcus Aurelius");

            IWebElement saveChangesOnProfileInfo = driver.FindElement(By.CssSelector("input[type='submit']"));
            saveChangesOnProfileInfo.Click();

            IWebElement successMessage = wait2.Until(ElementIsVisible(By.XPath("//div[@id='messages']//h3[text()='Your changes have been saved.']")));

            Assert.IsTrue(successMessage.Displayed, "Success message 'Your changes have been saved.' is displayed.");


        }

        public string GenerateRandomFirstName()
        {
            List<string> firstNames = new List<string>
            {
                 "John",
                 "Jane",
                 "Michael",
                 "Emily",
                 "Ben",
                 "Rachel",
                 "Chandler",
                 "Eva",
                 "Dimitar",
        
            };

            Random random = new Random();
            int index = random.Next(0, firstNames.Count);
            return firstNames[index];
        }

        public string GenerateRandomLastName()
        {
            List<string> lastNames = new List<string>
            {
                 "Smith",
                 "Johnson",
                 "Brown",
                 "Davis",
                 "Tennison",
                 "Kenley",
                 "Newman",
                 "Bush",
        
            }; 

            Random random = new Random();
            int index = random.Next(0, lastNames.Count);
            return lastNames[index];
        }



        [Test]
        public void ProductNameOnProductPageAndShoppingCartPage()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/listing/1109747524/casio-gold-a168wg-original-digital?click_key=ca4ba66e932cebdb97109f4fcf17dcc78adfa2c2%3A1109747524&click_sum=a87c8fb9&ref=hp_rv-1&pro=1&frs=1");

            WebDriverWait wait2;
            wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement elementProductName = driver.FindElement(By.CssSelector("h1[data-buy-box-listing-title='true']"));

            string productNameOnProductPage = elementProductName.Text;

            IWebElement addToCartButton = driver.FindElement(By.CssSelector("div[data-selector='add-to-cart-button']"));
            addToCartButton.Click();

            wait2.Until(ElementExists(By.CssSelector("div[class='wt-overlay__modal']")));

            IWebElement viewCartButton = wait2.Until(ElementExists(By.CssSelector("a[data-selector='post-atc-overlay-go-to-cart-button']")));
            viewCartButton.Click();
            

            IWebElement cartProductNameElement = driver.FindElement(By.CssSelector("a[data-title='CASIO Gold A168WG Original Digital Illuminator Watch - Casio Alarm Chrono Watch, Water resist watch Unisexs Wristwatch, Japan watch']"));

            string cartProductName = cartProductNameElement.Text;

            Assert.AreEqual(productNameOnProductPage, cartProductName);


        }

        [Test]
        public void FilteringProducts()
        {
            driver.Navigate().GoToUrl("https://www.etsy.com/featured/");

            WebDriverWait wait2;
            wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            IWebElement homeAndLivingHoverButton = driver.FindElement(By.Id("catnav-primary-link-891"));

            Actions action = new Actions(driver);
            action.MoveToElement(homeAndLivingHoverButton).Perform();

            IWebElement candlesAndHoldersButton = driver.FindElement(By.Id("catnav-l4-10965"));
            candlesAndHoldersButton.Click();

            IWebElement filterButton = wait2.Until(ElementToBeClickable(By.CssSelector("button[aria-controls='search-filters-overlay']")));
            filterButton.Click();

            IWebElement priceRangeButton = wait2.Until(ElementToBeClickable(By.CssSelector("label[for='price-input-2']")));
            priceRangeButton.Click();

            IWebElement freeShippingButton = driver.FindElement(By.CssSelector("label[for='special-offers-free-shipping']"));
            freeShippingButton.Click();

            IWebElement colorButton = driver.FindElement(By.CssSelector("label[for='attr_1-2']"));
            colorButton.Click();

            IWebElement locationEuropeButton = driver.FindElement(By.CssSelector("label[for='shop-location-input-1']"));
            locationEuropeButton.Click();

            List<IWebElement> shippingLocation = driver.FindElements(By.Id("ship_to_select")).ToList();

            IWebElement macedoniaButton = shippingLocation.FirstOrDefault(el => el.Text.Contains("Macedonia"));
            macedoniaButton.Click();

            IWebElement applyButton = driver.FindElement(By.CssSelector("button[form='search-filter-form']"));
            applyButton.Click();

            List<IWebElement> priceFilterElements = driver.FindElements(By.CssSelector("a[aria-label='Remove USD 25 – USD 50 Filter']")).ToList();
            List<IWebElement> europeFilterElements = driver.FindElements(By.CssSelector("a[aria-label='Remove Items from Europe Filter']")).ToList();

            // Assert that the elements are present
            Assert.IsTrue(priceFilterElements.Count > 0, "Price filter element is not present on the page.");
            Assert.IsTrue(europeFilterElements.Count > 0, "Europe filter element is not present on the page.");





        }


    }
}