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
	public partial class BuildingsWindow : Window
    {
		public Visibility IsSeller => UserManager.CurrentUser.Role == UserRole.Seller ? Visibility.Visible : Visibility.Collapsed;
		public Visibility IsAdmin => UserManager.CurrentUser.Role == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;

		public BuildingsWindow(Guid? userId = null)
		{
			_userId = userId ?? UserManager.CurrentUser.Id;
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			InitializeComponent();

			_buildingManager = new BuildingManager();

			lvUsers.ItemsSource = _buildingManager.GetBuildings(_userId);
		}

		private GridViewColumnHeader listViewSortCol = null;
		private SortAdorner listViewSortAdorner = null;
		private BuildingManager _buildingManager;
		private Guid _userId;

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

		private void BackToUsers(object sender, RoutedEventArgs e)
		{
			var usersWindow = new UsersWindow();
			usersWindow.Show();
			Close();
		}

		private void AddBuilding(object sender, RoutedEventArgs e)
		{
			var addBuildingsWindow = new AddEditBuildingWindow();
			addBuildingsWindow.Show();
			Close();
		}

		private void EditBuilding(object sender, RoutedEventArgs e)
		{
			var editBuilding = (BuildingModel)((Button)sender).Tag;
			var addBuildingsWindow = new AddEditBuildingWindow(editBuilding);
			addBuildingsWindow.Show();
			Close();
		}

		private void Delete(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				var buildingId = (Guid)((Button)sender).Tag;

				var deleteResult = _buildingManager.DeleteBuilding(buildingId);

				if (deleteResult)
				{
					MessageBox.Show("Building has been deleted.", "Delete Result", MessageBoxButton.OK);

					lvUsers.ItemsSource = _buildingManager.GetBuildings(_userId);
				}
			}
		}

		private void Search(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(SearchValue.Text))
			{
				lvUsers.ItemsSource = _buildingManager.GetBuildings(_userId);
			}
			else
			{
				lvUsers.ItemsSource = _buildingManager.GetBuildings(_userId).Where(
					b => b.Id.ToString().Contains(SearchValue.Text) ||
					b.Street.Contains(SearchValue.Text) ||
					b.Number.Contains(SearchValue.Text) ||
					b.Square.ToString().Contains(SearchValue.Text) ||
					b.Price.ToString().Contains(SearchValue.Text) ||
					b.Year.ToString().Contains(SearchValue.Text));
			}
		}

		private void Logout(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}
	}

	public class SortAdorner : Adorner
	{
		private static Geometry ascGeometry =
			Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

		private static Geometry descGeometry =
			Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

		public ListSortDirection Direction { get; private set; }

		public SortAdorner(UIElement element, ListSortDirection dir)
			: base(element)
		{
			this.Direction = dir;
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			if (AdornedElement.RenderSize.Width < 20)
				return;

			TranslateTransform transform = new TranslateTransform
				(
					AdornedElement.RenderSize.Width - 15,
					(AdornedElement.RenderSize.Height - 5) / 2
				);
			drawingContext.PushTransform(transform);

			Geometry geometry = ascGeometry;
			if (this.Direction == ListSortDirection.Descending)
				geometry = descGeometry;
			drawingContext.DrawGeometry(Brushes.Black, null, geometry);

			drawingContext.Pop();
		}
	}
}
