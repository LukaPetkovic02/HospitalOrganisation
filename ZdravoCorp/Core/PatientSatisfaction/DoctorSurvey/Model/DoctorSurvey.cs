using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientSatisfaction.DoctorSurvey.Model
{
    public class DoctorSurvey
    {
        [JsonProperty("DoctorJMBG:")]
        public string DoctorJmbg;
        [JsonProperty("Service.cs")]
        public int Service;
        [JsonProperty("Recommendation")]
        public int Recommendation;
        [JsonProperty("Comment")]
        public string Comment;
        [JsonProperty("FinalGrade")]
        public int FinalGrade;
        public DoctorSurvey(string doctorJmbg, int service, int recommendation, string comment)
        {
            DoctorJmbg = doctorJmbg;
            Service = service;
            Recommendation = recommendation;
            Comment = comment;
            double avg = (recommendation + service) / 2;
            FinalGrade =(int)Math.Round(avg, MidpointRounding.ToPositiveInfinity);
        }
    }
}
