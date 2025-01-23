using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using System.Net.Http.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string _receivedFullName;
        private string _receivedPosition;
        private string _receivedDepartment;
        private User currentUser;
        private List<User> users;
        private TaskUser taskUsers;

        public MainPage(string fullName, string department, string position)
        {
            InitializeComponent();
            _receivedFullName = fullName;
            ReceivedFullNameText.Text = _receivedFullName;

            _receivedPosition = position;
            ReceivedPositionText.Text = _receivedPosition;

            _receivedDepartment = department;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.0.78:8080/");

            currentUser = new User
            {
                FullName = _receivedFullName,
                Position = _receivedPosition,
                Department = _receivedDepartment
            };

            LoadDataAsync();
        }
        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MyFrame.Content = new SignIn();
        }

        static HttpClient httpClient = new HttpClient();
        private async Task<List<User>> GetUsersAsync()
        {
            try
            {
                // Отправляем GET-запрос на сервер
                var response = await httpClient.GetAsync("/api/users");

                if (response.IsSuccessStatusCode)
                {
                    // Десериализуем JSON-ответ в список пользователей
                    var users = await response.Content.ReadFromJsonAsync<List<User>>();
                    return users;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении данных с сервера.");
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return new List<User>();
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Получаем список пользователей с сервера
                users = await GetUsersAsync();

                UpdateAssigneeComboBox(); // Вызываем метод здесь

                // Если текущий пользователь уже установлен (например, после авторизации), обновляем интерфейс
                if (currentUser != null)
                {
                    UpdateUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }

            
        }

        private void UpdateAssigneeComboBox()
        {
            if (users == null || !users.Any())
            {
                MessageBox.Show("Список пользователей пуст.");
                return;
            }

            if (currentUser == null)
            {
                MessageBox.Show("Текущий пользователь не установлен.");
                return;
            }

            if (currentUser.Position == "Директор" || currentUser.Position == "Зам директора")
            {
                // Директор и зам директора могут назначить задачу всем
                AssigneeComboBox.ItemsSource = users;
            }
            else if (currentUser.Position == "Начальник")
            {
                // Начальник может назначить задачу только работникам своего отдела
                AssigneeComboBox.ItemsSource = users
                    .Where(u => u.Department == currentUser.Department && u.Position != "Начальник")
                    .ToList();
            }
            else
            {
                // Работники не могут назначать задачи
                AssigneeComboBox.IsEnabled = false;
            }
        }
        private void UpdateUI()
        {
            if (currentUser != null)
            {
                ReceivedFullNameText.Text = currentUser.FullName;
                ReceivedPositionText.Text = currentUser.Position;
            }
            else
            {
                ReceivedFullNameText.Text = "Не авторизован";
                ReceivedPositionText.Text = "Не авторизован";
            }
        }
        private async void ShowTaskForUser_Click(object sender, RoutedEventArgs e)
        {
            await LoadTasksForUserAsync(taskUsers.AssignedToUserId); // Загружаем задачу для пользователя
        }
        private async Task LoadTasksForUserAsync(int userId)
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    // Получаем задачу и связанного пользователя
                    var taskWithUser = await dbContext.Tasks
                        .Include(t => t.AssignedToUser) // Подгружаем связанного пользователя
                        .FirstOrDefaultAsync(t => t.AssignedToUserId == userId);

                    if (taskWithUser != null)
                    {
                        // Логируем данные задачи и пользователя
                        Console.WriteLine($"Задача: {taskWithUser.Title}, назначена пользователю: {taskWithUser.AssignedToUser.FullName}");

                        // Отображаем данные задачи
                        TaskTitleTextBox.Text = taskWithUser.Title;
                        TaskDescriptionTextBox.Text = taskWithUser.Description;
                        TaskDueDatePicker.SelectedDate = taskWithUser.DueDate;

                        //// Отображаем данные пользователя
                        //AssignedToUserTextBlock.Text = taskWithUser.AssignedToUser.FullName;
                        //AssignedToUserPositionTextBlock.Text = taskWithUser.AssignedToUser.Position;
                        //AssignedToUserDepartmentTextBlock.Text = taskWithUser.AssignedToUser.Department;
                    }
                    else
                    {
                        MessageBox.Show("Задача не найдена.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке задачи: {ex.Message}");
            }
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                // Используем контекст базы данных для загрузки задач
                using (var dbContext = new AppDbContext())
                {
                    // Загружаем задачи из базы данных
                    var tasks = await dbContext.Tasks
                        .Include(t => t.AssignedToUser) // Подгружаем связанного пользователя
                        .ToListAsync();

                    // Обновляем ListView
                    TasksListView.ItemsSource = tasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке задач: {ex.Message}");
            }
        }
        private async void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Вы не авторизованы.");
                return;
            }

            if (AssigneeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите работника для назначения задачи.");
                return;
            }

            // Получаем выбранного пользователя
            var selectedUser = AssigneeComboBox.SelectedItem as User;

            // Создаем задачу
            var newTask = new TaskUser
            {
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                DueDate = TaskDueDatePicker.SelectedDate ?? DateTime.Now,
                IsCompleted = false,
                AssignedToUserId = selectedUser.Id // Привязываем задачу к пользователю через Id
            };

            try
            {
                // Сохраняем задачу в базу данных
                using (var dbContext = new AppDbContext())
                {
                    dbContext.Tasks.Add(newTask);
                    await dbContext.SaveChangesAsync();
                }

                // Логируем создание задачи
                Console.WriteLine($"Задача создана: {newTask.Title}, назначена пользователю: {selectedUser.FullName}");

                // Очищаем поля после создания задачи
                TaskTitleTextBox.Clear();
                TaskDescriptionTextBox.Clear();
                TaskDueDatePicker.SelectedDate = null;

                // Обновляем список задач для выбранного пользователя
                await UpdateTasksForUserAsync(selectedUser.Id);

                // Сообщаем о назначении задачи
                MessageBox.Show($"Задача \"{newTask.Title}\" успешно назначена пользователю {selectedUser.FullName}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании задачи: {ex.Message}");
            }
        }

        private async Task UpdateTasksForUserAsync(int userId)
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    // Получаем задачи, назначенные данному пользователю
                    var userTasks = await dbContext.Tasks
                        .Where(t => t.AssignedToUserId == userId)
                        .ToListAsync();

                    // Обновляем ListView или другой элемент интерфейса для отображения задач
                    TasksListView.ItemsSource = userTasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении задач для пользователя: {ex.Message}");
            }
        }

        public class AppDbContext : DbContext
        {
            public DbSet<User> Users { get; set; } // Таблица пользователей
            public DbSet<TaskUser> Tasks { get; set; } // Таблица задач

            protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseMySql(
                    "server=192.168.0.78;database=project2;user=root2;password=000000;",
                    new MySqlServerVersion(new Version(8, 0, 23))
            );

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Настройка первичного ключа для TaskUser
                modelBuilder.Entity<TaskUser>()
                    .HasKey(t => t.Id); // Указываем, что Id является первичным ключом

                // Настройка связи между TaskUser и User
                modelBuilder.Entity<TaskUser>()
                    .HasOne(t => t.AssignedToUser) // Задача связана с одним пользователем
                    .WithMany() // У пользователя может быть много задач
                    .HasForeignKey(t => t.AssignedToUserId); // Внешний ключ
            }
        }

    }
}
