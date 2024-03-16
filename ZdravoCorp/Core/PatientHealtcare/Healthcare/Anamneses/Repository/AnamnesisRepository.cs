using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository
{
    public class AnamnesisRepository : IAnamnesisRepository
    {
        public List<Anamnesis> Anamneses { get; set; }

        public AnamnesisRepository()
        {
            GetAllAnamneses();
        }

        public void CreateAnamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description,
            string appointmentId)
        {
            Anamneses.Add(new Anamnesis(doctorJmbg, patientJmbg, date, description, appointmentId));
            Save();
        }

        public void EditAnamnesis(string newDescription, string appointmentId)
        {
            Anamnesis anamnesis = FindAnamnesisByAppointmentId(appointmentId);
            anamnesis.Description += newDescription;
            Save();
        }

        public Anamnesis? FindAnamnesisByAppointmentId(string appointmentId)
        {
            return Anamneses.FirstOrDefault(anamnesis => anamnesis.AppointmentId == appointmentId);
        }

        public List<Anamnesis>? FindAnamnesesByPatientJmbg(string patientJmbg)
        {
            return Anamneses.Where(anamnesis => anamnesis.PatientJmbg == patientJmbg).ToList();
        }
        public Anamnesis? FindAnamnesisById(string id)
        {
            return Anamneses.FirstOrDefault(anamnesis => anamnesis.AppointmentId == id);
        }
        public void GetAllAnamneses()
        {
            Anamneses = JsonConvert.DeserializeObject<List<Anamnesis>>(File.ReadAllText("../../../Data/anamneses.json"));
        }
        public List<Anamnesis> CreateAmamnesisFromAppointments(List<Appointment> appointments)
        {
            List<Anamnesis> retList = new List<Anamnesis>();
            foreach(Appointment appointment in appointments)
            {
                retList.Add(FindAnamnesisById(appointment.Id));
            }
            return retList;
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/anamneses.json", JsonConvert.SerializeObject(Anamneses, Formatting.Indented));
        }
    }
}
