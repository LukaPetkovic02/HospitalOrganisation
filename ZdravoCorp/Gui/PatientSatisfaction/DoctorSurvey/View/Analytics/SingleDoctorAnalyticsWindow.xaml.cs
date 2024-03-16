using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.View.Analytics
{
    /// <summary>
    /// Interaction logic for SingleDoctorAnalyticsWindow.xaml
    /// </summary>
    public partial class SingleDoctorAnalyticsWindow : Window
    {
        public SingleDoctorAnalyticsWindow(DoctorSurveyService doctorSurveyService, Core.Utilities.Doctor.Model.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new SingleDoctorAnalyticsViewModel(doctorSurveyService, doctor);
        }
    }
}
