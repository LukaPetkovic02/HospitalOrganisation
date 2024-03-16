using System;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;
using Validation = ZdravoCorp.Core.Utilities.Validation;

namespace ZdravoCorp.Gui.PatientHealthcare.Pharmacy.Prescriptions.View
{
    public partial class PrescriptionWindow : Window
    {
        public Core.Utilities.Patient.Model.Patient Patient;
        public PrescriptionService PrescriptionService;
        public DrugService DrugService;
        public Core.Utilities.Doctor.Model.Doctor Doctor;
        public PrescriptionWindow(Core.Utilities.Patient.Model.Patient patient, Core.Utilities.Doctor.Model.Doctor doctor)
        {   
            Patient = patient;
            Doctor = doctor;
            PrescriptionService = new PrescriptionService();
            DrugService = new DrugService();
            InitializeComponent();
            InitializeDrugs();
            InitializeConsumptionTime();
        }

        public void InitializeDrugs()
        {
            foreach (var drug in DrugService.GetAllDrugs())
            {
                drugsBox.Items.Add(drug.Name);
            }
        }

        public void InitializeConsumptionTime()
        {
            foreach (Prescription.Consumption consumption in Enum.GetValues(typeof(Prescription.Consumption)))
            {
                consumptionTime.Items.Add(consumption);
            }
        }

        public bool IsDailyDoseValid()
        {
            if (!Validation.IsStringAPositiveNumber(dailyDose.Text))
            {
                MessageBox.Show("Daily dose needs to be a number and > 0! ");
                return false;
            }
            return true;
        }

        public bool IsDateValid()
        {
            try
            {
                if (startDate.SelectedDate.Value < DateTime.Today || endDate.SelectedDate.Value <= startDate.SelectedDate.Value)
                {
                    MessageBox.Show("Please insert correct dates!");
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Please insert correct dates!");
                return false;
            }
            return true;
        }

        public bool IsDrugValid()
        {
            if (drugsBox.SelectedIndex == -1)
            {
                MessageBox.Show("You need to choose a drug to prescript! ");
                return false;
            }
            return true;
        }

        public bool IsConsumptionValid()
        {
            if (consumptionTime.SelectedIndex == -1)
            {
                MessageBox.Show("You need to choose consumption time! ");
                return false;
            }
            return true;
        }

        public bool IsPrescriptionFormValid()
        {
            if (!IsDrugValid() || !IsDateValid() || !IsConsumptionValid() || !IsDailyDoseValid())
            {
                return false;
            }
            return true;
        }

        public bool IsPatientAllergicToDrug()
        {   
            Drug drug = DrugService.FindByName(drugsBox.SelectedValue.ToString());
            if (DrugService.IsPatientAllergicToDrug(Patient,drug))
            {
                MessageBox.Show($"Patient is allergic to {drug.Name}! ");
                return false;
            }

            return true;
        }

        public void ClearFormFields()
        {
            drugsBox.SelectedIndex = -1;
            consumptionTime.SelectedIndex = -1;
            dailyDose.Text = "";

        }

        public void CreatePrescription()
        {
            PrescriptionService.CreatePrescription(drugsBox.SelectedValue.ToString(), startDate.SelectedDate.Value,endDate.SelectedDate.Value,
                int.Parse(dailyDose.Text),(Prescription.Consumption)consumptionTime.SelectedValue,Patient.Jmbg, Doctor.Jmbg);
        }

        private void prescribeMedicine_Click(object sender, RoutedEventArgs e)
        {
            if (IsPrescriptionFormValid() && IsPatientAllergicToDrug())
            {
                CreatePrescription();
                ClearFormFields();
                MessageBox.Show($"You successfully prescripted a drug for patient {Patient.Name} {Patient.LastName}");
            }
        }
    }
}
