using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Utilities.Patient.Model;
using ZdravoCorp.Core.Utilities.Patient.Repository;

namespace ZdravoCorp.Core.Utilities.Patient
{
    public class PatientService
    {
        public PatientRepository PatientRepository { get; set; }
        public PatientService()
        {
            PatientRepository = new PatientRepository();
        }
        public PatientService(PatientRepository patientRepository)
        {
            PatientRepository = patientRepository;
        }
        public List<Model.Patient> AllPatients()
        {
            return PatientRepository.AllPatients();
        }
        public Model.Patient? FindByJmbg(string jmbg)
        {
            return PatientRepository.FindByJmbg(jmbg);
        }

        public void EditMedicalCard(double weight, double height, Model.Patient patient)
        {
            PatientRepository.EditMedicalCard(weight,height,patient);
        }

        public void DeleteMedicalCondition(string medicalCondition, Model.Patient patient)
        {
            PatientRepository.DeleteMedicalCondition(medicalCondition,patient);
        }

        public void AddMedicalCondition(MedicalCondition medicalCondition, Model.Patient patient)
        {
            PatientRepository.AddMedicalCondition(medicalCondition,patient);
        }

        public MedicalCondition? FindByName(string medicalCondition, Model.Patient patient)
        {
            return PatientRepository.FindByName(medicalCondition, patient);
        }

        public Dictionary<string, List<DateTime>> AllPatientAppointmentCreations()
        {
            return PatientRepository.AllPatientAppointmentCreations();
        }
        public Dictionary<string, List<DateTime>> AllPatientAppointmentEdits()
        {
            return PatientRepository.AllPatientAppointmentEdits();
        }
        public bool PatientCreatedTooManyAppointments(string jmbg)
        {
            return PatientRepository.PatientCreatedTooManyAppointments(jmbg);
        }
        public bool PatientEditedTooManyAppointments(string jmbg)
        {
            return PatientRepository.PatientEditedTooManyAppointments(jmbg);
        }
        public bool IsPatientBlocked(string jmbg)
        {
            return PatientRepository.IsPatientBlocked(jmbg);
        }
        public void Save()
        {
            PatientRepository.Save();
        }
    }
}
