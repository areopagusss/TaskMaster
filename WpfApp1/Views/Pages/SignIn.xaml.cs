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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        public SignIn()
        {
            InitializeComponent();
        }
        private void Button_RegMain_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new SignUp();
        }
        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new MainPage();
        }
    }
}
