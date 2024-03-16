using System.Windows;
using ZdravoCorp.Core.Scheduling.Schedule;
using ZdravoCorp.Core.VacationRequest;
using ZdravoCorp.Gui.VacationRequest.ViewModel.ApproveVacation;

namespace ZdravoCorp.Gui.VacationRequest.View.ApproveVacation
{
    //Click="{Binding ApproveVacationCommand}"
    public partial class ApproveVacationRequestView : Window
    {
        public ApproveVacationRequestView(VacationRequestService requestService, ScheduleService scheduleService)
        {
            InitializeComponent();
            DataContext = new ApproveVacationRequestViewModel(requestService, scheduleService);
        }
    }
}
