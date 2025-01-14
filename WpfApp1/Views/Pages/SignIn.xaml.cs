using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MySqlConnector;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        private const string ConnectionString = "Server=192.168.0.5;Database=project;User ID=root4;Password=000000;";
        public SignIn()
        {
            InitializeComponent();
        }
        private void Button_RegMain_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new SignUp();
        }
        private async void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            // Получаем введенные данные
            string username = TextBoxLogin.Text;
            string password = passBox.Password;

            // Проверяем, что поля не пустые
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Подключаемся к базе данных
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();

                    // Ищем пользователя в базе данных по Username
                    var query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        // Выполняем запрос
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Получаем хэшированный пароль из базы данных
                                string hashedPasswordFromDatabase = reader.GetString("PasswordHash");

                                // Проверяем пароль
                                if (BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDatabase))
                                {
                                    MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
            MyFrame.Content = new MainPage();

        }
    }
}
