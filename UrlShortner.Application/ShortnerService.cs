
namespace UrlShortner.Application
{
    public class ShortnerService
    {
        private const string randomizer = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private static Random random = new();

        public async Task<string> ShortUrlGenerator()
        {
            try
            {

            }
            catch (Exception ex)
            {
                // await GeneralHelper.WriteErrorToLog
            }

            return string.Empty;
        }

        public async Task<string> GetOriginalUrl(string pShortUrl)
        {
            try
            {

            }
            catch (Exception ex)
            {
                // await GeneralHelper.WriteErrorToLog
            }

            return string.Empty;
        }

    }
}