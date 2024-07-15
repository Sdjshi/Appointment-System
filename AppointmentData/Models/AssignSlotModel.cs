using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.Models
{
   public class AssignSlotModel
    {
        public int SlotId { get; set; }
        public int ServiceID { get; set; }
        //public int ServiceProviderId { get; set; }
        public int Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<SlotModel> SlotModels { get; set; }
        public List<Service> Services { get; set; }
        public List<ServiceProviders> ServiceProviders { get; set; }
        public IEnumerable<int> ServiceProviderId { get; set; }

        public List<ServiceAndServiceProvider> ServiceAndServiceProvider { get; set; }
        public List<int> SelectedServiceProviderIds { get; set; }

    }
}
