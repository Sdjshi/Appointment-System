using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.Models
{
    public class SlotModel
    {
        public int SlotId { get; set; }
        public int ServiceID { get; set; }
        public int ServiceProviderID { get; set; }
        public string SlotName { get; set; }
        public int Duration { get; set; }   
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan SlotTime { get; set; }
        public int IsAvailable { get; set; }

    }
}
