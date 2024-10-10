using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public MovieService MovieService { get; set; }
        public OrderService OrderService { get; set; }
        public CinemaService CinemaService { get; set; }
        public ScheduleSTService ScheduleSTService { get; set; }

        // working with UI
        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<ScheduleShowTime> ScheduleShowTimes { get; set; }

        public Order Order { get; set; }
        public List<BoughtSeat> boughtSeats { get; set; }
        public ObservableCollection<UIDetailOrder> UIDetailOrders { get; set; }

        public UserWindow()
        {
            InitializeComponent();

            MovieService = new MovieService();
            OrderService = new OrderService();
            CinemaService = new CinemaService();
            ScheduleSTService = new ScheduleSTService();

            LoadCbMovie();
            Orders = new ObservableCollection<Order>(OrderService.Gets());

            lstView.lstUIScheduleST.Clear();
            AddComboBoxType();

            this.DataContext = this;
        }

        public void LoadCbMovie()
        {
            List<Movie> lstMovie = new List<Movie>();
            foreach (var item in MovieService.Gets())
            {
                if (item.Status == true)
                {
                    lstMovie.Add(item);
                }
            }

            Movies = new ObservableCollection<Movie>(lstMovie);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnBookTicket_Click(sender, e);
            if (e.Key == Key.Escape)
                this.Hide();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private void btnBookTicket_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtCustomerName.Text) || string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                MessageBox.Show("Please complete all information");
                return;
            }
            else
            {
                Order.CustomerName = txtCustomerName.Text;
                Order.PhoneNumber = txtPhoneNumber.Text;
            }

            if (cbMovie.SelectedItem == null)
            {
                MessageBox.Show("Please choose any Movie");
                return;
            }

            if (lstView.lvScheduleShowTime.SelectedItem == null)
            {
                MessageBox.Show("Please choose any Show time");
                return;
            }

            if (Order.lstDetailOrder.Count == 0)
            {
                MessageBox.Show("Please choose your seat");
                return;
            }

            ScheduleSTService.AddBoughtSeat(Order.ScheduleST, boughtSeats);
            Orders.Add(Order);
            OrderService.Add(Order);
            UIDetailOrders = new ObservableCollection<UIDetailOrder>(GetUIDetailOrder(Order));

            MessageBox.Show("Ticket booking successfully. Please come back!");
            btnReset_Click(sender, e);
            PrintTickets printTickets = new PrintTickets();
            printTickets.DataContext = this;
            printTickets.ShowDialog();
        }

        public List<UIDetailOrder> GetUIDetailOrder(Order order)
        {
            List<UIDetailOrder> uIDetailOrders = new List<UIDetailOrder>();
            UIDetailOrder uIDetailOrder;
            foreach (var item in order.lstDetailOrder)
            {
                uIDetailOrder = new UIDetailOrder();
                uIDetailOrder.MapFrom(item);
                uIDetailOrder.Type = order.ScheduleST.Schedule.Cinema.Type;
                uIDetailOrder.IdOrder = order.Id;
                uIDetailOrder.DateBook = order.Date.ToString(Ulti.date);
                uIDetailOrder.CustomerName = order.CustomerName;
                uIDetailOrder.PhoneNumber = order.PhoneNumber;
                uIDetailOrder.Movie = order.ScheduleST.Schedule.Movie.Name;
                uIDetailOrder.Cinema = order.ScheduleST.Schedule.Cinema.Name;
                uIDetailOrder.ShowTime = order.ScheduleST.AirDate.ToString(Ulti.date);

                uIDetailOrders.Add(uIDetailOrder);
            }
            return uIDetailOrders;

        }

        public void FilterScheduleShowTime()
        {
            Movie movie = cbMovie.SelectedItem as Movie;
            txtNotify.Visibility = Visibility.Visible;
            if (movie == null)
            {
                ScheduleShowTimes.Clear();
                txtNotify.Visibility = Visibility.Hidden;
                return;
            }
            foreach (var item in ScheduleSTService.Gets())
            {
                if (string.Compare(movie.Name, item.Schedule.Movie.Name, true) == 0
                    && item.Status == true)
                {
                    ScheduleShowTimes.Add(item);
                    txtNotify.Visibility = Visibility.Hidden;
                }
            }
        }

        private void cbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScheduleShowTimes = new ObservableCollection<ScheduleShowTime>();
            FilterScheduleShowTime();

            CreateOrder();
            myCanvas.Children.Clear();
            txtQuan.Text = "0";
            txtTotal.Text = "0";
            lstView.lstScheduleST = ScheduleShowTimes;
            lstView.lvScheduleShowTime.ItemsSource = lstView.lstScheduleST;
            lstView.LoadUI();
        }

        public bool CheckBoughtSeat(ScheduleShowTime ScheduleST, string seat)
        {
            foreach (var item in ScheduleST.lstBoughtSeat)
                if (string.Compare(item.Seat.Name, seat, true) == 0)
                    return true;
            return false;
        }

        private void lstView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var itemUI = lstView.lvScheduleShowTime.SelectedItem as UIScheduleST;
            if (itemUI == null)
                return;
            var selected = ScheduleSTService.Get(itemUI.Id);
            myCanvas.Children.Clear();

            int S = 50;
            int margin = 20;
            int left = 0;
            int top = 0;
            int lenght = selected.Schedule.Cinema.lstSeat.Count;

            boughtSeats = new List<BoughtSeat>();
            Button btn;
            foreach (var item in selected.Schedule.Cinema.lstSeat)
            {
                btn = new Button();
                btn.Content = item.Name;
                btn.Width = S;
                btn.Height = S;
                btn.Name = item.Id;
                if (CheckBoughtSeat(selected, item.Name))
                    btn.Background = Brushes.Red;
                else
                {
                    btn.Background = Brushes.LightGreen;
                    btn.Click += Btn_Click;
                }
                
                Canvas.SetLeft(btn, margin + 55 + (S + (S / 2)) * left);
                Canvas.SetTop(btn, margin + (S + (S / 2)) * top);
                myCanvas.Children.Add(btn);

                left++;
                if (left == 8)
                {
                    left = 0;
                    top += 1;
                    if (lenght == 20 && top == 2)
                        left = 2;
                }
            }

            txtQuan.Text = "0";
            txtTotal.Text = "0";
        }

        public void AddComboBoxType()
        {
            List<string> listType = new List<string>();
            listType.Add("Adult");
            listType.Add("Child");
            cbType.ItemsSource = listType;
            cbType.SelectedIndex = 0;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var itemUI = lstView.lvScheduleShowTime.SelectedItem as UIScheduleST;
            if (itemUI == null)
                return;
            var selected = ScheduleSTService.Get(itemUI.Id);

            Order.IdScheduleShowTime = selected.Id;
            Order.CinemaType = selected.Schedule.Cinema.Type;
            Order.ScheduleST = selected;

            if (btn.Background == Brushes.DeepPink)
            {
                btn.Background = Brushes.LightGreen;
                int n = Int32.Parse(txtQuan.Text);
                txtQuan.Text = (n - 1).ToString();
                CinemaService.DeleteSeat(boughtSeats, btn.Name);
            }
            else
            {
                btn.Background = Brushes.DeepPink;
                int n = Int32.Parse(txtQuan.Text);
                txtQuan.Text = (n + 1).ToString();
                BoughtSeat item = CinemaService.getSeatByName(selected.Schedule.Cinema, btn.Content.ToString());
                item.Type = cbType.SelectedItem as string;
                boughtSeats.Add(item);
            }

            string age;
            double total = 0;
            txtTotal.Text = null;

            Order.lstDetailOrder.Clear();
            DetailOrder detailOrder;
            foreach (var item in boughtSeats)
            {
                age = item.Type;
                double price = selected.Schedule.Cinema.getPrice();
                double discount = selected.Schedule.Cinema.getDiscount(age, selected.AirDate.Day, selected.AirDate.Month, selected.AirDate.Year);
                detailOrder = new DetailOrder(age, item.SeatId, price, discount);
                detailOrder.Seat = item.Seat;
                detailOrder.SeatName = item.Seat.Name;
                total += (price - discount);
                Order.lstDetailOrder.Add(detailOrder);
                Order.Total = total;
            }

            txtTotal.Text = total.ToString(Ulti.spec);
        }

        public void CreateOrder()
        {
            Order = new Order();
            Order.Id = (OrderService.Gets().Count + 1).ToString();
            Order.Date = DateTime.Now;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbMovie.SelectedIndex = -1;

            myCanvas.Children.Clear();
            txtQuan.Text = "0";
            txtTotal.Text = "0";
            lstView.lstScheduleST = ScheduleShowTimes;
            lstView.lvScheduleShowTime.ItemsSource = lstView.lstScheduleST;
            lstView.LoadUI();
        }
    }
}
