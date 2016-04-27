using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

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

        private void LoadItems(string url)
        {
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            this.Items = feed.Items.ToList();
        }
    }
}