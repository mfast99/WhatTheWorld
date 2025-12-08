import { useState, useEffect } from 'react'
import { CountryListItem } from '../domain/models/Country'
import { CountryRepository } from '../infrastructure/repository/CountryRepository'

interface UseCountriesReturn {
  countries: CountryListItem[]
  isLoading: boolean
  error: Error | null
}

export function useCountries(): UseCountriesReturn {
  const [countries, setCountries] = useState<CountryListItem[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<Error | null>(null)

  useEffect(() => {
    const fetchCountries = async () => {
      try {
        const data = await CountryRepository.getList()
        
        setCountries(data)
        setIsLoading(false)
      } catch (err) {
        console.error('❌ [useCountries] Error:', err)
        setError(err as Error)
        setIsLoading(false)
      }
    }

    fetchCountries()
  }, [])

  return { countries, isLoading, error }
}
