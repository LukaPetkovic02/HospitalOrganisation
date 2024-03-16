using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.Utilities.Patient.Model
{
    public class MedicalCard
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<MedicalCondition> MedicalHistory { get; set; }
        public List<String> Alergies { get; set; }

        public MedicalCard(double height, double weight, List<MedicalCondition> medicalHistory, List<string> alergies)
        {
            Height = height;
            Weight = weight;
            MedicalHistory = medicalHistory;
            Alergies = alergies;
        }

    }
}
