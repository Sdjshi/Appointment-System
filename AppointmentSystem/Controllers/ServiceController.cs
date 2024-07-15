//using AppointmentSystem.DataAccessLayer;
using AppointmentData.DataAccessLayer;
using AppointmentData.Interfaces;
//using AppointmentSystem.Models;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly IServiceDAL _serviceDAL;
        private readonly IServiceProviderDAL _serviceProviderDAL;

        public ServiceController(IServiceDAL serviceDAL, IServiceProviderDAL serviceProviderDAL)
        {
            _serviceDAL = serviceDAL;
            _serviceProviderDAL = serviceProviderDAL;
        }
        public ActionResult GetService()
        {
            IEnumerable<Service> service = _serviceDAL.GetAllService();
            return View(service);
        }


        [HttpPost]

        public ActionResult CreateService(Service service)
        {

            try
            {
                _serviceDAL.AddService(service);
                return RedirectToAction(nameof(GetService));
            }
            catch
            {
                return View();
            }


        }
        [HttpPost]

        public ActionResult EditService(Service service)
        {

            try
            {
                _serviceDAL.EditService(service);
                return RedirectToAction(nameof(GetService));
            }
            catch
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult StatusDeleteService(Service service)
        {
            try
            {
                _serviceDAL.StatusDeleteService(service.ServiceID);
                return Json(service);
            }
            catch
            {
                return Json(service);
            }
        }
        [HttpPost]
        public ActionResult GetDetailsById(int ServiceID)
        {
            Service service = _serviceDAL.GetServiceData(ServiceID);
            return Json(service);
        }



        public IActionResult AssignService()
        {

            var services = _serviceDAL.GetAllService();
            var serviceProviders = _serviceProviderDAL.GetAllServiceProviders();

            
            var model = new AssignServiceModel
            {

                Services = _serviceDAL.GetAllService().ToList(),
                ServiceProviders = _serviceProviderDAL.GetAllServiceProviders().ToList()
            };

          
            return View(model);
        }


        [HttpPost]
        public IActionResult AssignService(AssignServiceModel model)
        {
            
            
                _serviceDAL.AssignService(model.ServiceID, model.ServiceProviderID);
                return RedirectToAction("Index", "Home"); 
            
            
            
        }

        public IActionResult GetAssignedService()
        {
            var services = _serviceDAL.GetAllService();
            var model = new AssignServiceModel
            {

                Services = _serviceDAL.GetAllService().ToList(),

            };
            return View(model);
        }
        public IActionResult UnassignServiceToServiceProvider(int serviceID, int providerID)
        {
            _serviceDAL.UnassignServiceToServiceProvider(serviceID, providerID);
            return Json(serviceID);
        }
    }
}
