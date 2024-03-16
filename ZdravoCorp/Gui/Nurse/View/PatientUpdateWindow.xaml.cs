using System;
using System.Windows;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Patient;

namespace ZdravoCorp.Gui.Nurse.View
{
    /// <summary>
    /// Interaction logic for PatientUpdateWindow.xaml
    /// </summary>
    public partial class PatientUpdateWindow : Window
    {

        public NurseService NurseService;
        public PatientService PatientService;
        public Core.Utilities.Patient.Model.Patient Patient;

        public PatientUpdateWindow(String jmbg)
        {
            NurseService = new NurseService();
            PatientService = new PatientService();
            Patient = PatientService.FindByJmbg(jmbg);
            
            InitializeComponent();
            SetFields();
        }
        public void SetFields()
        {
            jmbgTxt.Text = Patient.Jmbg;
            passwordTxt.Text = Patient.Password;
            nameTxt.Text = Patient.Name;
            lastNameTxt.Text = Patient.LastName;
            birthDatePicker.SelectedDate = Patient.BirthDate;
        }

        public void ErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void UpdatePatient()
        {
            Patient.Password = passwordTxt.Text;
            Patient.Name = nameTxt.Text;
            Patient.LastName = lastNameTxt.Text;
            Patient.BirthDate = birthDatePicker.SelectedDate.Value;
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!NurseService.ValidatePatientInfo(
                jmbgTxt.Text, nameTxt.Text, lastNameTxt.Text,
                passwordTxt.Text, birthDatePicker.SelectedDate))
            {
                ErrorMessage("Invalid input");
                this.Close();
            }
            else
            {
                UpdatePatient();
                PatientService.Save();
                Close();
            }
        }

        private void cancleBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
