using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Model
{
    public class JoinRoomRenovation : Renovation
    {
        [JsonProperty("First Room Id")]
        public string FirstId { get; set; }

        [JsonProperty("Second Room Id")]
        public string SecondId { get; set; }

        [JsonProperty("New Type")]
        public RoomType NewType { get; set; }

        [JsonProperty("New Id")]
        public string NewId { get; set; }

        public JoinRoomRenovation():base() { }
        
        public JoinRoomRenovation(string firstId, string secondId, TimeSlot timeSlot, RoomType newType, string newId) : base(timeSlot)
        {
            FirstId = firstId;
            SecondId = secondId;
            TimeSlot = timeSlot;
            NewType = newType;
            NewId = newId;
        }

        public JoinRoomRenovation(string firstId, string secondId, DateTime startDate, DateTime endDate, RoomType newType, string newId) : base(startDate, endDate)
        {
            FirstId = firstId;
            SecondId = secondId;
            TimeSlot = new TimeSlot(startDate, endDate);
            NewType = newType;
            NewId = newId;
        }

        public override string ToString()
        {
            return $"SplitRoomRenovation: [First Room Id: {FirstId}, Second Room Id: {SecondId}, New Type: {NewType}, New Id: {NewId}]";

        }
    }
}
