using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Models;
using Sales.DataContext;

namespace Orders.Controllers
{
    public class OrdersController : Controller
    {
        
        // GET: Orders
        public ActionResult Index()
        {
            using (OrdersContext context = new OrdersContext())

            {
                var orders = context
                    .SalesOrder
                    .Select(o => new ViewModelOrder
                    {
                        Id = o.Id,
                        Customer = o.Customer.Name,
                        Status = o.Status.Name,
                        Comment = o.Comment,
                        OrderDate = o.OrderDate,
                        Total = o.SalesOrderDetail.Sum(d => d.OrderQty * d.UnitPrice)
                    })
                    .ToList();

                return View(orders);
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            using (OrdersContext context = new OrdersContext())

            {
                var orderDetails = context
                .SalesOrderDetail
                .Where(d => d.SalesOrderId == id)
                .Select(d => new ViewModelOrderDetail
                {
                    Id = d.Id,
                    ModifyDate = d.ModifyDate,
                    OrderQty = d.OrderQty,
                    ProductId = d.ProductId,
                    Product = d.Product.Name,
                    UnitPrice = d.UnitPrice
                })
                .ToList();

                ViewData["OrderId"] = id;

                return View(orderDetails);
            }
        }

        // GET: Orders/Add
        public ActionResult Add(int id)
        {
            using (OrdersContext context = new OrdersContext())

            {
                var products = context
                .Product
                .Select(p => new ViewModelProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    ListPrice = p.ListPrice,
                    Comment = p.Comment
                })
                .ToList();

                ViewData["OrderId"] = id;

                return View(new ViewModelAddProducts
                {
                    OrderId = id,
                    Products = products
                });
            }
        }

        // Post: Orders/Add
        [HttpPost]
        public ActionResult Add(ViewModelAddProducts model) 
        {
            using (OrdersContext context = new OrdersContext())

            {
                context.SalesOrderDetail.AddRange(
                model.Products
                    .Where(it => it.Selected)
                    .Select(it => new SalesOrderDetail
                    {
                        ModifyDate = DateTime.Now,
                        OrderQty = 1,
                        ProductId = it.Id,
                        SalesOrderId = model.OrderId,
                        UnitPrice = it.ListPrice
                    }));

                context.SaveChanges();

                return RedirectToAction("Details", new { id = model.OrderId });
            }
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders.Models.ViewModelOrder model) 
        {
            try
            {
                using (OrdersContext context = new OrdersContext())

                {

                    Customer customer =
                    context.Customer.FirstOrDefault(c =>
                        c.Name.Equals(model.Customer, StringComparison.CurrentCultureIgnoreCase));

                    if (customer == null)
                    {
                        customer = new Customer { Name = model.Customer };
                        context.Customer.Add(customer);
                        context.SaveChanges();
                    }

                    context.SalesOrder
                        .Add(new SalesOrder
                        {
                            CustomerId = customer.Id,
                            Comment = model.Comment,
                            OrderDate = DateTime.Now,
                            StatusId = GetOrderStatusId(context, "подтвержден")
                        });

                    context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            using (OrdersContext context = new OrdersContext())

            {
                SalesOrder order = context.SalesOrder.FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    return View(new ViewModelOrder
                    {
                        Comment = order.Comment,
                        Customer = order.Customer?.Name,
                        Status = order.Status?.Name,
                        OrderDate = order.OrderDate,
                        Statuses = context.SalesStatus.Select(s => new ViewModelStatus
                        {
                            Name = s.Name
                        }).ToList()
                    });
                }
                else
                {
                    // todo - error
                    return View();
                }
            }
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orders.Models.ViewModelOrder model)
        {
            try
            {
                using (OrdersContext context = new OrdersContext())

                {

                    // проверка, не изменился ли клиент:
                    Customer customer =
                    context.Customer.FirstOrDefault(c =>
                        c.Name.Equals(model.Customer, StringComparison.CurrentCultureIgnoreCase));

                    // создаем нового клиента, если изменился:
                    if (customer == null)
                    {
                        customer = new Customer { Name = model.Customer };
                        context.Customer.Add(customer);
                        context.SaveChanges();
                    }

                    SalesOrder order = context.SalesOrder.FirstOrDefault(o => o.Id == model.Id);
                    if (order != null)
                    {
                        order.OrderDate = DateTime.Now;
                        order.Comment = model.Comment;
                        order.StatusId = GetOrderStatusId(context, model.Status);
                        order.CustomerId = customer.Id;
                        context.SaveChanges();
                    }
                    else
                    {
                        //todo - inform client
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetOrderStatusId(OrdersContext context, string name)
        {
            return context
                .SalesStatus
                .FirstOrDefault(s =>
                    s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Id;
        }
    }
}