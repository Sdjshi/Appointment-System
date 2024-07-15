using AppointmentData.DataAccessLayer;
using AppointmentData.Interfaces;
using AppointmentData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlotController : Controller
    {

        private readonly SlotDAL _slotDAL;
        private readonly ServiceDAL _serviceDAL;
        private readonly ServiceProviderDAL _serviceProviderDAL;

        public SlotController(SlotDAL slotDAL,ServiceDAL serviceDAL, ServiceProviderDAL serviceProviderDAL)
        {
            _slotDAL = slotDAL;
            _serviceDAL= serviceDAL;
            _serviceProviderDAL= serviceProviderDAL;
        }

        [HttpPost]

        public ActionResult CreateSlot(SlotModel slotModel)
        {

            try
            {
                _slotDAL.AddSlot(slotModel);
                return RedirectToAction(nameof(GetAllSlots));
            }
            catch
            {
                return View();
            }


        }
        public IActionResult GetAllSlots()
        {
            IEnumerable<SlotModel> slotModels = _slotDAL.GetAllSlot();
            return View(slotModels); 
        }

        public ActionResult GetSlotsByDate(string date)
        {
            DateTime clickedDate = DateTime.Parse(date);
            IEnumerable<SlotModel> slotModels = _slotDAL.GetSlotsByDate(clickedDate);

            return PartialView("_SlotsPartial", slotModels);
        }


       
        [HttpGet]
        public IActionResult AssignSlot()
        {
            var viewModel = new AssignSlotModel
            {

                SlotModels = _slotDAL.GetAllSlot().ToList(),

               
                ServiceProviders = _serviceProviderDAL.GetAllServiceProviders().ToList(),



            };
            var task = viewModel;
            
            return View(viewModel);
        }

        

        [HttpPost]
        public IActionResult AssignSlot(AssignSlotModel model)
        {
            
                int slotId = model.SlotId;
                List<int> selectedServiceProviderIds = model.SelectedServiceProviderIds;

                try
                {
                     bool assignmentResult = _slotDAL.AssignSlot(slotId, selectedServiceProviderIds);

                       
                           if (assignmentResult)
                           {

                               ViewBag.Message = "Slot assigned successfully.";
                               return RedirectToAction("Index", "Home", new { message = ViewBag.Message });
                           }
                           else
                           {

                               ViewBag.Message = "Slot is already assigned to this service provider.";
                               return RedirectToAction("Index", "Home", new { message = ViewBag.Message });
                           }

                 }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError("", "An error occurred while assigning service providers to the slot.");
                   
                }
          
            return View(model);
        }

        [HttpPost]
        public IActionResult GetSlotsById(int? SlotId)
        {
            SlotModel slotModel = _slotDAL.GetSlotData(SlotId);
            return Json(slotModel);
        }


        [HttpPost]

        public ActionResult EditSlot(SlotModel slotModel)
        {

            try
            {
                _slotDAL.EditSlot(slotModel);
                //return RedirectToAction(nameof(GetAllSlots));
                return Json(new { success = true, message = "Slot created successfully." });
            }
            catch
            {
                return View();
            }


        }

        public IActionResult StatusDeleteSlot(int SlotId)
        {
            _slotDAL.DeleteSlot(SlotId);
            return RedirectToAction(nameof(GetAllSlots));
        }

        public IActionResult GetAssignedSlot()
        {
           
            var model = new AssignSlotModel
            {

                SlotModels = _slotDAL.GetAllSlot().ToList(),

            };
            return View(model);
        }

        public IActionResult UnassignSlotToServiceProvider(int slotId, int providerId)
        {
            _slotDAL.UnassignSlotToServiceProvider(slotId, providerId);
            return Json(slotId);
        }


        public IActionResult AssignDayToServiceProviders()
        {
           

            var model = new AssignServiceModel
            {
                ServiceProviders = _serviceProviderDAL.GetAllServiceProviders().ToList()
            };

            return View(model);
        }

    }
}
