using System.Windows.Input;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.Command;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel
{
    public class DoctorSurveyViewModel : BaseViewModel
    {
        public string Service { get; set; }
        public string Recommendation { get; set; }
        public string Comment { get; set; }

        public ICommand SubmitDoctorSurveyCommand { get; }
        public DoctorSurveyViewModel(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            Service = "";
            Recommendation = "";
            Comment = "";
            SubmitDoctorSurveyCommand = new SubmitDoctorSurveyCommand(this, doctor);
        }
    }
}
