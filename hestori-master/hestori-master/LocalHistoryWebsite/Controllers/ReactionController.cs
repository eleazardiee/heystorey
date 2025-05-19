// Controllers/ReactionsController.cs
using Microsoft.AspNetCore.Mvc;
using LocalHistoryWebsite.Data;
using LocalHistoryWebsite.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LocalHistoryWebsite.Controllers
{
    public class ReactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction(int postId, ReactionType reactionType)
        {
            // Get or create a unique session ID for the current user
            string sessionId = HttpContext.Session.Id;

            // Check if user already reacted to this post
            var existingReaction = await _context.PostReactions
                .FirstOrDefaultAsync(r => r.HistoryPostId == postId && r.SessionId == sessionId);

            if (existingReaction != null)
            {
                // Update existing reaction
                existingReaction.ReactionType = reactionType;
            }
            else
            {
                // Create new reaction
                var reaction = new PostReaction
                {
                    HistoryPostId = postId,
                    ReactionType = reactionType,
                    SessionId = sessionId
                };

                _context.PostReactions.Add(reaction);
            }

            await _context.SaveChangesAsync();

            // Return counts for all reaction types
            var heartCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Heart);

            var likeCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Like);

            var dislikeCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Dislike);

            return Json(new
            {
                success = true,
                heartCount,
                likeCount,
                dislikeCount,
                userReaction = reactionType.ToString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetReactions(int postId)
        {
            string sessionId = HttpContext.Session.Id;

            var heartCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Heart);

            var likeCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Like);

            var dislikeCount = await _context.PostReactions
                .CountAsync(r => r.HistoryPostId == postId && r.ReactionType == ReactionType.Dislike);

            // Get the current user's reaction
            var userReaction = await _context.PostReactions
                .Where(r => r.HistoryPostId == postId && r.SessionId == sessionId)
                .Select(r => r.ReactionType.ToString())
                .FirstOrDefaultAsync();

            return Json(new
            {
                heartCount,
                likeCount,
                dislikeCount,
                userReaction
            });
        }
    }
}