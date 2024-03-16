using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.ChatSupport;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs;
using ZdravoCorp.Core.PhysicalAsets.Renovations;
using ZdravoCorp.Core.PhysicalAsets.Room;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Repository;
using ZdravoCorp.Gui.ChatSupport.View;
using ZdravoCorp.Gui.Nurse;
using ZdravoCorp.Gui.Nurse.View;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.View;
using ZdravoCorp.Gui.Scheduling.View;
using ZdravoCorp.Gui.VacationRequest.View;
using Validation = ZdravoCorp.Core.Utilities.Validation;


namespace ZdravoCorp.Gui.Doctor.View
{
    public partial class DoctorWindow : Window
    {
        public Core.Utilities.Doctor.Model.Doctor Doctor;
        public DoctorService DoctorService;
        public PatientRepository Patients;
        public PatientService PatientService;
        public ScheduleService ScheduleService;
        public DrugService DrugService;
        public List<string> DrugIngredients;
        public RoomService RoomService;
        public RenovationSchedule RenovationSchedule;
        public ChatService ChatService;
        public NurseService NurseService;

        public DoctorWindow(Core.Utilities.Doctor.Model.Doctor doctor,RenovationSchedule renovationSchedule)
        {
            Doctor = doctor;
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            PatientService = new PatientService();
            DrugService = new DrugService();
            DrugIngredients = new List<string>();
            RoomService = new RoomService();
            ChatService = new ChatService();
            NurseService=new NurseService();
            RenovationSchedule = renovationSchedule;
            InitializeComponent();
            InitializeVacationRequestView();
            InitializePatientVisitationView();
            InitializePatientsTable();
            InitializeDrugList();
            InitializeChatSupportView();
        }

        private void doctorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        public void InitializeChatSupportView()
        {
            ChatSupportView chatSupportView = new ChatSupportView(Doctor);
            chatSupportGrid.Children.Add(chatSupportView);
            chatSupportTab.Content = chatSupportGrid;
        }
        public void InitializeVacationRequestView()
        {
            VacationRequestView vacationRequestView = new VacationRequestView(Doctor);
            vacationRequestGrid.Children.Add(vacationRequestView);
            vacationRequestTab.Content = vacationRequestGrid;
        }

        public void InitializePatientVisitationView()
        {
            PatientVisitationView patientVisitationView = new PatientVisitationView();
            patientVisitationGrid.Children.Add(patientVisitationView);
            patientVisitationTab.Content = patientVisitationGrid;
        }


        public void InitalizeAppointmentsTable(List<Appointment> doctorAppointments)
        {
            DataTable dataTable = CreateAppointmentsTableHeaders();

            foreach (var appointment in doctorAppointments)
            {
                dataTable.Rows.Add(ExtractAppointmentInfo(appointment));
            }

            appGrid.ItemsSource = new DataView(dataTable);
            appGrid.Items.Refresh();
        }

        public void InitializeDrugList()
        {
            foreach (var drug in DrugService.GetAllDrugs())
            {
                drugsList.Items.Add(drug.Name);
            }
        }

        public void LoadAppointmentsTable()
        {
            ScheduleService = new ScheduleService();
            InitalizeAppointmentsTable(ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value));
        }

        public void LoadPatientsTable()
        {
            PatientService = new PatientService();
            InitializePatientsTable();
        }

        public void InitializePatientsTable()
        {
            DataTable dataTable = CreatePatientsTableHeaders();
            List <Core.Utilities.Patient.Model.Patient> patients = PatientService.AllPatients();

            foreach (var patient in patients)
            {
                dataTable.Rows.Add(ExtractPatientInfo(patient));
            }

            patientsGrid.ItemsSource = new DataView(dataTable);
            patientsGrid.Items.Refresh();
        }

        public void EditAppointment()
        {
            Appointment? appointment = ExtractAppointmentFromTable();
            if (appointment != null)
            {
                ScheduleAppointmentWindow editAppointmentWindow = new ScheduleAppointmentWindow(Doctor,appointment,RenovationSchedule);
                editAppointmentWindow.ShowDialog();
                LoadAppointmentsTable();
            }

        }

        public void CancelAppointment()
        {
            Appointment? appointment = ExtractAppointmentFromTable();
            if (appointment != null && appointment.Status != AppointmentStatus.CANCELED)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this appointment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    ScheduleService.CancelAppointment(appointment);
                    LoadAppointmentsTable();
                }
            }

        }

        public void ShowPatientMedicalCard(string table,DataGrid grid)
        {
            Core.Utilities.Patient.Model.Patient? patient = ExtractPatientFromTable(table, grid);
            if (patient != null)
            {
                MedicalRecordWindow mrw = new MedicalRecordWindow(patient);
                mrw.ShowDialog();
            }
        }

        public Appointment? ExtractAppointmentFromTable()
        {
            
            DataRowView row = appGrid.SelectedItem as DataRowView;

            if (row != null)
            {
                return ScheduleService.FindAppointmentById(row["Id"].ToString());
            }
            
        
            MessageBox.Show("Please select an appointment! ");
            return null;
        }

        public Core.Utilities.Patient.Model.Patient? ExtractPatientFromTable(string table,DataGrid dg)
        {
            DataRowView row = dg.SelectedItem as DataRowView;

            if (row != null)
            {
                return PatientService.FindByJmbg(row["Patient JMBG"].ToString());
            }

            if (table == "appointment")
            {
                MessageBox.Show("Please select an appointment! ");
            }

            return null;
        }


        public DataTable CreateAppointmentsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Patient JMBG", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Start", typeof(string));
            dt.Columns.Add("End", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Room", typeof(string));
            return dt;
        }

        public DataTable CreatePatientsTableHeaders()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Patient JMBG", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Last name", typeof(string));
            dt.Columns.Add("Birth date", typeof(string));
            return dt;
        }

        public string[] ExtractAppointmentInfo(Appointment appointment)
        {
           string[] appointmentInfo =
           {
               appointment.JmbgPatient,appointment.TimeSlot.Start.Date.ToString("dd.MM.yyyy"),appointment.TimeSlot.Start.TimeOfDay.ToString(),
               appointment.TimeSlot.End.TimeOfDay.ToString(),appointment.Type.ToString(),appointment.Status.ToString(),appointment.Id,appointment.RoomId
           };
           return appointmentInfo;
        }

        public string[] ExtractPatientInfo(Core.Utilities.Patient.Model.Patient patient)
        {
            string[] patientInfo =
            {
                patient.Jmbg,patient.Name,patient.LastName,patient.BirthDate.Date.ToString("dd.MM.yyyy")
            };
            return patientInfo;
        }

        private bool ValidateAppointments()
        {
            if (ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value) == null)
            {
                MessageBox.Show("There is no appointments for selected date! ");
            }

            return true;
        }


        private void appointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value).Count == 0)
            {
                MessageBox.Show("There is no appointments for selected date! ");
            }

            LoadAppointmentsTable();
        }


        private void scheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            ScheduleAppointmentWindow scheduleAppointment = new ScheduleAppointmentWindow(Doctor,RenovationSchedule);
            scheduleAppointment.ShowDialog();

            if (appointmentDate.SelectedDate != null)
            {
                LoadAppointmentsTable();
            }

        }

        private void editAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            EditAppointment();
        }

        private void cancelAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelAppointment();
        }

        private void medicalCardBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPatientMedicalCard("appointment",appGrid);
            PatientService = new PatientService();
        }

        private void patientSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = patientSearch.Text;
            DataView dataView = (DataView)patientsGrid.ItemsSource;
            DataTable dataTable = dataView.Table;

            DataView dv = new DataView(dataTable);

            dv.RowFilter = string.Format("[Patient JMBG] like '%{0}%' OR Name like '%{0}%' OR [Last name] like '%{0}%' OR [Birth date] like '%{0}%'", filterText);
            patientsGrid.ItemsSource = dv;

        }

        private void PatientsGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Core.Utilities.Patient.Model.Patient? selectedPatient = ExtractPatientFromTable("patient",patientsGrid);
            if (selectedPatient != null)
            {
                bool hasBeenExaminated = ScheduleService.HasPatientBeenExaminatedByDoctor(Doctor, selectedPatient);
                if (hasBeenExaminated)
                {
                    mcPatientTable.Visibility = Visibility.Visible;
                }
                else
                {
                    mcPatientTable.Visibility = Visibility.Hidden;
                }
            }
        }

        private void mcPatientTable_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPatientMedicalCard("patient",patientsGrid);
            LoadPatientsTable();
        }

        public bool IsAppointmentReadyForExamination(Appointment appointment)
        {
            if(!appointment.IsReady() || !appointment.IsScheduled())
            {
                MessageBox.Show("You can't begin examination of this appointment!");
                return false;
            }
            return true;
        }

        private void startExamination_Click(object sender, RoutedEventArgs e)
        {
            Appointment? slectedAppointment = ExtractAppointmentFromTable();
            if (slectedAppointment != null && IsAppointmentReadyForExamination(slectedAppointment))
            {
                ExaminationWindow ew = new ExaminationWindow(slectedAppointment);
                ew.ShowDialog();
                LoadAppointmentsTable();
            }
        }

        public bool IsDrugFormValid()
        {
            if (!IsDrugNameValid(drugName.Text) || DrugIngredients.Count == 0 || !DoesDrugExist() || !IsDrugQuantityValid(drugQuantity.Text)
                || !IsNumberOfPillsValid(numOfPills.Text))
            {
                return false;
            }
            return true;
        }

        public bool DoesDrugExist()
        {
            if (DrugService.FindByName(drugName.Text) != null)
            {
                MessageBox.Show("Drug name already exits! ");
                return false;
            }
            return true;
        }


        public bool IsDrugNameValid(string text)
        {
            if (!Validation.IsStringAWord(text))
            {
                MessageBox.Show("Please insert valid drug name.");
                return false;
            }
            return true;
        }

        public bool IsDrugQuantityValid(string text)
        {
            if (!Validation.IsStringAPositiveNumber(text))
            {
                MessageBox.Show("Please insert valid drug quanity.");
                return false;
            }
            return true;
        }

        public bool IsNumberOfPillsValid(string text)
        {
            if (!Validation.IsStringAPositiveNumber(text))
            {
                MessageBox.Show("Please insert valid number of pills.");
                return false;
            }
            return true;
        }

        public void CreateDrug()
        {
            DrugService.CreateDrug(drugName.Text,DrugIngredients,int.Parse(drugQuantity.Text),int.Parse(numOfPills.Text));
            drugsList.Items.Add(drugName.Text);
            DrugService = new DrugService();
        }

        private void addIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.IsStringAWord(ingredient.Text))
            {
                DrugIngredients.Add(ingredient.Text);
                ingredientsList.Items.Add(ingredient.Text);
                ingredient.Text = "";
            }
        }

        public void ClearDrugFormFields()
        {
            drugName.Text = "";
            DrugIngredients.Clear();
            ingredientsList.Items.Clear();
            drugQuantity.Text = "";
            numOfPills.Text = "";
        }

        private void createDrug_Click(object sender, RoutedEventArgs e)
        {
            if (IsDrugFormValid())
            {
                CreateDrug();
                MessageBox.Show("Successfully created new drug! ");
            }
            ClearDrugFormFields();
        }

        private void DrugsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            drugIngredientsList.Items.Clear();
            foreach (var ingredient in DrugService.FindByName(drugsList.SelectedValue.ToString()).Ingredients)
            {
                drugIngredientsList.Items.Add(ingredient);
            }
        }

        
      
    }
}
