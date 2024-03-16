using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Room.Repository
{

    public class RoomRepository : IRoomRepository
    {

        public List<Model.Room> Rooms { get; set; }
        private string FilePath = "../../../Data/room.json";

        public RoomRepository()
        {
            GetAllRoom();
        }

        public Model.Room FindById(string Id)
        {
            foreach (Model.Room room in Rooms)
            {
                if (room.Id == Id)
                {
                    return room;
                }
            }
            return null;
        }

        public List<Model.Room>? FindByType(RoomType roomType)
        {
            List<Model.Room> rooms = new List<Model.Room>();
            foreach (var room in Rooms)
            {
                if (room.Type == roomType)
                {
                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public bool IsAvailable(string roomId, List<Appointment> appointments,TimeSlot duration)//ovde dodati dodatan uslov za renovacije sobe
        {
            foreach (var appointment in appointments)
            {
                if (appointment.RoomId == roomId && appointment.Status == AppointmentStatus.SCHEDULED && appointment.TimeSlot.OverlapsWith(duration))
                {
                    return false;
                }
            }

            return true;
        }

        public Model.Room? FindFirstAvailableRoom(RoomType roomType, List<Appointment> appointments, TimeSlot duration)
        {
            List<Model.Room>? rooms = FindByType(roomType);
            if (rooms.Count != 0)
            {
                foreach (var room in rooms)
                {
                    if (IsAvailable(room.Id, appointments, duration))
                    {
                        return room;
                    }
                }
            }
            
            return null;
        }

        public List<Model.Room> GetAvailableRoomsForHospitalTreatment(DateTime start, List<HospitalTreatment> treatments)
        {
            var availableRooms = Rooms.Where(room => room.Type == RoomType.PatientRoom && treatments.Count(treatment => treatment.RoomId == room.Id) < 3).ToList();

            availableRooms = availableRooms.Where(room => !treatments.Any(treatment =>
                treatment.RoomId == room.Id && treatment.Status == HospitalTreatment.TreatmentStatus.FINISHED
            )).ToList();

            return availableRooms;
        }

        public void GetAllRoom()
        {
            Rooms = JsonConvert.DeserializeObject<List<Model.Room>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Rooms, Formatting.Indented));
        }

        public void CreateRoom(Model.Room newRoom)
        {
            Rooms.Add(newRoom);
            Save();
        }

        public void DeleteRoom(string id)
        {
            List<Model.Room> newRooms = new List<Model.Room>();
            foreach(Model.Room room in Rooms)
            {
                if(room.Id != id)
                {
                    newRooms.Add(room);
                }
            }
            Rooms = newRooms;
            Save();
        }

        public List<Model.Room> GetRooms()
        {
            return Rooms;
        }
    }
}
