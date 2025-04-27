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
        Task<bool> DoesShortCodeExistsAsync(string pShortCode);
        Task<bool> DoesIdExistsAsync(int pJisUid);
        Task<ServiceResult<bool>> DoesShortCodeExistsAsync(string pShortCode);
        Task<ServiceResult<bool>> DoesIdExistsAsync(int pJisUid);
        Task<int> GetClickCount(int pJisUid);
        Task<int> GetClickCount(string pShortCode);
    }
}