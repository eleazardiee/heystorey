using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore; // Add this line
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LocalHistoryWebsite.Models;
using LocalHistoryWebsite.Data;
using LocalHistoryWebsite.Models.ViewModels;

public class HistoryPostController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public HistoryPostController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    [HttpPost]
    public async Task<IActionResult> Create(HistoryPostViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create new history post
            var post = new LocalHistoryWebsite.Models.HistoryPost
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now
            };

            _context.HistoryPosts.Add(post);
            await _context.SaveChangesAsync();

            // Save images
            if (model.Images != null && model.Images.Count > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "historyImages");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in model.Images)
                {
                    if (image.Length > 0)
                    {
                        // Generate unique filename
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save file to disk
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Create history image record
                        var historyImage = new HistoryImage
                        {
                            FileName = uniqueFileName,
                            Description = "Image for " + post.Title, // Add a default description
                            HistoryPostId = post.Id
                        };

                        _context.HistoryImages.Add(historyImage);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        // If we got this far, something failed, redisplay form
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.HistoryPosts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.HistoryPosts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        // Delete associated images from file system
        if (post.Images != null && post.Images.Any())
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "historyImages");

            foreach (var image in post.Images)
            {
                var imagePath = Path.Combine(uploadsFolder, image.FileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }

        // Remove from database
        _context.HistoryPosts.Remove(post);
        await _context.SaveChangesAsync();

        // Redirect to home page
        return RedirectToAction("Index", "Home");
    }
}