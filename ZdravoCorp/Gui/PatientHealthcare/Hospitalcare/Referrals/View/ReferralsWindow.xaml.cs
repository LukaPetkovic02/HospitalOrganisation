using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Repository;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Patient;
using Validation = ZdravoCorp.Core.Utilities.Validation;


namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.Referrals.View
{
    public partial class ReferralsWindow : Window
    {
        public Core.Utilities.Doctor.Model.Doctor Doctor;
        public Core.Utilities.Patient.Model.Patient Patient;
        public DoctorService DoctorService;
        public PatientService PatientService;
        public DoctorReferralService DoctorReferralService;
        public TreatmentReferralService TreatmentReferralService;
        public List<string> AdditionalExaminations;

        public ReferralsWindow(Core.Utilities.Doctor.Model.Doctor doctor, Core.Utilities.Patient.Model.Patient patient)
        {
            Doctor = doctor;
            Patient = patient;
            DoctorService = new DoctorService();
            PatientService = new PatientService();
            DoctorReferralService = new DoctorReferralService();
            TreatmentReferralService = new TreatmentReferralService();
            AdditionalExaminations = new List<string>();
            InitializeComponent();
        }


        public void InitializeDoctorSpecializations()
        {  
            doctorsSpecilaziatons.Items.Clear();
            foreach (Core.Utilities.Doctor.Model.Doctor.Specialty type in Enum.GetValues(typeof(Core.Utilities.Doctor.Model.Doctor.Specialty)))
            {
                doctorsSpecilaziatons.Items.Add(type);
            }
        }

        public void InitializeDoctors()
        {
            doctorsSpecilaziatons.Items.Clear();
            foreach (Core.Utilities.Doctor.Model.Doctor doctor in DoctorService.Doctors())
            {   
                if(Doctor.Jmbg != doctor.Jmbg) doctorsSpecilaziatons.Items.Add(doctor.Jmbg);
            }
        }

        public bool IsDoctorReferralFormValid()
        {
            if (doctorsSpecilaziatons.SelectedIndex == -1)
            {
                return false;
            }
            if (specializationRadio.IsChecked == true)
            {
                return IsSpecializationValid();
            }


            return true;
        }

        public bool IsTreatmentReferralFormValid()
        {
            if (!IsDurationValid() || !IsTextBoxEmpty(therapyBox.Text))
            {
                return false;
            }

            return true;
        }

        public bool IsSpecializationValid()
        {
            if (DoctorService.FindBySpecialization((Core.Utilities.Doctor.Model.Doctor.Specialty) doctorsSpecilaziatons.SelectedValue,Doctor) == null)
            {
                MessageBox.Show("There is no doctor with chosen specialization! ");
                return false;
            }
            return true;
        }

        public bool IsDurationValid()
        {
            if (!Validation.IsStringAPositiveNumber(durationBox.Text))
            {
                MessageBox.Show("Please insert correct number of days! ");
                return false;
            }

            return true;
        }

        public bool IsTextBoxEmpty(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("You need to write something! ");
                return false;
            }

            return true;

        }

        public void CreateTreatmentReferral()
        {
            TreatmentReferralService.CreateTreatmentReferral(Doctor.Jmbg,Patient.Jmbg,int.Parse(durationBox.Text),
                    therapyBox.Text,AdditionalExaminations);
            MessageBox.Show($"Successfully issued treatment referral for patient - {Patient.Name} {Patient.LastName}");
            Close();
        }

        public void CreateDoctorReferral()
        {
            if (specializationRadio.IsChecked == true)
            {
                Core.Utilities.Doctor.Model.Doctor doctor =
                    DoctorService.FindBySpecialization((Core.Utilities.Doctor.Model.Doctor.Specialty)doctorsSpecilaziatons.SelectedValue, Doctor);
                DoctorReferralService.CreateDoctorReferral(doctor.Jmbg,Patient.Jmbg);
            }
            else
            {
                Core.Utilities.Doctor.Model.Doctor doctor = DoctorService.FindByJmbg((string)doctorsSpecilaziatons.SelectedValue);
                DoctorReferralService.CreateDoctorReferral(doctor.Jmbg, Patient.Jmbg);
            }
        }

        private void issueDoctorReferral_Click(object sender, RoutedEventArgs e)
        {
            if (IsDoctorReferralFormValid())
            {
                CreateDoctorReferral();
                MessageBox.Show("You successfully issued referral for patient: " + Patient.Name + " " + Patient.LastName);
                Close();
            }
        }

        private void specializationRadio_Checked(object sender, RoutedEventArgs e)
        {
            InitializeDoctorSpecializations();
        }

        private void doctorRadio_Checked(object sender, RoutedEventArgs e)
        {
            InitializeDoctors();
        }

        private void issueTreatmentReferral_Click(object sender, RoutedEventArgs e)
        {
            if (IsTreatmentReferralFormValid())
            {
                CreateTreatmentReferral();
            }
        }

        private void addExamination_Click(object sender, RoutedEventArgs e)
        {
            if (IsTextBoxEmpty(examinationBox.Text))
            {
                AdditionalExaminations.Add(examinationBox.Text);
                additionalExaminationsList.Items.Add(examinationBox.Text);
            }
        }
    }
}
