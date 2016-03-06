using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan
{
    class InputProcessor
    {

        public class CalEvent
        {
            // member variables
            public string EventName { get; set; }
            public DateTime EventTime { get; set; }
            public TimeSpan Duration { get; set; }

            // constructors
            public CalEvent(string name, int yr, int mo, int day,
                            int hr, int min, int sec, Double length)
            {
                this.EventName = name;
                this.EventTime = new DateTime(yr, mo, day, hr, min, sec);
                this.Duration = new TimeSpan();
                this.Duration = TimeSpan.FromMinutes(length);
            }
        }

        public class AddEvent
        {
            
        }
    }
}
