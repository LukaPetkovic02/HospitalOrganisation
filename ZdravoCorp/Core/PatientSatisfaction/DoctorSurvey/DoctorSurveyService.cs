using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey.Repository;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Doctor.Model;
using ZdravoCorp.Gui.PatientSatisfaction.DoctorSurvey.ViewModel.Analytics;

namespace ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey
{
    public class DoctorSurveyService
    {
        public DoctorSurveyRepository DoctorSurveyRepository { get; set; }
        public DoctorSurveyService()
        {
            DoctorSurveyRepository = new DoctorSurveyRepository();
        }
        public void GetAllDoctorSurveys()
        {
            DoctorSurveyRepository.GetAllDoctorSurveys();
        }
        public void AddDoctorSurvey(Model.DoctorSurvey doctorSurvey)
        {
            DoctorSurveyRepository.AddDoctorSurvey(doctorSurvey);
        }

        public List<DoctorSurveyDataViewModel> GetDoctorSurveyData(Doctor doctor)
        {
            return DoctorSurveyRepository.DoctorSurveys
                .Where(survey => survey.DoctorJmbg == doctor.Jmbg)
                .Select(survey => new DoctorSurveyDataViewModel(survey))
                .ToList();
        }

        public void Save()
        {
            DoctorSurveyRepository.Save();
        }
    }
}
