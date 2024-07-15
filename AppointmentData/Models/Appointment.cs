using System.ComponentModel.DataAnnotations;

namespace AppointmentData.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceProviderName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public int ServiceProviderID { get; set; }
        public int ServiceID { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
       
    }

}
