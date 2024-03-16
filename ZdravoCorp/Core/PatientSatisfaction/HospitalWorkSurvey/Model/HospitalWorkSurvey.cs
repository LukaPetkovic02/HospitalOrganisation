using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientSatisfaction.HospitalWorkSurvey.Model
{
    public class HospitalWorkSurvey
    {
        [JsonProperty("Service.cs")]
        public int Service;
        [JsonProperty("Cleanliness")]
        public int Cleanliness;
        [JsonProperty("Recommendation")]
        public int Recommendation;
        [JsonProperty("Comment")]
        public string Comment;
        public HospitalWorkSurvey(int service, int cleanliness, int recommendation, string comment)
        {
            Service = service;
            Cleanliness = cleanliness;
            Recommendation = recommendation;
            Comment = comment;
        }
    }
}
