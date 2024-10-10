using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for ucScheduleShowTime.xaml
    /// </summary>
    public partial class ucScheduleShowTime : UserControl
    {
        public event RoutedEventHandler SelectionChanged;

        public ScheduleSTService ScheduleSTService { get; set; }
        public ScheduleSTDetailView ScheduleSTDetailView { get; set; }

        public ScheduleService ScheduleService { get; set; }

        public ObservableCollection<ScheduleShowTime> lstScheduleST { get; set; }
        public ObservableCollection<UIScheduleST> lstUIScheduleST { get; set; }

        ScheduleShowTime scheduleST { get; set; }
        public ucScheduleShowTime()
        {
            ScheduleSTService = new ScheduleSTService();

            ScheduleService = new ScheduleService();

            InitializeComponent();
            lstScheduleST = new ObservableCollection<ScheduleShowTime>(ScheduleSTService.Gets());

            lvScheduleShowTime.ItemsSource = lstScheduleST;
            LoadUI();
        }

        public void LoadUI()
        {
            lstUIScheduleST = new ObservableCollection<UIScheduleST>();
            UIScheduleST itemUI;
            foreach (var item in lstScheduleST)
            {
                itemUI = new UIScheduleST();
                itemUI.Id = item.Id;
                itemUI.NameMovie = item.Schedule.Movie.Name;
                itemUI.NameCinema = item.Schedule.Cinema.Name;
                itemUI.AirDate = item.AirDate;
                itemUI.SeatActive = (item.lstBoughtSeat.Count).ToString() + "/" + item.Schedule.Cinema.lstSeat.Count.ToString();
                itemUI.Price = item.Schedule.Cinema.PriceCenter;
                if (item.Status == false)
                    itemUI.Status = "Unavailable";
                else
                    itemUI.Status = "Available";

                lstUIScheduleST.Add(itemUI);
            }

            lvScheduleShowTime.ItemsSource = lstUIScheduleST;
        }

        private List<Schedule> ScheduleSTDetailView_DelegateSchedule()
        {
            return ScheduleService.Gets();
        }

        public void EventAdd()
        {
            ScheduleSTDetailView.clickBtnAdd += ScheduleSTDetailView_clickBtnAdd;
            ScheduleSTDetailView.DelegateSchedule += ScheduleSTDetailView_DelegateSchedule;
            ScheduleSTDetailView.DelegateScheduleST += ScheduleSTDetailView_DelegateScheduleST;
            ScheduleSTDetailView.txtId.Text = (ScheduleSTService.Gets().Count + 1).ToString();
            ScheduleSTDetailView.ShowDialog();
            ScheduleSTDetailView.clickBtnAdd -= ScheduleSTDetailView_clickBtnAdd;
        }

        private List<ScheduleShowTime> ScheduleSTDetailView_DelegateScheduleST()
        {
            return ScheduleSTService.Gets();
        }

        public void LoadComboBox(List<Cinema> lstCinema, List<Movie> lstMovie) 
        {
            foreach (var item in lstMovie)
                if (item.Status == true)
                    ScheduleSTDetailView.cbMovie.Items.Add(item);

            ScheduleSTDetailView.cbMovie.SelectedIndex = -1;
        }

        private void ScheduleSTDetailView_clickBtnAdd(object sender, EventArgs e)
        {
            ScheduleShowTime item = sender as ScheduleShowTime;
            if (ScheduleSTService.isExisted(item.Id) == false)
            {
                if (!ScheduleSTService.CheckDateShowTime(item))
                {
                    MessageBox.Show("The film's release has ended");
                    return;
                }

                if (ScheduleSTService.OverlapTime(item))
                {
                    MessageBox.Show("The start time overlapped. Please try again");
                    return;
                }

                if (!ScheduleSTService.CheckThePresentTime(item))
                {
                    MessageBox.Show("The showtime of this movie is invalid");
                    return;
                }
                lstScheduleST.Add(item);
                ScheduleSTService.Add(item);

                UIScheduleST itemUI = new UIScheduleST();
                itemUI.MapFrom(item);
                lstUIScheduleST.Add(itemUI);
                ScheduleSTDetailView.LoadScheduleSTListView();
                MessageBox.Show("Showtime added successfully");
                return;
            }
            else
            {
                MessageBox.Show("Showtime is existed!");
            }

        }

        private void lvScheduleShowTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged.Invoke(this, new RoutedEventArgs());
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UIScheduleST item = new UIScheduleST();
            if (btn != null)
            {
                item = btn.DataContext as UIScheduleST;
                scheduleST = ScheduleSTService.Get(item.Id);

                if (scheduleST != null)
                    lvScheduleShowTime.SelectedItem = scheduleST;
            }

            if (scheduleST.Status == false)
                MessageBox.Show("This schedule showtime is locked");
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to lock this schedule showtime?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    item.Status = "Unavailable";
                    scheduleST.Status = false;
                    MessageBox.Show("The schedule showtime locked successfully.");
                }
            }
            lvScheduleShowTime.Items.Refresh();
        }

        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UIScheduleST item = new UIScheduleST();
            if (btn != null)
            {
                item = btn.DataContext as UIScheduleST;
                scheduleST = ScheduleSTService.Get(item.Id);

                if (scheduleST != null)
                    lvScheduleShowTime.SelectedItem = scheduleST;
            }

            if (scheduleST.Status == true)
                MessageBox.Show("This schedule showtime is unlock");
            else
            {
                if (DateTime.Compare(scheduleST.AirDate, DateTime.Now) < 0)
                {
                    MessageBox.Show("This schedule showtime has expired!\nUnable to unlock");
                    return;
                }

                if (scheduleST.Schedule.Status == false)
                {
                    MessageBox.Show("Its schedule is locked, please open it!");
                    return;
                }

                item.Status = "Available";
                scheduleST.Status = true;
                MessageBox.Show("The schedule showtime unlock successfully.");
            }
            lvScheduleShowTime.Items.Refresh();
        }
    }
}
