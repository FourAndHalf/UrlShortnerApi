using Microsoft.AspNetCore.Mvc;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiJisShortUrlController : ControllerBase
    {
        #region Global Variables



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

        #region  Create Shortened Url

        [HttpPost("CreateShortenedUrl")]
        public async Task<IActionResult> CreateShortenedUrl(JisShortUrl pJisShortUrl)
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
    }
}