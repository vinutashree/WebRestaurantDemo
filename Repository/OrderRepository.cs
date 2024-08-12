using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRestaurantDemo.Models;
using WebRestaurantDemo.ViewModel;

namespace WebRestaurantDemo.Repository
{
    public class OrderRepository
    {
        private RestaurantDBEntities objRestaurantDbEntities;
        public OrderRepository() 
        {
            //this context used to interact with db
            objRestaurantDbEntities =new RestaurantDBEntities();
        }
        public bool AddOrder(OrderViewModel objorderViewModel)
        {
            try
            {
                // Check if objorderViewModel is null
                if (objorderViewModel == null)
                {
                    throw new ArgumentNullException(nameof(objorderViewModel), "OrderViewModel is null");
                }

                // Ensure ListOfOrderDetailViewModel is not null
                if (objorderViewModel.ListOfOrderDetailViewModel == null)
                {
                    objorderViewModel.ListOfOrderDetailViewModel = new List<OrderDetailViewModel>();
                }

                // Create and initialize Order object
                Order objOrder = new Order
                {
                    CustomerId = objorderViewModel.CustomerId,
                    FinalTotal = objorderViewModel.FinalTotal,
                    OrderDate = DateTime.Now,
                  //need check
                    OrderNumber = String.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now),
                    PaymentTypeId = objorderViewModel.PaymentTypeId
                };

                 // Add Order to database context
                 objRestaurantDbEntities.Orders.Add(objOrder);
                 objRestaurantDbEntities.SaveChanges(); // Save changes to get the OrderId

                int OrderId = objOrder.OrderId;

                // Process OrderDetail items
                foreach (var item in objorderViewModel.ListOfOrderDetailViewModel)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Discount = item.Discount,
                        ItemId = item.ItemId,
                        OrderId = OrderId,
                        Total = item.Total,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };

                    objRestaurantDbEntities.OrderDetails.Add(orderDetail);
                    objRestaurantDbEntities.SaveChanges();

                    // Create and add Transaction object
                    Transaction objTransaction = new Transaction
                    {
                        ItemId = item.ItemId,
                        Quantity = (-1) * item.Quantity,
                        TransactionDate= objorderViewModel.OrderDate,
                        TypeId = 2 // Example TypeId
                    };

                    objRestaurantDbEntities.Transactions.Add(objTransaction);

                    // Save changes for each detail and transaction
                    objRestaurantDbEntities.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}