using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRestaurantDemo.Models;
using WebRestaurantDemo.Repository;
using WebRestaurantDemo.ViewModel;

namespace WebRestaurantDemo.Controllers
{
    public class HomeController : Controller
    {
        private RestaurantDBEntities objRestaurantDbEntities;
        public HomeController()
        {
            objRestaurantDbEntities = new RestaurantDBEntities();
        }
        // GET: Home
        public ActionResult Index()
        {
           // CustomerRepository
           CustomerRepository objcustomerRepository = new CustomerRepository();
            // ItemRepository
            ItemRepository objitemRepository = new ItemRepository();
            //PaymentTypeRepository
            PaymentTypeRepository PaymentTypeRepository = new PaymentTypeRepository();

            var objMultipleModel = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
             (objcustomerRepository.GetAllCustomers(), objitemRepository.GetAllItems(), PaymentTypeRepository.GetAllPaymentType());
            
            return View(objMultipleModel);
        }

        [HttpGet]
        public JsonResult GetItemUnitPrice(int itemId)
        {
            decimal UnitPrice= objRestaurantDbEntities.Items.Single(model=> model.ItemId==itemId).ItemPrice;
            return Json(UnitPrice,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Index(OrderViewModel objorderViewModel)
        {
            if (ModelState.IsValid)
            {
                OrderRepository objorderRepository = new OrderRepository();
                bool isStatus = objorderRepository.AddOrder(objorderViewModel);
                if (isStatus)
                {
                    return Json(new { success = true, message = "Your Order Has Been Successfully Placed." });
                }
                else
                {
                    return Json(new { success = false, message = "There is Some Issue While Placing Order" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid order data." });
            }
        }
    }
}