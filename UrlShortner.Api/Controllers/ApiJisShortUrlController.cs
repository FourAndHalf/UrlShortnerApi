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

        #region  Create Shortened Url

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

                int result = await _shortUrlRepository.CreateAsync(pJisShortUrl);

                if (result == 1)
                {
                    return StatusCode(200, new { Message = $"Created short url successfully. Details = {pShortCode}" });
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

        #region Get Click Count

        [HttpGet("GetClickCount")]
        public async Task<IActionResult> GetClickCount(string shortUrl)
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

        #region Get Original Url

        [HttpGet("GetOriginalUrl")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
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