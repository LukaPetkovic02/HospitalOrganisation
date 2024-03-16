using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Model
{
    public class SimpleRenovation : Renovation
    {

        [JsonProperty("Room Id")]
        public string Id { get; set; }

        [JsonProperty("Original Type")]
        public RoomType OriginalType { get; set; }

        [JsonProperty("New Type")]
        public RoomType NewType { get; set; }

        public SimpleRenovation() : base() { }

        public SimpleRenovation(string id, DateTime startDate, DateTime endDate, RoomType originalType, RoomType newType) : base(startDate, endDate)
        {
            Id = id;
            TimeSlot = new TimeSlot(startDate, endDate);
            NewType = newType;
            OriginalType = originalType;
        }

        public SimpleRenovation(string id, TimeSlot timeSlot, RoomType originalType, RoomType newType) : base(timeSlot)
        {
            Id = id;
            TimeSlot = timeSlot;
            OriginalType = originalType;
            NewType = newType;
        }

        public override string ToString()
        {
            return $"ChangeRoomTypeRenovation: [Room Id: {Id}, Original Type: {OriginalType}, New Type: {NewType}]";

        }

    }
}
