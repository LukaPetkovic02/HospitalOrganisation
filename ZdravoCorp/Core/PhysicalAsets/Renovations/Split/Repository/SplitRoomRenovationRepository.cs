using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Repository
{
    public class SplitRoomRenovationRepository
    {
        private string FilePath = "../../../Data/splitRoomRenovation.json";
        private List<SplitRoomRenovation> splitRoomRenovations;
        public SplitRoomRenovationRepository()
        {
            GetAllItems();
        }

        public List<SplitRoomRenovation> GetRenovations()
        {
            return splitRoomRenovations;
        }

        public List<SplitRoomRenovation> GetRenovatedRooms()
        {
            return (from SplitRoomRenovation splitRoomRenovation in splitRoomRenovations where splitRoomRenovation.IsFinished() select splitRoomRenovation).ToList();
        }
        public void GetAllItems()
        {
            splitRoomRenovations = JsonConvert.DeserializeObject<List<SplitRoomRenovation>>(File.ReadAllText(FilePath));
        }
        public void CancelSameNameRenovations(string newId)
        {
            List<SplitRoomRenovation> newSplitRoomRenovations = new List<SplitRoomRenovation>();
            foreach (SplitRoomRenovation splitRoomRenovation in splitRoomRenovations)
            {
                if (splitRoomRenovation.NewIdFirstRoom != newId && splitRoomRenovation.NewIdSecondRoom != newId)
                {
                    newSplitRoomRenovations.Add(splitRoomRenovation);
                }
            }
            splitRoomRenovations = newSplitRoomRenovations;
            Save();
        }
        public void CancelRenovations(string id)
        {
            List<SplitRoomRenovation> newSplitRoomRenovations = new List<SplitRoomRenovation>();
            foreach (SplitRoomRenovation splitRoomRenovation in splitRoomRenovations)
            {
                if (splitRoomRenovation.Id!=id)
                {
                    newSplitRoomRenovations.Add(splitRoomRenovation);
                }
            }
            splitRoomRenovations = newSplitRoomRenovations;
            Save();
        }
        public void DeleteOutdated()
        {
            List<SplitRoomRenovation> newSplitRoomRenovations = new List<SplitRoomRenovation>();
            foreach (SplitRoomRenovation splitRoomRenovation in splitRoomRenovations)
            {
                if (!splitRoomRenovation.IsFinished())
                {
                    newSplitRoomRenovations.Add(splitRoomRenovation);
                }
            }
            splitRoomRenovations = newSplitRoomRenovations;
            Save();
        }

        public List<TimeSlot> GetAllRoomTimeslots(List<TimeSlot> roomTimeslots, string roomId)
        {
            roomTimeslots.AddRange(from SplitRoomRenovation splitRoomRenovation in splitRoomRenovations
                                   where splitRoomRenovation.Id == roomId
                                   select splitRoomRenovation.TimeSlot);
            return roomTimeslots;
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(splitRoomRenovations, Formatting.Indented));
        }
        
        public void AddSplitRoomRenovation(string id, DateTime startDate, DateTime endDate, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond)
        {
            splitRoomRenovations.Add(new SplitRoomRenovation(id, startDate, endDate, newTypeFirst, newTypeSecond, newIdFirst, newIdSecond));
            Save();
        }

        public void AddSplitRoomRenovation(string id, TimeSlot timeSlot, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond)
        {
            splitRoomRenovations.Add(new SplitRoomRenovation(id, timeSlot, newTypeFirst, newTypeSecond, newIdFirst, newIdSecond));
            Save();
        }

        
    }
}
