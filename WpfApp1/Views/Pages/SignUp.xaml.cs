using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class SignUp : Page
    {

        public SignUp()
        {
            InitializeComponent();
        }

        private async void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var username = TextBoxLogin.Text;
                var email = TextBoxEmail.Text;
                var password = passBox.Password;
                var confirmPassword = passBox_2.Password;

                // Проверка, что пароли совпадают
                if (password != confirmPassword)
                {
                    MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MyFrame.Content = new QuestionnairePage(username, password, email);

            }
            catch (Exception ex)
                {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                }
        }
        private async void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new SignIn();
        }
    }
}
