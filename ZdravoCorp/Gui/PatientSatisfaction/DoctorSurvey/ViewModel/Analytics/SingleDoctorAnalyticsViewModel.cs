using System.Collections.Generic;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics
{
    public class SingleDoctorAnalyticsViewModel : BaseViewModel
    {
        public DoctorSurveyService DoctorSurveyService;

        private List<DoctorSurveyDataViewModel> doctorSurveyDataViewModels;
        public List<DoctorSurveyDataViewModel> Surveys
        {
            get { return doctorSurveyDataViewModels; }
            set
            {
                doctorSurveyDataViewModels = value;
                OnPropertyChanged(nameof(Surveys));
            }
        }

        private string doctorId;
        public string DoctorId
        {
            get { return doctorId; }
            set
            {
                doctorId = value;
                OnPropertyChanged(nameof(DoctorId));
            }
        }

        public SingleDoctorAnalyticsViewModel(DoctorSurveyService DoctorSurveyService, Core.Utilities.Doctor.Model.Doctor doctor)
        {
            this.DoctorSurveyService = DoctorSurveyService;
            doctorSurveyDataViewModels = DoctorSurveyService.GetDoctorSurveyData(doctor);
            doctorId = doctor.Jmbg;
        }
    }
}
