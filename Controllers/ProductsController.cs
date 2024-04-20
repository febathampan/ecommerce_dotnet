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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        [HttpGet]
        //Method to fetch image in file format for displaying
        public async Task<IActionResult> GetImage(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product.Image != null)
            {
                return File(product.Image, "image/jpeg");
            }
            return NotFound();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductView pdtView)
        {
            //In create, all fields are mandatory
            if (pdtView.Image != null)
            {
                //using memory stream to convert image to byte array inorder to save
                using (var memoryStream = new MemoryStream())
                {
                    await pdtView.Image.CopyToAsync(memoryStream);

                    Product product = new Product
                    {
                        Description = pdtView.Description,
                        ProductName = pdtView.ProductName,
                        ProductCode = pdtView.ProductCode,
                        PricePerUnit = pdtView.PricePerUnit,
                        Stock = pdtView.Stock,
                        SalesUnit = pdtView.SalesUnit,
                        ManufactureDate = pdtView.ManufactureDate,
                        ExpiryDate = pdtView.ExpiryDate,
                        Image = memoryStream.ToArray()
                    };
                    _context.Add(product);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductView pdtView = new ProductView()
            {
                ProductName = product.ProductName,
                Id = product.Id,
                Description = product.Description,
                ManufactureDate = product.ManufactureDate,
                ExpiryDate = product.ExpiryDate,
                Stock = product.Stock,
                SalesUnit = product.SalesUnit,
                PricePerUnit = product.PricePerUnit,
                ProductCode = product.ProductCode
            };
            return View(pdtView);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductView productView)
        {
            if (id != productView.Id)
            {
                return NotFound();
            }

            //Convert view to model and save in DB

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _context.Product.FindAsync(id); //result fetched from db

                    product.Id = productView.Id;
                    product.ProductName = productView.ProductName;
                    product.ProductCode = productView.ProductCode;
                    product.SalesUnit = productView.SalesUnit;
                    product.PricePerUnit = productView.PricePerUnit;
                    product.Stock = productView.Stock;
                    product.ManufactureDate = productView.ManufactureDate;
                    product.ExpiryDate = productView.ExpiryDate;
                    product.Description = productView.Description;

                    //If image is present, add image. Image update is not compulsory in edit.
                    if (productView.Image != null) // You can edit image, but not remove it
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await productView.Image.CopyToAsync(memoryStream);

                            product.Image = memoryStream.ToArray();
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productView.Id))
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
            return View(productView);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
