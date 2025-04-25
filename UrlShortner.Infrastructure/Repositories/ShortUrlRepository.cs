using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain;
using UrlShortner.Application;

namespace UrlShortner.Infrastructure
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly UrlShortnerDbContext _context;

        public ShortUrlRepository(UrlShortnerDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(JisShortUrl pJisShortUrl)
        {
            _context.JisShortUrls.Add(pJisShortUrl);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<JisShortUrl> GetByShortCodeAsync(string pShortCode)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.JisShortenUrl == pShortCode);

            if (objJisShortUrl == null)
                return null;

            return objJisShortUrl;
        }

        public async Task IncrementClickCountAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE \"ShortenedUrls\" SET \"ClickCount\" = \"ClickCount\" + 1 WHERE \"Id\" = {0}", id);
        }

        public async Task<JisShortUrl> GetByIdAsync(int pJisUid)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.JisUid == pJisUid);

            if (objJisShortUrl == null)
                return null;

            return objJisShortUrl;
        }

        public async Task<bool> ShortCodeExistsAsync(string pShortCode)
        {
            return await _context.JisShortUrls
                .AnyAsync(u => u.JisShortenUrl == pShortCode);
        }
    }

}