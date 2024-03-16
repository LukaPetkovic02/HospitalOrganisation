using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation.Repository
{
    public interface INurseVisitationRepository
    {
        public List<Model.NurseVisitation> NurseVisitations { get; set; }
        public void CreateNurseVisitation(string treatmentId, DateTime date, Model.NurseVisitation.VisitationTime time, string observation);

        public string GenerateNurseVisitationId();
        public Model.NurseVisitation? FindNurseVisitationById(string id);
        public void GetAllNurseVisitations();
        public void Save();

    }
}
