using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.View.Analytics
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DoctorAnalyticsWindow : Window
    {
        public DoctorAnalyticsWindow(DoctorService doctorService, DoctorSurveyService doctorSurveyService)
        {
            InitializeComponent();
            DataContext = new DoctorAnalyticsViewModel(doctorService, doctorSurveyService);
        }
    }
}
