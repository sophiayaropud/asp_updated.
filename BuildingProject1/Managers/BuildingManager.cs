using BuildingProject.Models;
using Dapper;
using System;
using System.Collections.Generic;

namespace BuildingProject.Managers
{
    public class BuildingManager
    {
        public List<BuildingModel> GetBuildings(Guid userId)
        {
            var connection = DbService.GetConnection();

            if(UserManager.CurrentUser.Role == UserRole.Seller || UserManager.CurrentUser.Role == UserRole.Admin)
            {
                return connection.Query<BuildingModel>("select b.* FROM UserBuildings AS ub JOIN Buildings as b ON b.Id = ub.BuildingId WHERE ub.UserId = @userId", new { userId }).AsList();
            }

            return connection.Query<BuildingModel>("select * FROM Buildings").AsList();
        }

        public bool AddBuilding(BuildingModel buildingModel)
        {
            var connection = DbService.GetConnection();

            var buildingId = Guid.NewGuid();

            string sql = "INSERT INTO Buildings (Id, Street, Number, Square, Price, Year) Values (@Id, @Street, @Number, @Square, @Price, @Year);";

            var affectedRowsCount = connection.Execute(sql, new { Id = buildingId, Street = buildingModel.Street, Number = buildingModel.Number, Square = buildingModel.Square, Price = buildingModel.Price, Year = buildingModel.Year });

            if(affectedRowsCount > 0)
            {
                affectedRowsCount = connection.Execute("INSERT INTO UserBuildings (UserId, BuildingId) Values (@UserId, @BuildingId);", new { UserId = UserManager.CurrentUser.Id, BuildingId = buildingId });

                return affectedRowsCount > 0;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateBuilding(BuildingModel buildingModel)
        {
            var connection = DbService.GetConnection();

            string sql = "UPDATE Buildings SET Street = @Street, Number = @Number, Square = @Square, Price = @Price, Year = @Year Where Id = @Id";

            var affectedRowsCount = connection.Execute(sql, new { Street = buildingModel.Street, Number = buildingModel.Number, Square = buildingModel.Square, Price = buildingModel.Price, Year = buildingModel.Year, Id = buildingModel.Id});

            return affectedRowsCount > 0;
        }

        public bool DeleteBuilding(Guid buildingId)
        {
            var connection = DbService.GetConnection();

            string sql = "DELETE FROM Buildings WHERE Id = @Id";

            var affectedRowsCount = connection.Execute(sql, new { Id = buildingId});

            affectedRowsCount += connection.Execute("DELETE FROM UserBuildings WHERE BuildingId = @BuildingId", new { BuildingId = buildingId });

            return affectedRowsCount > 0;
        }
    }
}
