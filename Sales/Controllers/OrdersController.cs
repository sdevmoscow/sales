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

            var orders = new OrdersContext()
                .SalesOrder
                .Select(o => new ViewModelOrder
                {
                    Id = o.Id,
                    Customer = o. Customer.Name,
                    Status = o.Status.Name,
                    Comment = o.Comment,
                    OrderDate = o.OrderDate,
                    Total = o.SalesOrderDetail.Sum(d => d.OrderQty * d.UnitPrice)
                })
                .ToList();

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var orderDetails = new OrdersContext()
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

        // GET: Orders/Add
        public ActionResult Add(int id)
        {
            var products = new OrdersContext()
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

            return View( new ViewModelAddProducts {
                    OrderId = id,
                    Products = products
                });
        }

        // Post: Orders/Add
        [HttpPost]
        public ActionResult Add(ViewModelAddProducts model) 
        {
            OrdersContext ctx = new OrdersContext();

            ctx.SalesOrderDetail.AddRange(
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

            ctx.SaveChanges();

            return RedirectToAction("Details", new { id = model.OrderId } );
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders.Models.ViewModelOrder model) // IFormCollection collection)
        {
            try
            {
                OrdersContext ctx = new OrdersContext();

                Customer customer = 
                    ctx.Customer.FirstOrDefault(c => 
                        c.Name.Equals(model.Customer, StringComparison.CurrentCultureIgnoreCase));
                
                if (customer == null)
                {
                    customer = new Customer {  Name = model.Customer  };
                    ctx.Customer.Add(customer);
                    ctx.SaveChanges();
                }

                ctx.SalesOrder
                    .Add(new SalesOrder
                    {
                        CustomerId = customer.Id,
                        Comment = model.Comment,
                        OrderDate = DateTime.Now,
                        StatusId = ctx.SalesStatus.FirstOrDefault(s => s.Name.Equals("подтвержден", StringComparison.CurrentCultureIgnoreCase)).Id,
                    });

                ctx.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            OrdersContext ctx = new OrdersContext();
            SalesOrder order = ctx.SalesOrder.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                ViewData["Status"] = order.Status?.Name; //,ctx.SalesStatus.FirstOrDefault(s => s.Id == order.StatusId).Name;
                ViewData["StatusId"] = order.StatusId;
                ViewData["Statuses"] = ctx.SalesStatus.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = s.Id.ToString(), Text = s.Name, Selected = s.Id == 2 }).ToList();

                return View(new ViewModelOrder
                {
                    Comment = order.Comment,
                    Customer = order.Customer?.Name,
                    Status = order.Status?.Name,
                    OrderDate = order.OrderDate,
                    Statuses = ctx.SalesStatus.Select( s => new ViewModelStatus
                    {
                        Id = s.Id,
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

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orders.Models.ViewModelOrder model)
        {
            try
            {
                OrdersContext ctx = new OrdersContext();

                // проверка, не изменился ли клиент:
                Customer customer =
                    ctx.Customer.FirstOrDefault(c =>
                        c.Name.Equals(model.Customer, StringComparison.CurrentCultureIgnoreCase));

                // создаем нового клиента, если изменился:
                if (customer == null)
                {
                    customer = new Customer { Name = model.Customer };
                    ctx.Customer.Add(customer);
                    ctx.SaveChanges();
                }

                SalesOrder order = ctx.SalesOrder.FirstOrDefault(o => o.Id == model.Id);
                if (order != null)
                {
                    order.OrderDate = DateTime.Now;
                    order.Comment = model.Comment;
                    order.StatusId = model.StatusId;
                    order.CustomerId = customer.Id;
                    ctx.SaveChanges();
                }
                else
                {
                    //todo - inform client
                }

                return RedirectToAction(nameof(Index));
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
    }
}