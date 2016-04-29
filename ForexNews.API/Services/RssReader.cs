using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using ForexNews.API.Extensions;

namespace ForexNews.API.Services
{
    public class RssReader
    {
        public List<SyndicationItem> Items { get; private set; }

        public RssReader(string url)
        {
            var crawler = new Crawler(url);

            if (!crawler.Connected)
            {
                throw new HttpException("Unable to connect to: " + url);
            }

            this.LoadItems(url);
        }

        public RssReader(string[] urls)
        {
            if (urls == null || urls?.Count() == 0)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            var canConnect = (from c in urls
                              select new Crawler(c).Connected).All(con => con == true);

            if (!canConnect)
            {
                throw new HttpException("Unable to connect to one or more of supplied URLs");
            }

            this.LoadItems(urls);
        }

        private void LoadItems(string[] urls)
        {
            if (urls == null || urls?.Count() == 0)
            {
                throw new ArgumentNullException(nameof(urls));
            }

            this.Items = (from url in urls
                          from xmlReader in XmlReader.Create(url).Use()
                          let feed = SyndicationFeed.Load(xmlReader)
                          from item in feed.Items
                          select item).Distinct().ToList();
        }

        private void LoadItems(string url)
        {
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            this.Items = feed.Items.ToList();
        }
    }
}