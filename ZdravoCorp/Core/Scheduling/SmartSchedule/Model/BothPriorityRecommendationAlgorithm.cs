using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Scheduling.SmartSchedule.m;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Core.Scheduling.SmartSchedule.Model
{
    public class BothPriorityRecommendationAlgorithm : AppointmentRecommendationAlgorithm
    {
        
        public BothPriorityRecommendationAlgorithm(string jmbgOfLogged) : base(jmbgOfLogged)
        {
            
        }
        public override List<Appointment> GetAvailableAppointments(Doctor doctor, DateTime endDate, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime today = DateTime.Now;
            today = RoundTime(today);
            if (ResolveTodaySchedule(doctor, today, timeRange) != null)
                availableAppointments.AddRange(ResolveTodaySchedule(doctor, today, timeRange));
            for (DateTime currDate = today.AddDays(1); currDate <= endDate; currDate = currDate.AddDays(1))
            {
                List<Appointment> appointmentsForDay = GetAvailableAppointmentsSingleDay(doctor, currDate, timeRange);

                availableAppointments.AddRange(appointmentsForDay);
            }

            return availableAppointments;
        }
        public List<Appointment> ResolveTodaySchedule(Doctor doctor, DateTime today, TimeSlot timeRange)
        {
            DateTime todayStart = new DateTime(today.Year, today.Month, today.Day, timeRange.Start.Hour, timeRange.Start.Minute, timeRange.Start.Second);
            DateTime todayEnd = new DateTime(today.Year, today.Month, today.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            TimeSpan differenceStart = today - todayStart;
            TimeSpan differenceEnd = today - todayEnd;

            if (differenceStart.TotalSeconds > 0 && differenceEnd.TotalSeconds < 0)
            {
                TimeSlot todayTimeSlot = new TimeSlot(today, todayEnd);
                return GetAvailableAppointmentsSingleDay(doctor, today, todayTimeSlot);
            }
            else if (differenceStart.TotalSeconds < 0)
                return GetAvailableAppointmentsSingleDay(doctor, today, timeRange);
            else
                return null;
        }
    }
}
