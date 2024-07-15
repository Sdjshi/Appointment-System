using AppointmentData.Models;

namespace AppointmentData.Interfaces
{
    public interface IServiceProviderDAL
    {
        void AddServiceProvider(ServiceProviders serviceProviders);
        void EditServiceProvider(ServiceProviders serviceProviders);
        IEnumerable<ServiceProviders> GetAllServiceProviders();
        string GetConnectionString();
        ServiceProviders GetServiceProviderData(int? ServiceProviderID);
        void StatusDeleteServiceProvider(int? id);
        IEnumerable<ServiceProviders> GetServiceAssignedServiceProviders(int? serviceId);
        IEnumerable<ServiceProviders> GetServiceAssignedServiceProvidersWithSlots(int? serviceId);
        List<int> GetAssignedServiceProviderIdsForSlot(int slotId);
        IEnumerable<ServiceProviders> GetAssignedServiceProviderForSlot(int slotId);
        

    }
}