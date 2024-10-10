using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for ScheduleDetailView.xaml
    /// </summary>
    public partial class ScheduleDetailView : Window
    {
        public event EventHandler clickBtnAdd;

        public ScheduleDetailView()
        {
            InitializeComponent();

        }
        
        private void cbCinema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCinema.SelectedItem == null)
                return;
        }

        private void cbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void btn_Add(object sender, RoutedEventArgs e)
        {
            Schedule schedule = new Schedule();
            schedule.Id = txtId.Text;
            if (cbCinema.SelectedItem == null || cbMovie.SelectedItem == null || AirDate.SelectedDate == null)
            {
                MessageBox.Show("Can not be empty\nPlease enter in full");
                return;
            }
            schedule.AirDate = AirDate.SelectedDate.Value;
            schedule.Movie = cbMovie.SelectedItem as Movie;
            schedule.Cinema = cbCinema.SelectedItem as Cinema;
            schedule.IdCinema = schedule.Cinema.Id;
            schedule.IdMovie = schedule.Movie.Id;
            schedule.Status = true;

            clickBtnAdd?.Invoke(schedule, EventArgs.Empty);
        }

        private void btn_Exit(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
