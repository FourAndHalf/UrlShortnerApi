namespace UrlShortner.Application
{
    public class CreateShortUrlRequestDto
    {
        public string OriginalUrl { get; set; }
        public string ShortenUrl { get; set; }
        public int DaysToExpiry { get; set; }
    }

    public class CreateShortUrlResponseDto
    {

    }
}