using System.Collections.Generic;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command.Analytics
{
    public class GetBestDoctorsCommand : BaseCommand
    {
        private DoctorAnalyticsViewModel doctorAnalyticsViewModel;

        public GetBestDoctorsCommand(DoctorAnalyticsViewModel doctorAnalyticsViewModel)
        {
            this.doctorAnalyticsViewModel = doctorAnalyticsViewModel;
        }
        public override void Execute(object? parameter)
        {
            List<DoctorDataViewModel> bestDoctors = doctorAnalyticsViewModel.DoctorService.GetBestDoctorsData(3);
            doctorAnalyticsViewModel.Doctors = bestDoctors;
        }
    }
}
