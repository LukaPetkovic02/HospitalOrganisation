using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Equipment.Repository;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PhysicalAsets.Equipment
{
    public class EquipmentService
    {
        public IEquipmentRepository EquipmentRepository { get; set; }


        public EquipmentService()
        {
            EquipmentRepository = new EquipmentRepository();
        }

        public  Model.Equipment FindById(string Id)
        {
            return EquipmentRepository.FindById(Id);
        }

        public void GetAllEquipment()
        {
            EquipmentRepository.GetAllEquipment();
        }

        public bool HasAttribute(string Id, string attribute)
        {
            return FindById(Id).HasAttribute(attribute);
        }

        public void Save()
        {
            EquipmentRepository.Save();
        }

        public void CreateEquipment(Model.Equipment newEquipment)
        {
            EquipmentRepository.CreateEquipment(newEquipment);
            Save();

        }

        internal List<Model.Equipment> GetEquipments()
        {
            return EquipmentRepository.GetEquipments();
        }
    }
}
