using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain
{
    public class JisShortUrl
    {
        [Key]
        public int JisUid { get; set; }

        [Required]
        public required string JisOriginalUrl { get; set; }

        [Required]
        public required string JisShortenUrl { get; set; }

        public DateTime JisCreatedAt { get; set; }

        public DateTime JisExpiresAt { get; set; }

        public int JisClickCount { get; set; }
    }
}