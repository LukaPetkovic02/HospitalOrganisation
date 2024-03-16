using System.Windows;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.VacationRequest.ViewModel.ApproveVacation;

namespace ZdravoCorp.Gui.VacationRequest.Command.ApproveVacation
{
    public class ApproveVacationCommand  : BaseCommand
    {
        private ApproveVacationRequestViewModel approveVacationRequestViewModel;

        public ApproveVacationCommand(ApproveVacationRequestViewModel approveVacationRequestViewModel)
        {
            this.approveVacationRequestViewModel = approveVacationRequestViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            if (approveVacationRequestViewModel.SelectedVacationRequest != null)
            {
                if (approveVacationRequestViewModel.SelectedVacationRequest.vacationRequest != null)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(this))
            {
                Core.VacationRequest.Model.VacationRequest request = approveVacationRequestViewModel.SelectedVacationRequest.vacationRequest;
                approveVacationRequestViewModel.RequestService.AcceptVacationRequest(request);
                approveVacationRequestViewModel.ScheduleService.CancelAppointmensByApprovedVacation(request);

                MessageBox.Show("Approved Doctors:" + request.DoctorJmbg + " Request: " + request.Reason + " , for vacation.", "Message Box Title", MessageBoxButton.OK, MessageBoxImage.Information);
                
                approveVacationRequestViewModel.VacationRequests =
                    approveVacationRequestViewModel.RequestService.GetAllVacationRequestData();
            }
            else
            {
                MessageBox.Show("Please select a vacation request in the table to approve!", "Message Box Title", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
