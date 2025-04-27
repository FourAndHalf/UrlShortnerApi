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



        #endregion

        #region Fetch Record From Database

        public async Task<JisShortUrl> GetByShortCodeAsync(string pShortCode)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.JisShortenUrl == pShortCode);

            return objJisShortUrl;
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

        #endregion

        #region Duplicate Check

        public async Task<bool> DoesShortCodeExistsAsync(string pShortCode)
        {
            return await _context.JisShortUrls
                .AnyAsync(u => u.JisShortenUrl == pShortCode);
        }

        public async Task<bool> DoesIdExistsAsync(int pJisUid)
        {
            return await _context.JisShortUrls
                .AnyAsync(u => u.JisUid == pJisUid);
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

        public async Task<int> GetClickCount(int pJisUid)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.JisUid == pJisUid);

            if (objJisShortUrl != null)
            {
                return objJisShortUrl.JisClickCount;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetClickCount(string pJisShortUrl)
        {
            var objJisShortUrl = await _context.JisShortUrls
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.JisShortenUrl == pJisShortUrl);

            if (objJisShortUrl != null)
            {
                return objJisShortUrl.JisClickCount;
            }
            else
            {
                return 0;
            }
        }

        #endregion 
    }

}