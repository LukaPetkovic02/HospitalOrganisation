using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.PhysicalAsets.Inventory;
using ZdravoCorp.Core.PhysicalAsets.Renovations;
using ZdravoCorp.Core.PhysicalAsets.Room.Model;
using ZdravoCorp.Core.Utilities.Model;

namespace ZdravoCorp.Gui.PhysicalAsets.View
{
    /// <summary>
    /// Interaction logic for AllocateItemWindow.xaml
    /// </summary>
    public partial class RenovateRoomsWindow : Window
    {
        private RenovationSchedule RenovationSchedule;
        private InventoryService InventoryService;
        private bool join;
        private bool split;
        private TimeSlot timeSlot;
        private bool success = false;
        public RenovateRoomsWindow(RenovationSchedule renovationSchedule, InventoryService inventoryService, bool join, bool split)
        {
            InitializeComponent();
            RenovationSchedule = renovationSchedule;
            InventoryService = inventoryService;
            this.split = split;
            this.join = join;
            InitializeWindow();      
        }

        private void InitializeWindow()
        {
            AddRoomIds();
            AddRoomTypes();
            if (join) InitializeJoin();
            else if (split) InitializeSplit();
            else InitializeSimple();

        }

        private void InitializeSimple()
        {
            InitializeSplit();
            InitializeJoin();
            newId.Visibility = Visibility.Hidden;
            NewIdLabel.Visibility = Visibility.Hidden;
        }

        private void InitializeSplit()
        {
            SecondIdLabel.Visibility = Visibility.Hidden;
            renovatedRoomSecond.Visibility = Visibility.Hidden;
        }

        private void InitializeJoin()
        {
            secondNewId.Visibility = Visibility.Hidden;
            roomTypesSecond.Visibility = Visibility.Hidden;
            NewTypeSecondLabel.Visibility = Visibility.Hidden;
            NewIdSecondLabel.Visibility = Visibility.Hidden;
        }

        private void AddRoomTypes()
        {
            roomTypes.Items.Add(RoomType.OperationRoom);
            roomTypes.Items.Add(RoomType.ExaminationRoom);
            roomTypes.Items.Add(RoomType.PatientRoom);
            roomTypes.Items.Add(RoomType.WaitingRoom);

            roomTypesSecond.Items.Add(RoomType.OperationRoom);
            roomTypesSecond.Items.Add(RoomType.ExaminationRoom);
            roomTypesSecond.Items.Add(RoomType.PatientRoom);
            roomTypesSecond.Items.Add(RoomType.WaitingRoom);
        }
        

        private void AddRoomIds()
        {
            List<Room> rooms = InventoryService.GetRooms();
            foreach (Room room in rooms)
            {
                if (room.Id != "00")
                {
                    renovatedRoom.Items.Add(room.Id);
                    renovatedRoomSecond.Items.Add(room.Id);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (success)
            {
                MessageBox.Show("Renovation scheduled.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Do you want to cancel renovation scheduling?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckDateParse()) return;

            if (join) ParseJoin();
            else if (split) ParseSplit();
            else ParseSimple();
        }

        private void ParseSimple()
        {
            if (renovatedRoom.SelectedItem is not string id) { MessageBox.Show("You need to select a room to renovate.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (roomTypes.SelectedItem is not RoomType newRoomType) { MessageBox.Show("New room type needs to be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (!RenovationSchedule.AddRenovation(id, timeSlot, newRoomType)) { MessageBox.Show("Time slot for this rooms renovation isn't available. Try another time slot.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            success = true;
            Close();
        }

        private void ParseSplit()
        {
            if (renovatedRoom.SelectedItem is not string id) { MessageBox.Show("You need to select a room to renovate.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (roomTypes.SelectedItem is not RoomType newRoomType || roomTypesSecond.SelectedItem is not RoomType secondNewRoomType) { MessageBox.Show("New room types need to be both selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            string newIdFirst = newId.Text;
            string newIdSecond = secondNewId.Text;
            if(newIdFirst== newIdSecond)
            {
                MessageBox.Show("New room id-s need to be diferent.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); 
                return;
            }

            if (!RenovationSchedule.AddRenovation(id, timeSlot, newRoomType, secondNewRoomType, newIdFirst, newIdSecond)) { MessageBox.Show("Time slot for this rooms renovation isn't available. Try another time slot. Or new room Id-s aren't available.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            success = true;
            Close();
        }

        private void ParseJoin()
        {
            if (renovatedRoom.SelectedItem is not string idFirst || renovatedRoomSecond.SelectedItem is not string idSecond) { MessageBox.Show("You need to select both rooms to renovate.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if(idFirst == idSecond) { MessageBox.Show("Rooms that need to be joined need to have different Id-s.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (roomTypes.SelectedItem is not RoomType newRoomType){MessageBox.Show("New room type needs to be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;}

            string idNew = newId.Text;

            if (!RenovationSchedule.AddRenovation(idFirst,idSecond, timeSlot, newRoomType, idNew)) { MessageBox.Show("Time slot for this rooms renovation isn't available. Try another time slot. Or new room Id-s aren't available.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            success = true;
            Close();
        }

        private bool CheckDateParse()
        {
            if (!ItemsSelected(hourComboBox,minuteComboBox,ampmComboBox,Start) || !ItemsSelected(hourComboBoxEnd, minuteComboBoxEnd, ampmComboBoxEnd, End)) { return false; }

            DateTime start;
            if (!DateTime.TryParse(FormatDate(hourComboBox, minuteComboBox, ampmComboBox, Start), out start)) { return false; }

            DateTime end;
            if (!DateTime.TryParse(FormatDate(hourComboBoxEnd, minuteComboBoxEnd, ampmComboBoxEnd, End), out end)) { return false; }
            
            if (start >= end)
            {
                MessageBox.Show("Start date should be before end date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (start < DateTime.Now)
            {
                MessageBox.Show("Start date needs to be in future.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            timeSlot = new TimeSlot(start, end);

            return true;
        }
        private string FormatDate(ComboBox Hour,ComboBox Minute, ComboBox Ampm, DatePicker datePicker)
        {
            DateTime Date;
            int hour = Convert.ToInt32(((ComboBoxItem)Hour.SelectedItem).Content);
            int minute = Convert.ToInt32(((ComboBoxItem)Minute.SelectedItem).Content);
            string am_pm = ((ComboBoxItem)Ampm.SelectedItem).Content.ToString();

            string formattedDate = string.Format("{0:d/M/yyyy} {1}:{2} {3}", datePicker.SelectedDate.Value, hour, minute, am_pm);
            if (!DateTime.TryParse(formattedDate, out Date))
            {
                MessageBox.Show("Start or end date didn't parse well:" + formattedDate, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return formattedDate;
        }
        private bool ItemsSelected(ComboBox hour, ComboBox minute, ComboBox ampm, DatePicker datePicker)
        {
            if (!datePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Every date (start and end) need to be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            bool TimeNotSelected = hour.SelectedItem == null || minute.SelectedItem == null || ampm.SelectedItem == null;

            if (TimeNotSelected)
            {
                MessageBox.Show("Renovation (start and end) time needs to be selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
