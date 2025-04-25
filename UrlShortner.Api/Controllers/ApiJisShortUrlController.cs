using Microsoft.AspNetCore.Mvc;
using UrlShortner.Application;

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
        public async Task<IActionResult> CreateShortenedUrl(CreateShortUrlDto pCreateShortUrl)
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