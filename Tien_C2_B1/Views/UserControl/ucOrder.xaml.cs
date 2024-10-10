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
    /// Interaction logic for ucOrder.xaml
    /// </summary>
    public partial class ucOrder : UserControl
    {
        public OrderService orderService { get; set; }

        ObservableCollection<UIOrder> lstUIOrder { get; set; }
        public Order Order { get; set; }
        public UIOrder UIOrder { get; set; }
        public ucOrder()
        {
            InitializeComponent();

            orderService = new OrderService();
            LoadUI();
        }

        public void LoadUI()
        {
            lstUIOrder = new ObservableCollection<UIOrder>();
            UIOrder itemUI;
            foreach (var item in orderService.Gets())
            {
                itemUI = new UIOrder();
                itemUI.MapFrom(item);

                lstUIOrder.Add(itemUI);
            }

            lvOrder.ItemsSource = lstUIOrder;
        }

        public void FilterOrderByMovie(Movie movie)
        {
            lstUIOrder = new ObservableCollection<UIOrder>();
            UIOrder itemUI;
            foreach (var item in orderService.Gets())
            {
                if (item.ScheduleST.Schedule.IdMovie == movie.Id)
                {
                    itemUI = new UIOrder();
                    itemUI.MapFrom(item);

                    lstUIOrder.Add(itemUI);
                }
            }

            lvOrder.ItemsSource = lstUIOrder;
        }

        public void FilterOrderByDate(DateTime date)
        {
            lstUIOrder = new ObservableCollection<UIOrder>();
            UIOrder itemUI;
            foreach (var item in orderService.Gets())
            {
                var dateItem = item.Date;
                if (dateItem.Year == date.Year && dateItem.Month == date.Month && dateItem.Day == date.Day)
                {
                    itemUI = new UIOrder();
                    itemUI.MapFrom(item);

                    lstUIOrder.Add(itemUI);
                }
            }

            lvOrder.ItemsSource = lstUIOrder;
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                UIOrder = btn.DataContext as UIOrder;
                if (Order != null)
                    lvOrder.SelectedItem = UIOrder;
                Order = orderService.Get(UIOrder.Id);
            }

            OrderDetailView orderDetailView = new OrderDetailView();
            orderDetailView.DataContext = this;
            orderDetailView.ShowDialog();
        }
    }
}
