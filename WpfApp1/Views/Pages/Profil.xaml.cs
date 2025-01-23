using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
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
using WpfApp1.Models;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profil.xaml
    /// </summary>
    public partial class Profil : Page
    {
        private string _username;
        private string _password;
        private string _fullName;
        private string _position;
        private string _department;
        private string _gender;
        private DateTime _birthday;
        private string _phone;
        private string _pathPhoto;
        private string _email;

        public Profil(User user)
        {
            InitializeComponent();
            DataContext = user;
            _username = user.Username;
            _fullName = user.FullName;
            _position = user.Position;
            _department = user.Department;
            _gender = user.Gender;
            _birthday = user.BirthDate;
            _phone = user.PhoneNumber;
            _pathPhoto = user.PhotoPath;
            _email = user.Email;
            

        }
        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MyFrame.Content = new SignIn();
        }
        private async void Task_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MyFrame.Content = new MainPage(_fullName, _department, _position);
            //var Data = new
            //{
            //    UserName = _username,
            //    FullName = _fullName,
            //    Possition = _position,
            //    Department = _department,
            //    BirthDate = _birthday,
            //    Gender = _gender,
            //    PhoneNumber = _phone,
            //    PhotoPath = _pathPhoto,
            //    Email = _email

            //};

            //var json = JsonConvert.SerializeObject(Data);

            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //try
            //{
            //    using (var client = new HttpClient())
            //    {
            //        var response = await client.PostAsync("http://192.168.0.7:8080/api/login", content);
            //        var responseContent = await response.Content.ReadAsStringAsync();


            //        if (response.IsSuccessStatusCode)
            //        {
            //            var jsonStart = responseContent.IndexOf('{');
            //            var jsonEnd = responseContent.LastIndexOf('}');

            //            var json_cut = responseContent.Substring(jsonStart, jsonEnd - jsonStart + 1);

            //            var user = JsonConvert.DeserializeObject<User>(json_cut);


            //            MyFrame.Content = new MainPage(user);
            //        }
            //        else
            //        {
            //            MessageBox.Show($"Ошибка страницы!: {responseContent}");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка подключения: {ex.Message}");
            //}
        }

    }
}
