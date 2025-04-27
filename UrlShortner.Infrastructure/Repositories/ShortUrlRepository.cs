using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain;
using UrlShortner.Application;
using UrlShortner.Shared;

namespace UrlShortner.Infrastructure
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly UrlShortnerDbContext _context;

        public ShortUrlRepository(UrlShortnerDbContext context)
        {
            _context = context;
        }

        #region Save Record

        public async Task<ServiceResult<JisShortUrl>> CreateAsync(JisShortUrl pJisShortUrl)
        {
            _context.JisShortUrls.Add(pJisShortUrl);
            int result = await _context.SaveChangesAsync();

            // Free Plan Exhaustion

            if (result == 1)
                return ServiceResult<JisShortUrl>.Success(pJisShortUrl);
            else
                return ServiceResult<JisShortUrl>.Failure("Failed to save the given url");
        }

        #endregion

        #region Update Record

        public async Task<ServiceResult<JisShortUrl>> UpdateAsync(JisShortUrl pJisShortUrl)
        {
            var entity = await _context.JisShortUrls.FindAsync(pJisShortUrl.JisUid);

            if (entity == null)
                return ServiceResult<JisShortUrl>.Failure("Failed to find given record");

            entity.JisOriginalUrl = pJisShortUrl.JisOriginalUrl;

            _context.JisShortUrls.Update(entity);
            int result = await _context.SaveChangesAsync();

            if (result == 1)
                return ServiceResult<JisShortUrl>.Success(pJisShortUrl);
            else
                return ServiceResult<JisShortUrl>.Failure("Failed to update given url");
        }

        #endregion

        #region Extend Period

        public async Task<ServiceResult<JisShortUrl>> ExtendPeriodAsync(JisShortUrl pJisShortUrl)
        {
            var entity = await _context.JisShortUrls.FindAsync(pJisShortUrl.JisUid);

            if (entity == null)
                return ServiceResult<JisShortUrl>.Failure("Failed to find given record");

            entity.JisOriginalUrl = pJisShortUrl.JisOriginalUrl;
            entity.JisExpiresAt = entity.JisExpiresAt.AddDays(Constants.defaultExpirationDays);

            _context.JisShortUrls.Update(entity);
            int result = await _context.SaveChangesAsync();

            if (result == 1)
                return ServiceResult<JisShortUrl>.Success(pJisShortUrl);
            else
                return ServiceResult<JisShortUrl>.Failure("Failed to extend period for given url");
        }

        #endregion

        #region Fetch Record From Database

        public async Task<ServiceResult<JisShortUrl>> GetByShortCodeAsync(string pShortCode)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.JisShortenUrl == pShortCode);

            if (objJisShortUrl != null)
                return ServiceResult<JisShortUrl>.Success(objJisShortUrl);
            else
                return ServiceResult<JisShortUrl>.Failure("Failed to fetch record");

        }

        public async Task<ServiceResult<JisShortUrl>> GetByIdAsync(int pJisUid)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.JisUid == pJisUid);

            if (objJisShortUrl != null)
                return ServiceResult<JisShortUrl>.Success(objJisShortUrl);
            else
                return ServiceResult<JisShortUrl>.Failure("Failed to fetch record");
        }

        #endregion

        #region Duplicate Check

        public async Task<ServiceResult<bool>> DoesShortCodeExistsAsync(string pShortCode)
        {
            bool isDuplicate = await _context.JisShortUrls
                                    .AnyAsync(u => u.JisShortenUrl == pShortCode);

            if (isDuplicate)
            {
                return ServiceResult<bool>.Success(true);
            }
            else
            {
                return ServiceResult<bool>.Success(false);
            }
        }

        public async Task<ServiceResult<bool>> DoesIdExistsAsync(int pJisUid)
        {
            bool isDuplicate = await _context.JisShortUrls
                                    .AnyAsync(u => u.JisUid == pJisUid);

            if (isDuplicate)
            {
                return ServiceResult<bool>.Success(true);
            }
            else
            {
                return ServiceResult<bool>.Success(false);
            }
        }

        #endregion

        #region Is Record Expired Check

        public async Task<ServiceResult<bool>> IsRecordExpiredByShortCodeAsync(string pShortCode)
        {
            var pJisShortUrl = await _context.JisShortUrls
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(u => u.JisShortenUrl == pShortCode);
            if (pJisShortUrl != null)
            {
                if (pJisShortUrl.JisExpiresAt >= System.DateTime.Today)
                {
                    return ServiceResult<bool>.Success(true);
                }
                else
                {
                    return ServiceResult<bool>.Success(false);
                }
            }

            return ServiceResult<bool>.Failure("Failed to fetch the record for checking expiry");
        }

        public async Task<ServiceResult<bool>> IsRecordExpiredByIdAsync(int pJisUid)
        {
            var pJisShortUrl = await _context.JisShortUrls
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(u => u.JisUid == pJisUid);
            if (pJisShortUrl != null)
            {
                if (pJisShortUrl.JisExpiresAt >= System.DateTime.Today)
                {
                    return ServiceResult<bool>.Success(true);
                }
                else
                {
                    return ServiceResult<bool>.Success(false);
                }
            }

            return ServiceResult<bool>.Failure("Failed to fetch the record for checking expiry");
        }

        #endregion

        #region  Increment Click Count        

        public async Task IncrementClickCountAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE \"ShortenedUrls\" SET \"ClickCount\" = \"ClickCount\" + 1 WHERE \"Id\" = {0}", id);
        }

        #endregion

        #region Get Click Count

        public async Task<ServiceResult<int>> GetClickCount(int pJisUid)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.JisUid == pJisUid);

            if (objJisShortUrl != null)
            {
                return ServiceResult<int>.Success(objJisShortUrl.JisClickCount);
            }
            else
            {
                return ServiceResult<int>.Failure("Failed to fetch record");
            }
        }

        public async Task<ServiceResult<int>> GetClickCount(string pJisShortUrl)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.JisShortenUrl == pJisShortUrl);

            if (objJisShortUrl != null)
            {
                return ServiceResult<int>.Success(objJisShortUrl.JisClickCount);
            }
            else
            {
                return ServiceResult<int>.Failure("Failed to fetch record");
            }
        }

        #endregion 
    }

}