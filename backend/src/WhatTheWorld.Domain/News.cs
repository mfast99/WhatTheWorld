namespace WhatTheWorld.Domain
{
    public record NewsDto(
        int Id,
        DateTime PublishedAt,
        DateTime CreatedAt,
        string Title,
        string Url,
        string Source,
        string Summary);

    public class NewsEntity
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        // Navigation Property
        public CountryEntity Country { get; set; } = new();
    }
}
