using System.Collections.Generic;

namespace ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey.Repository
{
    public interface IHospitalWorkSurveyRepository
    {
        List<Model.HospitalWorkSurvey> GetHospitalWorkSurveys();
        void GetAllHospitalWorkSurveys();
        void AddHospitalWorkSurvey(Model.HospitalWorkSurvey hospitalWorkSurvey);
        void Save();
    }
}
