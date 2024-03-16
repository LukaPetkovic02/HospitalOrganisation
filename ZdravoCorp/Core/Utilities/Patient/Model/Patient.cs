using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utilities.Patient.Model
{
    

    public class Patient : Person.Model.Person
    {
        public MedicalCard MedicalCard { get; set; }
        [JsonProperty("Notification time")]
        public TimeSpan NotificationTime;

        [JsonConstructor]
        public Patient(string jmbg, string name, string lastName, string password, DateTime birthDate, MedicalCard medicalCard, TimeSpan notificationTime)
        {
            Jmbg = jmbg;
            Name = name;
            LastName = lastName;
            Password = password;
            BirthDate = birthDate;
            MedicalCard = medicalCard;
            NotificationTime = notificationTime;
        }

    }
}
