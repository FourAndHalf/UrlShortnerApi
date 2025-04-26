using UrlShortner.Domain;
using UrlShortner.Shared;

namespace UrlShortner.Application
{
    public interface IShortUrlRepository
    {
        Task<ServiceResult<JisShortUrl>> CreateAsync(JisShortUrl pJisShortUrl);
        Task<ServiceResult<JisShortUrl>> UpdateAsync(JisShortUrl pJisShortUrl);
        Task<JisShortUrl> GetByShortCodeAsync(string pShortCode);
        Task<JisShortUrl> GetByIdAsync(int pJisUid);
        Task IncrementClickCountAsync(int pJisUid);
        Task<bool> ShortCodeExistsAsync(string pShortCode);
        Task<bool> IdExistsAsync(int pJisUid);
        Task<int> GetClickCount(int pJisUid);
        Task<int> GetClickCount(string pShortCode);
    }
}