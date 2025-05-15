using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Employee")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Product/Index?farmerId=123&search=apple
    public async Task<IActionResult> Index(int? farmerId, string search)
    {
        var productsQuery = _context.Products.Include(p => p.Farmer).AsQueryable();

        if (farmerId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.FarmerId == farmerId.Value);
        }

        if (!string.IsNullOrEmpty(search))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(search) ||
                p.Category.Contains(search));
        }

        var products = await productsQuery.ToListAsync();
        return View(products);
    }
}