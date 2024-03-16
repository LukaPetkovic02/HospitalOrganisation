using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Scheduling.SmartSchedule.m
{
    public abstract class AppointmentRecommendationAlgorithm
    {
        public ScheduleService ScheduleService;
        public string JmbgOfLogged;
        public PatientService PatientService;
        public RoomService RoomService;
        public DoctorService DoctorService;
        public AppointmentRecommendationAlgorithm(string jmbgOfLogged)
        {
            ScheduleService = new ScheduleService();
            JmbgOfLogged = jmbgOfLogged;
            PatientService = new PatientService();
            RoomService = new RoomService();
            DoctorService = new DoctorService();
        }
        public abstract List<Appointment> GetAvailableAppointments(Doctor doctor, DateTime date, TimeSlot timeRange);
        public DateTime RoundTime(DateTime day)
        {
            int minutes = DateTime.Now.Minute;
            int roundedHours = DateTime.Now.Hour;
            int roundedMinutes = ((int)Math.Ceiling(minutes / 15.0)) * 15;
            if (roundedMinutes == 60)
            {
                roundedMinutes = 0;
                if (roundedHours == 23)
                    roundedHours = 0;
                else
                    roundedHours++;
            }
            DateTime ret = new DateTime(day.Year, day.Month, day.Day, roundedHours, roundedMinutes, 0);
            return ret;
        }
        public List<Appointment> GetAvailableAppointmentsSingleDay(Doctor doctor, DateTime date, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime startTime = new DateTime(date.Year, date.Month, date.Day, timeRange.Start.Hour, timeRange.Start.Minute, timeRange.Start.Second);
            DateTime endTime = new DateTime(date.Year, date.Month, date.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            Patient patient = PatientService.FindByJmbg(JmbgOfLogged);

            for (DateTime currTime = startTime; currTime < endTime; currTime = currTime.AddMinutes(15))
            {
                TimeSlot timeSlot = new TimeSlot(currTime, currTime.AddMinutes(15));
                Room room = RoomService.FindFirstAvailableRoom(RoomType.ExaminationRoom, ScheduleService.Appointments(), timeSlot);
                if (ScheduleService.IsAvailable(doctor, timeSlot) && ScheduleService.IsAvailable(patient, timeSlot)
                    && room != null)
                {
                    Appointment appointment = new Appointment(timeSlot, AppointmentType.EXAMINATION, doctor.Jmbg, patient.Jmbg, AppointmentStatus.SCHEDULED, ScheduleService.ScheduleRepository.GenerateAppointmentId(), room.Id);
                    availableAppointments.Add(appointment);
                }
            }
            return availableAppointments;
        }
    }
}
