using Microsoft.Extensions.Primitives;

namespace AppointmentData.Models
{
    public class ServiceProviders
    {
        public int ServiceProviderID { get; set; }
        public int ServiceID { get; set; }

        public string ServiceProviderName { get; set; }
        public string ServiceProviderAddress { get; set; }
        public string ServiceProviderPhone { get; set; }
        public string ServiceProviderEmail { get; set; }
        public string ServiceProviderFacebookLink { get; set; }
        public string ServiceProviderViber { get; set; }
        public string ServiceProviderWhatsapp { get; set; }
        public int OfficeId { get; set; }
      
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }

    }
}
