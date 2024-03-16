using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Model
{
    public abstract class Renovation
    {
        
        [JsonProperty("Time Slot")]
        public TimeSlot TimeSlot { get; set; }

        public Renovation(TimeSlot timeSlot)
        {
            TimeSlot = timeSlot;
        }

        public Renovation(DateTime start, DateTime end)
        {
            TimeSlot = new TimeSlot(start, end);
        }

        public Renovation()
        {

        }

        public bool IsFinished()
        {
            return DateTime.Now > TimeSlot.End;
        }
        public bool OverlapsWith(TimeSlot timeSlot)
        {
            return TimeSlot.OverlapsWith(timeSlot);
        }

        public abstract string ToString();
    }
}
