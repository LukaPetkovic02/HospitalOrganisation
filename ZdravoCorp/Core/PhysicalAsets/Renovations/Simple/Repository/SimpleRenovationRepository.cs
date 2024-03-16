using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Repository
{
    public class SimpleRenovationRepository
    {
        private string FilePath = "../../../Data/simpleRenovation.json";
        private List<SimpleRenovation> simpleRenovations;
        public SimpleRenovationRepository()
        {
            GetAllItems();
        }

        public List<SimpleRenovation> GetRenovations()
        {
            return simpleRenovations;
        }

        public List<SimpleRenovation> GetRenovatedRooms()
        {
            return (from SimpleRenovation simpleRenovation in simpleRenovations where simpleRenovation.IsFinished() select simpleRenovation).ToList();
        }
        public void GetAllItems()
        {
            simpleRenovations = JsonConvert.DeserializeObject<List<SimpleRenovation>>(File.ReadAllText(FilePath));
        }
        public void CancelRenovations(string id)
        {
            foreach (SimpleRenovation simpleRenovation in simpleRenovations)
            {
                List<SimpleRenovation> newSimpleRenovations = new List<SimpleRenovation>();
                if (simpleRenovation.Id!=id)
                {
                    newSimpleRenovations.Add(simpleRenovation);
                }
                simpleRenovations = newSimpleRenovations;
                Save();
            }
            Save();
        }
        public void UpdateRenovationOriginalType(string id, RoomType newType)
        {
            foreach (SimpleRenovation simpleRenovation in simpleRenovations)
            {
                if(simpleRenovation.Id == id)
                {
                    simpleRenovation.OriginalType = newType;
                }
            }
            Save();
        }
        public void DeleteOutdated()
        {
            List<SimpleRenovation> newSimpleRenovations = new List<SimpleRenovation>();
            foreach (SimpleRenovation simpleRenovation in simpleRenovations)
            {
                if (!simpleRenovation.IsFinished())
                {
                    newSimpleRenovations.Add(simpleRenovation);
                }
            }
            simpleRenovations = newSimpleRenovations;
            Save();
        }

        public List<TimeSlot> GetAllRoomTimeslots(List<TimeSlot> roomTimeslots,string roomId)
        {
            roomTimeslots.AddRange(from SimpleRenovation simpleRenovation in simpleRenovations
                                   where simpleRenovation.Id == roomId
                                   select simpleRenovation.TimeSlot);
            return roomTimeslots;
        }


        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(simpleRenovations, Formatting.Indented));
        }


        public void AddSimpleRenovation(string id, DateTime startDate, DateTime endDate, RoomType originalType, RoomType newType)
        {
            simpleRenovations.Add(new SimpleRenovation(id, startDate, endDate, originalType, newType));
            Save();
        }

        public void AddSimpleRenovation(string id, TimeSlot timeSlot, RoomType originalType, RoomType newType)
        {
            simpleRenovations.Add(new SimpleRenovation(id, timeSlot, originalType, newType));
            Save();
        }
    }
}
