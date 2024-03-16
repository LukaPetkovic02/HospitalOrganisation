using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using ZdravoCorp.Core.Login;
using ZdravoCorp.Core.Notifications;
using ZdravoCorp.Core.PatientHealtcare.Pharmacy.Drugs.DrugOrders;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey;
using ZdravoCorp.Core.PhysicalAsets.Inventory;
using ZdravoCorp.Core.PhysicalAsets.Renovations;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Core.Utilities.Nurse.Model;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;
using ZdravoCorp.Core.VacationRequest;
using ZdravoCorp.Core.VacationRequest.Repository;
using ZdravoCorp.Gui.Doctor;
using ZdravoCorp.Gui.Doctor.View;
using ZdravoCorp.Gui.PhysicalAsets.View;
using ZdravoCorp.Gui.Nurse;
using ZdravoCorp.Gui.Nurse.View;
using ZdravoCorp.Gui.Patient.View;
using ZdravoCorp.Gui.PhysicalAsets;
using ZdravoCorp.Gui.PhysicalAsets.View;

namespace ZdravoCorp
{

    public partial class MainWindow : Window
    {
        private InventoryService inventory;
        private RenovationSchedule RenovationSchedule;
        private DispatcherTimer timer;
        private DoctorService doctorService;
        private DoctorSurveyService DoctorSurveyService;
        private HospitalWorkSurveyService hospitalWorkSurveyService;
        public NotificationService NotificationService;
        public ScheduleService scheduleService;
        public DrugOrderService DrugOrderService;
        public VacationRequestService VacationRequestService;
        public static Doctor LoginDoctor;
        public MainWindow()
        {
            InitializeComponent();
            inventory = new InventoryService();
            scheduleService = new ScheduleService();
            RenovationSchedule = new RenovationSchedule(inventory, scheduleService);
            NotificationService = new NotificationService();
            VacationRequestService = new VacationRequestService(new VacationRequestRepository());
            DrugOrderService = new DrugOrderService();
            doctorService = new DoctorService();
            DoctorSurveyService = new DoctorSurveyService();
            hospitalWorkSurveyService = new HospitalWorkSurveyService();
            UpdateEvents();
            DrugOrderService.CheckForArrivedDrugs();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(loginJmbg.Text, loginPassword.Password);
            LoginDoctor = login.CheckDoctorLogin();
            Patient? patient = login.CheckPatientLogin();
            Nurse? nurse = login.CheckNurseLogin();


            if (loginJmbg.Text == "admin" && loginPassword.Password == "admin")
            {
                InventoryWindow inventoryWindow = new InventoryWindow(inventory, RenovationSchedule, VacationRequestService, doctorService, DoctorSurveyService, hospitalWorkSurveyService, scheduleService);
                inventoryWindow.Show();
                Close();
            }
            else if (LoginDoctor != null)
            {
                if (VacationRequestService.IsOnVacation(LoginDoctor.Jmbg))
                {
                    MessageBox.Show("Doctor is on a vacation! ");
                }
                else
                {
                    DoctorWindow doctorWindow = new DoctorWindow(LoginDoctor, RenovationSchedule);

                    doctorWindow.Show();
                    NotificationService.CheckForNotifications(LoginDoctor.Jmbg);
                    Close();
                }
            }
            else if (patient != null)
            {   
                PatientService service=new PatientService();
                PatientWindow patientWindow = new PatientWindow(patient.Jmbg);
                
                if (service.IsPatientBlocked(patient.Jmbg))
                {
                    MessageBox.Show("Patient is blocked!");
                }
                else
                {
                    patientWindow.Show();
                    NotificationService.CheckForPatientNotifications(patient.Jmbg);
                    Close();
                }
            }
            else if (nurse != null)
            {
                NurseWindow nurseWindow = new NurseWindow(nurse);
                nurseWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Wrong JMBG or password! ");
                loginJmbg.Text = string.Empty;
                loginPassword.Password = string.Empty;
            }


        }

        private void UpdateEvents()
        {
            inventory.UpdateInnerArrivedEquipment();
            inventory.UpdateArrivedEquipment();
            RenovationSchedule.UpdateRenovations();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateEvents();
        }
    }
}
