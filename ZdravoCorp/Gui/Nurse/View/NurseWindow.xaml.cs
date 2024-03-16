using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZdravoCorp.Core.ChatSupport;
using ZdravoCorp.Core.Notifications;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.Referrals.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.Model;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Prescriptions.Model;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Scheduling.Schedule.Model;
using ZdravoCorp.Core.Scheduling.UrgentSchedule;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Model;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Repository;
using ZdravoCorp.Gui.ChatSupport.View;
using ZdravoCorp.Gui.Scheduling.View;

namespace ZdravoCorp.Gui.Nurse.View
{
    using static Core.Utilities.Doctor.Model.Doctor;
    using static System.Net.Mime.MediaTypeNames;

    public partial class NurseWindow : Window
    {

        public Core.Utilities.Nurse.Model.Nurse Nurse;
        public PatientService PatientService;
        public NurseService NurseService;
        public ScheduleService ScheduleService;
        public DoctorReferralService DoctorReferralService;
        public PrescriptionService PrescriptionService;
        public DrugService DrugService;
        public DrugOrderService DrugOrderService;
        public UrgentScheduleService UrgentScheduleService;
        public ChatService ChatService;
        public DoctorService DoctorService;

        public NurseWindow(Core.Utilities.Nurse.Model.Nurse nurse)
        {
            //nurse = nurse_;
            Nurse = nurse;
            PatientService = new PatientService();
            NurseService = new NurseService();
            ScheduleService = new ScheduleService();
            DoctorReferralService = new DoctorReferralService();
            PrescriptionService = new PrescriptionService();
            DrugService = new DrugService();  
            DrugOrderService = new DrugOrderService();
            UrgentScheduleService = new UrgentScheduleService();
            ChatService = new ChatService();
            DoctorService = new DoctorService();
            InitializeComponent();
            LoadPatientsDataGrid();
            LoadAppointmentsDataGrid();
            LoadUrgentIntakeComboBoxes();
            InitializeChatSupportView();
        }
        public void InitializeChatSupportView()
        {
            ChatSupportView chatSupportView = new ChatSupportView(Nurse);
            chatSupportGrid.Children.Add(chatSupportView);
            chatSupportTab.Content = chatSupportGrid;
        }
        public void LoadUrgentIntakePatientsComboBox()
        {
            foreach (var patient in PatientService.AllPatients())
            {
                UrgentIntakePatientJmbgComboBox.Items.Add(patient.Jmbg);
            }
        }
        public void LoadUrgentIntakeSpecializationComboBox()
        {
            UrgentIntakeDoctorSpecializationComboBox.Items.Add("General");
            UrgentIntakeDoctorSpecializationComboBox.Items.Add("Cardiologist");
            UrgentIntakeDoctorSpecializationComboBox.Items.Add("Dermatologist");
            UrgentIntakeDoctorSpecializationComboBox.Items.Add("Neurologist");
            UrgentIntakeDoctorSpecializationComboBox.Items.Add("Surgeon");
        }
        public void LoadUrgentIntakeComboBoxes()
        {
            LoadUrgentIntakePatientsComboBox();
            LoadUrgentIntakeSpecializationComboBox();
        }


        public DataTable CreatePatientsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Jmbg");
            dt.Columns.Add("Password");
            dt.Columns.Add("Name");
            dt.Columns.Add("Last name");
            dt.Columns.Add("Birth date");

            return dt;
        }

        public string[] ExtractPatientInfo(Core.Utilities.Patient.Model.Patient patient)
        {
            string[] patientInfo =
            {
                patient.Jmbg,
                patient.Password,
                patient.Name,
                patient.LastName,
                patient.BirthDate.ToString("dd.MM.yyyy")
            };

            return patientInfo;
        }

        public void InitializePatientsDataGrid(List<Core.Utilities.Patient.Model.Patient> patients)
        {
            DataTable dt = CreatePatientsTableHeaders();

            foreach (Core.Utilities.Patient.Model.Patient patient in patients) 
            {
                dt.Rows.Add(ExtractPatientInfo(patient));
            }

            patientsDataGrid.ItemsSource = new DataView(dt);
            patientsDataGrid.Items.Refresh();
        }

        public void LoadPatientsDataGrid()
        {
            InitializePatientsDataGrid(PatientService.AllPatients());
        }

        private void patientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!NurseService.ValidateNewPatient(
                jmbgTxt.Text, nameTxt.Text, lastNameTxt.Text,
                passwordTxt.Text, weightTxt.Text, heightTxt.Text, birthDatePicker.SelectedDate))
            {
                ErrorMessage("Wrong input");
                ClearTxtFields();
            }
            else
            {
                Core.Utilities.Patient.Model.Patient newPatient = NurseService.CreatePatient(
                    jmbgTxt.Text, nameTxt.Text, lastNameTxt.Text,
                    passwordTxt.Text, birthDatePicker.SelectedDate.Value, weightTxt.Text, heightTxt.Text);

                PatientService.PatientRepository.Patients.Add(newPatient);
                PatientService.Save();
                LoadPatientsDataGrid();

                SuccesMessage("Succesfully added new patient");
                ClearTxtFields();

            }
        }

        public void DeletePatient(string jmbg)
        {
            PatientService.PatientRepository.Patients.Remove(PatientService.FindByJmbg(jmbg));
            PatientService.Save();
            LoadPatientsDataGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(patientsDataGrid.SelectedItem == null)
            {
                ErrorMessage("You must select patient to delete.");
            }
            else
            {
                DataRowView row = patientsDataGrid.SelectedItem as DataRowView;
                DeletePatient(row["Jmbg"].ToString());
                SuccesMessage("Succesfully deleted patient.");
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = patientsDataGrid.SelectedItem as DataRowView;
            
            if(row == null)
            {
                ErrorMessage("You must select patient to update.");
            }
            else
            {
                PatientUpdateWindow patientUpdateWindow = new PatientUpdateWindow(row["Jmbg"].ToString());
                patientUpdateWindow.ShowDialog();
                LoadPatientsDataGrid();
            }
        }
        private void NurseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        public void LoadAppointmentsDataGrid()
        {
            InitializeAppointmentsDataGrid(ScheduleService.Appointments());
        }
        public void InitializeAppointmentsDataGrid(List<Appointment> appointments)
        {
            DataTable dt = CreateAppointmentsTableHeaders();

            foreach (Appointment appointment in appointments)
            {
                if(appointment.Status == AppointmentStatus.SCHEDULED)
                {
                    dt.Rows.Add(ExtractAppointmentInfo(appointment));
                }
            }

            appointmentsDataGrid.ItemsSource = new DataView(dt);
            appointmentsDataGrid.Items.Refresh();
        }
        public string[] ExtractAppointmentInfo(Appointment appointment)
        {
            string[] patientInfo =
            {
                appointment.Id,
                appointment.JmbgPatient,
                appointment.JmbgDoctor,
                appointment.Type == AppointmentType.OPERATION ? "operation" : "examination",
                appointment.TimeSlot.Start.ToString()
            };

            return patientInfo;
        }
        public DataTable CreateAppointmentsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("patient jmbg");
            dt.Columns.Add("doctor jmbg");
            dt.Columns.Add("type");
            dt.Columns.Add("start time");

            return dt;
        }

        public void OpenAnamnesisCreationWindow(DataRowView row)
        {
            string doctorJmbg = row["doctor jmbg"].ToString();
            string patientJmbg = row["patient jmbg"].ToString();
            string appointmentId = row["Id"].ToString();

            AnamnesisCreationWindow anamnesisCreationWindow = new AnamnesisCreationWindow(doctorJmbg, patientJmbg, appointmentId);
            anamnesisCreationWindow.ShowDialog();
        }
        public void SetAppointmentReady(string id)
        {
            ScheduleService.ReadyForAppointment(ScheduleService.FindAppointmentById(id));
            SuccesMessage("Appointment is ready!");
        }
        private void intakeAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(appointmentsDataGrid.SelectedItem == null) 
            {
                ErrorMessage("You must select appointment to be accepted.");    
            }
            else
            {
                DataRowView row = appointmentsDataGrid.SelectedItem as DataRowView;
                if (!ScheduleService.IsAppointmentInNext15Minutes(ScheduleService.FindAppointmentById(row["Id"].ToString())))
                {
                    ErrorMessage("We cant intake appointment that doesnt start within next 15 minutes.");
                }
                else
                {
                    if (PatientService.FindByJmbg(row["patient jmbg"].ToString()) == null)
                    {
                        CreatePatientWindow createPatientWindow = new CreatePatientWindow(row["patient jmbg"].ToString());
                        createPatientWindow.ShowDialog();
                    }
                    OpenAnamnesisCreationWindow(row);
                    SetAppointmentReady(row["Id"].ToString());
                    LoadAppointmentsDataGrid();
                }
                    
            }

        }

        public bool ValidateUrgentAppointmentPatientJmbg()
        {
            if (string.IsNullOrEmpty(UrgentIntakePatientJmbgComboBox.Text))
            {
                ErrorMessage("Please choose correct patient JMBG! ");
                return false;
            }

            return true;
        }
        public bool ValidateUrgentAppointmentDoctorSpecialization()
        {
            if (string.IsNullOrEmpty(UrgentIntakeDoctorSpecializationComboBox.Text))
            {
                ErrorMessage("Please choose correct doctors specialization! ");
                return false;
            }

            return true;
        }

        public bool ValidateUrgentAppointmentDuration()
        {
            string durationText = UrgentIntakeDurationTextBox.Text;

            if (string.IsNullOrEmpty(durationText) || !(durationText.All(char.IsDigit)) || (int.Parse(durationText) < 15))
            {
                ErrorMessage("Please insert a valid duration time!");
                return false;
            }

            if (UrgentIntakeExaminationRadioButton.IsChecked == true && int.Parse(durationText) != 15)
            {
                ErrorMessage("The duration of an examination must be 15 minutes!");
                return false;
            }

            return true;
        }

        public bool ValidateUrgentAppointmentOperationExamination()
        {
            if(!(UrgentIntakeOperationRadioButton.IsChecked == true || UrgentIntakeExaminationRadioButton.IsChecked == true))
            {
                ErrorMessage("Please select examination or operation! ");
                return false;
            }
            return true;
        }

        public bool ValidateUrgentAppointmentInputs()
        {
            bool check = true;
            check = check && ValidateUrgentAppointmentDoctorSpecialization();
            check = check && ValidateUrgentAppointmentPatientJmbg();
            check = check && ValidateUrgentAppointmentDuration();
            check = check && ValidateUrgentAppointmentOperationExamination();
            
            return check;
        }
        public bool IsSelectedAdjournmentTerm()
        {
            return PostponeAppointmentsDataGrid.SelectedItems.Count > 0;
        }
        public int GetSpecializationFromComboBox()
        {
            switch (UrgentIntakeDoctorSpecializationComboBox.Text)
            {
                case "General": return 0;
                case "Cardiologist": return 1;
                case "Dermatologist": return 2;
                case "Neurologist": return 3;
                default: return 4;
            }
        }
        public bool FindFreeSlotForUrgentAppointment()
        {
            int appType = UrgentIntakeExaminationRadioButton.IsChecked == true ? 1 : 0;

            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                (DateTime.Now).AddMinutes(1).Minute, 0);

            TimeSlot ts = UrgentScheduleService.FindSlotForUrgentAppointmentWithin2Hours((Core.Utilities.Doctor.Model.Doctor.Specialty)GetSpecializationFromComboBox(), UrgentIntakePatientJmbgComboBox.Text,
                (AppointmentType)appType, startTime, int.Parse(UrgentIntakeDurationTextBox.Text));
           
            return ts != null;
        }
        public DataTable CreatePostponeAppointmentsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("jmbg doctor");
            dt.Columns.Add("jmbg patient");
            dt.Columns.Add("room id");
            dt.Columns.Add("start");

            return dt;
        }
        public string[] ExtractUrgentAppointmentInfo(Appointment app)
        {
            string[] appointmentInfo =
            {
                app.Id,
                app.JmbgDoctor,
                app.JmbgPatient,
                app.RoomId,
                app.TimeSlot.Start.ToString()
            };

            return appointmentInfo;
        }
        private List<KeyValuePair<Appointment, int>> GetAppointmentsToPostpone()
        {
            PatientRepository patientRepository = new PatientRepository();
            return UrgentScheduleService.FindAppointmentsToPostpone(
                (Core.Utilities.Doctor.Model.Doctor.Specialty)GetSpecializationFromComboBox(),
                int.Parse(UrgentIntakeDurationTextBox.Text), patientRepository.FindByJmbg(UrgentIntakePatientJmbgComboBox.SelectedItem.ToString()));
        }
        public void FillUrgentAppointmentDataTable(DataTable dataTable, List<KeyValuePair<Appointment, int>> appointments)
        {
            foreach (var appointmentMinutesPair in Enumerable.Reverse(appointments))
            {
                dataTable.Rows.Add(ExtractUrgentAppointmentInfo(appointmentMinutesPair.Key));
            }
        }
        public void RefreshPosponeAppointmentsDataGrid(DataTable dt)
        {
            PostponeAppointmentsDataGrid.ItemsSource = new DataView(dt);
            PostponeAppointmentsDataGrid.Items.Refresh();
        }
        public void InitializeUrgentAppointmentDataGrid()
        {
            List<KeyValuePair<Appointment, int>> appointmentsToPostpone = GetAppointmentsToPostpone();

            DataTable dt = CreatePostponeAppointmentsTableHeaders();

            if (appointmentsToPostpone.Count == 0) 
            {
                ErrorMessage("Theres no available appointments to postpone.");
            }

            FillUrgentAppointmentDataTable(dt, appointmentsToPostpone);

            RefreshPosponeAppointmentsDataGrid(dt);
        }
        public void LoadUrgentAppointmentDataGrid()
        {
            InitializeUrgentAppointmentDataGrid();
        }
        public void CreatePostponeNotification(string patientJmbg, string doctorJmbg, string appointmentId)
        {
            NotificationService notificationService = new NotificationService();
            string message = "Your appointment " + appointmentId + " has been postponed.";
            notificationService.CreateNotification(doctorJmbg, message,DateTime.Now.AddHours(1));
            notificationService.CreateNotification(patientJmbg, message,DateTime.Now.AddHours(1));
        }
        public int GetSelectedAppointmentType()
        {
            return UrgentIntakeExaminationRadioButton.IsChecked == true ? 1 : 0;
        }
        public TimeSlot CalculateNewAppointmentTimeSlot(Appointment appointment)
        {
            return new TimeSlot(appointment.TimeSlot.Start,
                appointment.TimeSlot.Start.AddMinutes(int.Parse(UrgentIntakeDurationTextBox.Text)));
        }
        public void ClearPosponeAppointmentDataGrid()
        {
            PostponeAppointmentsDataGrid.ItemsSource = null;
        }
        public void CreateUrgentAppointment()
        {
            DataRowView row = PostponeAppointmentsDataGrid.SelectedItem as DataRowView;
            Appointment appointmentToPostpone = ScheduleService.FindAppointmentById(row["ID"].ToString());

            int appType = GetSelectedAppointmentType();
            TimeSlot newTs = CalculateNewAppointmentTimeSlot(appointmentToPostpone);

            ScheduleService.CreateAppointment(appointmentToPostpone.JmbgDoctor, UrgentIntakePatientJmbgComboBox.Text,
                newTs, (AppointmentType)appType, appointmentToPostpone.RoomId);

            UrgentScheduleService.PostponeAppointment(appointmentToPostpone);

            SuccesMessage("Succesfully created urgent appointment.");
            LoadAppointmentsDataGrid();
            ClearPosponeAppointmentDataGrid();
            CreatePostponeNotification(appointmentToPostpone.JmbgPatient, appointmentToPostpone.JmbgDoctor, appointmentToPostpone.Id);
        }
        private void AcceptUrgentAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidateUrgentAppointmentInputs()) 
            {
                return;
            }
            if (IsSelectedAdjournmentTerm())
            {
                CreateUrgentAppointment();
            }
            else
            {
                if(FindFreeSlotForUrgentAppointment())
                {
                    SuccesMessage("Succesfully created urgent appointment.");
                    LoadAppointmentsDataGrid();
                }
                else
                {
                    ErrorMessage("Theres no free doctors for appointments, please select appointment to postpone.");
                    LoadUrgentAppointmentDataGrid();
                }
                
            }
        }
        public DataTable CreateReferralsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("patient jmbg");
            dt.Columns.Add("doctor jmbg");

            return dt;
        }
        public string[] ExtractReferralsInfo(DoctorReferral referral)
        {
            string[] referralInfo =
            {
                referral.Id,
                referral.PatientJmbg,
                referral.DoctorJmbg,
            };

            return referralInfo;
        }
        public void InitializeReferralsDataGrid(List<DoctorReferral> referrals)
        {
            DataTable dt = CreateReferralsTableHeaders();

            foreach (DoctorReferral referral in referrals)
            {
                if (!referral.IsUsed)
                {
                    dt.Rows.Add(ExtractReferralsInfo(referral));
                }
            }

            ReferralsDataGrid.ItemsSource = new DataView(dt);
            ReferralsDataGrid.Items.Refresh();
        }
        public void LoadReferralsDataGrid()
        {
            InitializeReferralsDataGrid(DoctorReferralService.FindDoctorReferralsByPatientJmbg(patientReferralTxt.Text));
        }
        private void FindReferralsBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadReferralsDataGrid();
        }
        public void ClearReferralFields()
        {
            ReferralsDataGrid.ItemsSource = null;
            ReferralsDataGrid.Items.Clear();
            patientReferralTxt.Text = "";
        }
        public void ScheduleUsedReferral(DataRowView row)
        {
            DoctorReferral referral = DoctorReferralService.FindDoctorReferralById(row["Id"].ToString());
            ScheduleAppointmentWindow scheduleAppointmentWindow = new ScheduleAppointmentWindow(referral);
            scheduleAppointmentWindow.ShowDialog();
            DoctorReferralService.UseDoctorReferral(referral);
        }
        private void ScheduleRefferalBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = ReferralsDataGrid.SelectedItem as DataRowView;

            if (row == null)
            {
                ErrorMessage("You must select patient to update.");
            }
            else
            {
                ScheduleUsedReferral(row);
                ClearReferralFields();
            }
            
        }

        public DataTable CreatePrescriptionTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("Patient jmbg");
            dt.Columns.Add("Drug");
            dt.Columns.Add("Start");
            dt.Columns.Add("End");
            dt.Columns.Add("Daily dose");

            return dt;
        }
        public string[] ExtractPrescriptionInfo(Prescription prescription)
        {
            string[] prescriptionInfo =
            {
                prescription.Id,
                prescription.PatientJmbg,
                prescription.Drug,
                prescription.Start.ToString(),
                prescription.End.ToString(),
                prescription.DailyDose.ToString()
            };

            return prescriptionInfo;
        }
        public void InitializePrescriptionDataGrid(List<Prescription> prescriptions)
        {
            DataTable dt = CreatePrescriptionTableHeaders();
            foreach (var prescription in from Prescription prescription in prescriptions
                                         where !prescription.IsUsed
                                         select prescription)
            {
                dt.Rows.Add(ExtractPrescriptionInfo(prescription));
            }

            prescriptionsDataGrid.ItemsSource = new DataView(dt);
            prescriptionsDataGrid.Items.Refresh();
        }
        public void LoadPrescriptionDataGrid()
        {
            InitializePrescriptionDataGrid(PrescriptionService.FindPrescriptionByJmbg(patientPrescriptionTxt.Text));
        }

        private void findPatientsPrescriptionsBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadPrescriptionDataGrid();
        }
        public int CalculateQuantityPerPrescription(Prescription prescription)
        {
            Drug drug = DrugService.FindByName(prescription.Drug);
            int amountNeeded = (prescription.End - prescription.Start).Days * prescription.DailyDose;
            double packagesNeeded = (double)amountNeeded / drug.NumberOfPills;
            return (int)Math.Ceiling(packagesNeeded);
        }

        public void UseSelectedPrescription(Prescription prescription, Core.Utilities.Patient.Model.Patient patient)
        {
            if (!prescription.ValidatePrescriptionUse(DrugService.FindByName(prescription.Drug)))
            {
                ErrorMessage("We are not able to give you these drugs");
            }
            else if (!PrescriptionService.IsPatientDoneWithLastDose(patient.Jmbg, prescription.Drug))
            {
                ErrorMessage("You need to finish your last dose before getting new one.");
            }
            else
            {
                DrugService.GiveDrug(prescription.Drug, CalculateQuantityPerPrescription(prescription));
                prescription.IsUsed = true;
                PrescriptionService.Save();
            }
        }

        public void IsPatientFeelingGood(Prescription prescription)
        {
            if (MessageBox.Show("Are you feeling good?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
            {
                ScheduleAppointmentWindow scheduleAppointmentWindow = new ScheduleAppointmentWindow(prescription.PatientJmbg, prescription.DoctorJmbg);
                scheduleAppointmentWindow.ShowDialog();
            }
        }
        public void ClearPrescriptionFields()
        {
            prescriptionsDataGrid.ItemsSource = null;
            prescriptionsDataGrid.Items.Clear();
            patientPrescriptionTxt.Text = "";
        }
        private void usePrescription_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = prescriptionsDataGrid.SelectedItem as DataRowView;

            if (row == null)
            {
                ErrorMessage("You must select prescription to update.");
            }
            else
            {
                UseSelectedPrescription(PrescriptionService.FindPrescriptionById(row["Id"].ToString()), PatientService.FindByJmbg(row["Patient jmbg"].ToString()));
                IsPatientFeelingGood(PrescriptionService.FindPrescriptionById(row["Id"].ToString()));
                ClearPrescriptionFields();
            }
        }

        public DataTable CreateMedicineStorageTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Quantity");

            return dt;
        }
        public string[] ExtractMedicineStorageInfo(Drug drug)
        {
            string[] drugInfo =
            {
                drug.Name,
                drug.Quantity.ToString(),
            };

            return drugInfo;
        }
        private void medicineStorageDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;

            if (rowView == null)
            {
                return;
            }
            DataRow row = rowView.Row;
            int quantity = Convert.ToInt32(row["Quantity"]);
            if (quantity == 0)
            {
                e.Row.Background = Brushes.Red;
                e.Row.Foreground = Brushes.White;
            }
        }
        private void medicineStorageDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (medicineStorageDataGrid.SelectedItem == null)
            {
                addDrugToStorageTxt.Text = string.Empty;
            }
            else
            {
                DataRowView selectedRow = medicineStorageDataGrid.SelectedItem as DataRowView;
                addDrugToStorageTxt.Text = selectedRow.Row[0].ToString();
            }
        }
        public void InitializeMedicineStorageDataGrid(List<Drug> drugs)
        {
            DataTable dt = CreateMedicineStorageTableHeaders();
            foreach (var drug in from Drug drug in drugs
                                 where drug.Quantity < 5
                                 select drug)
            {
                dt.Rows.Add(ExtractMedicineStorageInfo(drug));
            }

            medicineStorageDataGrid.ItemsSource = new DataView(dt);
            medicineStorageDataGrid.Items.Refresh();
        }
        public void LoadMedicineStorageDataGrid()
        {
            InitializeMedicineStorageDataGrid(DrugService.GetAllDrugs());
        }
        private void loadMedicineStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadMedicineStorageDataGrid();
        }

        public bool ValidateDrugInput(string drug)
        {
            return DrugService.FindByName(drug) != null; 
        }
        public bool ValidateQuantityInput(string quantity)
        {
            return !string.IsNullOrEmpty(quantity) && quantity.All(char.IsDigit) && int.Parse(quantity) >= 0;
        }
        public void ClearMedicineStorageFields()
        {
            LoadMedicineStorageDataGrid();
            addDrugToStorageTxt.Text = "";
            quantityToAddTxt.Text = "";
        }
        private void addDrugToStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDrugInput(addDrugToStorageTxt.Text) && ValidateQuantityInput(quantityToAddTxt.Text))
            {
                DrugOrderService.CreateDrugOrder(addDrugToStorageTxt.Text, int.Parse(quantityToAddTxt.Text));
                ClearMedicineStorageFields();
                MessageBox.Show("Successfuly ordered drug!");
            }
            else
            {
                ErrorMessage("Wrong input");
            }
        }
    }
}
