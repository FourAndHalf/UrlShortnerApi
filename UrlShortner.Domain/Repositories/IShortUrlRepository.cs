
using UrlShortner.Domain.Entities;

namespace UrlShortner.Domain.Repository
{
    public interface IShortUrlRepository
    {
        Task<JisShortUrl> CreateAsync(JisShortUrl pJisShortUrl);
        Task<JisShortUrl> GetByShortCodeAsync(string pShortCode);
        Task<JisShortUrl> GetByIdAsync(string pJisUid);
        Task IncrementClickCountAsync(int pJisUid);
        Task<bool> ShortCodeExistsAsync(string pShortCode);
    }
}