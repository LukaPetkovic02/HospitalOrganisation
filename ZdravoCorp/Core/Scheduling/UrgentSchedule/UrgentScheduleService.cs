using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Scheduling.UrgentSchedule
{
    public class UrgentScheduleService
    {
        public Schedule.Repository.Schedule ScheduleRepository { get; set; }

        public UrgentScheduleService()
        {
            ScheduleRepository = new Schedule.Repository.Schedule();
        }
        public TimeSlot FindSlotForUrgentAppointmentWithin2Hours(Doctor.Specialty specialty, string patientJmbg,
           AppointmentType appointmentType, DateTime start, int duration)
        {
            return ScheduleRepository.FindSlotForUrgentAppointmentWithin2Hours(specialty, patientJmbg,
                appointmentType, start, duration);
        }

        public List<KeyValuePair<Appointment, int>> FindAppointmentsToPostpone(Doctor.Specialty specialty, int duration, Patient patient)
        {
            return ScheduleRepository.FindAppointmentsToPostpone(specialty, duration, patient);
        }
        public void PostponeAppointment(Appointment app)
        {
            ScheduleRepository.PostponeAppointment(app);
        }
    }
}
