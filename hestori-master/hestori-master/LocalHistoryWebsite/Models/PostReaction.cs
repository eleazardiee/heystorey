// Models/PostReaction.cs
using System.ComponentModel.DataAnnotations;

namespace LocalHistoryWebsite.Models
{
    public enum ReactionType
    {
        Heart,
        Like,
        Dislike
    }

    public class PostReaction
    {
        public int Id { get; set; }
        public ReactionType ReactionType { get; set; }

        // Store a session or user identifier
        [Required]
        public string SessionId { get; set; }

        // Foreign key to HistoryPost
        public int HistoryPostId { get; set; }

        // Navigation property
        public HistoryPost HistoryPost { get; set; }
    }
}