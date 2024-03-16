using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.View.Analytics
{
    /// <summary>
    /// Interaction logic for HospitalAnalyticsWindow.xaml
    /// </summary>
    public partial class HospitalAnalyticsWindow : Window
    {
        public HospitalAnalyticsWindow(HospitalWorkSurveyService service)
        {
            InitializeComponent();
            DataContext = new HospitalAnalyticsViewModel(service);
        }
    }
}
