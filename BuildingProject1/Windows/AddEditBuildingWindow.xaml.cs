using System.Windows;
using BuildingProject.Managers;
using BuildingProject.Models;
using System;

namespace BuildingProject
{
    public partial class AddEditBuildingWindow : Window
    {
        private BuildingManager _buildingManager;
        private bool _isEdit;
        private BuildingModel _editModel;

        public AddEditBuildingWindow(BuildingModel editModel = null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _buildingManager = new BuildingManager();

            if (editModel != null)
            {
                _isEdit = true;
                Street.Text = editModel.Street;
                Number.Text = editModel.Number;
                Square.Text = editModel.Square.ToString();
                Price.Text = editModel.Price.ToString();
                Year.Text = editModel.Year.ToString();
                _editModel = editModel;
            }
            else
            {
                _isEdit = false;
            }
        }

        public void Confirm(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (!_isEdit)
                {
                    var buildingCreated = _buildingManager.AddBuilding(GetBuilding());

                    if (buildingCreated)
                    {
                        MessageBoxResult result = MessageBox.Show("Building has been created. You will be navigated to the buildings window.", "Creation Result", MessageBoxButton.OK);

                        if (result == MessageBoxResult.OK)
                        {
                            Back(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Building creation has been failed.", "Creation Result", MessageBoxButton.OK);
                    }
                }
                else
                {
                    var buildingUpdated = _buildingManager.UpdateBuilding(GetUpdatedBuilding());

                    if (buildingUpdated)
                    {
                        MessageBoxResult result = MessageBox.Show("Building has been updated. You will be navigated to the buildings window.", "Update Result", MessageBoxButton.OK);

                        if (result == MessageBoxResult.OK)
                        {
                            Back(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Building update has been failed.", "Update Result", MessageBoxButton.OK);
                    }
                }
            }
        }

        public void Back(object sender, RoutedEventArgs e)
        {
            var buildingsWindow = new BuildingsWindow();
            buildingsWindow.Show();
            Close();
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Street.Text))
            {
                MessageBox.Show("Street can not be empty", "Validation Error");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Number.Text))
            {
                MessageBox.Show("Number can not be empty", "Validation Error");
                return false;
            }

            if (!double.TryParse(Square.Text, out var square) || square < 0)
            {
                MessageBox.Show("Invalid square value", "Validation Error");
                return false;
            }

            if (!double.TryParse(Price.Text, out var price) || price < 0)
            {
                MessageBox.Show("Invalid price value", "Validation Error");
                return false;
            }

            if (!int.TryParse(Year.Text, out var year) || year > DateTime.UtcNow.Year)
            {
                MessageBox.Show("Invalid year value", "Validation Error");
                return false;
            }

            return true;
        }

        private BuildingModel GetBuilding() => new BuildingModel(Street.Text, Number.Text, Square.Text, Price.Text, Year.Text);

        private BuildingModel GetUpdatedBuilding() {
            _editModel.Update(Street.Text, Number.Text, Square.Text, Price.Text, Year.Text);

            return _editModel;
        }
    }
}
