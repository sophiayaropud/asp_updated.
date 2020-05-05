using BuildingProject.Managers;
using BuildingProject.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BuildingProject
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class UsersWindow : Window
    {
		public Visibility IsAdmin => UserManager.CurrentUser.Role == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;

		public UsersWindow()
		{
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			InitializeComponent();

			_userManager = new UserManager();

			lvUsers.ItemsSource = _userManager.GetUsers();
		}

		private GridViewColumnHeader listViewSortCol = null;
		private SortAdorner listViewSortAdorner = null;
		private UserManager _userManager;

		private void Sort_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = (sender as GridViewColumnHeader);
			string sortBy = column.Tag.ToString();
			if (listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
				lvUsers.Items.SortDescriptions.Clear();
			}

			ListSortDirection newDir = ListSortDirection.Ascending;
			if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
				newDir = ListSortDirection.Descending;

			listViewSortCol = column;
			listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
			lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		private void AddUser(object sender, RoutedEventArgs e)
		{
			var registrationUser = new RegistrationWindow();
			registrationUser.Show();
			Close();
		}

		private void ViewBuildings(object sender, RoutedEventArgs e)
		{
			var userId = (Guid)((Button)sender).Tag;
			var buildingsWindow = new BuildingsWindow(userId);
			buildingsWindow.Show();
			Close();
		}

		private void ChangeStatus(object sender, RoutedEventArgs e)
		{
			var user = (UserModel)((Button)sender).Tag;
			var newStatus = user.Status == ActiveStatus.Active ? ActiveStatus.Inactive : ActiveStatus.Active;

			MessageBoxResult result = MessageBox.Show($"Are you sure? You want to change user status from {user.StatusName} to {newStatus:F}", "Confirmation", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				var deleteResult = _userManager.ChangeStatus(user.Id, newStatus);

				if (deleteResult)
				{
					MessageBox.Show("User status has been updated.", "Update Result", MessageBoxButton.OK);

					Search(null, null);
				}
			}
		}

		private void Search(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(SearchValue.Text))
			{
				lvUsers.ItemsSource = _userManager.GetUsers();
			}
			else
			{
				lvUsers.ItemsSource = _userManager.GetUsers().Where(
					b => b.Id.ToString().Contains(SearchValue.Text) ||
					b.FirstName.Contains(SearchValue.Text) ||
					b.LastName.Contains(SearchValue.Text) ||
					b.Email.ToString().Contains(SearchValue.Text) ||
					b.RoleName.ToString().Contains(SearchValue.Text));
			}
		}

		private void Logout(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}
	}
}
