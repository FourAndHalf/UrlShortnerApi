
namespace UrlShortner.Application
{
    public interface IUrlShortnerService
    {
        Task<string> CreateRandomShortCode();
    }
}