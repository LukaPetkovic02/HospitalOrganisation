using System.Windows.Input;
using ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.Command;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel
{
    public class HospitalWorkSurveyViewModel : BaseViewModel
    {
        public string Service { get; set; }
        public string Cleanliness { get; set; }
        public string Recommendation { get; set; }
        public string Comment { get; set; }

        public ICommand SubmitHospitalSurveyCommand { get; }
        public HospitalWorkSurveyViewModel()
        {
            Service = "";
            Cleanliness = "";
            Recommendation = "";
            Comment = "";
            SubmitHospitalSurveyCommand = new SubmitHospitalSurveyCommand(this);
        }
    }
}
