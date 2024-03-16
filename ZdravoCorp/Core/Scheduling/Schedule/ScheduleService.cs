using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Scheduling.Schedule
{
    public class ScheduleService
    {
        public Repository.Schedule ScheduleRepository { get; set; }

        public ScheduleService()
        {
            ScheduleRepository = new Repository.Schedule();
        }

        public List<Appointment> Appointments()
        {
            return ScheduleRepository.Appointments;
        }

        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration, AppointmentType type, string roomId)
        {
            ScheduleRepository.CreateAppointment(doctorJmbg, patientJmbg, appointmentDuration, type, roomId);
        }

        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration, string roomId)
        {
            ScheduleRepository.CreateAppointment(doctorJmbg, patientJmbg, appointmentDuration, roomId);
        }

        public void EditAppointment(string id, string newId, string patientJmbg, TimeSlot appointmentDuration, AppointmentType type, AppointmentStatus status,string roomId)
        {
           ScheduleRepository.EditAppointment(id,newId,patientJmbg,appointmentDuration,type,status,roomId);
        }

        public void CancelAppointmentsByRoomId(string roomId)
        {
            ScheduleRepository.CancelAppointmentsByRoomId(roomId);
        }

        public void CancelAppointmensByApprovedVacation(VacationRequest.Model.VacationRequest vacationRequest)
        {
            TimeSlot vacaTimeSlot = new TimeSlot(vacationRequest.Start, vacationRequest.End);
            foreach (Appointment appointment in GetAppointmentsByDoctorJmbg(vacationRequest.DoctorJmbg))
            {
                if (appointment.Status != AppointmentStatus.CANCELED && appointment.Status!=AppointmentStatus.FINISHED && appointment.TimeSlot.OverlapsWith(vacaTimeSlot))
                {
                    CancelAppointment(appointment);
                }
            }
            
        }

        public List<TimeSlot> GetAppointmentTimeSlotsByRoomId(string roomId)
        {
            return ScheduleRepository.GetAppointmentTimeSlotsByRoomId(roomId);
        }

        public void CancelAppointment(Appointment appointmentForCancel)
        {
            ScheduleRepository.CancelAppointment(appointmentForCancel);
        }

        public void FinishAppointment(Appointment appointmentForFinish)
        {
            ScheduleRepository.FinishAppointment(appointmentForFinish);
        }

        public void ReadyForAppointment(Appointment readyAppointment)
        {
            ScheduleRepository.ReadyForAppointment(readyAppointment);
        }

        public bool IsAppointmentInNext15Minutes(Appointment appointment)
        {
            return ScheduleRepository.IsAppointmentInNext15Minutes(appointment);
        }

        public bool HasPatientBeenExaminatedByDoctor(Doctor doctor, Patient patient)
        {
            return ScheduleRepository.HasPatientBeenExaminatedByDoctor(doctor, patient);
        }

        public bool IsAvailable(Doctor doctor, TimeSlot duration)
        {
            return ScheduleRepository.IsAvailable(doctor, duration);
        }

        public bool IsAvailable(Patient patient, TimeSlot duration)
        {
            return ScheduleRepository.IsAvailable(patient, duration);
        }

        public bool AreDoctorAndPatientAvailable(Doctor doctor, TimeSlot timeSlot, string patientJmbg)
        {
            return ScheduleRepository.AreDoctorAndPatientAvailable(doctor,timeSlot,patientJmbg);
        }

        public bool IsAppointmentInRange(string doctorJmbg, DateTime start, DateTime end)
        {
            return ScheduleRepository.IsAppointmentInRange(doctorJmbg, start, end);
        }

        public bool CreateAppointmentIn10Days(Doctor doctor, DateTime start, string patientJmbg)
        {
            return ScheduleRepository.CreateAppointmentIn10Days(doctor, start, patientJmbg);
        }

        public List<Appointment>? FindAllApointmetsWithDifferentId(string id)
        {
            return ScheduleRepository.FindAllApointmetsWithDifferentId(id);
        }

        public Appointment? FindAppointmentById(string id)
        {
            return ScheduleRepository.FindAppointmentById(id);
        }

        public List<Appointment>? GetAppointmentsByDate(Doctor doctor, DateTime startDate)
        {
            return ScheduleRepository.GetAppointmentsByDate(doctor,startDate);
        }

        public List<Appointment>? GetAppointmentsByDoctorJmbg(string doctorJmbg,bool isScheduled = true)
        {
            return ScheduleRepository.GetAppointmentsByDoctorJmbg(doctorJmbg);
        }
        public List<Appointment> SortAppointmentsByStartTime(List<Appointment> appointments)
        {
            return ScheduleRepository.SortAppointmentsByStartTime(appointments);
        }
        public List<Appointment> SortAppointmentsByDoctorJmbg(List<Appointment> appointments)
        {
            return ScheduleRepository.SortAppointmentsByDoctorJmbg(appointments);
        }
        public List<Appointment> SortAppointmentsByDoctorSpecialty(List<Appointment> appointments, Dictionary<string, Doctor.Specialty> doctorSpecialties)
        {
            return ScheduleRepository.SortAppointmentsByDoctorSpecialty(appointments,doctorSpecialties);
        }
        public List<Appointment> FindAppointmentsByAnamnesisDescriptionSubstring(List<Appointment> appointments, string searchString)
        {
            AnamnesisService anamnesisService=new AnamnesisService(new AnamnesisRepository());
            List<Appointment> matchingAppointments = new List<Appointment>();
            foreach (Appointment appointment in appointments)
            {
                Anamnesis anamnesis = anamnesisService.FindAnamnesisById(appointment.Id);
                
                if (anamnesis.Description.ToUpper().Contains(searchString.ToUpper()))
                {
                    matchingAppointments.Add(appointment);
                }
            }
            return matchingAppointments;
        }
        public List<Appointment>? GetAppointmentsByPatientJmbg(string patientJmbg)
        {
            return ScheduleRepository.GetAppointmentsByPatientJmbg(patientJmbg);
        }
        public List<Appointment>? GetFinishedAppointmentsByPatientJmbg(string patientJmbg)
        {
            return ScheduleRepository.GetFinishedAppointmentsByPatientJmbg(patientJmbg);
        }

        public void Save()
        {
            ScheduleRepository.Save();
        }

    }
}
