namespace ForexNews.API.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CalendarEvent
    {
        public string Title { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }

        public string Impact { get; set; }

        public string Forecast { get; set; }

        public string Previous { get; set; }
    }
}