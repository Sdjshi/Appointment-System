using AppointmentData.Models;

namespace AppointmentData.Interfaces
{
    public interface ICustomerDAL
    {
        void AddCustomer(Customer Customer);
        void EditCustomer(Customer Customer);
        IEnumerable<Appointment> GetAllCustomer();
        string GetConnectionString();
        Customer GetCustomerData(int? CustomerId);
        void StatusDeleteCustomer(int? id);
       Customer GetCustomerByPhone(string phoneNumber);
    }
}