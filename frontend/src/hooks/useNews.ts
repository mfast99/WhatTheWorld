import { useState, useEffect, useRef } from 'react'
import { News } from '../domain/models/News'
import { NewsRepository } from '../infrastructure/repository/NewsRepository'

interface UseNewsReturn {
  news: News[]
  isLoadingCached: boolean
  isRefreshing: boolean
  error: Error | null
  wasRefreshed: boolean
  nextRefreshAllowed: Date | null
}

export function useNews(countryId: number): UseNewsReturn {
  const [news, setNews] = useState<News[]>([])
  const [isLoadingCached, setIsLoadingCached] = useState(true)
  const [isRefreshing, setIsRefreshing] = useState(false)
  const [error, setError] = useState<Error | null>(null)
  const [wasRefreshed, setWasRefreshed] = useState(false)
  const [nextRefreshAllowed, setNextRefreshAllowed] = useState<Date | null>(null)

  const prevCountryIdRef = useRef<number | null>(null)

  useEffect(() => {
    const hasCountryChanged = prevCountryIdRef.current !== countryId
    prevCountryIdRef.current = countryId

    if (!hasCountryChanged && news.length > 0) {
      return
    }

    let isMounted = true

    const fetchNews = async (): Promise<void> => {
      try {
        if (isMounted) {
          setNews([])
          setIsLoadingCached(true)
          setIsRefreshing(false)
          setError(null)
          setWasRefreshed(false)
          setNextRefreshAllowed(null)
        }

        const cached = await NewsRepository.getCached(countryId)
        
        if (!isMounted) return
        
        setNews(cached)
        setIsLoadingCached(false)
        
        await new Promise(resolve => setTimeout(resolve, 500))

        if (!isMounted) return
        
        setIsRefreshing(true)

        const result = await NewsRepository.refresh(countryId)

        if (!isMounted) return

        setIsRefreshing(false)

        if (!result) {
          console.warn(`⚠️ [useNews] Refresh failed`)
          setError(new Error('Failed to refresh news'))
          return
        }

        if (result.wasRefreshed) {
          setNews(result.news)
          setWasRefreshed(true)
        } else {
          setNextRefreshAllowed(new Date(result.nextFetchAllowed))
        }
      } catch (err) {
        console.error(`❌ [useNews] Error:`, err)
        
        if (isMounted) {
          setError(err as Error)
          setIsLoadingCached(false)
          setIsRefreshing(false)
        }
      }
    }

    fetchNews()

    return () => {
      isMounted = false
    }
  }, [countryId, news.length])

  return {
    news,
    isLoadingCached,
    isRefreshing,
    error,
    wasRefreshed,
    nextRefreshAllowed,
  }
}
