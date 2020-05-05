using System;

namespace BuildingProject.Models
{
    public class BuildingModel : BuildingTable
    {
        public BuildingModel()
        {

        }

        public BuildingModel(string street, string number, string square, string price, string year)
        {
            Street = street;
            Number = number;
            Square = double.Parse(square);
            Price = double.Parse(price);
            Year = int.Parse(year);
        }

        public void Update(string street, string number, string square, string price, string year)
        {
            Street = street;
            Number = number;
            Square = double.Parse(square);
            Price = double.Parse(price);
            Year = int.Parse(year);
        }
    }
}