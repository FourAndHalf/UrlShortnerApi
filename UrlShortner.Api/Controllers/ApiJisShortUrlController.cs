using Microsoft.AspNetCore.Mvc;
using UrlShortner.Application;
using UrlShortner.Domain;
using UrlShortner.Shared;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiJisShortUrlController : ControllerBase
    {
        #region Global Variables

        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IUrlShortnerService _urlShortnerService;

        public ApiJisShortUrlController(IShortUrlRepository shortUrlRepository, IUrlShortnerService urlShortnerService)
        {
            _shortUrlRepository = shortUrlRepository;
            _urlShortnerService = urlShortnerService;
        }

        #endregion 

        #region  Create Record

        [HttpPost("CreateRecord")]
        public async Task<IActionResult> CreateShortenedUrl(CreateShortUrlRequestDto pCreateShortUrlRequest)
        {
            try
            {
                ServiceResult<bool> isDuplicate = new ServiceResult<bool> { IsSuccess = true, Data = true };
                string pShortCode = string.Empty;

                while (isDuplicate.Data && isDuplicate.IsSuccess)
                {
                    pShortCode = await _urlShortnerService.CreateRandomShortCode();
                    isDuplicate = await _shortUrlRepository.DoesShortCodeExistsAsync(pShortCode);
                }

                JisShortUrl pJisShortUrl = new JisShortUrl
                {
                    JisOriginalUrl = pCreateShortUrlRequest.OriginalUrl,
                    JisShortenUrl = pShortCode,
                    JisClickCount = 0,
                    JisCreatedAt = System.DateTime.Today,
                    JisExpiresAt = (pCreateShortUrlRequest.DaysToExpiry > Constants.defaultExpirationDays) ?
                                        System.DateTime.Today.AddDays(Constants.defaultExpirationDays) :
                                        System.DateTime.Today.AddDays(pCreateShortUrlRequest.DaysToExpiry)
                };

                ServiceResult<JisShortUrl> resJisShortUrl = await _shortUrlRepository.CreateAsync(pJisShortUrl);

                if (resJisShortUrl.IsSuccess == true)
                {
                    return StatusCode(201, new { Message = $"Created short url successfully. ShortUrl = {pShortCode}. Expires on {pJisShortUrl.JisExpiresAt.ToShortDateString()}" });
                }
                else
                {
                    return StatusCode(422, new { Message = $"We are unable to process your request right now. Please try after some time" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

        #region Update Record 

        [HttpPost("UpdateRecord")]
        public async Task<IActionResult> UpdateShortenedUrl(UpdateShortUrlRequestDto pUpdateShortUrlRequestDto)
        {
            try
            {
                ServiceResult<bool> isExpired = await _shortUrlRepository.IsRecordExpiredByIdAsync(pUpdateShortUrlRequestDto.Id);

                if (isExpired.IsSuccess == true)
                {
                    if (isExpired.Data == true)
                    {
                        return StatusCode(200, new { Message = $"This record seems to be expired" });
                    }
                    else
                    {
                        JisShortUrl pJisShortUrl = new JisShortUrl
                        {
                            JisOriginalUrl = pUpdateShortUrlRequestDto.OriginalUrl,
                            JisShortenUrl = pUpdateShortUrlRequestDto.ShortenUrl,
                            JisUid = pUpdateShortUrlRequestDto.Id
                        };

                        ServiceResult<JisShortUrl> resJisShortUrl = await _shortUrlRepository.UpdateAsync(pJisShortUrl);

                        if (resJisShortUrl.IsSuccess == true)
                        {
                            return StatusCode(201, new { Message = $"Updated short url successfully. ShortUrl = {resJisShortUrl.Data?.JisShortenUrl}. Expires on {resJisShortUrl.Data?.JisExpiresAt.ToShortDateString()}" });
                        }
                        else
                        {
                            return StatusCode(422, new { Message = $"We are unable to process your request right now. Please try after some time" });
                        }
                    }
                }
                else
                {
                    return StatusCode(500, new { Message = $"Looks like record doesn't exist" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

        #region Extend Period

        [HttpPost("ExtendPeriod")]
        public async Task<IActionResult> ExtendPeriodShortenedUrl(UpdateShortUrlRequestDto pUpdateShortUrlRequestDto)
        {
            try
            {
                ServiceResult<bool> doesExist = await _shortUrlRepository.DoesIdExistsAsync(pUpdateShortUrlRequestDto.Id);

                if (doesExist.IsSuccess == true)
                {
                    if (doesExist.Data == true)
                    {
                        JisShortUrl pJisShortUrl = new JisShortUrl
                        {
                            JisOriginalUrl = pUpdateShortUrlRequestDto.OriginalUrl,
                            JisShortenUrl = pUpdateShortUrlRequestDto.ShortenUrl,
                            JisUid = pUpdateShortUrlRequestDto.Id
                        };

                        ServiceResult<JisShortUrl> resJisShortUrl = await _shortUrlRepository.ExtendPeriodAsync(pJisShortUrl);

                        if (resJisShortUrl.IsSuccess == true)
                        {
                            return StatusCode(201, new { Message = $"Updated short url successfully. ShortUrl = {resJisShortUrl.Data?.JisShortenUrl}. Expires on {resJisShortUrl.Data?.JisExpiresAt.ToShortDateString()}" });
                        }
                        else
                        {
                            return StatusCode(422, new { Message = $"We are unable to process your request right now. Please try after some time" });
                        }
                    }
                    else
                    {
                        return StatusCode(200, new { Message = $"The record you're looking for seems to be moved or deleted" });
                    }
                }
                else
                {
                    return StatusCode(500, new { Message = $"Looks like record doesn't exist" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

        #region Get Click Count

        [HttpGet("GetClickCountByShortUrlAsync/{pShortUrl}")]
        public async Task<IActionResult> GetClickCount(string pShortUrl)
        {
            try
            {
                ServiceResult<bool> doesExist = await _shortUrlRepository.DoesShortCodeExistsAsync(pShortUrl);

                if (doesExist.IsSuccess && doesExist.Data)
                {
                    ServiceResult<int> count = await _shortUrlRepository.GetClickCount(pShortUrl);

                    if (count.IsSuccess)
                        return StatusCode(200, new { Message = $"Successfully processed the request. Click Count = {count.Data}" });
                    else
                        return StatusCode(204, new { Message = "Failed to get count from the database" });
                }
                else
                {
                    return StatusCode(404, new { Message = "The given short url was not found in the system" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        [HttpGet("GetClickCountByUidAsync/{pJisUid}")]
        public async Task<IActionResult> GetClickCount(int pJisUid)
        {
            try
            {
                ServiceResult<bool> doesExist = await _shortUrlRepository.DoesIdExistsAsync(pJisUid);

                if (doesExist.IsSuccess && doesExist.Data)
                {
                    ServiceResult<int> count = await _shortUrlRepository.GetClickCount(pJisUid);

                    if (count.IsSuccess)
                        return StatusCode(200, new { Message = $"Successfully processed the request. Click Count = {count.Data}" });
                    else
                        return StatusCode(204, new { Message = "Failed to get count from the database" });
                }
                else
                {
                    return StatusCode(404, new { Message = "The given short url was not found in the system" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

        #region Get Original Url

        [HttpGet("GetOriginalUrl")]
        public async Task<IActionResult> GetOriginalUrl(string pShortUrl)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

    }
}