using System.Windows.Controls;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.View
{
    /// <summary>
    /// Interaction logic for HospitalWorkSurveyView.xaml
    /// </summary>
    public partial class HospitalWorkSurveyView : UserControl
    {
        public HospitalWorkSurveyView()
        {
            InitializeComponent();
            DataContext = new HospitalWorkSurveyViewModel();
        }
    }
}
