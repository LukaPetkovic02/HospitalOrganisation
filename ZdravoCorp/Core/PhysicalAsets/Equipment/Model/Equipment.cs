using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Equipment.Model
{
    public enum EquipmentType { ExaminationEquipment, OperationEquipment, RoomEquipment, CorridorEquipment };
    public class Equipment
    {
        [JsonProperty("Equipment Id")]
        public string Id { get; set; }

        [JsonProperty("Equipment Name")]
        public string Name { get; set; }

        [JsonProperty("Equipment Type")]
        public EquipmentType Type { get; set; }

        [JsonProperty("Equipment Dynamic")]
        public bool Dynamic { get; set; }

        public bool HasAttribute(string atribute)
        {
            return Id.ToUpper().Contains(atribute.ToUpper()) || Name.ToUpper().Contains(atribute.ToUpper()) || Type.ToString().ToUpper().Contains(atribute.ToUpper());
        }

        public Equipment(string id, string name, EquipmentType type, bool dynamic)
        {
            Id = id;
            Name = name;
            Type = type;
            Dynamic = dynamic;
        }
    }
}
