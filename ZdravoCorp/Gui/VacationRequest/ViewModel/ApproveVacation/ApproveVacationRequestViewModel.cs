using System.Collections.Generic;
using System.Windows.Input;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.VacationRequest;
using ZdravoCorp.Gui.VacationRequest.Command.ApproveVacation;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.VacationRequest.ViewModel.ApproveVacation
{
    public class ApproveVacationRequestViewModel: BaseViewModel
    {
        public VacationRequestService RequestService;
        public ScheduleService ScheduleService;
        private List<VacationRequestDataViewModel> vacationRequests;
        private VacationRequestDataViewModel selectedVacationRequest;

        public ICommand ApproveVacationCommand { get; }

        public List<VacationRequestDataViewModel> VacationRequests
        {
            get { return RequestService.GetAllVacationRequestData(); }
            set
            {
                vacationRequests = value;
                OnPropertyChanged(nameof(VacationRequests));
            }
        }

        public VacationRequestDataViewModel SelectedVacationRequest
        {
            get { return selectedVacationRequest; }
            set
            {
                selectedVacationRequest = value;
                OnPropertyChanged(nameof(SelectedVacationRequest));
            }
        }

        public ApproveVacationRequestViewModel(VacationRequestService requestService, ScheduleService scheduleService)
        {
            RequestService = requestService;
            ScheduleService = scheduleService;
            vacationRequests = requestService.GetAllVacationRequestData();
            VacationRequests = vacationRequests;
            selectedVacationRequest = null;
            ApproveVacationCommand = new ApproveVacationCommand(this);
        }
    }
}
