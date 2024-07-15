using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentData.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class AccountRegisterModel
    {
        public string Username { get; set; }


        public string Password { get; set; }

       

        public int RoleId { get; set; }
        
        public int ServiceProviderID { get; set; }
    }
}
