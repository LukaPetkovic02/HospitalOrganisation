using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Model
{
    public class SplitRoomRenovation : Renovation
    {
        [JsonProperty("Room Id")]
        public string Id { get; set; }

        [JsonProperty("New Type First Room")]
        public RoomType NewTypeFirstRoom { get; set; }

        [JsonProperty("New Type Second Room")]
        public RoomType NewTypeSecondRoom { get; set; }

        [JsonProperty("New Id First Room")]
        public string NewIdFirstRoom { get; set; }

        [JsonProperty("New Id Second Room")]
        public string NewIdSecondRoom { get; set; }
        public SplitRoomRenovation() : base() { }

        public SplitRoomRenovation(string id, DateTime startDate, DateTime endDate, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond) : base(startDate, endDate)
        {
            
            Id = id;
            TimeSlot = new TimeSlot(startDate, endDate);
            NewTypeFirstRoom = newTypeFirst;
            NewTypeSecondRoom = newTypeSecond;
            NewIdFirstRoom = newIdFirst;
            NewIdSecondRoom = newIdSecond;
        }

        public SplitRoomRenovation(string id, TimeSlot timeSlot, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond) : base(timeSlot)
        {
            Id = id;
            TimeSlot = timeSlot;
            NewTypeFirstRoom = newTypeFirst;
            NewTypeSecondRoom = newTypeSecond;
            NewIdFirstRoom = newIdFirst;
            NewIdSecondRoom = newIdSecond;
        }

        public override string ToString()
        {
            return $"SplitRoomRenovation: [Room Id: {Id}, New Type First Room: {NewTypeFirstRoom}, New Type Second Room: {NewTypeSecondRoom}, New Id First Room: {NewIdFirstRoom}, New Id Second Room: {NewIdSecondRoom}]";
        }
    }
}
