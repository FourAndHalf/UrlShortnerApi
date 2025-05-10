namespace UrlShortner.Application
{
    public class CreateShortUrlRequestDto
    {
        public required string OriginalUrl { get; set; }
        public required string ShortenUrl { get; set; }
        public int DaysToExpiry { get; set; }
    }

    public class CreateShortUrlResponseDto
    {

    }
}
