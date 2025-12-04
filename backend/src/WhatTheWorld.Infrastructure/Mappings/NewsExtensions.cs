using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Mappings
{
    public static class NewsExtensions
    {
        public static NewsDto? ToDto(this NewsEntity? news)
        {
            if (news == null)
                return null;

            return new NewsDto(
                news.Id,
                news.PublishedAt,
                news.Title,
                news.Url,
                news.Source,
                news.Summary
            );
        }
    }
}
