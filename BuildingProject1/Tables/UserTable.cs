using System;

namespace BuildingProject.Models
{
    public class UserTable
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public byte ActiveStatus { get; set; }
    }
}