using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain.Entities;
using UrlShortner.Domain.Repository;
using UrlShortner.Infrastructure.Persistence;

namespace UrlShortner.Infrastructure.Repositories
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly UrlShortnerDbContext _context;

        public ShortUrlRepository(UrlShortnerDbContext context)
        {
            _context = context;
        }

        public async Task<JisShortUrl> CreateAsync(JisShortUrl pJisShortUrl)
        {
            _context.JisShortUrls.Add(pJisShortUrl);
            await _context.SaveChangesAsync();
            return pJisShortUrl;
        }

        public async Task<JisShortUrl> GetByShortCodeAsync(string pShortCode)
        {
            return await _context.JisShortUrls
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.JisShortCode == pShortCode);
        }

        public async Task IncrementClickCountAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE \"ShortenedUrls\" SET \"ClickCount\" = \"ClickCount\" + 1 WHERE \"Id\" = {0}", id);
        }

        public async Task<JisShortUrl> GetByIdAsync(int id)
        {
            return await _context.JisShortUrls
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.JisUid == id);
        }

        public async Task<JisShortUrl> GetByIdAsync(string pShortCode)
        {
            return await _context.JisShortUrls
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.JisShortCode == pShortCode);
        }

        public async Task<bool> ShortCodeExistsAsync(string pShortCode)
        {
            return await _context.JisShortUrls
                .AnyAsync(u => u.JisShortCode == pShortCode);
        }
    }

}