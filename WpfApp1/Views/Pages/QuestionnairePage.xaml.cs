using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySqlConnector;

namespace WpfApp1
{
    public partial class QuestionnairePage : Page
    {
        // Строка подключения к MariaDB
        private const string ConnectionString = "Server=192.168.0.5;Database=project;User ID=root4;Password=000000;";
        string username = "ara228"; // Полученное значение
        NavigationService.Navigate(new QuestionnairePage(username));

        public QuestionnairePage()
        {
            InitializeComponent();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {

            // Получаем данные из полей ввода
            string fullName = FullNameTextBox.Text;
            DateTime? birthDate = BirthDatePicker.SelectedDate;
            string gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string position = PositionTextBox.Text;
            string department = DepartmentTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string photoPath = EmployeePhoto.Source?.ToString(); // Путь к фото (если загружено)

            // Проверяем, что обязательные поля заполнены
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Поле 'ФИО' обязательно для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Подключаемся к базе данных
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных
                    var query = "INSERT INTO Users (FullName, BirthDate, Gender, Position, Department, PhoneNumber, PhotoPath) " +
                                "VALUES (@FullName, @BirthDate, @Gender, @Position, @Department, @PhoneNumber, @PhotoPath)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Добавляем параметры
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@BirthDate", birthDate);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@Position", position);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@PhotoPath", photoPath);

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Анкета успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при сохранении анкеты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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