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
using System.Windows.Shapes;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public AccountService accService { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        MainView mainView { get; set; }
        UserWindow userW { get; set; }

        public LoginView()
        {
            InitializeComponent();

            mainView = new MainView();

            accService = new AccountService();

            Messages = new ObservableCollection<string>();
            this.DataContext = this;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            loginEvent();
        }

        public void ResetUsernamePassword()
        {
            txtUser.Clear();
            txtPass.Clear();
        }

        public void loginEvent()
        {
            string strName = txtUser.Text;
            string strPass = txtPass.Password.ToString();

            Account account = accService.GetAccount(strName, strPass);
            if (account == null)
            {
                MessageBox.Show("Account invalid");
                return;
            }

            if (account.Permission == 1)
            {
                this.Hide();
                mainView.ShowDialog();
                ResetUsernamePassword();
                this.ShowDialog();
            }
            if (account.Permission == 0)
            {
                this.Hide();
                userW = new UserWindow();
                userW.ShowDialog();
                ResetUsernamePassword();
                this.ShowDialog();
            }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                loginEvent();
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();

        }
    }
}
