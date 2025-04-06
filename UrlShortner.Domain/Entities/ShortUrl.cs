using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Domain.Entities
{
    public class ShortUrl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string OriginalUrl { get; set; }

        [Required]
        public required string ShortenedUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int ClickCount { get; set; }
    }
}