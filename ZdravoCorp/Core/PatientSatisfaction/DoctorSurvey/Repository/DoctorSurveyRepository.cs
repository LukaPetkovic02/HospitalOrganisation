using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey.Repository
{
    public class DoctorSurveyRepository
    {
        public List<Model.DoctorSurvey> DoctorSurveys;

        public DoctorSurveyRepository()
        {
            GetAllDoctorSurveys();
        }

        public void GetAllDoctorSurveys()
        {
            DoctorSurveys = JsonConvert.DeserializeObject<List<Model.DoctorSurvey>>(File.ReadAllText("../../../Data/doctorSurveys.json"));
        }
        public void AddDoctorSurvey(Model.DoctorSurvey doctorSurvey)
        {
            DoctorSurveys.Add(doctorSurvey);
            Save();
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/doctorSurveys.json", JsonConvert.SerializeObject(DoctorSurveys, Formatting.Indented));
        }
    }
}
