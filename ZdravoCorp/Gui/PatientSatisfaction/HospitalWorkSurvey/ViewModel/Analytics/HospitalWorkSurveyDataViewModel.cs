using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel.Analytics
{
    public class HospitalWorkSurveyDataViewModel : BaseViewModel
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

        private int _cleanliness;
        public int Cleanliness
        {
            get { return _cleanliness; }
            set
            {
                _cleanliness = value;
                OnPropertyChanged(nameof(Cleanliness));
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
        

        public HospitalWorkSurveyDataViewModel(Core.PatientSatisfaction.HospitalWorkSurvey.Model.HospitalWorkSurvey survey)
        {
            _service = survey.Service;
            _recommendation = survey.Recommendation;
            _comment = survey.Comment;
            _cleanliness = survey.Cleanliness;
        }
    }
}
