using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Scheduling.SmartSchedule.m;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;

namespace ZdravoCorp.Core.Scheduling.SmartSchedule.Model
{
    public class AppointmentSmartSchedule
    {
        public ScheduleService ScheduleService;
        public string JmbgOfLogged;
        public PatientService PatientService;
        public RoomService RoomService;
        public DoctorService DoctorService;
        public AppointmentRecommendationAlgorithm RecommendationAlgorithm;
        public AppointmentSmartSchedule(string jmbgOfLogged,AppointmentRecommendationAlgorithm appointmentRecommendationAlgorithm)
        {
            ScheduleService = new ScheduleService();
            JmbgOfLogged = jmbgOfLogged;
            PatientService = new PatientService();
            RoomService = new RoomService();
            DoctorService = new DoctorService();
            RecommendationAlgorithm = appointmentRecommendationAlgorithm;
        }
        public List<Appointment> GetAvailableAppointments(Doctor doctor, DateTime date, TimeSlot timeRange)
        {
            return RecommendationAlgorithm.GetAvailableAppointments(doctor, date, timeRange);
        }
        
    }
}
