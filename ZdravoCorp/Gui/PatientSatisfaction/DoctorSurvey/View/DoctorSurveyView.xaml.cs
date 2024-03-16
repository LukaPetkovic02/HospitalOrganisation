using System.Windows.Controls;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.View
{
    /// <summary>
    /// Interaction logic for DoctorSurveyView.xaml
    /// </summary>
    public partial class DoctorSurveyView : UserControl
    {
        public DoctorSurveyView(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new DoctorSurveyViewModel(doctor);
        }
    }
}
