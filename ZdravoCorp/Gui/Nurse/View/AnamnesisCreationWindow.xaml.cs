using System;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;

namespace ZdravoCorp.Gui.Nurse.View
{
    /// <summary>
    /// Interaction logic for AnamnesisCreationWindow.xaml
    /// </summary>
    public partial class AnamnesisCreationWindow : Window
    {
        public string DoctorJmbg;
        public string PatientJmbg;
        public string AppointmentId;
        public AnamnesisService AnamnesisService;
        public AnamnesisCreationWindow(string doctorJmbg, string patientJmbg, string appointmentId)
        {
            InitializeComponent();
            DoctorJmbg = doctorJmbg;
            PatientJmbg = patientJmbg;
            AppointmentId = appointmentId;
            AnamnesisService = new AnamnesisService(new AnamnesisRepository());
        }

        private void CreateAnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            AnamnesisService.CreateAnamnesis(DoctorJmbg, PatientJmbg, DateTime.Now, "Symptoms: " + SymptomsTextBox.Text + " | ", AppointmentId);
            this.Close();
        }
    }
}
