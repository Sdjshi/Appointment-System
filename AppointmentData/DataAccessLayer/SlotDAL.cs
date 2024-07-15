using AppointmentData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.DataAccessLayer
{
    public class SlotDAL
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomerDAL> _logger;

        public SlotDAL(IConfiguration configuration, ILogger<CustomerDAL> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }


        public void AddSlot(SlotModel slotModel)
        {

            try 
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    SqlCommand sqlCommand = new SqlCommand("Slot_Create", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@SlotName", slotModel.SlotName);
                    sqlCommand.Parameters.AddWithValue("@StartTime", slotModel.StartTime);
                    sqlCommand.Parameters.AddWithValue("@EndTime", slotModel.EndTime);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }


            }

            catch(Exception ex) 
            {
                _logger.LogError($"Error occurred while adding customer: {ex.Message}");

                throw;
            }
           
        }

        public IEnumerable<SlotModel> GetAllSlot()
        {
            List<SlotModel> listSlotModels = new List<SlotModel>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Slots_Get", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    SlotModel SlotModel = new SlotModel();
                    SlotModel.SlotId = Convert.ToInt32(reader["SlotId"]);
                    SlotModel.SlotName = reader["SlotName"].ToString();
                    SlotModel.StartTime = reader.GetTimeSpan(2);
                    SlotModel.EndTime = reader.GetTimeSpan(3);

                   


                    listSlotModels.Add(SlotModel);
                }
                con.Close();
            }
            return listSlotModels;
        }
       

        public IEnumerable<SlotModel> GetSlotsByDate(DateTime date)
        {
            List<SlotModel> ListSlot = new List<SlotModel>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                
                    SqlCommand sqlCommand = new SqlCommand("GetSlotsByDate", connection);
                    sqlCommand.Parameters.AddWithValue("@SlotDate", date);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();


                    while (reader.Read())
                       {
                            SlotModel slotModels = new SlotModel();
                            slotModels.SlotId = Convert.ToInt32(reader["SlotId"]);
                            slotModels.SlotTime = reader.GetTimeSpan(1);
                            slotModels.IsAvailable = Convert.ToInt32(reader["IsAvailable"]);
                            ListSlot.Add(slotModels);
                       }
                    
                
            }

            return ListSlot;
        }


        

        public bool AssignSlot(int slotId, List<int> serviceProviderIds)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    foreach (int serviceProviderId in serviceProviderIds)
                    {
                        using (SqlCommand command = new SqlCommand("AssignSlotToServiceProviderProcedure", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@SlotId", slotId);
                            command.Parameters.AddWithValue("@ServiceProviderID", serviceProviderId);
                            command.Parameters.AddWithValue("@IsActive", 1);

                            command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();

                    return true;
                }
            }
            catch (SqlException ex)
            {

                _logger.LogError("error is " + ex.Message);
                return false;
            }
        }

        public SlotModel GetSlotData(int? SlotId)
        {
            SlotModel slotModel = new SlotModel();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "SELECT * FROM SlotTable WHERE SLotId =@SlotId";
                SqlCommand sqlCommand = new SqlCommand(query, con);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@SlotId", SlotId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    slotModel.SlotId = Convert.ToInt32(reader["SlotId"]);
                    slotModel.SlotName = reader["SlotName"].ToString();
                    slotModel.StartTime = reader.GetTimeSpan(2);
                    slotModel.EndTime = reader.GetTimeSpan(3);


                }
                con.Close();
            }
            return slotModel;
        }

        public void EditSlot(SlotModel slotModel)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("EditSlot", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@SlotId", slotModel.SlotId);
                sqlCommand.Parameters.AddWithValue("@SlotName", slotModel.SlotName);
                sqlCommand.Parameters.AddWithValue("@StartTime", slotModel.StartTime);
                sqlCommand.Parameters.AddWithValue("@EndTime", slotModel.EndTime);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteSlot (int SlotId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString())) 
            {
                SqlCommand sqlCommand = new SqlCommand("DeleteSlot", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@SlotId", SlotId);
                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UnassignSlotToServiceProvider (int slotId, int providerId)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("UnassignSlotToServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@SlotId", slotId);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", providerId);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

    }

}

