using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Room.Model
{
    public enum RoomType { OperationRoom, ExaminationRoom, PatientRoom, WaitingRoom, Storage };
    public class Room
    {
        [JsonProperty("Room Id")]
        public string Id { get; set; }
        [JsonProperty("Room Type")]
        public RoomType Type { get; set; }

        public Room() { }

        public Room(string id, RoomType type)
        {
            Id = id;
            Type = type;
        }
        public bool HasAtribute(string atribute)
        {
            return Id.ToUpper().Contains(atribute.ToUpper()) || Type.ToString().ToUpper().Contains(atribute.ToUpper());
        }
    }
}
