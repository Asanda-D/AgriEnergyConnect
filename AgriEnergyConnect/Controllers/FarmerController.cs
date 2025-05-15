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
}