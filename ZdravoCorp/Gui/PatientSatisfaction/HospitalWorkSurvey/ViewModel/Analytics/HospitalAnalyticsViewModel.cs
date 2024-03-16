using System.Collections.Generic;
using ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.HospitalWorkSurvey.ViewModel.Analytics
{
    public class HospitalAnalyticsViewModel : BaseViewModel
    {
        public HospitalWorkSurveyService HospitalWorkSurveyService;

        private List<HospitalWorkSurveyDataViewModel> _surveys;
        private List<HospitalWorkSurveyEntryDataViewModel> _entrys;

        public List<HospitalWorkSurveyDataViewModel> Surveys
        {
            get { return _surveys; }
            set
            {
                _surveys = value;
                OnPropertyChanged(nameof(Surveys));
            }
        }

        public List<HospitalWorkSurveyEntryDataViewModel> Entrys
        {
            get { return _entrys; }
            set
            {
                _entrys = value;
                OnPropertyChanged(nameof(Entrys));
            }
        }

        public HospitalAnalyticsViewModel(HospitalWorkSurveyService hospitalWorkSurveyService)
        {
            HospitalWorkSurveyService = hospitalWorkSurveyService;
            _surveys = HospitalWorkSurveyService.GetHospitalWorkSurveyData();
            _entrys = HospitalWorkSurveyService.GetEntryDataViewModels();
        }
    }
}
