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

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for ucButtonOrder.xaml
    /// </summary>
    public partial class ucButtonOrder : UserControl
    {
        public event EventHandler selectedMovie;
        public event EventHandler selectedDate;

        public OrderService orderService { get; set; }
        ObservableCollection<Movie> movies { get; set; }

        public ucButtonOrder()
        {
            InitializeComponent();
            orderService = new OrderService();
            movies = new ObservableCollection<Movie>();
            AddMovieCbBox();
        }

        public bool CheckMovie(Movie movie)
        {
            foreach (var item in movies)
                if (string.Compare(item.Id, movie.Id, true) == 0)
                    return true;
            return false;
        }

        public void AddMovieCbBox()
        {
            foreach (var item in orderService.Gets())
            {
                if (!CheckMovie(item.ScheduleST.Schedule.Movie))
                {
                    movies.Add(item.ScheduleST.Schedule.Movie);
                }
            }
            cbMovie.ItemsSource = movies;
        }

        private void cbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var movie = cbMovie.SelectedItem as Movie;
            selectedMovie?.Invoke(movie, EventArgs.Empty);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            cbMovie.SelectedIndex = -1;
            DatePicker item = sender as DatePicker;
            selectedDate?.Invoke(item, EventArgs.Empty);
        }
    }
}
