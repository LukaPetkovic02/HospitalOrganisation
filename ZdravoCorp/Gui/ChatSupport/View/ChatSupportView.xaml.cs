using System.Collections.Generic;
using System.Windows.Controls;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Gui.ChatSupport.ViewModel;

namespace ZdravoCorp.Gui.ChatSupport.View
{
    /// <summary>
    /// Interaction logic for ChatSupportView.xaml
    /// </summary>

    public partial class ChatSupportView : UserControl
    {
        public ChatSupportView(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            InitializeComponent();
            InitializeComboBoxDoctor(doctor);
            DataContext = new ChatSupportViewModel(this,doctor.Jmbg);
        }
        public ChatSupportView(Core.Utilities.Nurse.Model.Nurse nurse)
        {
            InitializeComponent();
            InitializeComboBoxNurse(nurse);
            DataContext = new ChatSupportViewModel(this,nurse.Jmbg);
        }
        public void InitializeComboBoxDoctor(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            NurseService nurseService = new NurseService();
            List<Core.Utilities.Nurse.Model.Nurse> nurses = nurseService.Nurses();
            foreach (Core.Utilities.Nurse.Model.Nurse nurse in nurses)
                chatsComboBox.Items.Add(nurse.Jmbg + " " + nurse.Name + " " + nurse.LastName);

        }
        public void InitializeComboBoxNurse(Core.Utilities.Nurse.Model.Nurse nurse)
        {
            DoctorService doctorService = new DoctorService();
            List<Core.Utilities.Doctor.Model.Doctor> doctors = doctorService.Doctors();
            foreach (Core.Utilities.Doctor.Model.Doctor doctor in doctors)
                chatsComboBox.Items.Add(doctor.Jmbg + " " + doctor.Name + " " + doctor.LastName);
        }
    }
}
