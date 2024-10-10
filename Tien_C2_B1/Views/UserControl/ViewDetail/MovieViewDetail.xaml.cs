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
    /// Interaction logic for addMovieWindow.xaml
    /// </summary>
    public partial class MovieViewDetail : Window
    {
        public event EventHandler clickBtnAdd;

        public delegate Movie MyDelegateUI();
        public event MyDelegateUI myDelegate;

        public delegate void DelegateEdit();
        public event DelegateEdit _DelegateEdit;
        
        public MovieViewDetail()
        {
            InitializeComponent();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateStart.SelectedDate == null || DateEnd.SelectedDate == null)
                return;
            if (DateTime.Compare(DateStart.SelectedDate.Value, DateEnd.SelectedDate.Value) > 0
                || DateTime.Compare(DateEnd.SelectedDate.Value, DateTime.Now) < 0)
            {
                DatePicker temp = sender as DatePicker;
                if (string.Compare(temp.Name, "DateStart") == 0)
                {
                    DateStart.SelectedDate = null;
                    MessageBox.Show("Date Invalid");
                }
                else
                {
                    DateEnd.SelectedDate = null;
                    MessageBox.Show("Date Invalid");
                }
            }

        }

        private void btn_Add(object sender, RoutedEventArgs e)
        {
            Movie movie = new Movie()
            {
                Id = txtId.Text,
                Name = txtName.Text,
                Description = txtDes.Text,
                Duration = txtDuration.Text,
                StartAirDate = DateStart.SelectedDate.Value,
                EndAirDate = DateEnd.SelectedDate.Value
            };
            clickBtnAdd?.Invoke(movie, EventArgs.Empty);
        }

        private void btn_Exit(object sender, RoutedEventArgs e)
        {
            txtId.Text = null;
            txtName.Text = null;
            txtDes.Text = null;
            txtDuration.Text = null;
            DateStart.SelectedDate = null;
            DateEnd.SelectedDate = null;
            this.Hide();
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

        private void btn_Edit(object sender, RoutedEventArgs e)
        {
            if (myDelegate != null)
            {
                Movie movie = myDelegate();
                if (movie.Status == false)
                {
                    MessageBox.Show("The movie is locked. cannot be edited!");
                    return;
                }
                movie.Name = txtName.Text;
                movie.Description = txtDes.Text;
                movie.Duration = txtDuration.Text;

                movie.StartAirDate = DateStart.SelectedDate.Value;
                movie.EndAirDate = DateEnd.SelectedDate.Value;
                if (DateTime.Compare(movie.EndAirDate, DateTime.Now) > 0)
                    movie.Status = true;

                _DelegateEdit.Invoke();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (myDelegate != null)
            {
                Movie item = myDelegate();
                txtId.Text = item.Id;
                txtName.Text = item.Name;
                txtDes.Text = item.Description;
                txtDuration.Text = item.Duration;

                DateStart.SelectedDate = item.StartAirDate;
                DateEnd.SelectedDate = item.EndAirDate;
            }
            txtId.IsReadOnly = true;

        }
    }
}
