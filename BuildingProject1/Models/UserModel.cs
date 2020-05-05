using System;

namespace BuildingProject.Models
{
    public class UserModel
    {
        public UserModel(UserTable userTable)
        {
            Id = userTable.Id;
            FirstName = userTable.FirstName;
            LastName = userTable.LastName;
            Email = userTable.Email;
            Role = (UserRole)userTable.RoleId;
            Status = (ActiveStatus)userTable.ActiveStatus;
        }

        public UserModel(string firstName, string lastName, string email, string password, int roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = (UserRole)roleId;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public string RoleName => $"{Role:F}";

        public ActiveStatus Status { get; set; }

        public string StatusName => $"{Status:F}";
    }
}