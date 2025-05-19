using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace LocalHistoryWebsite.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.HistoryPost> HistoryPosts { get; set; }
        public DbSet<Models.HistoryImage> HistoryImages { get; set; }
        public DbSet<Models.PostReaction> PostReactions { get; set; } // Add Models. namespace here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationship between HistoryPost and HistoryImage
            modelBuilder.Entity<Models.HistoryImage>()
                .HasOne(i => i.HistoryPost)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.HistoryPostId);

            modelBuilder.Entity<Models.PostReaction>() // Add Models. namespace here
                .HasOne(r => r.HistoryPost)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.HistoryPostId);

            base.OnModelCreating(modelBuilder);
        }
    }
}