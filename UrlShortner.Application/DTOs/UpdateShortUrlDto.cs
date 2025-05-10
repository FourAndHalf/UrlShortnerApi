namespace UrlShortner.Application
{
    public class UpdateShortUrlRequestDto
    {
        public int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortenUrl { get; set; }
    }

    public class UpdateShortUrlResponseDto
    {

    }
}
