using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.PhysicalAsets.Room.Repository;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Room
{
    public class RoomService
    {
        public RoomRepository RoomRepository { get; set; }

        public RoomService()
        {
            RoomRepository = new RoomRepository();
        }

        public Model.Room FindById(string Id)
        {
            return RoomRepository.FindById(Id);
        }

        public List<Model.Room>? FindByType(RoomType roomType)
        {
            return RoomRepository.FindByType(roomType);
        }

        public bool IsAvailable(string roomId, List<Appointment> appointments, TimeSlot duration)
        {
            return RoomRepository.IsAvailable(roomId, appointments, duration);
        }

        public Model.Room FindFirstAvailableRoom(RoomType roomType, List<Appointment> appointments, TimeSlot duration)
        {
            return RoomRepository.FindFirstAvailableRoom(roomType, appointments, duration);
        }
        public List<Model.Room> GetAvailableRoomsForHospitalTreatment(DateTime start, List<HospitalTreatment> treatments)
        {
            return RoomRepository.GetAvailableRoomsForHospitalTreatment(start, treatments);
        }
        public void GetAllRoom()
        {
            RoomRepository.GetAllRoom();
        }
        public List<Model.Room> GetRooms()
        {
            return RoomRepository.GetRooms();
        }
        public bool HasAttribute(string Id, string attribute)
        {
            return FindById(Id).HasAtribute(attribute);
        }

        public void UpdateRoom(string Id, RoomType NewRoomType)
        {
            Model.Room room = FindById(Id);
            room.Type = NewRoomType;
            Save();
        }

        public void CreateRoom(Model.Room newRoom)
        {
            RoomRepository.CreateRoom(newRoom);
        }
        public void Save()
        {
            RoomRepository.Save();
        }

        public void DeleteRoom(string id)
        {
            RoomRepository.DeleteRoom(id);
        }
    }
}
