using System.Windows.Controls;
using ZdravoCorp.Gui.VacationRequest.ViewModel;

namespace ZdravoCorp.Gui.VacationRequest.View
{
    public partial class VacationRequestView : UserControl
    {

        public VacationRequestView(Core.Utilities.Doctor.Model.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new VacationRequestViewModel(doctor);
        }
    }
}
