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
    /// Interaction logic for ucSchedule.xaml
    /// </summary>
    public partial class ucSchedule : UserControl
    {
        public ScheduleService scheduleService { get; set; }
        public ScheduleDetailView scheduleDetailV { get; set; }

        ObservableCollection<Schedule> lstSchedule { get; set; }
        Schedule schedule { get; set; }

        public ucSchedule()
        {
            InitializeComponent();

            scheduleService = new ScheduleService();

            lstSchedule = new ObservableCollection<Schedule>(scheduleService.Gets());
            lvSchedule.ItemsSource = lstSchedule;

            scheduleDetailV = new ScheduleDetailView();
        }

        public void LoadComboBox(List<Cinema> lstCinema, List<Movie> lstMovie)
        {
            scheduleDetailV.cbCinema.Items.Clear();
            scheduleDetailV.cbMovie.Items.Clear();
            
            foreach (var item in lstCinema)
                scheduleDetailV.cbCinema.Items.Add(item);
            foreach (var item in lstMovie)
                if (item.Status == true)
                    scheduleDetailV.cbMovie.Items.Add(item);
        }

        public void Add(object sender, EventArgs e)
        {
            var schedule = sender as Schedule;
            if (schedule == null)
                return;
            if (scheduleService.isExist(schedule.Id))
            {
                MessageBox.Show("IdSchedule is existed!!!");
                return;
            }

            if (schedule.Movie.Status == false)
            {
                MessageBox.Show("Movie not available!!!");
                return;
            }

            if (!scheduleService.ScheduleIsExist(schedule))
            {
                lstSchedule.Add(schedule);
                scheduleService.Add(schedule);
                MessageBox.Show("Schedule added successfully!");
                return;
            }
            else
                MessageBox.Show("Schedule is existed!!!");
        }

        public void EventAdd()
        {
            scheduleDetailV.clickBtnAdd += Add;
            scheduleDetailV.txtId.Text = (scheduleService.Gets().Count + 1).ToString();
            scheduleDetailV.cbMovie.SelectedIndex = -1;
            scheduleDetailV.cbCinema.SelectedIndex = -1;
            scheduleDetailV.ShowDialog();
            scheduleDetailV.clickBtnAdd -= Add;
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                schedule = btn.DataContext as Schedule;
                if (schedule != null)
                    lvSchedule.SelectedItem = schedule;
            }

            if (schedule.Status == false)
                MessageBox.Show("This schedule is locked");
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to lock this schedule?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    schedule.Status = false;
                    scheduleService.UpdateStatus(schedule);
                    MessageBox.Show("The schedule locked successfully.");
                }
            }
            lvSchedule.Items.Refresh();
        }

        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                schedule = btn.DataContext as Schedule;
                if (schedule != null)
                    lvSchedule.SelectedItem = schedule;
            }

            if (schedule.Status == true)
                MessageBox.Show("This schedule is unlock");
            else
            {
                if (DateTime.Compare(schedule.AirDate, DateTime.Now) < 0)
                {
                    MessageBox.Show("This schedule has expired!\nUnable to unlock");
                    return;
                }

                if (schedule.Movie.Status == false)
                {
                    MessageBox.Show("Its movie is locked, please open it!");
                    return;
                }

                schedule.Status = true;
                scheduleService.UpdateStatus(schedule);
                MessageBox.Show("The schedule unlock successfully.");

            }
            lvSchedule.Items.Refresh();
        }
    }
}
