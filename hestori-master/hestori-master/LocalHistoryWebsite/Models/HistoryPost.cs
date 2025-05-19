// HistoryPost.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalHistoryWebsite.Models
{
    public class HistoryPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<HistoryImage> Images { get; set; } = new List<HistoryImage>();

        // Add to HistoryPost.cs
        public ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();
    }
}