using System;

namespace BuildingProject.Models
{
    public class BuildingTable
    {
        public Guid Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public double Square { get; set; }

        public double Price { get; set; }

        public int Year { get; set; }
    }
}