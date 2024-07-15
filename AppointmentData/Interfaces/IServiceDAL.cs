using AppointmentData.Models;

namespace AppointmentData.Interfaces
{
    public interface IServiceDAL
    {
        void AddService(Service service);
        void EditService(Service service);
        IEnumerable<Service> GetAllService();
        string GetConnectionString();
        Service GetServiceData(int? ServiceID);
        void StatusDeleteService(int? id);

        void AssignService(int serviceId, int serviceProviderId);
        void UnassignServiceToServiceProvider(int serviceID, int providerID);
    }
}