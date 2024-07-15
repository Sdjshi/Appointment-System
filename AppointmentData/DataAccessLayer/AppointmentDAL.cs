using AppointmentData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.DataAccessLayer
{
    public class AppointmentDAL
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppointmentDAL> _logger;

        public AppointmentDAL(IConfiguration configuration, ILogger<AppointmentDAL> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetConnectionString()
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }


        

        public List<SlotModel> GetSlotsByServiceAndServiceProvider(int serviceId, int serviceProviderId)
        {
            List<SlotModel> slotModels = new List<SlotModel>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetSlotsByServiceAndServiceProvider", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ServiceID", serviceId);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderID", serviceProviderId);
                Debug.WriteLine($"ServiceID: {serviceId}, ServiceProviderID: {serviceProviderId}");

                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    SlotModel slotModel = new SlotModel();
                    slotModel.StartTime = reader.GetTimeSpan(0);
                    slotModel.EndTime = reader.GetTimeSpan(1);
                    slotModel.Duration = Convert.ToInt32(reader["Duration"]);
                    slotModel.ServiceID = serviceId;
                    slotModel.ServiceProviderID = serviceProviderId;
                    slotModels.Add(slotModel);
                }
                con.Close();
            }
            return slotModels;
        }

        public void BookAppointmentComplete(string date, string time, string name, string address, string phone, string email,int serviceId , int serviceProviderId)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("BookAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                   
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Time", time);
                    command.Parameters.AddWithValue("@CustomerName", name);
                    command.Parameters.AddWithValue("@CustomerAddress", address);
                    command.Parameters.AddWithValue("@CustomerPhone", phone);
                    command.Parameters.AddWithValue("@CustomerEmail", email);
                    command.Parameters.AddWithValue("@Status", 1);
                    command.Parameters.AddWithValue("@ServiceID", serviceId);
                    command.Parameters.AddWithValue("@ServiceProviderID", serviceProviderId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close(); 
                }
            }
        }

        public List<string> GetAppointments(DateTime date, int serviceId, int serviceProviderId)
        {
            List<string> appointmentTimes = new List<string>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
               
                string query = "SELECT AppointmentTime FROM Appointments WHERE AppointmentDate = @AppointmentDate AND ServiceID = @ServiceId AND ServiceProviderID = @ServiceProviderId AND Status=1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentDate", date.Date); 
                    command.Parameters.AddWithValue("@ServiceID", serviceId);
                    command.Parameters.AddWithValue("@ServiceProviderID", serviceProviderId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string appointmentTime = reader["AppointmentTime"].ToString();
                            appointmentTimes.Add(appointmentTime);
                        }
                    }
                }
            }

            return appointmentTimes;
        }
        public IEnumerable<Appointment> GetAllAppointment()
        {
            List<Appointment> listAppointment = new List<Appointment>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetAllAppointments", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    
                    appointment.ServiceName = reader["ServiceName"].ToString();
                    appointment.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    appointment.AppointmentTime = reader.GetTimeSpan(2);
                  
                    appointment.AppointmentDate = ((DateTime)reader["AppointmentDate"]).Date;
                    appointment.CustomerFullName = reader["CustomerFullName"].ToString();
                    appointment.CustomerPhone = reader["CustomerPhone"].ToString();


                    listAppointment.Add(appointment);
                }
                con.Close();
            }
            return listAppointment;
        }


      
        public void StatusCancelAppointment(Appointment appointment)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Appointment_Cancel", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                sqlCommand.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateAppointmentStatus()
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("Appointment_StatusUpdate_ON_DateUpdate", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                
                

                con.Open();
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }


        //public Appointment GetMyAppointments(int? userId)
        //{
        //    Appointment appointment = new Appointment();
        //    using (SqlConnection con = new SqlConnection(GetConnectionString()))
        //    {

        //        SqlCommand sqlCommand = new SqlCommand("GetAppointmentsByUserId", con);
        //        sqlCommand.CommandType = CommandType.StoredProcedure;
        //        sqlCommand.Parameters.AddWithValue("@UserId", userId);
        //        con.Open();
        //        SqlDataReader reader = sqlCommand.ExecuteReader();
        //        while (reader.Read())
        //        {

        //            appointment.ServiceName = reader["ServiceName"].ToString();
        //            appointment.ServiceProviderName = reader["ServiceProviderName"].ToString();
        //            appointment.AppointmentTime = reader.GetTimeSpan(2);

        //            appointment.AppointmentDate = ((DateTime)reader["AppointmentDate"]).Date;
        //            appointment.CustomerFullName = reader["CustomerFullName"].ToString();
        //            appointment.CustomerPhone = reader["CustomerPhone"].ToString();


        //        }
        //        con.Close();
        //    }
        //    return appointment;
        //}


        public IEnumerable<Appointment> GetMyAppointments(int? userId)
        {
            List<Appointment> listAppointment = new List<Appointment>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand("GetAppointmentsByUserId", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();

                    appointment.ServiceName = reader["ServiceName"].ToString();
                    appointment.ServiceProviderName = reader["ServiceProviderName"].ToString();
                    appointment.AppointmentTime = reader.GetTimeSpan(2);

                    appointment.AppointmentDate = ((DateTime)reader["AppointmentDate"]).Date;
                    appointment.CustomerFullName = reader["CustomerFullName"].ToString();
                    appointment.CustomerPhone = reader["CustomerPhone"].ToString();


                    listAppointment.Add(appointment);
                }
                con.Close();
            }
            return listAppointment;
        }

    }
}
