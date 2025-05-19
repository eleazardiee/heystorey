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

namespace LocalHistoryWebsite.Pages.History
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public HistoryPostViewModel HistoryPost { get; set; }

        public void OnGet()
        {
            // Initialize the form
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create new history post
            var post = new HistoryPost
            {
                Title = HistoryPost.Title,
                Description = HistoryPost.Description,
                CreatedAt = DateTime.Now
            };

            _context.HistoryPosts.Add(post);
            await _context.SaveChangesAsync();

            // Save images
            if (HistoryPost.Images != null && HistoryPost.Images.Count > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "historyImages");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var image in HistoryPost.Images)
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
                            Description = "Image for " + post.Title,
                            HistoryPostId = post.Id
                        };

                        _context.HistoryImages.Add(historyImage);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}