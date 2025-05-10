using UrlShortner.Domain;
using UrlShortner.Shared;

namespace UrlShortner.Application
{
    public interface IShortUrlRepository
    {
        Task<ServiceResult<JisShortUrl>> CreateAsync(JisShortUrl pJisShortUrl);
        Task<ServiceResult<JisShortUrl>> UpdateAsync(JisShortUrl pJisShortUrl);
        Task<ServiceResult<JisShortUrl>> ExtendPeriodAsync(JisShortUrl pJisShortUrl);
        Task<ServiceResult<JisShortUrl>> GetByShortCodeAsync(string pShortCode);
        Task<ServiceResult<JisShortUrl>> GetByIdAsync(int pJisUid);
        Task IncrementClickCountAsync(int pJisUid);
        Task<ServiceResult<bool>> DoesShortCodeExistsAsync(string pShortCode);
        Task<ServiceResult<bool>> DoesIdExistsAsync(int pJisUid);
        Task<ServiceResult<bool>> IsRecordExpiredByShortCodeAsync(string pShortCode);
        Task<ServiceResult<bool>> IsRecordExpiredByIdAsync(int pJisUid);
        Task<ServiceResult<int>> GetClickCount(int pJisUid);
        Task<ServiceResult<int>> GetClickCount(string pShortCode);
    }
}
