using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository
{
    public interface IAnamnesisRepository
    {
        public List<Anamnesis> Anamneses { get; set; }

        public void CreateAnamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description,
            string appointmentId);

        public void EditAnamnesis(string newDescription, string appointmentId);

        public Anamnesis? FindAnamnesisByAppointmentId(string appointmentId);

        public List<Anamnesis>? FindAnamnesesByPatientJmbg(string patientJmbg);

        public Anamnesis? FindAnamnesisById(string id);

        public void GetAllAnamneses();

        public List<Anamnesis> CreateAmamnesisFromAppointments(List<Appointment> appointments);

        public void Save();
    }
}
