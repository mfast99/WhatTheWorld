// src/presentation/components/NewsSection.tsx

import { JSX } from 'react'
import { useNews } from '../../hooks/useNews'

interface NewsSectionProps {
  countryId: number
  countryName: string
}

export default function NewsSection({
  countryId,
  countryName,
}: NewsSectionProps): JSX.Element {
  const { 
    news, 
    isLoadingCached, 
    isRefreshing, 
    error, 
    wasRefreshed,
  } = useNews(countryId)

  if (error && news.length === 0) {
    return (
      <div className="glass-card">
        <h3 className="text-lg font-semibold mb-4">📰 News</h3>
        <div className="text-center py-6 text-red-500">
          <p className="text-2xl mb-2">❌</p>
          <p className="font-semibold mb-1">Error during loading</p>
          <p className="text-sm opacity-75 mb-3">{error.message}</p>
          <button 
            onClick={() => window.location.reload()} 
            className="btn-primary"
          >
            Try again
          </button>
        </div>
      </div>
    )
  }

  if (isLoadingCached) {
    return (
      <div className="glass-card">
        <h3 className="text-lg font-semibold mb-4">📰 Nachrichten</h3>
        <div className="space-y-3">
          {[1, 2, 3].map((i) => (
            <div key={i} className="p-3 bg-black/10 dark:bg-white/5 rounded-lg animate-pulse">
              <div className="h-4 bg-black/20 dark:bg-white/10 rounded w-3/4 mb-2" />
              <div className="h-3 bg-black/20 dark:bg-white/10 rounded w-1/2 mb-2" />
              <div className="h-3 bg-black/20 dark:bg-white/10 rounded w-1/3" />
            </div>
          ))}
        </div>
      </div>
    )
  }

  return (
    <div className="glass-card">
      <h3 className="text-lg font-semibold mb-4">📰 News</h3>

      <div className="space-y-3">
        {isRefreshing && (
          <div className="p-3 bg-sky-500/10 border border-sky-500/30 rounded-lg">
            <div className="flex items-center gap-3">
              <div className="w-5 h-5 border-2 border-sky-500 border-t-transparent rounded-full animate-spin shrink-0" />
              <div className="flex-1">
                <p className="text-sm font-semibold text-sky-600 dark:text-sky-400">
                  Checking for news...
                </p>
                <p className="text-xs opacity-60 mt-0.5">
                  This can take up to 10 seconds
                </p>
              </div>
            </div>
          </div>
        )}

        {wasRefreshed && !isRefreshing && (
          <div className="p-2 bg-linear-to-r from-green-500/20 to-emerald-500/20 border border-green-500/50 rounded-lg text-center">
            <p className="text-sm font-bold text-green-700 dark:text-green-300">
              ✨ Updated news!
            </p>
          </div>
        )}

        {news.length === 0 && !isRefreshing && (
          <div className="text-center py-8 opacity-75">
            <p className="text-5xl mb-3">📭</p>
            <p className="text-lg font-semibold mb-1">No news available</p>
            <p className="text-sm opacity-60">
              Found no news for yet {countryName}
            </p>
          </div>
        )}

        {news.map((article, index) => (
          <a
            key={`${article.id}-${index}`}
            href={article.url}
            target="_blank"
            rel="noopener noreferrer"
            className="block p-4 bg-black/10 dark:bg-white/5 rounded-lg hover:bg-black/20 dark:hover:bg-white/10 transition-all duration-200 hover:scale-[1.02]"
          >
            <h4 className="font-semibold line-clamp-2 mb-2 text-[rgb(var(--color-text))]">
              {article.title}
            </h4>
            <p className="text-sm opacity-75 line-clamp-2 mb-3">
              {article.summary}
            </p>
            <div className="flex items-center justify-between text-xs opacity-60">
              <span className="font-medium">{article.source}</span>
              <time dateTime={article.publishedAt}>
                {new Date(article.publishedAt).toLocaleDateString('en-GB', {
                  day: '2-digit',
                  month: 'short',
                  year: 'numeric'
                })}
              </time>
            </div>
          </a>
        ))}
      </div>
    </div>
  )
}
