using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model;
using ZdravoCorp.Core.PhysicalAsets.Inventory;
using ZdravoCorp.Core.PhysicalAsets.Renovations;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Patient;

namespace ZdravoCorp.Gui.Scheduling.View
{
    public partial class ScheduleAppointmentWindow : Window
    {
        public Core.Utilities.Doctor.Model.Doctor Doctor;
        public DoctorService DoctorService;
        public PatientService PatientService;
        public ScheduleService ScheduleService;
        public RoomService RoomService;
        public RenovationSchedule RenovationSchedule;
        static string _appointmentId = "";

        public ScheduleAppointmentWindow(Core.Utilities.Doctor.Model.Doctor doctor,RenovationSchedule renovationSchedule)
        {
            Doctor = doctor;
            PatientService = new PatientService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            RoomService = new RoomService();
            RenovationSchedule = renovationSchedule;
            Title = "Schedule appointment";
            InitializeComponent();
            InitializePatients();
            InitializeAppointmentTypes();
        }

        public ScheduleAppointmentWindow(Core.Utilities.Doctor.Model.Doctor doctor,Appointment appointment,RenovationSchedule renovationSchedule)
        {
            Doctor = doctor;
            PatientService = new PatientService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            RoomService = new RoomService();
            RenovationSchedule = renovationSchedule;
            Title = "Edit appointment";
            InitializeComponent();
            InitializePatients();
            InitializeAppointmentTypes();
            InitializeAppointmentStatuses();
            InitializeAppointmentForEdit(appointment);
            InitializeWindowForEdit();
        }

        public ScheduleAppointmentWindow(DoctorReferral doctorReferral)
        {
            PatientService = new PatientService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            RoomService = new RoomService();
            Doctor = DoctorService.FindByJmbg(doctorReferral.DoctorJmbg);
            InventoryService inventory = new InventoryService();
            ScheduleService scheduleService = new ScheduleService();
            RenovationSchedule = new RenovationSchedule(inventory, scheduleService);
            InitializeComponent();
            InitializeAppointmentTypes();
            SetReferredPatient(doctorReferral.PatientJmbg);
        }

        public ScheduleAppointmentWindow(string patientsJmbg, string doctorJmbg)
        {
            PatientService = new PatientService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            RoomService = new RoomService();
            Doctor = DoctorService.FindByJmbg(doctorJmbg);
            InventoryService inventory = new InventoryService();
            ScheduleService scheduleService = new ScheduleService();
            RenovationSchedule = new RenovationSchedule(inventory, scheduleService);
            InitializeComponent();
            InitializeAppointmentTypes();
            SetReferredPatient(patientsJmbg);
        }


        public void InitializePatients()
        {
            foreach (var patient in PatientService.AllPatients())
            {
                patientJmbg.Items.Add(patient.Jmbg);
            }
        }

        public void SetReferredPatient(string patientsJmbg)
        {
            patientJmbg.Items.Add(patientsJmbg);
            patientJmbg.SelectedIndex = 0;
        }

        public void InitializeAppointmentTypes()
        {
            foreach (AppointmentType type in Enum.GetValues(typeof(AppointmentType)))
            {
                appointmentTypeBox.Items.Add(type);
            }
            appointmentTypeBox.SelectedValue = AppointmentType.EXAMINATION;
        }

        public void InitializeAppointmentStatuses()
        {
            foreach (AppointmentStatus status in Enum.GetValues(typeof(AppointmentStatus)))
            {
                appointmentStatus.Items.Add(status);
            }

        }

        public void InitializeAppointmentForEdit(Appointment appointment)
        {
            patientJmbg.Text = appointment.JmbgPatient;
            appointmentTypeBox.SelectedValue = appointment.Type;
            appDate.SelectedDate = appointment.TimeSlot.Start.Date;
            appointmentTime.Text = appointment.TimeSlot.Start.ToString("HH:mm");
            durationBox.Text = (appointment.TimeSlot.End - appointment.TimeSlot.Start).TotalMinutes.ToString();
            appointmentId.Text = appointment.Id;
            appointmentStatus.SelectedValue = appointment.Status;
            _appointmentId = appointment.Id;

        }

        public void InitializeWindowForEdit()
        {
            editWindow.Height = 530;
            patientJmbg.IsReadOnly = true;
            scheduleAppBtn.Visibility = Visibility.Hidden;
            editAppointmentBtn.Visibility = Visibility.Visible;
            cancelEditBtn.Visibility = Visibility.Visible;
            appIdLabel.Visibility = Visibility.Visible;
            appointmentId.Visibility = Visibility.Visible;
            appStatusLabel.Visibility = Visibility.Visible;
            appointmentStatus.Visibility = Visibility.Visible;


        }

        public bool EditAppointment(Appointment appointment)
        {
            if (ValidateAppointmentForm() && ValidateAppointmentId())
            {
                ScheduleService.ScheduleRepository.Appointments.Add(appointment);
                TimeSlot appointmentDuration = new TimeSlot(appDate.SelectedDate.Value, appointmentTime.Text, durationBox.Text);
                Room room = RoomService.FindFirstAvailableRoom(GetRoomTypeByAppointmentType(),
                    ScheduleService.Appointments(),
                    appointmentDuration);

                ScheduleService.EditAppointment(_appointmentId, appointmentId.Text, patientJmbg.Text,
                    appointmentDuration, (AppointmentType)appointmentTypeBox.SelectedValue, (AppointmentStatus)appointmentStatus.SelectedValue,room.Id);

                MessageBox.Show("Successfuly edited appointment!");
                Close();
                return true;
            }

            return false;
        }

        public void LimitAppointmentDuration()
        {
            AppointmentType selectedValue = (AppointmentType)appointmentTypeBox.SelectedValue;

            if (selectedValue == AppointmentType.EXAMINATION)
            {
                durationBox.Text = 15.ToString();
                durationBox.IsEnabled = false;
            }
            else
            {
                durationBox.IsEnabled = true;
            }
        }


        public bool ValidateAppointmentForm()
        {
            if (!ValidatePatientJmbg() || !ValidateAppointmentDateTime() || !ValidateDuration() || !ValidateAvailability())
            {
                return false;
            }
            return true;
        }


        public RoomType GetRoomTypeByAppointmentType()
        {
            AppointmentType appointmentType = (AppointmentType)appointmentTypeBox.SelectedValue;
            if (appointmentType == AppointmentType.EXAMINATION)
            {
                return RoomType.ExaminationRoom;
            }
            else
            {
                return RoomType.OperationRoom;
            }
        }

        public bool ValidateAppointmentId()
        {
            if (ScheduleService.FindAppointmentById(appointmentId.Text) == null && ValidateInputId())
            {
                return true;
            }

            MessageBox.Show("Appointment ID already exists or is invalid!");
            return false;
        }

        public bool ValidateInputId()
        {
            try
            {
                int id = int.Parse(this.appointmentId.Text);
                if (id < 1 || id > 100000)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool ValidateAvailability()
        {
            TimeSlot appointmentDuration = new TimeSlot(appDate.SelectedDate.Value, appointmentTime.Text, durationBox.Text);

            if (!(ScheduleService.IsAvailable(Doctor, appointmentDuration)))
            {
                MessageBox.Show("Doctor is not available at that time! ");
                return false;
            }

            if (!(ScheduleService.IsAvailable(PatientService.FindByJmbg(patientJmbg.Text), appointmentDuration)))
            {
                MessageBox.Show("Patient is not available at that time! ");
                return false;
            }

            Room? room = RoomService.FindFirstAvailableRoom(GetRoomTypeByAppointmentType(),
                ScheduleService.Appointments(), appointmentDuration);
            if (room == null || !RenovationSchedule.IsAvailable(room.Id,appointmentDuration))
            {
                MessageBox.Show("There is no available rooms at that time! ");
                return false;
            }

            return true;
        }


        public bool ValidateDuration()
        {
            if (!(durationBox.Text.All(char.IsDigit)) || (int.Parse(durationBox.Text) < 15))
            {
                MessageBox.Show("Please insert correct duration time! ");
                return false;
            }

            return true;
        }

        public bool ValidatePatientJmbg()
        {

            if (string.IsNullOrEmpty(patientJmbg.Text))
            {
                MessageBox.Show("Please choose correct patient JMBG! ");
                return false;
            }

            return true;
        }

        public bool ValidateAppointmentDateTime()
        {
            try
            {
                TimeSlot appointmentDuration = new TimeSlot(appDate.SelectedDate.Value, appointmentTime.Text, durationBox.Text);

                if (appDate.SelectedDate.Value < DateTime.Today)
                {
                    MessageBox.Show("Please insert correct appointment date/time!");
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Please insert correct appointment date/time!");
                return false;
            }

            return true;
        }

        private void appointmentTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LimitAppointmentDuration();
        }

        private void scheduleAppBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAppointmentForm())
            {
                TimeSlot appointmentDuration = new TimeSlot(appDate.SelectedDate.Value, appointmentTime.Text, durationBox.Text);
                Room room = RoomService.FindFirstAvailableRoom(GetRoomTypeByAppointmentType(),
                    ScheduleService.Appointments(),
                    appointmentDuration);

                ScheduleService.CreateAppointment(Doctor.Jmbg, patientJmbg.Text, appointmentDuration, (AppointmentType)appointmentTypeBox.SelectedValue,room.Id);
                

                MessageBox.Show("Successfuly scheduled appointment!");
                Close();

            }

        }

        private void cancelEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = ScheduleService.FindAppointmentById(_appointmentId);
            ScheduleService.ScheduleRepository.Appointments.Remove(appointment);
            
            if(!EditAppointment(appointment)) { ScheduleService.ScheduleRepository.Appointments.Add(appointment); }

        }
    }
}
