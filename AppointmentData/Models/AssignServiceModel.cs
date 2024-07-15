using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.Models
{
    public class AssignServiceModel
    {
        public int ServiceID { get; set; }
        public int ServiceProviderID { get; set; }
        public List<Service> Services { get; set; }
        public List<ServiceProviders> ServiceProviders { get; set; }
    }
}
