import { useState, useEffect, useRef } from 'react'
import { News } from '../domain/models/News'
import { NewsRepository } from '../infrastructure/repository/NewsRepository'

interface UseNewsReturn {
  news: News[]
  isLoadingCached: boolean
  isRefreshing: boolean
  error: Error | null
  wasRefreshed: boolean
}

export function useNews(countryId: number): UseNewsReturn {
  const [news, setNews] = useState<News[]>([])
  const [isLoadingCached, setIsLoadingCached] = useState(true)
  const [isRefreshing, setIsRefreshing] = useState(false)
  const [error, setError] = useState<Error | null>(null)
  const [wasRefreshed, setWasRefreshed] = useState(false)

  const prevCountryIdRef = useRef<number | null>(null)

  useEffect(() => {
    let active = true

    const fetchData = async () => {
      try {
        setNews([])
        setIsLoadingCached(true)
        setIsRefreshing(false)
        setError(null)
        setWasRefreshed(false)

        const cached = await NewsRepository.getCached(countryId)
        
        if (!active) return
        
        setNews(cached)
        setIsLoadingCached(false)

        await new Promise(r => setTimeout(r, 300))
        
        if (!active) return

        setIsRefreshing(true)
        
        const result = await NewsRepository.refresh(countryId)
        
        if (!active) return

        setIsRefreshing(false)

        if (result?.wasRefreshed && result.news?.length > 0) {
          setNews(result.news)
          setWasRefreshed(true)
        }
      } catch (err) {
        if (active) {
          setError(err as Error)
          setIsLoadingCached(false)
          setIsRefreshing(false)
        }
      }
    }

    if (prevCountryIdRef.current !== countryId) {
      prevCountryIdRef.current = countryId
      fetchData()
    }

    return () => {
      active = false
    }
  }, [countryId])

  return { news, isLoadingCached, isRefreshing, error, wasRefreshed }
}
