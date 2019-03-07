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

namespace SM4
{
    /// <summary>
    /// Secretkey.xaml 的交互逻辑
    /// </summary>
    public partial class Secretkey : Window
    {
        public Secretkey()
        {
            InitializeComponent();
        }

        public string key = string.Empty;

        private void button_Click(object sender, RoutedEventArgs e)
        {         
            string tempkey1 = passwordBox.Password.ToString();
            string tempkey2 = passwordBox1.Password.ToString();
            if (tempkey1 == tempkey2)
            {
                key = tempkey2;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("请重新输入密钥！");
                return;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //DialogResult = false;
            //Close();
        }
    }
}
