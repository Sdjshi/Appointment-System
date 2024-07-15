using AppointmentData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.DataAccessLayer
{
    public class AccountDAL
    {
        private readonly IConfiguration _configuration;

        public AccountDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }

        public User AuthenticateUser(string username, string password)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                var parameters = new { Username = username, Password = password };
                return connection.QueryFirstOrDefault<User>(query, parameters);
            }
        }

        public IEnumerable<Role> GetUserRoles(int userId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = @"SELECT r.*
                          FROM Roles r
                          JOIN UserRoles ur ON r.RoleId = ur.RoleId
                          WHERE ur.UserId = @UserId";
                var parameters = new { UserId = userId };
                return connection.Query<Role>(query, parameters);
            }
        }
        public List<string> GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = "SELECT RoleName FROM UserRoles join Roles on UserRoles.RoleId = Roles.RoleId join Users on Users.UserId = UserRoles.UserID Where Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return roles;
        }
        public bool CreateUser(AccountRegisterModel user)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand("CreateUserLogin", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", user.Username);

                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    command.Parameters.AddWithValue("@ServiceProviderID", user.ServiceProviderID);


                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public List<Role> GetRoles()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = "SELECT RoleId, RoleName FROM Roles";
                return connection.Query<Role>(query).AsList();
            }
        }
    }
}
