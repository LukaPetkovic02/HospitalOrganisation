using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey.Repository
{
    public class HospitalWorkSurveyRepository : IHospitalWorkSurveyRepository
    {
        public List<Model.HospitalWorkSurvey> HospitalWorkSurveys;

        public HospitalWorkSurveyRepository()
        {
            GetAllHospitalWorkSurveys();
        }

        public void GetAllHospitalWorkSurveys()
        {
            HospitalWorkSurveys = JsonConvert.DeserializeObject<List<Model.HospitalWorkSurvey>>(File.ReadAllText("../../../Data/hospitalWorkSurveys.json"));
        }
        public void AddHospitalWorkSurvey(Model.HospitalWorkSurvey hospitalWorkSurvey)
        {
            HospitalWorkSurveys.Add(hospitalWorkSurvey);
            Save();
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/hospitalWorkSurveys.json", JsonConvert.SerializeObject(HospitalWorkSurveys, Formatting.Indented));
        }
        public List<Model.HospitalWorkSurvey> GetHospitalWorkSurveys()
        {
            return HospitalWorkSurveys;
        }
    }
}
