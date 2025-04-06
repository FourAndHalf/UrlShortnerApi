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
    }
}