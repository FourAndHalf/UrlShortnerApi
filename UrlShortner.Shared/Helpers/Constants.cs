
using System.ComponentModel;

namespace UrlShortner.Shared
{
    public static class Constants
    {
        public const int shortCodeLength = 7;
        public const string validShortCodeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        public const int defaultExpirationDays = 30;
    }

    public enum UrlProcessingResult
    {
        Success,
        InvalidUrl,
        ExpiredUrl,
        ServerError,
        NotFound
    }
}