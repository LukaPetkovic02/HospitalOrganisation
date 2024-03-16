using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Inventory;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Join;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Join.Model;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Repository;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Simple;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Simple.Model;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Split;
using ZdravoCorp.Core.PhysicalAsets.Renovations.Split.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.PhysicalAsets.Renovations
{
    public class RenovationSchedule
    {
        private RenovationRepository renovationRepository;
        private InventoryService inventoryService;
        private RoomService roomService;
        private ScheduleService scheduleService;

        public RenovationSchedule(InventoryService inventoryService, ScheduleService scheduleService)
        {
            renovationRepository = new RenovationRepository();
            this.inventoryService = inventoryService;
            this.scheduleService = scheduleService;
            roomService = inventoryService.GetRoomService();
        }


        public bool AddRenovation(string firstId, string secondId, TimeSlot timeSlot, RoomType newType, string newId)
        {
            if (roomService.FindById(newId) != null) return false;

            if (!IsAvailable(firstId, timeSlot) || !IsAvailable(secondId, timeSlot)) return false;
            renovationRepository.AddReservation(firstId, secondId, timeSlot, newType, newId);

            return true;
        }
        
        public bool AddRenovation(string id, TimeSlot timeSlot, RoomType newType)
        {
            if (!IsAvailable(id, timeSlot)) return false;

            Room.Model.Room room = roomService.FindById(id);
            renovationRepository.AddReservation(id, timeSlot, room.Type, newType);
            return true;
        }

        public bool AddRenovation(string id, TimeSlot timeSlot, RoomType newTypeFirst, RoomType newTypeSecond, string newIdFirst, string newIdSecond)
        {
            if (roomService.FindById(newIdFirst) != null || roomService.FindById(newIdSecond) != null) return false;

            if (!IsAvailable(id, timeSlot)) return false;
            renovationRepository.AddReservation(id, timeSlot, newTypeFirst, newTypeSecond, newIdFirst, newIdSecond);

            return true;
        }

        public bool IsAvailable(string roomId, TimeSlot timeSlot)
        {
            foreach(TimeSlot time in GetAllRoomTimeslots(roomId))
            {
                if (time.OverlapsWith(timeSlot)) return false;
            }
            return true;
        }

        public List<TimeSlot> GetAllRoomTimeslots(string roomId)
        {
            List<TimeSlot> timeSlots = scheduleService.GetAppointmentTimeSlotsByRoomId(roomId); 
            return renovationRepository.GetAllRoomTimeslots(timeSlots, roomId);
        }

        public void UpdateRenovations()
        {
            UpdateSimpleRenovations();
            UpdateJoinRoomRenovations();
            UpdateSplitRoomRenovations();

            renovationRepository.DeleteOutdated();

            renovationRepository.Save();
        }

        public void UpdateSimpleRenovations()
        {
            List<SimpleRenovation> simpleRenovations = renovationRepository.GetSimpleRenovatedRooms();

            foreach (SimpleRenovation simpleRenovation in simpleRenovations)
            {
                UpdateSimpleRenovation(simpleRenovation);
            }
        }

        private void UpdateSimpleRenovation(SimpleRenovation simpleRenovation)
        {
            if (simpleRenovation.NewType != simpleRenovation.OriginalType)
            {
                scheduleService.CancelAppointmentsByRoomId(simpleRenovation.Id);
                roomService.UpdateRoom(simpleRenovation.Id, simpleRenovation.NewType);
                renovationRepository.UpdateRenovationOriginalType(simpleRenovation.Id, simpleRenovation.NewType);
            }
            
        }

        public void UpdateSplitRoomRenovations()
        {
            List<SplitRoomRenovation> splitRoomRenovations = renovationRepository.GetSplitRenovatedRooms();

            foreach (SplitRoomRenovation splitRoomRenovation in splitRoomRenovations)
            {
                UpdateSplitRoomRenovation(splitRoomRenovation);
            }
        }

        private void UpdateSplitRoomRenovation(SplitRoomRenovation splitRoomRenovation)
        {
            scheduleService.CancelAppointmentsByRoomId(splitRoomRenovation.Id);
            inventoryService.AllocateItemsToStorage(splitRoomRenovation.Id);
            if (inventoryService.CreateRoom(splitRoomRenovation.NewIdFirstRoom, splitRoomRenovation.NewTypeFirstRoom)) renovationRepository.CancelSameNameRenovations(splitRoomRenovation.NewIdFirstRoom);

            if (inventoryService.CreateRoom(splitRoomRenovation.NewIdSecondRoom, splitRoomRenovation.NewTypeSecondRoom)) renovationRepository.CancelSameNameRenovations(splitRoomRenovation.NewIdSecondRoom);

            renovationRepository.CancelRenovations(splitRoomRenovation.Id);
            inventoryService.DeleteRoom(splitRoomRenovation.Id);
        }

        public void UpdateJoinRoomRenovations()
        {
            List<JoinRoomRenovation> joinRoomRenovations = renovationRepository.GetJoinRenovatedRooms();

            foreach (JoinRoomRenovation joinRoomRenovation in joinRoomRenovations)
            {
                UpdateJoinRoomRenovation(joinRoomRenovation);
            }
        }

        private void UpdateJoinRoomRenovation(JoinRoomRenovation joinRoomRenovation)
        {
            scheduleService.CancelAppointmentsByRoomId(joinRoomRenovation.FirstId);
            scheduleService.CancelAppointmentsByRoomId(joinRoomRenovation.SecondId);
            inventoryService.AllocateItemsToStorage(joinRoomRenovation.FirstId);
            inventoryService.AllocateItemsToStorage(joinRoomRenovation.SecondId);

            if (inventoryService.CreateRoom(joinRoomRenovation.NewId, joinRoomRenovation.NewType)) renovationRepository.CancelSameNameRenovations(joinRoomRenovation.NewId);

            renovationRepository.CancelRenovations(joinRoomRenovation.FirstId);
            renovationRepository.CancelRenovations(joinRoomRenovation.SecondId);

            inventoryService.DeleteRoom(joinRoomRenovation.FirstId);
            inventoryService.DeleteRoom(joinRoomRenovation.SecondId);
        }
    }
}
