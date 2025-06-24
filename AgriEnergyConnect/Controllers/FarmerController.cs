using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using System.Threading.Tasks;

[Authorize(Roles = "Farmer")]
public class FarmerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public FarmerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Farmer/Products
    public async Task<IActionResult> Products()
    {
        var user = await _userManager.GetUserAsync(User);
        var products = await _context.Products
            .Include(p => p.Farmer)
            .Where(p => p.Farmer.Email == user.Email)
            .ToListAsync();

        return View(products);
    }

    // GET: Farmer/AddProduct
    public IActionResult AddProduct()
    {
        return View();
    }

    // POST: Farmer/AddProduct
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);

            if (farmer == null)
            {
                ModelState.AddModelError("", "Farmer profile not found.");
                return View(product);
            }

            product.FarmerId = farmer.Id;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }
        return View(product);
    }

    // GET: Farmer/EditProduct/5
    public async Task<IActionResult> EditProduct(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        // Ensure the product belongs to the logged-in farmer
        var user = await _userManager.GetUserAsync(User);
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
        if (farmer == null || product.FarmerId != farmer.Id)
            return Unauthorized();

        return View(product);
    }

    // POST: Farmer/EditProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, Product updatedProduct)
    {
        if (id != updatedProduct.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
                if (farmer == null || updatedProduct.FarmerId != farmer.Id)
                    return Unauthorized();

                _context.Update(updatedProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == updatedProduct.Id))
                    return NotFound();
                throw;
            }
        }
        return View(updatedProduct);
    }

    // GET: Farmer/DeleteProduct/5
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .Include(p => p.Farmer)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Farmer/DeleteProductConfirmed/5
    [HttpPost, ActionName("DeleteProductConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Products));
    }
}