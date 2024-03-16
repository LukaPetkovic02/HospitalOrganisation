using System.Collections.Generic;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command.Analytics
{
    public class GetAllDoctorsCommand : BaseCommand
    {
        private DoctorAnalyticsViewModel doctorAnalyticsViewModel;

        public GetAllDoctorsCommand(DoctorAnalyticsViewModel doctorAnalyticsViewModel)
        {
            this.doctorAnalyticsViewModel = doctorAnalyticsViewModel;
        }
        public override void Execute(object? parameter)
        {
            List<DoctorDataViewModel> bestDoctors = doctorAnalyticsViewModel.DoctorService.GetDoctorData();
            doctorAnalyticsViewModel.Doctors = bestDoctors;
        }
    }
}
