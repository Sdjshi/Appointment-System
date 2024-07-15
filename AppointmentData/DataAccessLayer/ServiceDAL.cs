using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.DataAccessLayer
{
    public class ServiceDAL : IServiceDAL
    {
        private readonly IConfiguration _configuration;

        public ServiceDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }
        public IEnumerable<Service> GetAllService()
        {
            List<Service> listServices = new List<Service>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPGetAllService", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Service Service = new Service();
                    Service.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    Service.ServiceName = reader["ServiceName"].ToString();
                    Service.Description = reader["Description"].ToString();
                    Service.Duration = Convert.ToInt32(reader["Duration"]);
                    Service.MSP = Convert.ToInt32(reader["MSP"]);
                    Service.MRP = Convert.ToInt32(reader["MRP"]);
                    Service.OfficeId = Convert.ToInt32(reader["OfficeId"]);

                    listServices.Add(Service);
                }
                con.Close();
            }
            return listServices;
        }
        public void AddService(Service service)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPAddService", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", service.ServiceID);

                sqlCommand.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                sqlCommand.Parameters.AddWithValue("@Description", service.Description);
                sqlCommand.Parameters.AddWithValue("@Duration", service.Duration);
                sqlCommand.Parameters.AddWithValue("@MSP", service.MSP);
                sqlCommand.Parameters.AddWithValue("@MRP", service.MRP);
                sqlCommand.Parameters.AddWithValue("@OfficeId", service.OfficeId);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                sqlCommand.Parameters.AddWithValue("@Status", 1);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
        public void EditService(Service service)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPEditService", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", service.ServiceID);

                sqlCommand.Parameters.AddWithValue("@ServiceName", service.ServiceName);
                sqlCommand.Parameters.AddWithValue("@Description", service.Description);
                sqlCommand.Parameters.AddWithValue("@Duration", service.Duration);
                sqlCommand.Parameters.AddWithValue("@MSP", service.MSP);
                sqlCommand.Parameters.AddWithValue("@MRP", service.MRP);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
        public void StatusDeleteService(int? id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPdeleteService", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", id);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public Service GetServiceData(int? ServiceID)
        {
            Service service = new Service();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM Services WHERE ServiceID =@ServiceID";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@ServiceID", ServiceID);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    service.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    service.ServiceName = reader["ServiceName"].ToString();
                    service.Description = reader["Description"].ToString();
                    service.Duration = Convert.ToInt32(reader["Duration"]);
                    service.MSP = Convert.ToInt32(reader["MSP"]);
                    service.MRP = Convert.ToInt32(reader["MRP"]);
                    service.OfficeId = Convert.ToInt32(reader["OfficeId"]);
                    service.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    service.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);


                }
                con.Close();
            }
            return service;
        }

        public void AssignService(int serviceId, int serviceProviderId)
        {
           
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand("AssignServiceStoredProcedure", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ServiceID", serviceId);
                command.Parameters.AddWithValue("@ServiceProviderID", serviceProviderId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ServiceAndServiceProvider> GetAllServiceAndServiceProviders()
        {
            List<ServiceAndServiceProvider> listServices = new List<ServiceAndServiceProvider>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPGetAllServiceAndServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceAndServiceProvider serviceAndServiceProvider = new ServiceAndServiceProvider();
                    serviceAndServiceProvider.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    serviceAndServiceProvider.ServiceProviderID = Convert.ToInt32(reader["ServiceID"]);
                    serviceAndServiceProvider.Duration = Convert.ToInt32(reader["ServiceID"]);
                    serviceAndServiceProvider.ServiceName = reader["ServiceName"].ToString();
                    serviceAndServiceProvider.ServiceProviderName = reader["ServiceProviderName"].ToString();


                    listServices.Add(serviceAndServiceProvider);
                }
                con.Close();
            }
            return listServices;
        }


        public IEnumerable<Service> GetAllAssignedServices()
        {
            List<Service> listServices = new List<Service>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetAllAssignedServices", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Service Service = new Service();
                    Service.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    Service.Duration = Convert.ToInt32(reader["Duration"]);
                    Service.ServiceName = reader["ServiceName"].ToString();


                    listServices.Add(Service);
                }
                con.Close();
            }
            return listServices;
        }

        public void UnassignServiceToServiceProvider(int serviceID, int providerID)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("UnassignServiceToServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", serviceID);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", providerID);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
    }

}
