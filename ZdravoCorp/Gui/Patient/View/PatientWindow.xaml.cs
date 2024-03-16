using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Scheduling.SmartSchedule;
using ZdravoCorp.Core.Scheduling.SmartSchedule.Model;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Gui.Nurse.View;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.View;
using ZdravoCorp.Gui.Scheduling.View;

namespace ZdravoCorp.Gui.Patient.View
{
    using ZdravoCorp.Gui.Nurse;
    using ZdravoCorp.Core.Notifications;
    using System.Configuration;
    using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey;

    public partial class PatientWindow : Window
    {
        public string JmbgOfLogged;
        public ScheduleService ScheduleService;
        public DoctorService DoctorService;
        public PatientService PatientService;
        public RoomService RoomService;
        public PrescriptionService PrescriptionService;
        public List<Prescription> Prescriptions;
        public HospitalWorkSurveyService HospitalWorkSurveyService;
        public void LoadAllServices()
        {
            DoctorService = new DoctorService();
            ScheduleService = new ScheduleService();
            PatientService = new PatientService();
            RoomService = new RoomService();
            PrescriptionService = new PrescriptionService();
            HospitalWorkSurveyService = new HospitalWorkSurveyService();
        }
        
        
        public PatientWindow(string jmbgOfLogged)
        {
            JmbgOfLogged = jmbgOfLogged;
            InitializeComponent();
            LoadAllServices();
            InitializeHospitalSurvey();
            Prescriptions = PrescriptionService.FindPrescriptionByJmbg(jmbgOfLogged);
            Core.Utilities.Patient.Model.Patient patient = PatientService.FindByJmbg(jmbgOfLogged);
            textBoxDrugNot.Text = patient.NotificationTime.ToString(@"hh\:mm");
            List<Appointment> patientAppointments = ScheduleService.GetAppointmentsByPatientJmbg(JmbgOfLogged);
            List<string> strings = new List<string>();
            foreach(Appointment a in patientAppointments)
            {
                string s = a.JmbgDoctor + " " + a.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm")+" "+a.Status;
                strings.Add(s);
            }
            foreach(string s in strings)
            {
                listBox.Items.Add(s);
            }
            
            strings = new List<string>();
            foreach (Core.Utilities.Doctor.Model.Doctor d in DoctorService.DoctorRepository.Doctors)
            {
                strings.Add(d.Jmbg+" "+d.Name+" "+d.LastName);
            }
            foreach(string s in strings)
            {
                doctorJmbg.Items.Add(s);
                doctorJmbgSmart.Items.Add(s);
            }
            FillListBoxWithDoctors(DoctorService.DoctorRepository.Doctors);
            comboBoxSpecialty.Items.Insert(0, "-");
            comboBoxSpecialty.SelectedIndex = 0;
            foreach (string s in Enum.GetNames(typeof(Core.Utilities.Doctor.Model.Doctor.Specialty)))
                comboBoxSpecialty.Items.Add(s);

            Loaded += PatientWindow_Loaded;
        }

        private void PatientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(Prescription prescription in Prescriptions)
                PrescriptionService.DrugNotification(prescription,JmbgOfLogged);
        }

        private void patientWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to log out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void InitializeHospitalSurvey()
        {
            HospitalWorkSurveyView hospitalWorkSurveyView = new HospitalWorkSurveyView();
            hospitalSurveyGrid.Children.Add(hospitalWorkSurveyView);
            hospitalSurveyTab.Content = hospitalSurveyGrid;
        }
        public bool ValidFieldsInput()
        { 
            string timeString = comboBoxTime.Text;
            DateTime time;
            if (doctorJmbg.SelectedIndex==-1)
            {
                MessageBox.Show("Jmbg field can't be empty!");
                return false;
            }
            else if (datePicker.SelectedDate == null || datePicker.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Date is not selected or isn't in the future!");
                return false;
            }
            
            else if (!DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                MessageBox.Show("Wrong time input!");
                return false;
            }
            else if(DateTime.Now.Date==datePicker.SelectedDate.Value.Date && time.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                MessageBox.Show("Selected today's date but time is in the past!");
                return false;
            }
            return true;   
        }
        public bool ValidFieldsInputTab2()
        {
            string timeStringStart = comboBoxStartTime.Text;
            string timeStringEnd = comboBoxEndTime.Text;
            DateTime startTime,endTime;
            if (doctorJmbgSmart.SelectedIndex == -1)
            {
                MessageBox.Show("Jmbg field can't be empty!");
                return false;
            }
            else if (datePickerLastDay.SelectedDate == null || datePickerLastDay.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Date is in the past or is not selected!");
                return false;
            }
            else if (!DateTime.TryParseExact(timeStringStart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
            {
                MessageBox.Show("Wrong time input!");
                return false;
            }
            else if (!DateTime.TryParseExact(timeStringEnd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
            {
                MessageBox.Show("Wrong time input!");
                return false;
            }
            else if(endTime<=startTime)
            {
                MessageBox.Show("Start time must be before end time!");
                return false;
            }
            else if(datePickerLastDay.SelectedDate.Value.Date == DateTime.Now.Date && endTime.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                MessageBox.Show("Selected today's date but end time has already passed!");
                return false;
            }
            return true;
        }
        public bool IsDoctorAvailable()
        {  
            DateTime selectedDate = datePicker.SelectedDate.Value.Date;
            string sTime = comboBoxTime.Text;
            TimeSpan time = TimeSpan.Parse(sTime);

            DateTime selectedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, time.Hours, time.Minutes, time.Seconds);
            TimeSlot timeSlot = new TimeSlot(selectedDateTime, selectedDateTime.AddMinutes(15));

            Core.Utilities.Doctor.Model.Doctor docWithJmbg = DoctorService.FindByJmbg(doctorJmbg.SelectedItem.ToString().Split(" ")[0]);

            if (RoomService.FindFirstAvailableRoom(RoomType.ExaminationRoom, ScheduleService.Appointments(),
                    timeSlot) == null)
            {
                MessageBox.Show("There is no available rooms at that time!");
                return false;
            }

            return ScheduleService.IsAvailable(docWithJmbg, timeSlot) && ScheduleService.IsAvailable(PatientService.FindByJmbg(JmbgOfLogged),timeSlot); 
        }
        
        private void Schedule_Appointment_Click(object sender, RoutedEventArgs e)
        {
            if (ValidFieldsInput())
            {
                DateTime date = datePicker.SelectedDate.Value;
                string timeString = comboBoxTime.Text;
                DateTime time = DateTime.ParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture);
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                if (IsDoctorAvailable())
                {
                    if (!PatientService.PatientRepository.PatientAppointmentCreations.ContainsKey(JmbgOfLogged))
                    {
                        PatientService.PatientRepository.PatientAppointmentCreations[JmbgOfLogged] = new List<DateTime>();
                    }
                    PatientService.PatientRepository.PatientAppointmentCreations[JmbgOfLogged].Add(DateTime.Now);

                    TimeSlot timeSlot = new TimeSlot(dateTime, dateTime.AddMinutes(15));
                    Appointment appointment = new Appointment(doctorJmbg.SelectedItem.ToString().Split(" ")[0], JmbgOfLogged, timeSlot,ScheduleService.ScheduleRepository.GenerateAppointmentId(),"for error handling");
                    appointment.Status=AppointmentStatus.SCHEDULED;
                    Room room = RoomService.FindFirstAvailableRoom(RoomType.ExaminationRoom, ScheduleService.Appointments(), timeSlot);
                    ScheduleService.CreateAppointment(doctorJmbg.SelectedItem.ToString().Split(" ")[0], JmbgOfLogged, timeSlot,room.Id);
                    MessageBox.Show("Successfuly scheduled appointment!");
                    PatientService.Save();
                    string s = appointment.JmbgDoctor + " " + appointment.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm")+ " "+appointment.Status;
                    listBox.Items.Add(s);
                    listBox.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Doctor (or logged patient) is not available in that time!");
                }
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                List<string> strings = new List<string>();
                List<Appointment> patientAppointments = ScheduleService.GetAppointmentsByPatientJmbg(JmbgOfLogged);
                int selectedIndex = listBox.SelectedIndex;
                Appointment selectedAppointment = patientAppointments[selectedIndex];

                if(DateTime.Now.AddDays(1)>=selectedAppointment.TimeSlot.Start)
                {
                    MessageBox.Show("You can't edit or delete appointments which begin in less than 24 hours!");
                }
                else if (selectedAppointment.Status != AppointmentStatus.SCHEDULED)
                {
                    MessageBox.Show("You can't cancel appointments that are finished or canceled!");
                }
                else
                {
                    PopUpForm popUp = new PopUpForm(selectedAppointment, JmbgOfLogged);

                    
                    foreach (Core.Utilities.Doctor.Model.Doctor d in DoctorService.DoctorRepository.Doctors)
                    {
                        strings.Add(d.Jmbg);
                    }
                    foreach (string s in strings)
                    {
                        popUp.doctorJmbg.Items.Add(s);
                    }
                    

                    popUp.doctorJmbg.SelectedItem = selectedAppointment.JmbgDoctor;
                    popUp.datePicker.Text = selectedAppointment.TimeSlot.Start.Date.ToString();
                    popUp.comboBoxTime.Text = selectedAppointment.TimeSlot.Start.ToString("HH:mm");
                    popUp.ShowDialog();
                }

                listBox.Items.Clear();
                patientAppointments = ScheduleService.GetAppointmentsByPatientJmbg(JmbgOfLogged);
                strings = new List<string>();
                foreach (Appointment a in patientAppointments)
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
            else
            {
                MessageBox.Show("Please select an appointment to edit!");
            }
        }
        private void CancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                List<Appointment> patientAppointments = ScheduleService.GetAppointmentsByPatientJmbg(JmbgOfLogged);
                int selectedIndex = listBox.SelectedIndex;
                Appointment appointment = patientAppointments[selectedIndex];

                if (DateTime.Now.AddDays(1) >= appointment.TimeSlot.Start)
                {
                    MessageBox.Show("You can't edit or cancel appointments which begin in less than 24 hours!");
                }
                else if (appointment.Status != AppointmentStatus.SCHEDULED)
                {
                    MessageBox.Show("You can't cancel appointments that are finished or canceled!");
                }
                else
                {
                    
                    ScheduleService.CancelAppointment(appointment);
                    listBox.Items.RemoveAt(selectedIndex);
                    listBox.Items.Add(appointment.JmbgDoctor + " " + appointment.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm") + " " + appointment.Status);
                    listBox.Items.Refresh();

                    if (!PatientService.PatientRepository.PatientAppointmentEdits.ContainsKey(JmbgOfLogged))
                    {
                        PatientService.PatientRepository.PatientAppointmentEdits[JmbgOfLogged] = new List<DateTime>();
                    }
                    PatientService.PatientRepository.PatientAppointmentEdits[JmbgOfLogged].Add(DateTime.Now);

                    MessageBox.Show("Successfuly canceled appointment!");
                    PatientService.Save();
                    ScheduleService.Save();
                }
            }
            else
            {
                MessageBox.Show("Please select an appointment to cancel");
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            listBoxTab2.Items.Clear();
            if (ValidFieldsInputTab2()) {
                Core.Utilities.Doctor.Model.Doctor d = DoctorService.FindByJmbg(doctorJmbgSmart.Text.Split(" ")[0]);
                DateTime selectedDate = datePickerLastDay.SelectedDate.Value.Date;
                string sStartTime = comboBoxStartTime.Text;
                DateTime startTime = DateTime.Parse(sStartTime);
                string sEndTime = comboBoxEndTime.Text;
                DateTime endTime = DateTime.Parse(sEndTime);
                TimeSlot t=new TimeSlot(startTime, endTime);
                AppointmentSmartSchedule appointmentSmartScheduleBoth = new AppointmentSmartSchedule(JmbgOfLogged, new BothPriorityRecommendationAlgorithm(JmbgOfLogged));
                AppointmentSmartSchedule appointmentSmartScheduleDoctor = new AppointmentSmartSchedule(JmbgOfLogged, new DoctorPriorityRecommendationAlgorithm(JmbgOfLogged));
                AppointmentSmartSchedule appointmentSmartScheduleTime = new AppointmentSmartSchedule(JmbgOfLogged, new TimePriorityRecommendationAlgorithm(JmbgOfLogged));

                selectedDate =new DateTime(selectedDate.Year,selectedDate.Month,selectedDate.Day,23,59,59);
                List<Appointment> appointments = appointmentSmartScheduleBoth.GetAvailableAppointments(d,selectedDate,t);

                if (appointments.Count == 0)
                {
                    MessageBox.Show("There are no appointments for both doctor and time range. Checking priority...");
                    
                    if (rbDoctor.IsChecked==true)
                    {
                        appointments = appointmentSmartScheduleDoctor.GetAvailableAppointments(d,selectedDate, t);
                    }
                    else if (rbTime.IsChecked == true)
                    {
                        appointments = appointmentSmartScheduleTime.GetAvailableAppointments(d, selectedDate, t);
                        if(appointments.Count == 0)
                        {
                            MessageBox.Show("There isn't a single available appointment in this time range. Expanding time range...");
                            appointments = appointmentSmartScheduleDoctor.GetAvailableAppointments(d,selectedDate, t);
                        }
                    }
                }
                
                foreach(Appointment appointment in appointments)
                {
                    listBoxTab2.Items.Add(appointment.JmbgDoctor+" "+appointment.TimeSlot.Start.ToString());
                }
                listBoxTab2.Items.Refresh();
            }
            
        }

        private void Smart_Schedule_Click(object sender, RoutedEventArgs e)
        {
            if (ValidFieldsInputTab2())
            {
                Core.Utilities.Doctor.Model.Doctor d = DoctorService.FindByJmbg(doctorJmbgSmart.Text.Split(" ")[0]);
                DateTime selectedDate = datePickerLastDay.SelectedDate.Value.Date;
                string sStartTime = comboBoxStartTime.Text;
                DateTime startTime = DateTime.Parse(sStartTime);
                string sEndTime = comboBoxEndTime.Text;
                DateTime endTime = DateTime.Parse(sEndTime);
                TimeSlot t = new TimeSlot(startTime, endTime);
                BothPriorityRecommendationAlgorithm bothPriority = new BothPriorityRecommendationAlgorithm(JmbgOfLogged);
                DoctorPriorityRecommendationAlgorithm doctorPriority = new DoctorPriorityRecommendationAlgorithm(JmbgOfLogged);
                TimePriorityRecommendationAlgorithm timePriority = new TimePriorityRecommendationAlgorithm(JmbgOfLogged);
                AppointmentSmartSchedule appointmentSmartScheduleBoth = new AppointmentSmartSchedule(JmbgOfLogged, bothPriority);
                AppointmentSmartSchedule appointmentSmartScheduleDoctor = new AppointmentSmartSchedule(JmbgOfLogged, doctorPriority);
                AppointmentSmartSchedule appointmentSmartScheduleTime = new AppointmentSmartSchedule(JmbgOfLogged, timePriority);

                selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 23, 59, 59);
                List<Appointment> appointments = appointmentSmartScheduleBoth.GetAvailableAppointments(d, selectedDate, t);
                if (appointments.Count == 0)
                {
                    
                    if (rbDoctor.IsChecked == true)
                    {
                        appointments = appointmentSmartScheduleDoctor.GetAvailableAppointments(d,selectedDate, t);
                    }
                    else if (rbTime.IsChecked == true)
                    {
                        appointments = appointmentSmartScheduleTime.GetAvailableAppointments(d,selectedDate, t);
                        if (appointments.Count == 0)
                        {
                            MessageBox.Show("There are no available appointments for this time range");
                        }
                    }
                }

                int index=listBoxTab2.SelectedIndex;
                if(index == -1)
                {
                    MessageBox.Show("Select one of the appointments to schedule!");
                }
                else
                {
                    Appointment appointment=appointments.ElementAt(index);
                    ScheduleService.ScheduleRepository.Appointments.Add(appointment);
                    ScheduleService.Save();
                    if (!PatientService.PatientRepository.PatientAppointmentCreations.ContainsKey(JmbgOfLogged))
                    {
                        PatientService.PatientRepository.PatientAppointmentCreations[JmbgOfLogged] = new List<DateTime>();
                    }
                    PatientService.PatientRepository.PatientAppointmentCreations[JmbgOfLogged].Add(DateTime.Now);

                    string s = appointment.JmbgDoctor + " " + appointment.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm") + " " + appointment.Status;
                    listBox.Items.Add(s);
                    listBox.Items.Refresh();
                    PatientService.Save();
                    MessageBox.Show("Successfuly scheduled appointment!");
                    listBoxTab2.Items.Remove(listBoxTab2.SelectedItem);
                    listBoxTab2.Items.Refresh();
                }

            }
        }

        private void Medical_Card_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordWindow mrw = new MedicalRecordWindow(PatientService.FindByJmbg(JmbgOfLogged));
            mrw.ShowDialog();
        }
        private void Finished_Appointments_Click(object sender, RoutedEventArgs e)
        {
            PatientFinishedAppointments pfa = new PatientFinishedAppointments(JmbgOfLogged);
            pfa.ShowDialog();
        }
        public void FillListBoxWithDoctors(List<Core.Utilities.Doctor.Model.Doctor> doctors)
        {
            listBoxDoctors.Items.Clear();
            foreach (Core.Utilities.Doctor.Model.Doctor d in doctors)
            {
                listBoxDoctors.Items.Add(d.Jmbg + " "+d.Name + " " + d.LastName + " " + d.Specialization+" "+d.AverageScore);
            }
            listBoxDoctors.Items.Refresh();
        }
        private void Search_Doctors_Click(object sender, RoutedEventArgs e)
        {
            List<Core.Utilities.Doctor.Model.Doctor> foundDocsByName = DoctorService.GetDoctorsByNameSubstring(DoctorService.DoctorRepository.Doctors, nameBox.Text);
            List<Core.Utilities.Doctor.Model.Doctor> foundDocsByLastName = DoctorService.GetDoctorsByLastNameSubstring(foundDocsByName, lastNameBox.Text);
            
            if(comboBoxSpecialty.SelectedIndex > 0)
            {
                if (Enum.TryParse(comboBoxSpecialty.Text, out Core.Utilities.Doctor.Model.Doctor.Specialty specialty))
                {
                    List<Core.Utilities.Doctor.Model.Doctor> foundDocsBySpec = DoctorService.GetDoctorsBySpecialization(foundDocsByLastName, specialty);
                    FillListBoxWithDoctors(foundDocsBySpec);
                }
            }
            else
            {
                FillListBoxWithDoctors(foundDocsByLastName);
            }
        }
        private void Reset_Doctors_Click(object sender, RoutedEventArgs e)
        {
            FillListBoxWithDoctors(DoctorService.DoctorRepository.Doctors);
        }
        public List<Core.Utilities.Doctor.Model.Doctor> FillDoctorsFromListBox()
        {
            List<Core.Utilities.Doctor.Model.Doctor> ret = new List<Core.Utilities.Doctor.Model.Doctor>();
            foreach(string s in listBoxDoctors.Items)
            {
                string jmbg = s.Split(" ")[0];
                Core.Utilities.Doctor.Model.Doctor doc = DoctorService.FindByJmbg(jmbg);
                ret.Add(doc);
            }
            return ret;
        }
        private void SortName_Click(object sender, RoutedEventArgs e)
        {
            List<Core.Utilities.Doctor.Model.Doctor> input = FillDoctorsFromListBox();
            input = DoctorService.SortDoctorsByName(input);
            FillListBoxWithDoctors(input);
        }

        private void SortLastName_Click(object sender, RoutedEventArgs e)
        {
            List<Core.Utilities.Doctor.Model.Doctor> input = FillDoctorsFromListBox();
            input = DoctorService.SortDoctorsByLastName(input);
            FillListBoxWithDoctors(input);
        }

        private void SortSpec_Click(object sender, RoutedEventArgs e)
        {
            List<Core.Utilities.Doctor.Model.Doctor> input = FillDoctorsFromListBox();
            input = DoctorService.SortDoctorsBySpecialty(input);
            FillListBoxWithDoctors(input);
        }

        private void Select_Doctor_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxDoctors.SelectedItem != null)
            {
                string selectedString = listBoxDoctors.SelectedItem.ToString().Split(" ")[0];
                Core.Utilities.Doctor.Model.Doctor selectedDoctor = DoctorService.FindByJmbg(selectedString);
                doctorJmbg.SelectedItem = selectedDoctor.Jmbg + " " + selectedDoctor.Name + " " + selectedDoctor.LastName;
                doctorJmbg.Items.Refresh();
                MessageBox.Show("Successfuly selected doctor!");
            }
        }

        private void Sort_Score_Click(object sender, RoutedEventArgs e)
        {
            List<Core.Utilities.Doctor.Model.Doctor> input = FillDoctorsFromListBox();
            input = DoctorService.SortDoctorsByAverageScore(input);
            FillListBoxWithDoctors(input);
        }

        private void Save_Settings_Click(object sender, RoutedEventArgs e)
        {
            Core.Utilities.Patient.Model.Patient patient = PatientService.FindByJmbg(JmbgOfLogged);
            if(TimeSpan.TryParseExact(textBoxDrugNot.Text,@"hh\:mm",null,out patient.NotificationTime))
            {
                 MessageBox.Show("Successfuly changed settings!");
                PatientService.Save();
            }
            else
            {
                MessageBox.Show("Wrong time input!");
            }
        }
        public bool ValidFieldsInputTab4()
        {
            string timeString = textTimeNot.Text;
            DateTime time;
            if (textMessage.Text=="")
            {
                MessageBox.Show("Message can't be empty!");
                return false;
            }
            else if (dateNot.SelectedDate == null || dateNot.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Date is not selected or isn't in the future!");
                return false;
            }

            else if (!DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                MessageBox.Show("Wrong time input!");
                return false;
            }
            else if (DateTime.Now.Date == dateNot.SelectedDate.Value.Date && time.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                MessageBox.Show("Selected today's date but time is in the past!");
                return false;
            }
            return true;
        }
        private void Create_Custom_Notification_Click(object sender, RoutedEventArgs e)
        {
            if (ValidFieldsInputTab4())
            {
                DateTime date = dateNot.SelectedDate.Value;
                string timeString = textTimeNot.Text;
                DateTime time = DateTime.ParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture);
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                NotificationService notificationService = new NotificationService();
                notificationService.CreateNotification(JmbgOfLogged, textMessage.Text, dateTime);
                MessageBox.Show("Successfuly created notification!");
            }
        }
    }
}
