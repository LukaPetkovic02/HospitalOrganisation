using System.Windows;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Patient;

namespace ZdravoCorp.Gui.Nurse.View
{
    /// <summary>
    /// Interaction logic for CreatePatientWindow.xaml
    /// </summary>
    public partial class CreatePatientWindow : Window
    {
        public NurseService NurseService;
        public PatientService PatientService;

        public CreatePatientWindow(string jmbg)
        {
            jmbgTxt.Text = jmbg;
            InitializeComponent();
            NurseService = new NurseService();
            PatientService = new PatientService();
        }
        public void ErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ClearTxtFields()
        {
            jmbgTxt.Clear();
            nameTxt.Clear();
            lastNameTxt.Clear();
            passwordTxt.Clear();
            weightTxt.Clear();
            heightTxt.Clear();
            birthDatePicker.SelectedDate = null;
        }
        public void SuccesMessage(string message)
        {
            MessageBox.Show(message, "Succesfull", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void createNewPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NurseService.ValidateNewPatient(
                jmbgTxt.Text, nameTxt.Text, lastNameTxt.Text,
                passwordTxt.Text, weightTxt.Text, heightTxt.Text, birthDatePicker.SelectedDate))
            {
                ErrorMessage("Wrong input");
                ClearTxtFields();
            }
            Core.Utilities.Patient.Model.Patient newPatient = NurseService.CreatePatient(
                    jmbgTxt.Text, nameTxt.Text, lastNameTxt.Text,
                    passwordTxt.Text, birthDatePicker.SelectedDate.Value, weightTxt.Text, heightTxt.Text);

            PatientService.PatientRepository.Patients.Add(newPatient);
            PatientService.Save();

            SuccesMessage("Succesfully added new patient");
            this.Close();
        }
    }
}
