namespace AppointmentData.Models
{
    public class ServiceAndServiceProvider
    {
        public int ServiceProviderID { get; set; }
        public string ServiceProviderName { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int Duration { get; set; }
        public List<Service> Services { get; set; }
        public List<ServiceProviders> ServiceProviders { get; set; }

    }
}
