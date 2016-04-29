using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using ForexNews.API.Model;

namespace ForexNews.API.Services
{
    public class CalendarReader
    {
        private string url;

        public string CalendarRootName { get; set; } = "weekweeklyevents";

        public CalendarReaderDescendant ParentEventDescendant { get; set; }

        //child descendants
        public CalendarReaderDescendant TitleDescendant { get; set; }

        public CalendarReaderDescendant CountryDescendant { get; set; }
        public CalendarReaderDescendant DateDescendant { get; set; }

        public CalendarReaderDescendant TimeDescendant { get; set; }

        public CalendarReaderDescendant ImpactDescendant { get; set; }

        public CalendarReaderDescendant ForecastDescendant { get; set; }

        public CalendarReaderDescendant PreviousDescendant { get; set; }

        private IEnumerable<XElement> calendarDocument;

        public CalendarReader(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var crawler = new Crawler(url);
            if (!crawler.Connected)
            {
                throw new HttpException("Unable to connect to: " + url);
            }

            this.url = url;

            this.CreateDefaultParentEventDescendant();
            this.CreateDefaultChildDescendants();
        }

        private void CreateDefaultParentEventDescendant()
        {
            this.ParentEventDescendant = new CalendarReaderDescendant();
            this.ParentEventDescendant.DescendantName = "event";
        }

        private void CreateDefaultChildDescendants()
        {
            var titleDescendant = new CalendarReaderDescendant();
            titleDescendant.DescendantName = "title";

            var countryDescendant = new CalendarReaderDescendant();
            countryDescendant.DescendantName = "country";

            var dateDescendant = new CalendarReaderDescendant();
            dateDescendant.DescendantName = "date";

            var timeDescendant = new CalendarReaderDescendant();
            timeDescendant.DescendantName = "time";

            var impactDescendant = new CalendarReaderDescendant();
            impactDescendant.DescendantName = "impact";

            var forecastDescendant = new CalendarReaderDescendant();
            forecastDescendant.DescendantName = "forecast";

            var previousDescendant = new CalendarReaderDescendant();
            previousDescendant.DescendantName = "previous";

            this.TitleDescendant = titleDescendant;
            this.CountryDescendant = countryDescendant;
            this.DateDescendant = dateDescendant;
            this.TimeDescendant = timeDescendant;
            this.ImpactDescendant = impactDescendant;
            this.ForecastDescendant = forecastDescendant;
            this.PreviousDescendant = previousDescendant;
        }

        public void LoadCalendar()
        {
            if (string.IsNullOrWhiteSpace(this.url))
            {
                throw new ArgumentNullException(nameof(this.url));
            }

            this.calendarDocument = XDocument.Load(this.url).Root.Elements(this.ParentEventDescendant.DescendantName);
        }

        public IEnumerable<CalendarEvent> ReadEvents()
        {
            if (this.calendarDocument == null)
            {
                throw new ArgumentNullException(nameof(this.calendarDocument));
            }

            var culture = new System.Globalization.CultureInfo("en-US", true);

            return from e in this.calendarDocument
                   select new CalendarEvent
                   {
                       Title = (string)e.Element(this.TitleDescendant.DescendantName),
                       Country = (string)e.Element(this.CountryDescendant.DescendantName),
                       Date = Convert.ToDateTime((string)e.Element(this.DateDescendant.DescendantName) + " " + (string)e.Element(this.TimeDescendant.DescendantName), culture),
                       Impact = (string)e.Element(this.ImpactDescendant.DescendantName),
                       Forecast = (string)e.Element(this.ForecastDescendant.DescendantName),
                       Previous = (string)e.Element(this.PreviousDescendant.DescendantName)
                   };
        }
    }
}