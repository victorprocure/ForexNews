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

        public List<CalendarReaderDescendant> ChildDescendants { get; set; }

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

            this.ChildDescendants = new List<CalendarReaderDescendant>();

            this.ChildDescendants.Add(titleDescendant);
            this.ChildDescendants.Add(countryDescendant);
            this.ChildDescendants.Add(dateDescendant);
            this.ChildDescendants.Add(timeDescendant);
            this.ChildDescendants.Add(impactDescendant);
            this.ChildDescendants.Add(forecastDescendant);
            this.ChildDescendants.Add(previousDescendant);
        }

        public void LoadCalendar()
        {
            if (string.IsNullOrWhiteSpace(this.url))
            {
                throw new ArgumentNullException(nameof(this.url));
            }

            this.calendarDocument = XDocument.Load(this.url).Root.Elements(this.CalendarRootName);
        }

        public IEnumerable<CalendarEvent> ReadEvents()
        {
            if (this.calendarDocument == null)
            {
                throw new ArgumentNullException(nameof(this.calendarDocument));
            }

            from e in this.calendarDocument
            select new CalendarEvent
            {
                Title = (string)e.Element(this.ChildDescendants[0])
            }
        }
    }
}