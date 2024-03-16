using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utilities.Person.Model
{
    public class Person
    {
        [JsonProperty("Jmbg")]
        public string Jmbg { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("BirthDate")]
        public DateTime BirthDate { get; set; }
        [JsonConstructor]
        public Person(string jmbg, string name, string lastName, string password, DateTime birthDate)
        {
            Jmbg = jmbg;
            Name = name;
            LastName = lastName;
            Password = password;
            BirthDate = birthDate;
        }
        public Person() { }
    }
}
