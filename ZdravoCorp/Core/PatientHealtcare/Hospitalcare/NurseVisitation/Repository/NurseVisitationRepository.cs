using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation.Repository
{
    public class NurseVisitationRepository : INurseVisitationRepository
    {
        public List<Model.NurseVisitation> NurseVisitations { get; set; }

        public NurseVisitationRepository()
        {
            GetAllNurseVisitations();
        }

        public void CreateNurseVisitation(string treatmentId, DateTime date, Model.NurseVisitation.VisitationTime time, string observation)
        {
            NurseVisitations.Add(new Model.NurseVisitation(GenerateNurseVisitationId(), treatmentId, date, time, observation));
            Save();
        }

        public string GenerateNurseVisitationId()
        {
            while (true)
            {
                string visitationId = Generator.GenerateRandomId();

                if (!(FindNurseVisitationById(visitationId) == null))
                {
                    continue;
                }
                return visitationId;
            }
        }
        public Model.NurseVisitation? FindNurseVisitationById(string id)
        {
            return NurseVisitations.FirstOrDefault(visitation => visitation.Id == id);
        }

        public void GetAllNurseVisitations()
        {
            NurseVisitations = JsonConvert.DeserializeObject<List<Model.NurseVisitation>>(File.ReadAllText("../../../Data/nursevisitation.json"));
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/nursevisitation.json", JsonConvert.SerializeObject(NurseVisitations, Formatting.Indented));
        }
    }
}
