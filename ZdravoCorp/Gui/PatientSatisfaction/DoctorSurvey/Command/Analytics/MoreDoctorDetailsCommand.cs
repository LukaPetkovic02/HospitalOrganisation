using System.Windows;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.View.Analytics;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command.Analytics
{
    public class MoreDoctorDetailsCommand : BaseCommand
    {
        private DoctorAnalyticsViewModel doctorAnalyticsViewModel;

        public MoreDoctorDetailsCommand(DoctorAnalyticsViewModel doctorAnalyticsViewModel)
        {
            this.doctorAnalyticsViewModel = doctorAnalyticsViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            if (doctorAnalyticsViewModel.SelectedDoctor != null){
                if (doctorAnalyticsViewModel.SelectedDoctor.Doctor != null)
                {
                    return true;
                }
            }
            return false;
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(this))
            {
                Core.Utilities.Doctor.Model.Doctor selectedDoctor = doctorAnalyticsViewModel.SelectedDoctor.Doctor;

                SingleDoctorAnalyticsWindow singleDoctorAnalyticsWindow = new SingleDoctorAnalyticsWindow(doctorAnalyticsViewModel.DoctorSurveyService, selectedDoctor);
                singleDoctorAnalyticsWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select a doctor in the table in order to see more information about them!", "Message Box Title", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
