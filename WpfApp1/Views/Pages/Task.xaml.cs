using System.Windows.Controls;
using System.Windows.Input;


namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Task.xaml
    /// </summary>
    public partial class Task : Page
    {
        public Task()
        {
            InitializeComponent();
        }
        private void StackPanel_MouseLeftButtonOut(object sender, MouseButtonEventArgs e)
        {
            MyFrame.Content = new SignIn();
        }
    }
}
