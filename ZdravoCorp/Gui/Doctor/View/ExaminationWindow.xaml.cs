using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;
using ZdravoCorp.Core.PhysicalAsets.Inventory;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Gui.Nurse;
using ZdravoCorp.Gui.Nurse.View;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.Referrals.View;
using ZdravoCorp.Gui.PatientHealthcare.Pharmacy.Prescriptions.View;

namespace ZdravoCorp.Gui.Doctor.View
{
    public partial class ExaminationWindow : Window
    {
        public Appointment Appointment;
        public PatientService PatientService;
        public AnamnesisService AnamnesisService;
        public InventoryService InventoryService;
        public ScheduleService ScheduleService;
        public DoctorService DoctorService;

        public ExaminationWindow(Appointment appointment)
        {
            
            InitializeComponent();
            Appointment = appointment;
            PatientService = new PatientService();
            AnamnesisService = new AnamnesisService(new AnamnesisRepository());
            InventoryService = new InventoryService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            InitializeExaminationWindow();
        }

        public void InitializeExaminationWindow()
        {
            Core.Utilities.Patient.Model.Patient patient = PatientService.FindByJmbg(Appointment.JmbgPatient);
            patientName.Text = patient.Name;
            patientLastName.Text = patient.LastName;
        }


        private void medicalCard_Click(object sender, RoutedEventArgs e)
        {
            Core.Utilities.Patient.Model.Patient patient = PatientService.FindByJmbg(Appointment.JmbgPatient);
            MedicalRecordWindow mrw = new MedicalRecordWindow(patient);
            mrw.ShowDialog();
            PatientService = new PatientService();

        }

        public bool ValidateAnamnesis(string anamnesis)
        {
            if (string.IsNullOrEmpty(anamnesis))
            {
                MessageBox.Show("You need to write anamnesis first! ");
                return false;
            }

            return true;

        }

        public void ShowWindowsAfterExamination()
        {
            PrescriptionWindow pw = new PrescriptionWindow(PatientService.FindByJmbg(Appointment.JmbgPatient), DoctorService.FindByJmbg(Appointment.JmbgDoctor));
            pw.ShowDialog();
            EquipmentStatusWindow esw = new EquipmentStatusWindow(Appointment.RoomId);
            esw.ShowDialog();
        }

        private void finishExamination_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAnamnesis(doctorAnamnesis.Text))
            {
                Anamnesis anamnesis = AnamnesisService.FindAnamnesisByAppointmentId(Appointment.Id);

                if (anamnesis != null)
                {
                    AnamnesisService.EditAnamnesis(doctorAnamnesis.Text, Appointment.Id);
                }
                else
                {
                    AnamnesisService.CreateAnamnesis(Appointment.JmbgDoctor,Appointment.JmbgPatient,Appointment.TimeSlot.Start,
                        doctorAnamnesis.Text,Appointment.Id);
                }

                Close();
                ScheduleService.FinishAppointment(Appointment);

                ShowWindowsAfterExamination();
            }

        }

        private void IssueReferral_Click(object sender, RoutedEventArgs e)
        {
            Core.Utilities.Doctor.Model.Doctor doctor = DoctorService.FindByJmbg(Appointment.JmbgDoctor);
            Core.Utilities.Patient.Model.Patient patient = PatientService.FindByJmbg(Appointment.JmbgPatient);

            ReferralsWindow rw = new ReferralsWindow(doctor, patient);
            rw.ShowDialog();
        }
    }
}
