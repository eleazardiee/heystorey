using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalHistoryWebsite.Models.ViewModels
{
    public class HistoryPostViewModel
    {
        [Required(ErrorMessage = "Please enter a title for your post")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a description or documentation")]
        public string Description { get; set; }

        // For handling multiple image uploads
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}