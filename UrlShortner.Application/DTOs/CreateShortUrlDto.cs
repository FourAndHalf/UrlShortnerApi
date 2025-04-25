namespace UrlShortner.Application
{
    public class CreateShortUrlDto
    {
        string originalUrl;
        string shortenUrl;
        int daysToExpiry;
    }
}