using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WpfApp1.Models;
using WpfApp1.Views.Pages;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        private static readonly HttpClient test = new HttpClient();
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

            var loginData = new
            {
                Username = TextBoxLogin.Text.Trim(),
                Password = passBox.Password
            };

            var json = JsonConvert.SerializeObject(loginData, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://192.168.0.78:8080/api/login", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    //Console.WriteLine($"Ответ сервера: {response.StatusCode} - {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Вход выполнен!");
                        var jsonStart = responseContent.IndexOf('{');
                        var jsonEnd = responseContent.LastIndexOf('}');

                        var json_cut = responseContent.Substring(jsonStart, jsonEnd - jsonStart + 1);

                        var user = JsonConvert.DeserializeObject<User>(json_cut);


                        //var user = JsonConvert.DeserializeObject<User>(responseContent);

                        //var userPage = new Profil(user);
                        //NavigationService.Navigate(userPage);
                        MyFrame.Content = new Profil(user);
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка авторизации!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        

        }
    }
}
