using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokBookStore.Areas.Manage.ViewModels;
using PustokBookStore.DAL;
using PustokBookStore.Entities;

namespace PustokBookStore.Areas.Manage.Controllers
{
	[Authorize(Roles="Admin, SuperAdmin")]
	[Area("manage")]
	public class OrderController : Controller
	{
		private readonly PustokDbContext _context;

		public OrderController(PustokDbContext context)
		{
			_context = context;
		}
		public IActionResult Index(int page = 1)
		{
			var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();

			return View(PaginatedList<Order>.Create(query,page,4));
		}

		public IActionResult Edit(int id)
		{
			Order order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Book).FirstOrDefault(x => x.Id == id);

			if (order == null) return View("Error");

			return View(order);
		}

		public IActionResult Accept(int id)
		{
			Order order = _context.Orders.Find(id);

			if (order == null || order.Status != Enums.OrderStatus.Pending) return View("Error");

			order.Status = Enums.OrderStatus.Accepted; 
			_context.SaveChanges();
			
			return RedirectToAction("Index");
		}

		public IActionResult Reject(int id)
		{
			Order order = _context.Orders.Find(id);

			if (order == null || order.Status != Enums.OrderStatus.Pending) return View("Error");

			order.Status = Enums.OrderStatus.Rejected;
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
