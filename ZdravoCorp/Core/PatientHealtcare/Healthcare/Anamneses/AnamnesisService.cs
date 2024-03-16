using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses
{
    public class AnamnesisService
    {
        public IAnamnesisRepository AnamnesisRepository;

        public AnamnesisService(IAnamnesisRepository anamnesisRepository)
        {
            AnamnesisRepository = anamnesisRepository;
        }

        public void CreateAnamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description,
            string appointmentId)
        {
            AnamnesisRepository.CreateAnamnesis(doctorJmbg, patientJmbg, date, description, appointmentId);
        }

        public Anamnesis? FindAnamnesisByAppointmentId(string appointmentId)
        {
            return AnamnesisRepository.FindAnamnesisByAppointmentId(appointmentId);
        }

        public List<Anamnesis>? FindAnamnesesByPatientJmbg(string patientJmbg)
        {
            return AnamnesisRepository.FindAnamnesesByPatientJmbg(patientJmbg);
        }

        public void EditAnamnesis(string newDescription, string appointmentId)
        {
            AnamnesisRepository.EditAnamnesis(newDescription, appointmentId);
        }

        public Anamnesis? FindAnamnesisById(string id)
        {
            return AnamnesisRepository.FindAnamnesisById(id);

        }

        public List<Anamnesis> GetAllAnamneses()
        {
            return AnamnesisRepository.Anamneses;
        }
        public List<Anamnesis> CreateAmamnesisFromAppointments(List<Appointment> appointments)
        {
            return AnamnesisRepository.CreateAmamnesisFromAppointments(appointments);
        }
        public void Save()
        {
            AnamnesisRepository.Save();
        }
    }
}
