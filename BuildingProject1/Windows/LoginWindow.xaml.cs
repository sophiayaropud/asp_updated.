using BuildingProject.Managers;
using System.Windows;
using System.Windows.Input;

namespace BuildingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserManager _userManager;

        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _userManager = new UserManager();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            if (_userManager.LoginUser(Email.Text, Password.Password))
            {
                if(UserManager.CurrentUser.Role == Models.UserRole.Admin)
                {
                    var usersWindow = new UsersWindow();
                    usersWindow.Show();
                    Close();

                    return;
                }

                var buildingsWindow = new BuildingsWindow();
                buildingsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid user data. User does not exist.", "Login Result", MessageBoxButton.OK);
            }
        }

        private void Register(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegistrationWindow(true);
            registerWindow.Show();
            Close();
        }

        private void ForgotPassword(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("It is very sad.", "Forget Password", MessageBoxButton.OK);
        }
    }
}
