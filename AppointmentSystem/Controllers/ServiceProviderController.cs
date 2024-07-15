using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppointmentSystem.Controllers
{
    
    public class ServiceProviderController : Controller
    {
        private readonly IServiceProviderDAL _serviceProviderDAL;

        public ServiceProviderController(IServiceProviderDAL serviceProviderDAL)
        {
            _serviceProviderDAL = serviceProviderDAL;
        }



        [Authorize(Roles = "Admin")]
        public ActionResult GetServiceProviders()
        {
            IEnumerable<ServiceProviders> serviceProviders = _serviceProviderDAL.GetAllServiceProviders();
            return View(serviceProviders);
        }


        [Authorize(Roles = "Admin")]

        [HttpPost]

        public ActionResult CreateServiceProvider(ServiceProviders serviceProviders)
        {

            try
            {
                _serviceProviderDAL.AddServiceProvider(serviceProviders);
                return RedirectToAction(nameof(GetServiceProviders));
            }
            catch
            {
                return View();
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]

        public ActionResult EditServiceProvider(ServiceProviders serviceProviders)
        {

            try
            {
                _serviceProviderDAL.EditServiceProvider(serviceProviders);
                return RedirectToAction(nameof(GetServiceProviders));
            }
            catch
            {
                return View();
            }


        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult DeleteServiceProvider(ServiceProviders serviceProviders)
        {
            try
            {
                _serviceProviderDAL.StatusDeleteServiceProvider(serviceProviders.ServiceProviderID);
                return Json(serviceProviders);
            }
            catch
            {
                return Json(serviceProviders);
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult GetServiceProviderDetailsById(int ServiceProviderID)
        {
            ServiceProviders serviceProviders = _serviceProviderDAL.GetServiceProviderData(ServiceProviderID);
            return Json(serviceProviders);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public ActionResult GetServiceProviderDetailsById()
        {

            var response = new { success = true };
            return Json(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetServiceAssignedServiceProviders(int serviceId)
        {
            var serviceProviders = _serviceProviderDAL.GetServiceAssignedServiceProviders(serviceId);
            return Json(serviceProviders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetServiceAssignedServiceProvidersWithSlots(int serviceId)
        {
            var serviceProviders = _serviceProviderDAL.GetServiceAssignedServiceProvidersWithSlots(serviceId);
            return Json(serviceProviders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetSlotAssignedServiceProviders(int slotId)
        {
          
            var assignedServiceProviderIds = _serviceProviderDAL.GetAssignedServiceProviderForSlot(slotId);

            return Json(assignedServiceProviderIds);
     
        
        
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetSlotAssignedServiceProviderID(int slotId)
        {

            var assignedServiceProviderIds = _serviceProviderDAL.GetAssignedServiceProviderIdsForSlot(slotId);

            return Json(assignedServiceProviderIds);



        }
    }

}
