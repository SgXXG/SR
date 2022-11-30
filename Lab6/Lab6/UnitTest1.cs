using NUnit.Framework;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lab6
{
    public class Tests
    {
        public ChromeDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Url = "https://sch4sol.schools.by/";
        }

        [Test]
        public void questionTest()
        {
            var _executor = _driver as IJavaScriptExecutor;
            Assert.That(_executor, Is.Not.Null);

            //var waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            for (int i = 0; i < 20; i++)
            {
                var questionBtn = _driver.FindElement(By.XPath("//*[@id='page_layout']/div[2]/div/div[2]/ul/li[5]/a"));
                questionBtn.Click();
                System.Threading.Thread.Sleep(1000);

                char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
                Random rand = new Random();

                string message = "";
                for (int j = 1; j <= 100; j++)
                {
                    int letter_num = rand.Next(0, letters.Length - 1);

                    message += letters[letter_num];
                }

                var textArea = _driver.FindElement(By.XPath("//*[@id='support_wb_text']"));
                Assert.That(textArea, Is.Not.Null);
                _executor.ExecuteScript("arguments[0].textContent = '" + message + "';", textArea);
                System.Threading.Thread.Sleep(1000);

                var sendBtn = _driver.FindElement(By.XPath("//*[@id='support_wb_btn']"));
                sendBtn.Click();
                System.Threading.Thread.Sleep(1000);
            }
        }

        [Test]
        public void loginingTest()
        {
            var _executor = _driver as IJavaScriptExecutor;
            Assert.That(_executor, Is.Not.Null);

            //var waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                
            var loginSpan = _driver.FindElement(By.XPath("//*[@id='sch_login_lnk']/span"));
            loginSpan.Click();
            System.Threading.Thread.Sleep(1000);

            var sendBtn = _driver.FindElement(By.XPath("//*[@id='sch_login_box']/div/div[4]/input[2]"));
            sendBtn.Click();
            System.Threading.Thread.Sleep(1000);

            for (int i = 0; i < 50; i++)
            {
                char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
                char[] numbers = "0123456789".ToCharArray();
                Random rand = new Random();

                string login = "";
                for (int j = 1; j <= 3; j++)
                {
                    int number_num = rand.Next(0, numbers.Length - 1);
                    login += numbers[number_num];
                }

                var loginInput = _driver.FindElement(By.XPath("//*[@id='id_username']"));
                Assert.That(loginInput, Is.Not.Null);
                _executor.ExecuteScript("arguments[0].value = '" + login + "';", loginInput);
                System.Threading.Thread.Sleep(1000);

                string passwd = "";
                for (int j = 1; j <= 10; j++)
                {
                    int letter_num = rand.Next(0, letters.Length - 1);

                    passwd += letters[letter_num];
                }

                var passwdInput = _driver.FindElement(By.XPath("//*[@id='id_password']"));
                Assert.That(passwdInput, Is.Not.Null);
                _executor.ExecuteScript("arguments[0].value = '" + passwd + "';", passwdInput);
                System.Threading.Thread.Sleep(1000);

                var entryBtn = _driver.FindElement(By.XPath("//*[@id='page_layout']/div/div[1]/div[2]/div/div/div[2]/div/div/div[2]/div[2]/form/div[3]/div/div[2]/div/input[2]"));
                entryBtn.Click();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}