using System.Windows.Controls;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.ViewModel;

namespace ZdravoCorp.Gui.PatientHealthcare.Hospitalcare.HospitalTreatment.View
{
    /// <summary>
    /// Interaction logic for TreatmentAccommodationView.xaml
    /// </summary>
    public partial class TreatmentAccommodationView : UserControl
    {
        public TreatmentAccommodationView()
        {
            InitializeComponent();
            DataContext = new TreatmentAccommodationViewModel();
        }
        private void AvailableAccommodationRoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Room SelectedRoom)
            {
                var viewModel = DataContext as TreatmentAccommodationViewModel;
                viewModel.SelectedRoom = SelectedRoom;
            }
        }
    }
}
