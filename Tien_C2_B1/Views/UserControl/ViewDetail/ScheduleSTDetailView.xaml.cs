using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for ScheduleSTDetailView.xaml
    /// </summary>
    public partial class ScheduleSTDetailView : Window
    {
        public event EventHandler clickBtnAdd;

        public delegate List<Schedule> MyDelegateSchedule();
        public event MyDelegateSchedule DelegateSchedule;

        public delegate List<ScheduleShowTime> MyDelegateScheduleST();
        public event MyDelegateScheduleST DelegateScheduleST;

        public delegate ScheduleShowTime MyDelegateUI();
        public event MyDelegateUI myDelegate;

        private TimePicker TimePicker { get; set; }

        public ObservableCollection<Schedule> Schedules { get; set; }
        public ObservableCollection<UIScheduleST> lstUIScheduleST { get; set; }

        public ScheduleSTDetailView()
        {
            InitializeComponent();
            TimePicker = new TimePicker();
        }

        private void LoadListSchedule()
        {
            Movie movie = cbMovie.SelectedItem as Movie;
            Schedules = new ObservableCollection<Schedule>();
            int flag = 0;
            foreach (var item in DelegateSchedule())
            {
                if (string.Compare(item.Movie.Id, movie.Id, true) == 0 && item.Status == true)
                {
                    Schedules.Add(item);
                    flag = 1;
                }
            }
            lvSchedule.ItemsSource = Schedules;
            if (flag == 0)
                txtMessage.Text = "Schedule is empty";
            else
                txtMessage.Text = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridTimePicker.Children.Add(TimePicker);
            if (myDelegate != null)
            {
                ScheduleShowTime item = myDelegate();
                txtId.Text = item.Id;
                cbMovie.Items.Clear();
                cbMovie.SelectedIndex = 0;
                cbMovie.Items.Add(item.Schedule.Movie);
              
                txtSeatActive.Text = (item.lstBoughtSeat.Count.ToString() + "/" + item.Schedule.Cinema.lstSeat.Count.ToString());
                txtPrice.Text = item.Schedule.Cinema.PriceCenter.ToString(Ulti.spec);
                
                TimePicker.hoursComboBox.SelectedIndex = item.AirDate.Hour - 1;
                TimePicker.minutesComboBox.SelectedIndex = item.AirDate.Minute;

                if (item.Status)
                    txtStt.Text = "Available";
                else
                    txtStt.Text = "Unavailable";

                cbMovie.IsReadOnly = true;
                TimePicker.hoursComboBox.IsReadOnly = true;
                TimePicker.minutesComboBox.IsReadOnly = true;
            }
            txtId.IsReadOnly = true;
            txtPrice.IsReadOnly = true;
            txtSeatActive.IsReadOnly = true;
            txtStt.IsReadOnly = true;
        }

        private void btn_Add(object sender, RoutedEventArgs e)
        {
            ScheduleShowTime item = new ScheduleShowTime();

            if (cbMovie.SelectedItem == null || TimePicker.hoursComboBox.SelectedItem == null ||lvSchedule.SelectedItems == null)
            {
                MessageBox.Show("Can not be empty\nPlease enter in full");
                return;
            }

            item.Id = txtId.Text;
            item.Schedule = GetSchedule();
            item.IdSchedule = item.Schedule.Id;

            int day = item.Schedule.AirDate.Day;
            int month = item.Schedule.AirDate.Month;
            int year = item.Schedule.AirDate.Year;
            int hour = Convert.ToInt32(TimePicker.hoursComboBox.SelectedItem);
            int minute = Convert.ToInt32(TimePicker.minutesComboBox.SelectedItem);

            item.AirDate = new DateTime(year, month, day, hour, minute, 0);

            clickBtnAdd?.Invoke(item, EventArgs.Empty);
        }

        public Schedule GetSchedule()
        {
            var item = lvSchedule.SelectedItem as Schedule;
            return item;
        }

        private void btn_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void cbCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadListSchedule();
            if (lstUIScheduleST != null)
                lstUIScheduleST.Clear();
            txtStt.Text = "Available";
        }

        private void lvSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var schedule = lvSchedule.SelectedItem as Schedule;

            if (schedule != null)
            {
                txtSeatActive.Text = "0/" + schedule.Cinema.lstSeat.Count;
                txtPrice.Text = schedule.Cinema.PriceCenter.ToString(Ulti.spec);
            }
            else
            {
                txtSeatActive.Text = null;
                txtPrice.Text = null;
                //MessageBox.Show("Schedule does not exist\nPlease check the Schedule and try again");
            }

            LoadScheduleSTListView();
        }

        public void LoadScheduleSTListView()
        {
            var selected = lvSchedule.SelectedItem as Schedule;
            if (selected == null)
                return;
            lstUIScheduleST = new ObservableCollection<UIScheduleST>();
            UIScheduleST itemUI;
            foreach (var item in DelegateScheduleST())
            {
                itemUI = new UIScheduleST();
                if (string.Compare(item.IdSchedule, selected.Id, true) == 0)
                {
                    itemUI.Id = item.Id;
                    itemUI.AirDate = item.AirDate;
                    itemUI.SeatActive = (item.lstBoughtSeat.Count).ToString() + "/" + item.Schedule.Cinema.lstSeat.Count.ToString();
                    itemUI.Duration = item.Schedule.Movie.Duration;
                    itemUI.Price = item.Schedule.Cinema.PriceCenter;
                    lstUIScheduleST.Add(itemUI);
                }
            }
            lvScheduleST.ItemsSource = lstUIScheduleST;
        }
    }
}
