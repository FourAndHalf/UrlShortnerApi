using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Infrastructure.Persistence
{
    public class UrlShortnerDbContext : DbContext
    {
        public UrlShortnerDbContext(DbContextOptions<UrlShortnerDbContext> options) : base(options)
        {
        }

        public DbSet<JisShortUrl> JisShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JisShortUrl>(entity =>
            {
                entity.HasKey(e => e.JisUid);

                entity.Property(e => e.JisOriginalUrl)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.JisShortenUrl)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.JisCreatedAt)
                    .IsRequired();

                entity.Property(e => e.JisExpiresAt)
                    .IsRequired();

                entity.Property(e => e.JisClickCount)
                    .IsRequired();
            });
        }
    }
}