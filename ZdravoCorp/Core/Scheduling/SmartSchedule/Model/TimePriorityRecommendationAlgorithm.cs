using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Scheduling.SmartSchedule.m;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.Scheduling.SmartSchedule.Model
{
    public class TimePriorityRecommendationAlgorithm : AppointmentRecommendationAlgorithm
    {
        
        public TimePriorityRecommendationAlgorithm(string jmbgOfLogged) : base(jmbgOfLogged)
        {
            
        }
        public override List<Appointment> GetAvailableAppointments(Doctor doctor, DateTime endDate, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime today = DateTime.Now;
            today = RoundTime(today);
            if (ResolveTodayScheduleTimePriority(today, timeRange) != null)
                availableAppointments.AddRange(ResolveTodayScheduleTimePriority(today, timeRange));
            if (availableAppointments.Count() == 0)
            {
                foreach (Doctor doctor1 in DoctorService.Doctors())
                {
                    for (DateTime currDate = today.AddDays(1); currDate <= endDate; currDate = currDate.AddDays(1))
                    {
                        List<Appointment> appointmentsForDay = GetAvailableAppointmentsSingleDay(doctor1, currDate, timeRange);

                        availableAppointments.AddRange(appointmentsForDay);
                    }
                    if (availableAppointments.Count() > 0)
                        return availableAppointments;
                }
            }
            return availableAppointments;
        }
        
        
        public List<Appointment> ResolveTodayScheduleTimePriority(DateTime today, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime todayStart = new DateTime(today.Year, today.Month, today.Day, timeRange.Start.Hour, timeRange.Start.Minute, timeRange.Start.Second);
            DateTime todayEnd = new DateTime(today.Year, today.Month, today.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            TimeSpan differenceStart = today - todayStart;
            TimeSpan differenceEnd = today - todayEnd;

            if (differenceStart.TotalSeconds > 0 && differenceEnd.TotalSeconds < 0)
            {
                TimeSlot todayTimeSlot = new TimeSlot(today, todayEnd);
                foreach (Doctor doctor in DoctorService.Doctors())
                {
                    List<Appointment> appointmentsForDay = GetAvailableAppointmentsSingleDay(doctor, today, todayTimeSlot);
                    availableAppointments.AddRange(appointmentsForDay);
                }
            }
            else if (differenceStart.TotalSeconds < 0)
            {
                foreach (Doctor doctor in DoctorService.Doctors())
                {
                    List<Appointment> appointmentsForDay = GetAvailableAppointmentsSingleDay(doctor, today, timeRange);
                    availableAppointments.AddRange(appointmentsForDay);
                }
            }

            return availableAppointments;
        }
    }
}
