using System.Collections.Generic;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command.Analytics
{
    public class GetWorstDoctorsCommand : BaseCommand
    {
        private DoctorAnalyticsViewModel doctorAnalyticsViewModel;

        public GetWorstDoctorsCommand(DoctorAnalyticsViewModel doctorAnalyticsViewModel)
        {
            this.doctorAnalyticsViewModel = doctorAnalyticsViewModel;
        }
        public override void Execute(object? parameter)
        {
            List<DoctorDataViewModel> bestDoctors = doctorAnalyticsViewModel.DoctorService.GetWorstDoctorsData(3);
            doctorAnalyticsViewModel.Doctors = bestDoctors;
        }
    }
}
