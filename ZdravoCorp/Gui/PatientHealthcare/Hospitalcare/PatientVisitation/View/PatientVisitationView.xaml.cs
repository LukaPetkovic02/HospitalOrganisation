using System.Windows.Controls;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.PatientVisitation.View
{

    public partial class PatientVisitationView : UserControl
    {
        public PatientVisitationView()
        {
            InitializeComponent();
            DataContext = new PatientVisitationViewModel();
        }

        private void HospitalCares_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Core.PatientHealtcare.Hospitalcare.HospitalTreatment.Model.HospitalTreatment SelectedHospitalTreatment)
            {
                var viewModel = DataContext as PatientVisitationViewModel;
                viewModel.SelectedHospitalTreatment = SelectedHospitalTreatment;
            }
        }
    }
}
