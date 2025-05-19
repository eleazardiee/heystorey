using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LocalHistoryWebsite.Data;
using LocalHistoryWebsite.Models;
using LocalHistoryWebsite.Models.ViewModels; // Add this namespace

namespace LocalHistoryWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Get all history posts with their images
        var posts = await _context.HistoryPosts
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        // Create view model
        var viewModel = new HomeViewModel
        {
            Posts = posts,
            CreatePostViewModel = new HistoryPostViewModel()
        };

        return View(viewModel);
    }
}
