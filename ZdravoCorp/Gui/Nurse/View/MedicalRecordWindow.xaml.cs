using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;
using ZdravoCorp.Gui.Doctor.View;

namespace ZdravoCorp.Gui.Nurse.View
{
    public partial class MedicalRecordWindow : Window
    {
        public Core.Utilities.Patient.Model.Patient Patient;
        public PatientService PatientService;
        public AnamnesisService AnamnesisService;

        public MedicalRecordWindow(Core.Utilities.Patient.Model.Patient patient)
        {
            Patient = patient;
            PatientService = new PatientService();
            AnamnesisService = new AnamnesisService(new AnamnesisRepository());
            InitializeComponent();
            InitializePatientInfo();
            InitializeMedicalHistory();
            InitializePatientAnamneses();
            InitializeDoctorReferrals();
            InitializeTreatmentReferrals();
            Title = $"{patient.Name} {patient.LastName} - Medical card";
        }
        public DataTable CreateTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Diagnosis name");
            dt.Columns.Add("Diagnosis date");

            return dt;
        }

        public DataTable CreateTableHeadersForDoctorReferrals()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Refered to");
            dt.Columns.Add("ID");
            dt.Columns.Add("Used");
            return dt;
        }

        public DataTable CreateTableHeadersForTreatmentReferrals()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Days");
            dt.Columns.Add("Therapy");
            dt.Columns.Add("ID");
            dt.Columns.Add("Used");
            return dt;
        }

        public string[] ExtractDoctorReferralInfo(DoctorReferral dr)
        {
            string[] doctorReferralInfo =
            {
                dr.DoctorJmbg,
                dr.Id,
                dr.IsUsed.ToString()
            };

            return doctorReferralInfo;
        }

        public string[] ExtractTreatmentReferralInfo(TreatmentReferral tr)
        {
            string[] treatmentReferralInfo =
            {
                tr.Duration.ToString(),
                tr.Therapy,
                tr.Id,
                tr.IsUsed.ToString()
            };

            return treatmentReferralInfo;
        }

        public void LoadDoctorReferrals(List<DoctorReferral> doctorReferrals)
        {
            DataTable dt = CreateTableHeadersForDoctorReferrals();

            foreach (DoctorReferral dr in doctorReferrals)
            {
                dt.Rows.Add(ExtractDoctorReferralInfo(dr));
            }

            doctorReferralsTable.ItemsSource = new DataView(dt);
            doctorReferralsTable.Items.Refresh();
        }

        public void LoadTreatmentReferrals(List<TreatmentReferral> treatmentReferrals)
        {
            DataTable dt = CreateTableHeadersForTreatmentReferrals();

            foreach (TreatmentReferral tr in treatmentReferrals)
            {
                dt.Rows.Add(ExtractTreatmentReferralInfo(tr));
            }

            treatmentReferralsTable.ItemsSource = new DataView(dt);
            treatmentReferralsTable.Items.Refresh();
        }


        public string[] ExtractMedicalConditionInfo(MedicalCondition mc) 
        {
            string[] medicalConditionInfo =
            {
                mc.DiagnosisName,
                mc.DiagnosisDate.ToString("dd.MM.yyyy"),
            };

            return medicalConditionInfo;
        }
        public void LoadMedicalHistory(List<MedicalCondition> medicalConditions)
        {
            DataTable dt = CreateTableHeaders();

            foreach (MedicalCondition mc in medicalConditions)
            {
                dt.Rows.Add(ExtractMedicalConditionInfo(mc));
            }

            medicalHistoryDataGrid.ItemsSource = new DataView(dt);
            medicalHistoryDataGrid.Items.Refresh();
        }

        public void InitializeMedicalHistory()
        {
            LoadMedicalHistory(Patient.MedicalCard.MedicalHistory);
        }

        public void InitializeDoctorReferrals()
        {
            DoctorReferralService doctorReferralService = new DoctorReferralService();
            LoadDoctorReferrals(doctorReferralService.FindDoctorReferralsByPatientJmbg(Patient.Jmbg));
        }

        public void InitializeTreatmentReferrals()
        {
            TreatmentReferralService treatmentReferralService = new TreatmentReferralService();
            LoadTreatmentReferrals(treatmentReferralService.FindTreatmentReferralsByPatientJmbg(Patient.Jmbg));
        }

        public void InitializePatientAnamneses()
        {
            List<Anamnesis> anamneses = AnamnesisService.FindAnamnesesByPatientJmbg(Patient.Jmbg);
            LoadAnamneses(anamneses);
        }

        public void LoadAnamneses(List<Anamnesis> anamneses)
        {
            foreach (var anamnesis in anamneses)
            {
                patientAnamneses.Items.Add(anamnesis.Description + " - " +  anamnesis.Date.Date.ToString("dd.MM.yyyy"));
            }
        }

        public void InitializePatientInfo()
        {
            patientName.Text = Patient.Name;
            patientLastName.Text = Patient.LastName;
            patientHeight.Text = Patient.MedicalCard.Height.ToString();
            patientWeight.Text = Patient.MedicalCard.Weight.ToString();
            patientBirthDate.Text = Patient.BirthDate.Date.ToString("dd.MM.yyyy");
            patientJmbg.Text = Patient.Jmbg;
        }


        private void editMedicalCard_Click(object sender, RoutedEventArgs e)
        {
            addDiagnosis.Visibility = Visibility.Visible;
            deleteDiagnosis.Visibility = Visibility.Visible;
            patientHeight.IsEnabled = true;
            patientWeight.IsEnabled = true;
            editMedicalCard.Visibility = Visibility.Hidden;
            editCard.Visibility = Visibility.Visible;

        }

        public MedicalCondition? ExtractConditionFromTable()
        {
            DataRowView row = medicalHistoryDataGrid.SelectedItem as DataRowView;

            if (row != null)
            {
                return PatientService.FindByName(row["Diagnosis name"].ToString(),Patient);
            }

            MessageBox.Show("Please select a condition! ");
            return null;
        }

        public void RefreshMedicalCard()
        {
            PatientService = new PatientService();
            Patient = PatientService.FindByJmbg(Patient.Jmbg);
            InitializeMedicalHistory();
            InitializePatientAnamneses();


        }

        private void addDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            ConditionWindow cw = new ConditionWindow(Patient);
            cw.ShowDialog();
            RefreshMedicalCard();
        }

        private void deleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            MedicalCondition? medicalCondition = ExtractConditionFromTable();

            if (medicalCondition != null)
            {
                PatientService.DeleteMedicalCondition(medicalCondition.DiagnosisName,Patient);
                RefreshMedicalCard();
            }
            
        }


        public bool ValidateWidthAndHeight(TextBox text)
        
        {
            double parsedValue;
            if (double.TryParse(text.Text,out parsedValue))
            {   
                if (parsedValue > 0) {return true;}
            }

            if (text == patientWeight)
            {
                MessageBox.Show("Please insert correct weight! ");

            }
            else
            {
                MessageBox.Show("Please insert correct height! ");
            }
            return false;
        }

        private void editCard_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateWidthAndHeight(patientWeight) && ValidateWidthAndHeight(patientHeight))
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to edit this medical card?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    PatientService.EditMedicalCard(Double.Parse(patientWeight.Text), Double.Parse(patientHeight.Text), Patient);
                    RefreshMedicalCard();
                    Close();
                }
            }
        }
    }
}
