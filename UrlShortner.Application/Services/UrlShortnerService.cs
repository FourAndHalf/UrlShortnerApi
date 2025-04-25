using System.Text;
using UrlShortner.Shared;

namespace UrlShortner.Application
{
    public class UrlShortnerService : IUrlShortnerService
    {

        public Task<string> CreateRandomShortCode()
        {
            Random random = new();
            StringBuilder shortCode = new();

            while (shortCode.Length < Constants.shortCodeLength)
            {
                int index = random.Next(0, Constants.validShortCodeCharacters.Length);
                shortCode.Append(Constants.validShortCodeCharacters[index]);
            }

            return Task.FromResult(shortCode.ToString());
        }
    }
}