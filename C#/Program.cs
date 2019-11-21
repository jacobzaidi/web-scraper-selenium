using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://www.intercity.co.nz/";
            Thread.Sleep(6000);

            var a = driver.FindElement(By.Id("BookTravelForm_getBookTravelForm_from"));
            var b = driver.FindElement(By.Id("BookTravelForm_getBookTravelForm_to"));
            var c = driver.FindElement(By.Id("BookTravelForm_getBookTravelForm_date"));
            Thread.Sleep(5000);

            a.SendKeys("Auckland - Central");
            b.SendKeys("Tokoroa");
            Thread.Sleep(3000);

            c.Click();
            Thread.Sleep(3000);

            var d = driver.FindElement(By.XPath("//div[contains(text(), '22')]"));

            d.Click();
            Thread.Sleep(2000);

            var e = driver.FindElement(By.Id("BookTravelForm_getBookTravelForm_action_submit"));
            e.Click();

            var year = "2019";


            while (true)
            {
                using (StreamWriter f = File.AppendText(@"AKL - TOK.txt"))
                {

                    Thread.Sleep(10000);
                    var dates = driver.FindElements(By.XPath("//ul[@class='travel-dates js-show-ajax-end']/li"));
                    var index = -1;
                    for (int i = 0; i < dates.Count - 1; i++)
                    {
                        if (dates[i].GetAttribute("class") == "travel-date js-travel-date animate-cascade-down selected")
                        {
                            index = i;
                            break;
                        }
                    }

                    Thread.Sleep(2000);
                    var today = dates[index].FindElement(By.XPath(".//a")).Text;
                    var tomorrow = dates[index + 1].FindElement(By.XPath(".//a"));

                    if (year == "2019" & today.Contains("Jan")) year = "2020";
                    f.Write(today + " " + year);

                    var prices = driver.FindElements(By.XPath("//div[contains(@class,'price')]/ancestor::form[contains(@method, 'post')]"));
                    foreach (var price in prices)
                    {
                        string fare;
                        try
                        {
                            fare = price.FindElement(By.XPath(".//div[@class='price']")).Text;
                        }
                        catch
                        {
                            fare = "SOLD OUT";
                        }
                        var times = price.FindElements(By.XPath(".//div[@class='fare-time']"));
                        if (fare == "$1.00")
                        {
                            f.Write(" | " + times[0].Text + " - " + times[1].Text);
                            Console.WriteLine("*** $1 FARE ***");
                            Console.WriteLine();
                        }

                    }
                    f.Write('\n');
                    f.Close();
                    driver.Url = tomorrow.GetAttribute("href");
                }
            }
           
        }
    }
}
