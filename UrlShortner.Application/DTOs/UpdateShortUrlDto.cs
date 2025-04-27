namespace UrlShortner.Application
{
    public class UpdateShortUrlRequestDto
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenUrl { get; set; }
    }

    public class UpdateShortUrlResponseDto
    {

    }
}