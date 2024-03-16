using System;
using System.Collections.Generic;
using System.Windows.Documents;
using ZdravoCorp.Core.VacationRequest.Repository;
using ZdravoCorp.Gui.VacationRequest.ViewModel.ApproveVacation;

namespace ZdravoCorp.Core.VacationRequest
{
    public class VacationRequestService
    {
        public IVacationRequestRepository VacationRequestRepository;

        public VacationRequestService(IVacationRequestRepository vacationRequestRepository)
        {
            VacationRequestRepository = vacationRequestRepository;
        }

        public void CreateVacationRequest(string doctorJmbg, string reason, DateTime start, DateTime end)
        {
            VacationRequestRepository.CreateVacationRequest(doctorJmbg,reason,start,end);
        }

        public bool IsAvailable(string doctorJmbg, DateTime startDate, DateTime endDate)
        {
            return VacationRequestRepository.IsAvailable(doctorJmbg, startDate, endDate);
        }
        public bool IsOnVacation(string doctorJmbg)
        {
            return VacationRequestRepository.IsOnVacation(doctorJmbg);
        }

        public void AcceptVacationRequest(Model.VacationRequest request)
        {
            VacationRequestRepository.FindVacationRequestById(request.Id).Status =
                Model.VacationRequest.VacationStatus.ACCEPTED;
            Save();
        }

        public Model.VacationRequest? FindVacationRequestById(string id)
        {
            return VacationRequestRepository.FindVacationRequestById(id);
        }

        public void GetAllVacationRequests()
        {
            VacationRequestRepository.GetAllVacationRequests();
        }

        public List<VacationRequestDataViewModel> GetAllVacationRequestData()
        {
            List < VacationRequestDataViewModel > requests = new List < VacationRequestDataViewModel >();
            foreach (Model.VacationRequest request in VacationRequestRepository.VacationRequests)
            {
                if (request.Status == Model.VacationRequest.VacationStatus.WAITING)
                {
                    requests.Add(new VacationRequestDataViewModel(request));
                }
            }
            return requests;
        }

        public void Save()
        {
            VacationRequestRepository.Save();
        }
    }
}
