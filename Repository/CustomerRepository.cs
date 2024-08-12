using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebRestaurantDemo.Models;

namespace WebRestaurantDemo.Repository
{
    public class CustomerRepository
    {
        private RestaurantDBEntities objRestaurantDbEntities;
        public CustomerRepository()
        {
            objRestaurantDbEntities = new RestaurantDBEntities();
        }
        public IEnumerable<SelectListItem> GetAllCustomers()
        {
            //var objSelectListItems = new List<SelectListItem>();
            IEnumerable<SelectListItem> objSelectListItem = new List<SelectListItem>();
            objSelectListItem = (from obj in objRestaurantDbEntities.Customers
                                  select new SelectListItem
                                  {
                                      Text = obj.CustomerName,
                                      Value = obj.CustomerId.ToString(),
                                      Selected = true
                                  }).ToList();

            return objSelectListItem;
        }
    }
}