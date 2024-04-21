using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test1app.Data;
using test1app.Models;

namespace test1app.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            // Get UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // Check if UserId is null
            if (userId == null)
            {
                // Handle scenario where UserId is not found in session, maybe redirect to login page
                return RedirectToAction("Login", "Users");
            }
            //var carts = _context.Cart.Include(c => c.Product).Include(c => c.User).ToList();
            // Retrieve carts for the logged-in user only
            var carts = _context
                .Cart.Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .Include(c => c.User)
                .ToList();
            // Calculate total amount
            decimal totalAmount = carts.Sum(c => c.Quantity * c.Product.PricePerUnit);

            ViewData["TotalAmount"] = totalAmount;
            return View(carts);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context
                .Cart.Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            // Get UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // Check if UserId is null
            if (userId == null)
            {
                // Handle scenario where UserId is not found in session, maybe redirect to login page
                return RedirectToAction("Login", "Users");
            }

            // Create a new Cart instance with UserId from session
            var cart = new Cart { UserId = userId.Value };

            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,UserId,ProductId,Quantity,Status")] Cart cart
        )
        {
            //if (ModelState.IsValid)
            //{
            var product = await _context.Product.FindAsync(cart.ProductId);
            cart.Product = product;

            var user = await _context.User.FindAsync(HttpContext.Session.GetInt32("UserId"));
            cart.User = user;

            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //  }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", cart.UserId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", cart.UserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,UserId,ProductId,Quantity,Status")] Cart cart
        )
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cart.ProductId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context
                .Cart.Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
