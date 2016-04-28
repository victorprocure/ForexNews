using System;
using ForexNews.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForexNews.Tests
{
    [TestClass]
    public class ExternalHTMLTests
    {
        [TestMethod]
        public void CrawlerCanConnectToWebsiteThroughVariousMethods()
        {
            var uri = new Uri("http://www.google.com");

            var url = "google.com";
            var url2 = "http://www.google.com";
            var url3 = "https://www.google.com";
            var url4 = "www.google.com";
            var url5 = "8.8.8.8";

            var crawler = new Crawler();
            Assert.IsTrue(crawler.TestConnection(url));
            Assert.IsTrue(crawler.TestConnection(url2));
            Assert.IsTrue(crawler.TestConnection(url3));
            Assert.IsTrue(crawler.TestConnection(url4));
            Assert.IsTrue(crawler.TestConnection(url5));

            Assert.IsTrue(crawler.TestConnection(uri));
        }

        [TestMethod]
        public void CrawlerWillReturnFalseWhenItCantConnectToWebsite()
        {
            var url = "adfjadflkasfasdfasf.com";

            var crawler = new Crawler();
            Assert.IsFalse(crawler.TestConnection(url));
        }

        [TestMethod]
        public void CrawlerWillTestConnectionWhenGivenRssFeed()
        {
            var rss = "http://www.forexfactory.com/ffcal_week_this.xml";

            var crawler = new Crawler(rss);

            Assert.IsTrue(crawler.Connected);
        }

        [TestMethod]
        public void RssServiceCanReadFeed()
        {
            var rss = "http://rss.forexfactory.net/forums/tradingdiscussion.xml";

            var rssReader = new RssReader(rss);

            Assert.IsTrue(rssReader.Items.Count > 0);
        }

        [TestMethod]
        public void CalendarServiceCanReadFeed()
        {
            var xml = "http://www.forexfactory.com/ffcal_week_this.xml";

            var calendarReader = new CalendarReader(xml);

            var calendar = new Calendar(calendarReader);

            Assert.IsTrue(calendar.Events.Count > 0);
        }
    }
}