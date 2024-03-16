using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation.Repository;

namespace ZdravoCorp.Core.PatientHealtcare.Hospitalcare.NurseVisitation
{
    public class NurseVisitationService
    {
        public INurseVisitationRepository NurseVisitationRepository { get; set; }  
        public NurseVisitationService(INurseVisitationRepository nurseVisitationRepository) 
        {
            NurseVisitationRepository = nurseVisitationRepository;
        }

        public void CreateNurseVisitation(string treatmentId, DateTime date, Model.NurseVisitation.VisitationTime time, string observation)
        {
            NurseVisitationRepository.CreateNurseVisitation(treatmentId, date, time, observation);
        }

        public Model.NurseVisitation? FindNurseVisitationById(string id)
        {
            return NurseVisitationRepository.FindNurseVisitationById(id);
        }

        public void Save()
        {
            NurseVisitationRepository.Save();
        }

    }
}
