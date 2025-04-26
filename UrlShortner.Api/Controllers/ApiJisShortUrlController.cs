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

        [HttpPost("CreateShortenedUrl")]
        public async Task<IActionResult> CreateShortenedUrl(CreateShortUrlRequestDto pCreateShortUrlRequest)
        {
            try
            {
                bool isDuplicate = true;
                string pShortCode = string.Empty;

                while (isDuplicate)
                {
                    pShortCode = await _urlShortnerService.CreateRandomShortCode();
                    isDuplicate = await _shortUrlRepository.ShortCodeExistsAsync(pShortCode);
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
                    return StatusCode(200, new { Message = $"Created short url successfully. ShortUrl = {pShortCode}. Expires on {pJisShortUrl.JisExpiresAt.ToShortDateString()}" });
                }
                else
                {
                    return StatusCode(405, new { Message = $"We are unable to process your request right now. Please try after some time" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while retrieving the original url, Details = {ex.Message}" });
            }
        }

        #endregion

        #region Update Record 



        #endregion

        #region Get Click Count

        [HttpGet("GetClickCountByShortUrlAsync/{pShortUrl}")]
        public async Task<IActionResult> GetClickCount(string pShortUrl)
        {
            try
            {
                bool doesExist = await _shortUrlRepository.ShortCodeExistsAsync(pShortUrl);

                if (doesExist)
                {
                    int count = await _shortUrlRepository.GetClickCount(pShortUrl);
                    return StatusCode(200, new { Message = $"Successfully processed the request. Click Count = {count}" });
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
                bool doesExist = await _shortUrlRepository.IdExistsAsync(pJisUid);

                if (doesExist)
                {
                    int count = await _shortUrlRepository.GetClickCount(pJisUid);
                    return StatusCode(200, new { Message = $"Successfully processed the request. Click Count = {count}" });
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