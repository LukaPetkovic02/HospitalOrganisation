using System.Collections.Generic;

namespace ZdravoCorp.Core.PhysicalAsets.Equipment.Repository
{
    public interface IEquipmentRepository
    {
        public Model.Equipment FindById(string Id);

        public void GetAllEquipment();

        public void Save();

        public void CreateEquipment(Model.Equipment newEquipment);

        internal List<Model.Equipment> GetEquipments();
    }
}
