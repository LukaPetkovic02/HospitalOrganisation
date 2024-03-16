using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions
{
    public class PrescriptionService
    {
        public PrescriptionRepository PrescriptionRepository;

        public PrescriptionService()
        {
            PrescriptionRepository = new PrescriptionRepository();
        }

        public void CreatePrescription(string drug, DateTime start, DateTime end, int dailyDose, Prescription.Consumption consumptionTime,string patientJmbg, string doctorJmbg)
        {
            PrescriptionRepository.CreatePrescription(drug,start,end,dailyDose,consumptionTime,patientJmbg, doctorJmbg);
        }
        public List<Prescription> FindPrescriptionByJmbg(string jmbg)
        {
            return PrescriptionRepository.FindPrescriptionByJmbg(jmbg);
        }
        public Prescription? FindPrescriptionById(string id)
        {
            return PrescriptionRepository.FindPrescriptionById(id);
        }
        public void GetAllPrescriptions()
        {
            PrescriptionRepository.GetAllPrescriptions();
        }

        public bool IsPatientDoneWithLastDose(string patientJmbg, string Drug)
        {
            return PrescriptionRepository.IsPatientDoneWithLastDose(patientJmbg, Drug);
        }
        public void OncePerDay(Prescription prescription,string JmbgOfLogged)
        {
            PrescriptionRepository.OncePerDay(prescription, JmbgOfLogged);
        }
        public void TwicePerDay(Prescription prescription,string JmbgOfLogged)
        {
            PrescriptionRepository.TwicePerDay(prescription, JmbgOfLogged);
        }
        public void ThricePerDay(Prescription prescription,string JmbgOfLogged)
        {
            PrescriptionRepository.ThricePerDay(prescription, JmbgOfLogged);
        }
        public void DrugNotification(Prescription prescription,string JmbgOfLogged)
        {
            PrescriptionRepository.DrugNotification(prescription, JmbgOfLogged);
        }
        public void Save()
        {
            PrescriptionRepository.Save();
        }
    }
}
