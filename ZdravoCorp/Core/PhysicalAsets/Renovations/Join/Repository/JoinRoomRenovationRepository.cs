using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Repository
{
    public class JoinRoomRenovationRepository
    {
        private string FilePath = "../../../Data/joinRoomRenovation.json";
        private List<JoinRoomRenovation> joinRoomRenovations;
        public JoinRoomRenovationRepository()
        {
            GetAllItems();
        }

        public List<JoinRoomRenovation> GetRenovations()
        {
            return joinRoomRenovations;
        }

        public List<JoinRoomRenovation> GetRenovatedRooms()
        {
            return (from JoinRoomRenovation joinRoomRenovation in joinRoomRenovations where joinRoomRenovation.IsFinished() select joinRoomRenovation).ToList();
        }
        public void GetAllItems()
        {
            joinRoomRenovations = JsonConvert.DeserializeObject<List<JoinRoomRenovation>>(File.ReadAllText(FilePath));
        }
        internal void CancelSameNameRenovations(string newId)
        {
            List<JoinRoomRenovation> newJoinRoomRenovations = new List<JoinRoomRenovation>();
            foreach (JoinRoomRenovation joinRoomRenovation in joinRoomRenovations)
            {
                if (joinRoomRenovation.NewId != newId)
                {
                    newJoinRoomRenovations.Add(joinRoomRenovation);
                }
            }
            joinRoomRenovations = newJoinRoomRenovations;
            Save();
        }
        public void CancelRenovations(string id)
        {
            List<JoinRoomRenovation> newJoinRoomRenovations = new List<JoinRoomRenovation>();
            foreach (JoinRoomRenovation joinRoomRenovation in joinRoomRenovations)
            {
                if (joinRoomRenovation.FirstId!=id && joinRoomRenovation.SecondId!= id)
                {
                    newJoinRoomRenovations.Add(joinRoomRenovation);
                }
            }
            joinRoomRenovations = newJoinRoomRenovations;
            Save();
        }
        public void DeleteOutdated()
        {
            List<JoinRoomRenovation> newJoinRoomRenovations = new List<JoinRoomRenovation>();
            foreach (JoinRoomRenovation joinRoomRenovation in joinRoomRenovations)
            {
                if (!joinRoomRenovation.IsFinished())
                {
                    newJoinRoomRenovations.Add(joinRoomRenovation);
                }
            }
            joinRoomRenovations = newJoinRoomRenovations;
            Save();
        }

        public List<TimeSlot> GetAllRoomTimeslots(List<TimeSlot> roomTimeslots, string roomId)
        {
            roomTimeslots.AddRange(from JoinRoomRenovation joinRoomRenovation in joinRoomRenovations
                                   where joinRoomRenovation.FirstId == roomId || joinRoomRenovation.SecondId == roomId
                                   select joinRoomRenovation.TimeSlot);
            return roomTimeslots;
        }
        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(joinRoomRenovations, Formatting.Indented));
        }
        
        public void AddJoinRoomRenovation(string firstId, string secondId, DateTime startDate, DateTime endDate, RoomType newType, string newId)
        {
            joinRoomRenovations.Add(new JoinRoomRenovation(firstId, secondId, startDate, endDate, newType, newId));
            Save();
        }
        public void AddJoinRoomRenovation(string firstId, string secondId, TimeSlot timeSlot, RoomType newType, string newId)
        {
            joinRoomRenovations.Add(new JoinRoomRenovation(firstId, secondId, timeSlot, newType, newId));
            Save();
        }

        
    }
}
