using System.Text.RegularExpressions;
using System.Windows;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Gui.Doctor.View
{
    public partial class ConditionWindow : Window
    {
        public Core.Utilities.Patient.Model.Patient Patient;
        PatientService PatientService;

        public ConditionWindow(Core.Utilities.Patient.Model.Patient patient)
        {
            Patient = patient;
            PatientService = new PatientService();
            InitializeComponent();
        }

        public bool ValidateConditionDate()
        {
            if (conditionDate.SelectedDate == null || conditionDate.SelectedDate < Patient.BirthDate.Date)
            {
                MessageBox.Show("Please insert correct condition date! ");
                return false;
            }

            return true;
        }

        public bool ValidateConditionName()
        {
            Regex regex = new Regex("^[a-zA-Z ]+$");
            if (string.IsNullOrEmpty(conditionName.Text) || !regex.IsMatch(conditionName.Text) 
                                                         || PatientService.FindByName(conditionName.Text,Patient) != null)

            {
                MessageBox.Show("Please insert correct condition! ");
                return false;
            }

            return true;
        }


        public bool ValidateCondition()
        {
            if (ValidateConditionDate() && ValidateConditionName())
            {
                return true;
            }

            return false;
        }

        private void addCondition_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCondition())
            {
                MedicalCondition medicalCondition =
                    new MedicalCondition(conditionName.Text, conditionDate.SelectedDate.Value);

                PatientService.AddMedicalCondition(medicalCondition,Patient);
                Close();
            }
        }
    }
}
