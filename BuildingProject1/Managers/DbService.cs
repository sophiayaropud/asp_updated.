using Dapper;
using MySql.Data.MySqlClient;

namespace BuildingProject.Managers
{
    public static class DbService
    {
        private static MySqlConnection _conn;

        private static string _connectionString = "Server=127.0.0.1;Database=buildingproject;Uid=root;Pwd=root;";

        public static MySqlConnection GetConnection()
        {
            if (_conn == null)
            {
                _conn = new MySqlConnection(_connectionString);
                _conn.Open();
                _conn.Execute(dbCreationScript);
            }

            return _conn;
        }

        private static string dbCreationScript =>
"        CREATE TABLE IF NOT EXISTS `buildings` ("
+ "  `Id` char (36) DEFAULT NULL,"
+ "   `Street` varchar(255) DEFAULT NULL,"
+ "   `Number` varchar(255) DEFAULT NULL,"
+ "   `Square` double DEFAULT NULL,"
+ "  `Price` double DEFAULT NULL,"
+ "  `Year` int DEFAULT NULL"
+ ");"
+ "CREATE TABLE IF NOT EXISTS `userbuildings` ("
+ "  `UserId` char (36) DEFAULT NULL,"
+ "   `BuildingId` char (36) DEFAULT NULL"
+ ");"
+ "        CREATE TABLE IF NOT EXISTS `users` ("
+ "  `Id` char (36) DEFAULT NULL,"
+ "   `LastName` varchar(255) DEFAULT NULL,"
+ "   `FirstName` varchar(255) DEFAULT NULL,"
+ "   `Email` varchar(255) DEFAULT NULL,"
+ "   `PasswordHash` varchar(255) DEFAULT NULL,"
+ "   `RoleId` int DEFAULT NULL,"
+ "   `ActiveStatus` TinyInt(1) DEFAULT 1"
+ ");";
    }
}