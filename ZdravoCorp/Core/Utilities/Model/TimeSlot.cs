using System;
using System.Globalization;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utilities.Model
{
    public class TimeSlot
    {
        [JsonProperty("Start")]
        public DateTime Start;
        [JsonProperty("End")]
        public DateTime End;

        [JsonConstructor]
        public TimeSlot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public TimeSlot(DateTime date, string appointmentTime,string duration)
        {   
            DateTime time = DateTime.ParseExact(appointmentTime, "HH:mm", CultureInfo.InvariantCulture);
            Start = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
            End = Start.AddMinutes(int.Parse(duration));
        }

        public bool OverlapsWith(TimeSlot timeSlot)
        {
            DateTime appointmentStart = timeSlot.Start;
            DateTime appointmentEnd = timeSlot.End;

            if (End <= appointmentStart || Start >= appointmentEnd)
            {
                return false;
            }

            return true;
        }

    }
}
