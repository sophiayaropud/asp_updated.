using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using BuildingProject.Managers;
using BuildingProject.Models;

namespace BuildingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private UserManager _userManager;
        private bool _isRegistration;

        public RegistrationWindow(bool isRegistration = false)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _userManager = new UserManager();
            _isRegistration = isRegistration;
        }

        public void Register(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var userCreated = _userManager.RegisterUser(GetUser());

                if (userCreated)
                {
                    if (_isRegistration)
                    {
                        MessageBoxResult result = MessageBox.Show("User has been created. You will be navigated to the login window.", "Creation Result", MessageBoxButton.OK);

                        if (result == MessageBoxResult.OK)
                        {
                            var loginWindow = new LoginWindow();
                            loginWindow.Show();
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("User has been created", "Creation Result", MessageBoxButton.OK);
                    }
                }
            }
        }

        private bool IsValid()
        {
            if (!Regex.Match(FirstName.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                MessageBox.Show("Invalid first name", "Validation Error");
                return false;
            }

            if (!Regex.Match(LastName.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                MessageBox.Show("Invalid last name", "Validation Error");
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(Email.Text))
            {
                MessageBox.Show("Invalid email", "Validation Error");
                return false;
            }

            if (string.IsNullOrEmpty(Password.Text))
            {
                MessageBox.Show("Invalid password", "Validation Error");
                return false;
            }
            else
            {
                if (Password.Text != ConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords does not match", "Validation Error");
                    return false;
                }
            }

            if (Role.SelectedItem == null)
            {
                MessageBox.Show("Select Role", "Validation Error");
                return false;
            }

            return true;
        }

        public void Back(object sender, RoutedEventArgs e)
        {
            if (_isRegistration)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
            else
            {
                var usersWindow = new UsersWindow();
                usersWindow.Show();
                Close();
            }
        }

        private UserModel GetUser() => new UserModel(FirstName.Text, LastName.Text, Email.Text, Password.Text, Role.SelectedIndex);
    }
}
