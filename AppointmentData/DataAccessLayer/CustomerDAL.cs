using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.DataAccessLayer
{
    public class CustomerDAL : ICustomerDAL
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomerDAL> _logger;

        public CustomerDAL(IConfiguration configuration, ILogger<CustomerDAL> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }
        public IEnumerable<Appointment> GetAllCustomer()
        {
            List<Appointment> listCustomers = new List<Appointment>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Customer_GetAll", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Appointment Customer = new Appointment();
                    //Customer.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    Customer.CustomerFullName = reader["CustomerFullName"].ToString();
                    Customer.CustomerPhone = reader["CustomerPhone"].ToString();
                    Customer.CustomerAddress = reader["CustomerAddress"].ToString();
                    Customer.CustomerEmail = reader["CustomerEmail"].ToString();


                    listCustomers.Add(Customer);
                }
                con.Close();
            }
            return listCustomers;
        }


        public void AddCustomer(Customer Customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    SqlCommand sqlCommand = new SqlCommand("Customer_Create", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;


                    sqlCommand.Parameters.AddWithValue("@CustomerFullName", Customer.CustomerFullName);
                    sqlCommand.Parameters.AddWithValue("@CustomerPhone", Customer.CustomerPhone);
                    sqlCommand.Parameters.AddWithValue("@CustomerAddress", Customer.CustomerAddress);
                    sqlCommand.Parameters.AddWithValue("@CustomerEmail", Customer.CustomerEmail);

                    sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                    sqlCommand.Parameters.AddWithValue("@Status", 1);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error occurred while adding customer: {ex.Message}");

                throw;
            }
        }



        public void EditCustomer(Customer Customer)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Customer_Edit", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CustomerId", Customer.CustomerId);
                sqlCommand.Parameters.AddWithValue("@CustomerFullName", Customer.CustomerFullName);
                sqlCommand.Parameters.AddWithValue("@CustomerPhone", Customer.CustomerPhone);
                sqlCommand.Parameters.AddWithValue("@CustomerAddress", Customer.CustomerAddress);
                sqlCommand.Parameters.AddWithValue("@CustomerEmail", Customer.CustomerEmail);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
        public void StatusDeleteCustomer(int? id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Customer_Delete", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CustomerId", id);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public Customer GetCustomerData(int? CustomerId)
        {
            Customer Customer = new Customer();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM Customers WHERE CustomerID =@CustomerID";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@CustomerId", CustomerId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    Customer.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    Customer.CustomerFullName = reader["CustomerFullName"].ToString();
                    Customer.CustomerPhone = reader["CustomerPhone"].ToString();
                    Customer.CustomerAddress = reader["CustomerAddress"].ToString();
                    Customer.CustomerAddress = reader["CustomerAddress"].ToString();
                    Customer.CustomerEmail = reader["CustomerEmail"].ToString();


                    Customer.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    Customer.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);


                }
                con.Close();
            }
            return Customer;
        }

        public Customer GetCustomerByPhone(string phoneNumber)
        {
            Customer Customer = new Customer();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM Customers WHERE CustomerPhone =@CustomerPhone and Status =1";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@CustomerPhone", phoneNumber);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    Customer.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                    Customer.CustomerFullName = reader["CustomerFullName"].ToString();
                    Customer.CustomerPhone = reader["CustomerPhone"].ToString();
                    Customer.CustomerAddress = reader["CustomerAddress"].ToString();                 
                    Customer.CustomerEmail = reader["CustomerEmail"].ToString();

                    Customer.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    Customer.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);


                }
                con.Close();
            }
            return Customer;
        }
    }
}
