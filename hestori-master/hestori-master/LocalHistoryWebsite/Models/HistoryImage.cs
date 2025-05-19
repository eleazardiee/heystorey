// HistoryImage.cs
using System.ComponentModel.DataAnnotations;

namespace LocalHistoryWebsite.Models
{
    public class HistoryImage
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? Description { get; set; }

        // Foreign key
        public int HistoryPostId { get; set; }

        // Navigation property
        public HistoryPost HistoryPost { get; set; }
    }
}