using System;
using System.Windows;
using System.Windows.Controls;

namespace Tien_C2_B1
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
                
            // Add hours to hoursComboBox
            for (int i = 1; i <= 24; i++)
            {
                hoursComboBox.Items.Add(i.ToString());
            }

            // Add minutes to minutesComboBox
            for (int i = 0; i <= 59; i++)
            {
                minutesComboBox.Items.Add(i.ToString().PadLeft(2, '0'));
            }

            // Add AM/PM to ampm ComboBox
            //ampmComboBox.Items.Add("AM");
            //ampmComboBox.Items.Add("PM");
        }

        // Properties to get/set selected time
        public int SelectedHour
        {
            get { return hoursComboBox.SelectedIndex + 1; }
            set { hoursComboBox.SelectedIndex = value - 1; }
        }

        public int SelectedMinute
        {
            get { return minutesComboBox.SelectedIndex; }
            set { minutesComboBox.SelectedIndex = value; }
        }

        //public string SelectedAMPM
        //{
        //    get { return ampmComboBox.SelectedItem.ToString(); }
        //    set { ampmComboBox.SelectedItem = value; }
        //}
    }
}
