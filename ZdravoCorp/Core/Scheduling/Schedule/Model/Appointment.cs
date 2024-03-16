using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.Scheduling.Schedule.Model
{
    public enum AppointmentType  {
        OPERATION,
        EXAMINATION
    }

    public enum AppointmentStatus
    {
        SCHEDULED,
        FINISHED,
        CANCELED,
        READY
    }

    public class Appointment
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("JmbgDoctor")]
        public string JmbgDoctor { get; set; }

        [JsonProperty("JmbgPatient")]
        public string JmbgPatient { get; set; }

        [JsonProperty("TimeSlot")]
        public TimeSlot TimeSlot;

        [JsonProperty("Type")]
        public AppointmentType Type;

        [JsonProperty("Status")]
        public AppointmentStatus Status;

        [JsonProperty("Room")]
        public string RoomId;
        //public void Cancel()
        //public void Confirm()

        [JsonConstructor]
        public Appointment(TimeSlot timeSlot, AppointmentType type, string jmbgDoctor, string jmbgPatient, AppointmentStatus status, string id, string roomId)
        {
            TimeSlot = timeSlot;
            Type = type;
            JmbgDoctor = jmbgDoctor;
            JmbgPatient = jmbgPatient;
            Status = status;
            Id = id;
            RoomId = roomId;
        }

        public Appointment(string jmbgdoctor, string jmbgpatient, TimeSlot timeSlot, string id,string roomId)
        {
            JmbgDoctor = jmbgdoctor;
            JmbgPatient = jmbgpatient;
            TimeSlot = timeSlot;
            Type = AppointmentType.EXAMINATION;
            Status = AppointmentStatus.SCHEDULED;
            Id = id;
            RoomId = roomId;
        }

        public void Cancel()
        {
            Status = AppointmentStatus.CANCELED;
        }

        public bool IsReady()
        {
            TimeSpan timeUntilAppointment = TimeSlot.Start - DateTime.Now;
            return timeUntilAppointment.TotalMinutes <= 15 && timeUntilAppointment.TotalMinutes >= 0;
        }

        public bool IsScheduled()
        {
            return Status == AppointmentStatus.READY;
        }

    }
}
