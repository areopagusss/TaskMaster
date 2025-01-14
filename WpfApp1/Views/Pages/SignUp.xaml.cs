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
        private static readonly HttpClient client = new HttpClient();

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

                // Сериализация данных в JSON
                var json = JsonSerializer.Serialize(new
                {
                    Username = username,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add this to see what we're sending
                Debug.WriteLine($"Sending to server: {await content.ReadAsStringAsync()}");

                HttpResponseMessage response = await client.PostAsync("http://192.168.0.5:8080/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Add this to see what we're receiving
                Debug.WriteLine($"Received from server: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        MessageBox.Show("Регистрация успешна!",
                                      "Успех",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        MyFrame.Content = new QuestionnairePage();
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Ошибка парсинга JSON:\nПолученные данные: {responseContent}\nОшибка: {ex.Message}",
                                      "Ошибка JSON",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Код ответа сервера: {response.StatusCode}\nСодержимое: {responseContent}",
                                  "Ошибка сервера",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void Button_Window1_Click(object sender, RoutedEventArgs e)
        {
            // Логика для кнопки (если нужно)
        }

    }
}