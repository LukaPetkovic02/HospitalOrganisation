using System;

namespace ZdravoCorp.Core.Utilities.Patient.Model
{
    public class MedicalCondition
    {
        public string DiagnosisName { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public MedicalCondition(string diagnosisName, DateTime diagnosisDate)
        {
            DiagnosisName = diagnosisName;
            DiagnosisDate = diagnosisDate;
        }

    }
}
