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
using System.Runtime.InteropServices;
using System.Runtime;
using System.Windows.Interop;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public ucScheduleShowTime ucScheduleST { get; set; }
        public ucSchedule ucSchedule { get; set; }
        public ucCinema ucCinema { get; set; }
        public ucMovie ucMovie { get; set; }
        public ucOrder ucOrder { get; set; }
        public ucButtonCrud ucBtnCrud { get; set; }

        private string selectedBtn;

        public MainView()
        {
            InitializeComponent();

            ucMovie = new ucMovie();
            ucCinema = new ucCinema();
            ucSchedule = new ucSchedule();
            ucScheduleST = new ucScheduleShowTime();
            ucOrder = new ucOrder();

            ucBtnCrud = new ucButtonCrud();

            ucBtnCrud.clickBtnAdd += ucControlBtnAdd;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Hide();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            stkListView.Children.Clear();
            stkButtonCrud.Children.Clear();
            RadioButton btn = sender as RadioButton;
            string str = btn.Name;
            selectedBtn = str;

            switch (str)
            {
                case "btnMovie":
                    stkListView.Children.Add(ucMovie);
                    break;
                case "btnCinema":
                    stkListView.Children.Add(ucCinema);
                    break;
                case "btnSchedule":
                    ucSchedule.LoadComboBox(ucCinema.cinemaService.Gets(), ucMovie.movieService.Gets());
                    ucSchedule.lvSchedule.Items.Refresh();
                    stkListView.Children.Add(ucSchedule);
                    break;
                case "btnScheduleST":
                    ucScheduleST.LoadUI();
                    stkListView.Children.Add(ucScheduleST);
                    break;
                case "btnOrder":
                    ucOrder.LoadUI();
                    stkListView.Children.Add(ucOrder);
                    break;
            }
            if (str != "btnOrder" && str != "btnCinema")
                stkButtonCrud.Children.Add(ucBtnCrud);
            if (string.Compare(str, "btnOrder", true) == 0)
            {
                ucButtonOrder ucButtonOrder = new ucButtonOrder();
                ucButtonOrder.selectedMovie += UcButtonOrder_selectedMovie;
                ucButtonOrder.selectedDate += UcButtonOrder_selectedDate;
                stkButtonCrud.Children.Add(ucButtonOrder);
            }
        }

        private void UcButtonOrder_selectedMovie(object sender, EventArgs e)
        {
            var movie = sender as Movie;
            if (movie == null) return;
            ucOrder.FilterOrderByMovie(movie);
        }

        private void UcButtonOrder_selectedDate(object sender, EventArgs e)
        {
            var item = sender as DatePicker;
            if (item == null)
                return;
            DateTime date = new DateTime();
            date = item.SelectedDate.Value;
            
            ucOrder.FilterOrderByDate(date);
        }

        private void ucControlBtnAdd(object sender, EventArgs e)
        {
            switch (selectedBtn)
            {
                case "btnMovie":
                    AddMovie();
                    break;
                case "btnSchedule":
                    ucSchedule.EventAdd();
                    break;
                case "btnScheduleST":
                    ucScheduleST.ScheduleSTDetailView = new ScheduleSTDetailView();
                    ucScheduleST.LoadComboBox(ucCinema.cinemaService.Gets(), ucMovie.movieService.Gets());
                    ucScheduleST.EventAdd();
                    break;
            }
        }

        public void AddMovie()
        {
            int count = ucMovie.movieService.Gets().Count;
            ucMovie.movieViewDetail.txtId.Text = "MV" + (count + 1).ToString();

            ucMovie.movieViewDetail.ShowDialog();

        }

    }
}
