using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Repository
{
    public class PrescriptionRepository
    {
        public List<Prescription> Prescriptions;

        public PrescriptionRepository()
        {
            GetAllPrescriptions();
        }

        public string GeneratePrescriptionId()
        {
            while (true)
            {
                string prescriptionId = Generator.GenerateRandomId();

                if (!(FindPrescriptionById(prescriptionId) == null))
                {
                    continue;
                }
                return prescriptionId;
            }
        }

        public bool IsNewDoseAvailable(Prescription prescription)
        {
            return ((DateTime.Today - prescription.End).TotalDays - 1) >= 0;
        }

        public bool IsPatientDoneWithLastDose(string patientJmbg, string Drug)
        {
            List<Prescription> prescriptions = FindPrescriptionByJmbg(patientJmbg);
            foreach (var _ in from Prescription prescription in prescriptions
                              where prescription.Drug == Drug && prescription.IsUsed
                              where !IsNewDoseAvailable(prescription)
                              select new { })
            {
                return false;
            }

            return true;
        }
        public Prescription? FindPrescriptionById(string id)
        {
            return Prescriptions.FirstOrDefault(prescription => prescription.Id == id);
        }
        public void CreatePrescription(string drug, DateTime start, DateTime end, int dailyDose, Prescription.Consumption consumptionTime, string patientJmbg, string doctorJmbg)
        {
            Prescriptions.Add(new Prescription(GeneratePrescriptionId(), drug,start,end,dailyDose,consumptionTime,patientJmbg, doctorJmbg));
            Save();
        }
        public List<Prescription> FindPrescriptionByJmbg(string jmbg)
        {
            return Prescriptions.Where(p => p.PatientJmbg == jmbg).ToList();
        }
        public void GetAllPrescriptions()
        {
            Prescriptions = JsonConvert.DeserializeObject<List<Prescription>>(File.ReadAllText("../../../Data/prescriptions.json"));
        }
        public void OncePerDay(Prescription prescription, string JmbgOfLogged)
        {
            PatientService patientService = new PatientService();
            Patient patient = patientService.FindByJmbg(JmbgOfLogged);
            DateTime drink1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
            DateTime notifyDrink1 = drink1.AddMinutes(-patient.NotificationTime.TotalMinutes);
            if (DateTime.Now >= notifyDrink1 && DateTime.Now <= drink1)
                MessageBox.Show("You should drink " + prescription.Drug + " at " + drink1.Hour + "h", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void TwicePerDay(Prescription prescription, string JmbgOfLogged)
        {
            PatientService patientService = new PatientService();
            Patient patient = patientService.FindByJmbg(JmbgOfLogged);
            DateTime drink1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            DateTime notifyDrink1 = drink1.AddMinutes(-patient.NotificationTime.TotalMinutes);
            DateTime drink2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
            DateTime notifyDrink2 = drink2.AddMinutes(-patient.NotificationTime.TotalMinutes);
            if (DateTime.Now >= notifyDrink1 && DateTime.Now <= drink1)
                MessageBox.Show("You should drink " + prescription.Drug + " at " + drink1.Hour + "h", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (DateTime.Now >= notifyDrink2 && DateTime.Now <= drink2)
                MessageBox.Show("You should drink " + prescription.Drug + " at " + drink2.Hour + "h", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ThricePerDay(Prescription prescription, string JmbgOfLogged)
        {
            PatientService patientService = new PatientService();
            Patient patient = patientService.FindByJmbg(JmbgOfLogged);
            DateTime drink1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            DateTime notifyDrink1 = drink1.AddMinutes(-patient.NotificationTime.TotalMinutes);
            DateTime drink2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0);
            DateTime notifyDrink2 = drink2.AddMinutes(-patient.NotificationTime.TotalMinutes);
            DateTime drink3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 0, 0, 0);
            DateTime notifyDrink3 = drink3.AddMinutes(-patient.NotificationTime.TotalMinutes);
            if (DateTime.Now >= notifyDrink1 && DateTime.Now <= drink1)
                MessageBox.Show("You should drink " + prescription.Drug + " at " + drink1.Hour + "h", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (DateTime.Now >= notifyDrink2 && DateTime.Now <= drink2)
                MessageBox.Show("You should drink " + prescription.Drug + " at " + drink2.Hour + "h", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (DateTime.Now >= notifyDrink3 && DateTime.Now <= drink3)
                MessageBox.Show("You should drink " + prescription.Drug + " at around midnight", "Drug notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void DrugNotification(Prescription prescription, string JmbgOfLogged)
        {
            if (prescription.Start <= DateTime.Now && prescription.End >= DateTime.Now)
            {
                switch (prescription.DailyDose)
                {
                    case 1:
                        OncePerDay(prescription,JmbgOfLogged);
                        break;
                    case 2:
                        TwicePerDay(prescription,JmbgOfLogged);
                        break;
                    case 3:
                        ThricePerDay(prescription, JmbgOfLogged);
                        break;
                    default:
                        break;
                }
            }
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/prescriptions.json", JsonConvert.SerializeObject(Prescriptions, Formatting.Indented));
        }
    }
}
