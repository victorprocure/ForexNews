using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForexNews.API.Services;

namespace ForexNews.API.Model
{
    public class Calendar
    {
        public List<CalendarEvent> Events { get; private set; }

        public Calendar(CalendarReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            reader.LoadCalendar();

            this.LoadEvents(reader);
        }

        public void LoadEvents(CalendarReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.Events = reader.ReadEvents().ToList();
        }
    }
}