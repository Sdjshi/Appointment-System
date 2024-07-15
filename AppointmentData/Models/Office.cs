using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.Models
{
    internal class Office
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string Alias { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
