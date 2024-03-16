using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics
{
    public class DoctorSurveyDataViewModel : BaseViewModel
    {
        private int _service;
        public int Service
        {
            get { return _service; }
            set
            {
                _service = value;
                OnPropertyChanged(nameof(Service));
            }
        }

        private int _recommendation;
        public int Recommendation
        {
            get { return _recommendation; }
            set
            {
                _recommendation = value;
                OnPropertyChanged(nameof(Recommendation));
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public DoctorSurveyDataViewModel(Core.PatientSatisfaction.DoctorSurvey.Model.DoctorSurvey survey)
        {
            _service = survey.Service;
            _recommendation = survey.Recommendation;
            _comment = survey.Comment;
        }
    }
}
