using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashCollector.Data;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public IActionResult Index()
        {
            Customer customer = new Customer();
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer = _context.Customers.Where(e => e.IdentityUserId == userId).Single();
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }

            return View(customer);
        }

        // GET: Customers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _context.Customers.Include(c => c.IdentityUser).FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            Customer customer = new Customer();
            List<DayOfWeek> dayOfWeek = new List<DayOfWeek>();
            dayOfWeek.Add(DayOfWeek.Monday);
            dayOfWeek.Add(DayOfWeek.Tuesday);
            dayOfWeek.Add(DayOfWeek.Wednesday);
            dayOfWeek.Add(DayOfWeek.Thursday);
            dayOfWeek.Add(DayOfWeek.Friday);
            ViewBag.DaysOfWeek = new SelectList(dayOfWeek, customer.TrashPickUpDay);
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,StreetName,City,State,ZipCode,PickupConfirmed,TrashPickUpDay,ExtraPickUpRequest,StartDate,EndDate,AccountBalance,IdentityUserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.IdentityUserId = userId;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public IActionResult Edit(int? id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = _context.Customers.Where(c => c.IdentityUserId == userId).Single();
            List<DayOfWeek> dayOfWeek = new List<DayOfWeek>();
            dayOfWeek.Add(DayOfWeek.Monday);
            dayOfWeek.Add(DayOfWeek.Tuesday);
            dayOfWeek.Add(DayOfWeek.Wednesday);
            dayOfWeek.Add(DayOfWeek.Thursday);
            dayOfWeek.Add(DayOfWeek.Friday);
            ViewBag.DaysOfWeek = new SelectList(dayOfWeek, customer.TrashPickUpDay);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,StreetName,City,State,ZipCode,PickupConfirmed,TrashPickUpDay,ExtraPickUpRequest,StartDate,EndDate,AccountBalance,IdentityUserId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Customer customerFromDb = _context.Customers.Where(c => c.IdentityUserId == userId).Single();
                    customerFromDb.FirstName = customer.FirstName;
                    customerFromDb.LastName = customer.LastName;
                    customerFromDb.StreetName = customer.StreetName;
                    customerFromDb.City = customer.City;
                    customerFromDb.State = customer.State;
                    customerFromDb.ZipCode = customer.ZipCode;
                    customerFromDb.PickupConfirmed = customer.PickupConfirmed;
                    customerFromDb.ExtraPickUpRequest = customer.ExtraPickUpRequest;
                    customerFromDb.TrashPickUpDay = customer.TrashPickUpDay;
                    customerFromDb.StartDate = customer.StartDate;
                    customerFromDb.EndDate = customer.EndDate;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletedCustomer = _context.Customers.Where(c => c.CustomerId == id).FirstOrDefault();
            _context.Customers.Remove(deletedCustomer);
            _context.SaveChanges();
            if (deletedCustomer == null)
            {
                return NotFound();
            }

            return View(deletedCustomer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
