using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Model;
using ZdravoCorp.Core.PatientHealtcare.Healthcare.Anamneses.Repository;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.View;

namespace ZdravoCorp.Gui.Scheduling.View
{
    public partial class PatientFinishedAppointments : Window
    {
        
        public string JmbgOfLogged;
        public List<Appointment> PatientAppointments;
        public List<Anamnesis> PatientAnamnesis;
        public ScheduleService ScheduleService;
        public AnamnesisService AnamnesisService;
        public DoctorService DoctorService;
        public DoctorSurveyService DoctorSurveyService;
        public PatientFinishedAppointments(string jmbgOfLogged)
        {
            
            JmbgOfLogged = jmbgOfLogged;
            InitializeComponent();
            AnamnesisService = new AnamnesisService(new AnamnesisRepository());
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            DoctorSurveyService = new DoctorSurveyService();
            PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
            PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
            FillListBox();
        }
        public void FillListBox()
        {
            listBox.Items.Clear();
            List<string> strings = new List<string>();
            foreach (Appointment a in PatientAppointments)
            {
                string s = a.JmbgDoctor + " " + a.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm") + " " + a.Status;
                strings.Add(s);
            }
            foreach (string s in strings)
            {
                listBox.Items.Add(s);
            }
            listBox.Items.Refresh();
        }
        private void Appointment_Anamnesis_Click(object sender, RoutedEventArgs e)
        {
            int index = listBox.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Select one of the appointments!");
            }
            else
            {
                MessageBox.Show(AnamnesisService.FindAnamnesisById(PatientAppointments.ElementAt(index).Id).Description);
            }
        }

        private void Sort_By_Date_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments=ScheduleService.SortAppointmentsByStartTime(PatientAppointments);
            FillListBox();
        }

        private void Sort_By_Doctor_Jmbg_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments = ScheduleService.SortAppointmentsByDoctorJmbg(PatientAppointments);
            FillListBox();
        }

        private void Sort_By_Doctor_Specialty_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, Core.Utilities.Doctor.Model.Doctor.Specialty> doctorSpecialties = new Dictionary<string, Core.Utilities.Doctor.Model.Doctor.Specialty>();
            foreach(Appointment appointment in PatientAppointments)
            {
                if (!doctorSpecialties.ContainsKey(appointment.JmbgDoctor))
                {
                    doctorSpecialties[appointment.JmbgDoctor] = DoctorService.FindByJmbg(appointment.JmbgDoctor).Specialization;
                }
            }
            PatientAppointments=ScheduleService.SortAppointmentsByDoctorSpecialty(PatientAppointments, doctorSpecialties);
            FillListBox();
        }

        private void Search_Anamnesis_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                
                listBox.Items.Clear();
                PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
                PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
                FillListBox();


                PatientAppointments = ScheduleService.FindAppointmentsByAnamnesisDescriptionSubstring(PatientAppointments, textBox.Text);
                if (PatientAppointments.Count() == 0)
                {
                    MessageBox.Show("There are no results");
                }
                PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
                FillListBox();
            }
            else
            {
                MessageBox.Show("Search can't be empty!");
            }
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
            PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
            FillListBox();
        }

        
        private void Grade_Selected_Doctor_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                string selected = AnamnesisService.FindAnamnesisById(PatientAppointments.ElementAt(listBox.SelectedIndex).Id).DoctorJmbg;
                Core.Utilities.Doctor.Model.Doctor selectedDoctor = DoctorService.FindByJmbg(selected);
                DoctorSurveyView doctorSurveyView = new DoctorSurveyView(selectedDoctor);
                Window doctorSurveyWindow = new Window
                {
                    Title = "Doctor survey",
                    Content = doctorSurveyView,
                    Height = 450,
                Width = 500,
                };
                doctorSurveyWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You must select an appointment!");
            }
                
        }
    }
}
