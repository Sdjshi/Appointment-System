using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace AppointmentData.DataAccessLayer
{
    public class ServiceProviderDAL : IServiceProviderDAL
    {
        private readonly IConfiguration _configuration;

        public ServiceProviderDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }
        public IEnumerable<ServiceProviders> GetAllServiceProviders()
        {
            List<ServiceProviders> listServicesProviders = new List<ServiceProviders>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPGetAllServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceProviders serviceProviders = new ServiceProviders();
                    serviceProviders.ServiceProviderID = Convert.ToInt32(reader["ServiceProviderID"]);
                    serviceProviders.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    serviceProviders.ServiceProviderAddress = reader["ServiceProviderAddress"].ToString();
                    serviceProviders.ServiceProviderPhone = reader["ServiceProviderPhone"].ToString();
                    serviceProviders.ServiceProviderEmail = reader["ServiceProviderEmail"].ToString();
                    serviceProviders.ServiceProviderFacebookLink = reader["ServiceProviderFacebookLink"].ToString();
                    serviceProviders.ServiceProviderViber = reader["ServiceProviderViber"].ToString();
                    serviceProviders.ServiceProviderWhatsapp = reader["ServiceProviderWhatsapp"].ToString();
                    serviceProviders.OfficeId = Convert.ToInt32(reader["OfficeId"]);
                    listServicesProviders.Add(serviceProviders);
                }
                con.Close();
            }
            return listServicesProviders;
        }

        public void AddServiceProvider(ServiceProviders serviceProviders)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPAddServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", serviceProviders.ServiceProviderID);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderName", serviceProviders.ServiceProviderName);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderAddress", serviceProviders.ServiceProviderAddress);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderPhone", serviceProviders.ServiceProviderPhone);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviders.ServiceProviderEmail);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderFacebookLink", serviceProviders.ServiceProviderFacebookLink);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderViber", serviceProviders.ServiceProviderViber);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderWhatsapp", serviceProviders.ServiceProviderWhatsapp);
                sqlCommand.Parameters.AddWithValue("@OfficeId", serviceProviders.OfficeId);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@Status", 1);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
        public void EditServiceProvider(ServiceProviders serviceProviders)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPEditServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", serviceProviders.ServiceProviderID);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderName", serviceProviders.ServiceProviderName);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderAddress", serviceProviders.ServiceProviderAddress);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderPhone", serviceProviders.ServiceProviderPhone);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderEmail", serviceProviders.ServiceProviderEmail);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderFacebookLink", serviceProviders.ServiceProviderFacebookLink);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderViber", serviceProviders.ServiceProviderViber);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderWhatsapp", serviceProviders.ServiceProviderWhatsapp);
                sqlCommand.Parameters.AddWithValue("@OfficeId", serviceProviders.OfficeId);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }
        public void StatusDeleteServiceProvider(int? id)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("SPdeleteServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", id);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public ServiceProviders GetServiceProviderData(int? ServiceProviderID)
        {
            ServiceProviders serviceProviders = new ServiceProviders();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM ServiceProviders WHERE ServiceProviderID =@ServiceProviderID";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", ServiceProviderID);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    serviceProviders.ServiceProviderID = Convert.ToInt32(reader["ServiceProviderID"]);
                    serviceProviders.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    serviceProviders.ServiceProviderAddress = reader["ServiceProviderAddress"].ToString();
                    serviceProviders.ServiceProviderPhone = reader["ServiceProviderPhone"].ToString();
                    serviceProviders.ServiceProviderEmail = reader["ServiceProviderEmail"].ToString();
                    serviceProviders.ServiceProviderFacebookLink = reader["ServiceProviderFacebookLink"].ToString();
                    serviceProviders.ServiceProviderViber = reader["ServiceProviderViber"].ToString();
                    serviceProviders.ServiceProviderWhatsapp = reader["ServiceProviderWhatsapp"].ToString();
                    serviceProviders.OfficeId = Convert.ToInt32(reader["OfficeId"]);
                    serviceProviders.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    serviceProviders.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);


                }
                con.Close();
            }
            return serviceProviders;
        }

         public IEnumerable<ServiceProviders> GetServiceAssignedServiceProviders(int? serviceId)
        {
            List<ServiceProviders> listServices = new List<ServiceProviders>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetAssignedServiceProviders", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", serviceId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceProviders serviceProviders = new ServiceProviders();
                    serviceProviders.ServiceProviderID = Convert.ToInt32(reader["ServiceProviderID"]);
                    serviceProviders.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    serviceProviders.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    serviceProviders.ServiceProviderAddress = reader["ServiceProviderAddress"].ToString();
                    serviceProviders.ServiceProviderPhone = reader["ServiceProviderPhone"].ToString();
                    serviceProviders.ServiceProviderEmail = reader["ServiceProviderEmail"].ToString();
                    serviceProviders.ServiceProviderFacebookLink = reader["ServiceProviderFacebookLink"].ToString();
                    serviceProviders.ServiceProviderViber = reader["ServiceProviderViber"].ToString();
                    serviceProviders.ServiceProviderWhatsapp = reader["ServiceProviderWhatsapp"].ToString();



                    listServices.Add(serviceProviders);
                }
                con.Close();
            }
            return listServices;
        }


        public IEnumerable<ServiceProviders> GetServiceAssignedServiceProvidersWithSlots(int? serviceId)
        {
            List<ServiceProviders> listServices = new List<ServiceProviders>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetServiceAssignedServiceProvidersWithSlots", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", serviceId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceProviders serviceProviders = new ServiceProviders();
                    serviceProviders.ServiceProviderID = Convert.ToInt32(reader["ServiceProviderID"]);
                    serviceProviders.ServiceID = Convert.ToInt32(reader["ServiceID"]);
                    serviceProviders.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    serviceProviders.ServiceProviderAddress = reader["ServiceProviderAddress"].ToString();
                    serviceProviders.ServiceProviderPhone = reader["ServiceProviderPhone"].ToString();
                    serviceProviders.ServiceProviderEmail = reader["ServiceProviderEmail"].ToString();
                    serviceProviders.ServiceProviderFacebookLink = reader["ServiceProviderFacebookLink"].ToString();
                    serviceProviders.ServiceProviderViber = reader["ServiceProviderViber"].ToString();
                    serviceProviders.ServiceProviderWhatsapp = reader["ServiceProviderWhatsapp"].ToString();



                    listServices.Add(serviceProviders);
                }
                con.Close();
            }
            return listServices;
        }

        public List<int> GetAssignedServiceProviderIdsForSlot(int slotId)
        {
            List<int> assignedServiceProviderIds = new List<int>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("GetSlotAssignedServiceProviders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.AddWithValue("@SlotId", slotId);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int serviceProviderId = Convert.ToInt32(reader["ServiceProviderId"]);
                                assignedServiceProviderIds.Add(serviceProviderId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return assignedServiceProviderIds;
        }

        public IEnumerable<ServiceProviders> GetAssignedServiceProviderForSlot(int slotId)
        {
            List<ServiceProviders> listServices = new List<ServiceProviders>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetSlotAssignedServiceProviders", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@SlotId", slotId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceProviders serviceProviders = new ServiceProviders();
                    serviceProviders.ServiceProviderID = Convert.ToInt32(reader["ServiceProviderID"]);
                    
                    serviceProviders.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    
                    serviceProviders.ServiceProviderPhone = reader["ServiceProviderPhone"].ToString();
                    



                    listServices.Add(serviceProviders);
                }
                con.Close();
            }
            return listServices;
        }




    }
}
