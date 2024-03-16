using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Model;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Repository;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Model;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Repository;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Model;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Repository;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations.Repository
{
    public class RenovationRepository
    {
        private JoinRoomRenovationRepository JoinRoomRenovationRepository { get; set; }
        private SplitRoomRenovationRepository SplitRoomRenovationRepository { get; set; }
        private SimpleRenovationRepository SimpleRenovationRepository { get; set; }

        public RenovationRepository()
        {
            JoinRoomRenovationRepository = new JoinRoomRenovationRepository();
            SimpleRenovationRepository = new SimpleRenovationRepository();
            SplitRoomRenovationRepository = new SplitRoomRenovationRepository();
        }
        public void GetAllItems()
        {
            JoinRoomRenovationRepository.GetAllItems();
            SplitRoomRenovationRepository.GetAllItems();
            SimpleRenovationRepository.GetAllItems();
        }

        public void DeleteOutdated()
        {
            JoinRoomRenovationRepository.DeleteOutdated();
            SplitRoomRenovationRepository.DeleteOutdated();
            SimpleRenovationRepository.DeleteOutdated();
        }

        public void Save()
        {
            JoinRoomRenovationRepository.Save();
            SplitRoomRenovationRepository.Save();
            SimpleRenovationRepository.Save();
        }

        public void AddReservation(string firstId, string secondId, DateTime startDate, DateTime endDate, RoomType newType, string newId)
        {
            JoinRoomRenovationRepository.AddJoinRoomRenovation(firstId, secondId, startDate, endDate, newType, newId);
        }

        public void AddReservation(string firstId, string secondId, TimeSlot timeSlot, RoomType newType, string newId)
        {
            JoinRoomRenovationRepository.AddJoinRoomRenovation(firstId, secondId, timeSlot, newType, newId);
        }

        public void AddReservation(string id, DateTime startDate, DateTime endDate, RoomType originalType, RoomType newType)
        {
            SimpleRenovationRepository.AddSimpleRenovation(id, startDate, endDate, originalType, newType);
        }

        public void AddReservation(string id, TimeSlot timeSlot, RoomType originalType, RoomType newType)
        {
            SimpleRenovationRepository.AddSimpleRenovation(id, timeSlot, originalType, newType);
        }

        public void AddReservation(string id, DateTime startDate, DateTime endDate, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond)
        {
            SplitRoomRenovationRepository.AddSplitRoomRenovation(id, startDate, endDate, newTypeFirst, newTypeSecond, newIdFirst, newIdSecond);
        }

        public void AddReservation(string id, TimeSlot timeSlot, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond)
        {
            SplitRoomRenovationRepository.AddSplitRoomRenovation(id, timeSlot, newTypeFirst, newTypeSecond, newIdFirst, newIdSecond);
        }
        
        public List<SimpleRenovation> GetSimpleRenovations()
        {
            return SimpleRenovationRepository.GetRenovations();
        }

        public List<SplitRoomRenovation> GetSplitRoomRenovations()
        {
            return SplitRoomRenovationRepository.GetRenovations();
        }

        public List<JoinRoomRenovation> GetJoinRoomRenovations()
        {
            return JoinRoomRenovationRepository.GetRenovations();
        }

        public List<TimeSlot> GetAllRoomTimeslots(List<TimeSlot> roomTimeslots, string roomId)
        {
            SimpleRenovationRepository.GetAllRoomTimeslots(roomTimeslots, roomId);
            JoinRoomRenovationRepository.GetAllRoomTimeslots(roomTimeslots, roomId);
            SplitRoomRenovationRepository.GetAllRoomTimeslots(roomTimeslots, roomId);
            return roomTimeslots;
        }

        public List<SimpleRenovation> GetSimpleRenovatedRooms()
        {
            return SimpleRenovationRepository.GetRenovatedRooms();
        }

        public List<SplitRoomRenovation> GetSplitRenovatedRooms()
        {
            return SplitRoomRenovationRepository.GetRenovatedRooms();
        }
        public List<JoinRoomRenovation> GetJoinRenovatedRooms()
        {
            return JoinRoomRenovationRepository.GetRenovatedRooms();
        }

        public void UpdateRenovationOriginalType(string id, RoomType newType)
        {
            SimpleRenovationRepository.UpdateRenovationOriginalType(id, newType);
        }

        public void CancelRenovations(string id)
        {
            SimpleRenovationRepository.CancelRenovations(id);
            SplitRoomRenovationRepository.CancelRenovations(id);
            JoinRoomRenovationRepository.CancelRenovations(id);
        }

        public void CancelSameNameRenovations(string newId)
        {
            SplitRoomRenovationRepository.CancelSameNameRenovations(newId);
            JoinRoomRenovationRepository.CancelSameNameRenovations(newId);
        }
    }
}
