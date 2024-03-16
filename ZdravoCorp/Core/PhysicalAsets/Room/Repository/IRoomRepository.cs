using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Room.Repository
{
    internal interface IRoomRepository
    {
        public Model.Room FindById(string Id);

        public List<Model.Room>? FindByType(RoomType roomType);

        public bool IsAvailable(string roomId, List<Appointment> appointments, TimeSlot duration);

        public Model.Room? FindFirstAvailableRoom(RoomType roomType, List<Appointment> appointments, TimeSlot duration);

        public void GetAllRoom();

        public void Save();

        public void CreateRoom(Model.Room newRoom);

        public void DeleteRoom(string id);

        public List<Model.Room> GetRooms();
    }
}
