using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: Employee/Farmers
    public async Task<IActionResult> Farmers()
    {
        var farmers = await _context.Farmers.ToListAsync();
        return View(farmers);
    }

    // GET: Employee/AddFarmer
    public IActionResult AddFarmer()
    {
        return View();
    }

    // POST: Employee/AddFarmer
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFarmer(FarmerViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Farmer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                }

                await _userManager.AddToRoleAsync(user, "Farmer");

                var farmer = new Farmer
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Location = model.Location
                };

                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Farmers));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return View(model);
    }

    // GET: Employee/DeleteFarmer/5
    public async Task<IActionResult> DeleteFarmer(int? id)
    {
        if (id == null)
            return NotFound();

        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Id == id);

        if (farmer == null)
            return NotFound();

        return View(farmer);
    }

    // POST: Employee/DeleteFarmer/5
    [HttpPost, ActionName("DeleteFarmer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFarmerConfirmed(int id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
            return NotFound();

        // Find the corresponding IdentityUser by email
        var user = await _userManager.FindByEmailAsync(farmer.Email);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Handle error if needed
                ModelState.AddModelError("", "Failed to delete user from Identity.");
                return View(farmer);
            }
        }

        _context.Farmers.Remove(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Farmers));
    }

    // GET: Employee/ViewProducts
    public async Task<IActionResult> ViewProducts(int? farmerId, string nameFilter, string categoryFilter, DateTime? fromDate, DateTime? toDate)
    {
        var farmers = await _context.Farmers.ToListAsync();
        ViewBag.Farmers = new SelectList(farmers, "Id", "FullName");

        if (farmerId == null)
        {
            ViewBag.Products = new List<Product>();
            return View();
        }

        var productsQuery = _context.Products
            .Include(p => p.Farmer)
            .Where(p => p.FarmerId == farmerId);

        if (!string.IsNullOrEmpty(nameFilter))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(nameFilter));
        }

        if (!string.IsNullOrEmpty(categoryFilter))
        {
            productsQuery = productsQuery.Where(p => p.Category == categoryFilter);
        }

        if (fromDate.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.ProductionDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.ProductionDate <= toDate.Value);
        }

        var products = await productsQuery.ToListAsync();
        ViewBag.Products = products;

        return View();
    }
}