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
    /// Interaction logic for ucMovie.xaml
    /// </summary>
    public partial class ucMovie : UserControl
    {
        public MovieService movieService { get; set; }
        public MovieViewDetail movieViewDetail { get; set; }

        public ObservableCollection<Movie> lstMovie { get; set; }
        Movie movie { get; set; }

        public ucMovie()
        {
            movieService = new MovieService();
            InitializeComponent();

            lstMovie = new ObservableCollection<Movie>(movieService.Gets());
            lvMovie.ItemsSource = lstMovie;

            movieViewDetail = new MovieViewDetail();
            movieViewDetail.clickBtnAdd += AddMovie;
        }

        public void AddMovie(object sender, EventArgs e)
        {
            var movie = sender as Movie;
            if (!movieService.isExist(movie.Id))
            {
                lstMovie.Add(movie);
                movieService.Add(movie);
                MessageBox.Show("Movie added successfully");
            }
            else
                MessageBox.Show("Movie is existed!!!");
        }
        
        public Movie getMovieDoubleClick()
        {
            if (lvMovie.SelectedItem == null)
                return null;
            var selected = lvMovie.SelectedItem as Movie;
            return selected;
        }

        public void Refresh()
        {
            lvMovie.Items.Refresh();
        }

        public void btnLock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                movie = btn.DataContext as Movie;
                if (movie != null)
                    lvMovie.SelectedItem = movie;
            }

            if (movie.Status == false)
            {
                MessageBox.Show("This movie is locked");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to lock this movie?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    movie.Status = false;
                    movieService.Update(movie);
                    MessageBox.Show("The movie locked successfully.");
                }
            }
            Refresh();

        }

        public void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                movie = btn.DataContext as Movie;
                if (movie != null)
                    lvMovie.SelectedItem = movie;
            }

            if (movie.Status == true)
                MessageBox.Show("This movie is unlock");
            else
            {
                if (DateTime.Compare(movie.EndAirDate, DateTime.Now) < 0)
                {
                    MessageBox.Show("This movie has expired!\nUnable to unlock");
                    return;
                }
                movie.Status = true;
                movieService.Update(movie);
                MessageBox.Show("The movie unlock successfully.");
            }
            Refresh();

        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                movie = btn.DataContext as Movie;
                if (movie != null)
                {
                    lvMovie.SelectedItem = movie;
                    var movieView = new MovieViewDetail();
                    movieView._DelegateEdit += Refresh;
                    movieView.myDelegate += getMovieDoubleClick;
                    movieView.ShowDialog();
                }
            }
           
        }
    }
}
