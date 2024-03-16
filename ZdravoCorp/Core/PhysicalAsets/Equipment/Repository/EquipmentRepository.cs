using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAsets.Equipment.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        public List<Model.Equipment> Equipments { get; set; }
        private string FilePath = "../../../Data/equipment.json";

        public EquipmentRepository()
        {
            GetAllEquipment();
        }

        public Model.Equipment FindById(string Id)
        {
            foreach (Model.Equipment equipment in Equipments)
            {
                if (equipment.Id == Id)
                {
                    return equipment;
                }
            }
            return null;
        }

        public void GetAllEquipment()
        {
            Equipments = JsonConvert.DeserializeObject<List<Model.Equipment>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Equipments, Formatting.Indented));
        }

        public void CreateEquipment(Model.Equipment newEquipment)
        {
            Equipments.Add(newEquipment);
        }

        public List<Model.Equipment> GetEquipments()
        {
            return Equipments;
        }
    }
}
