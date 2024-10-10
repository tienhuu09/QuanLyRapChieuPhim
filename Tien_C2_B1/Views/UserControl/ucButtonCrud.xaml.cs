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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for ucButtonCrud.xaml
    /// </summary>
    public partial class ucButtonCrud : UserControl
    {
        public event EventHandler clickBtnAdd;
        public event EventHandler clickBtnLock;
        public event EventHandler clickBtnUnlock;

        public ucButtonCrud()
        {
            InitializeComponent();
        }

        private void btnAdd(object sender, RoutedEventArgs e)
        {
            clickBtnAdd?.Invoke(this, EventArgs.Empty);
        }

        private void btnLock(object sender, RoutedEventArgs e)
        {
            clickBtnLock?.Invoke(this, EventArgs.Empty);
        }

        private void btnUnlock(object sender, RoutedEventArgs e)
        {
            clickBtnUnlock?.Invoke(this, EventArgs.Empty);
        }

    }
}
