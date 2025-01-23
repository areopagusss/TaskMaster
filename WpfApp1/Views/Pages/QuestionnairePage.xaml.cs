using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySqlConnector;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using WpfApp1.Views.Pages;

namespace WpfApp1
{
    public partial class QuestionnairePage : Page
    {
        // Строка подключения к MariaDB
        private static readonly HttpClient client = new HttpClient();
        private string _username;
        private string _password;
        private string _email;

        public QuestionnairePage(string username, string password, string email)
        {
            InitializeComponent();
            _username = username;
            _password = password;
            _email = email;
        }

        private async void Button_Save(object sender, RoutedEventArgs e)
        {
            var fullName = FullNameTextBox.Text;
            var position = PositionTextBox.Text;
            var departament = DepartmentTextBox;

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату рождения!");
                return;
            }

            var user = new
            {
                Username = _username,
                Email = _email,
                PasswordHash = _password,
                FullName = FullNameTextBox.Text.Trim(),
                BirthDate = BirthDatePicker.SelectedDate,
                Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Position = PositionTextBox.Text.Trim(),
                Department = DepartmentTextBox.Text.Trim(),
                PhoneNumber = PhoneNumberTextBox.Text.Trim(),
                PhotoPath = EmployeePhoto.Source is BitmapImage image ? image.UriSource.LocalPath : ""
            };

            var user_task = user;

            var json = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            });

            Console.WriteLine($"Отправляемый JSON: {json}"); // Для отладки

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://192.168.0.78:8080/api/users", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Ответ сервера: {response.StatusCode} - {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Регистрация успешна!");
                        MyFrame.Content = new SignIn();
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        private void Button_LoadPhoto(object sender, RoutedEventArgs e)
        {
            // Создаем диалоговое окно для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружаем выбранное изображение
                string filePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.EndInit();

                // Отображаем изображение в Image
                EmployeePhoto.Source = bitmap;
            }
        }
    }
}
