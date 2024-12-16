﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();
            string pass_2 = passBox_2.Password.Trim();
            string email = TextBoxEmail.Text.Trim().ToLower();

            if(login.Length < 5)
            {
                TextBoxLogin.ToolTip = "Логин должен быть больше 5 символов";
                TextBoxLogin.Background = Brushes.DarkRed;
            }
            else if(pass.Length < 5)
            {
                passBox.ToolTip = "Пароль должен быть больше 5 символов";
                passBox.Background = Brushes.DarkRed;
            }
            else if (pass != pass_2)
            {
                passBox_2.ToolTip = "Это поле введено не корректно";
                passBox_2.Background = Brushes.DarkRed;
            }
            else if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                TextBoxEmail.ToolTip = "Укажите корректную почту";
                TextBoxEmail.Background = Brushes.DarkRed;
            }
            else
            {
                TextBoxLogin.ToolTip = "";
                TextBoxLogin.Background = Brushes.Transparent;
                
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                passBox_2.ToolTip = "";
                passBox_2.Background = Brushes.Transparent;

                TextBoxEmail.ToolTip = "";
                TextBoxEmail.Background = Brushes.Transparent;

                MessageBox.Show("Пользователь зарегистирован");
            }
        }
    }
}