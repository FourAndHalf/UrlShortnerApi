using UrlShortner.Domain;

namespace UrlShortner.Application
{
    public interface IShortUrlRepository
    {
        Task<int> CreateAsync(JisShortUrl pJisShortUrl);
        Task<JisShortUrl> GetByShortCodeAsync(string pShortCode);
        Task<JisShortUrl> GetByIdAsync(int pJisUid);
        Task IncrementClickCountAsync(int pJisUid);
        Task<bool> ShortCodeExistsAsync(string pShortCode);
    }
}