import { useState, useEffect } from 'react'
import { Country } from '../domain/models/Country'
import { CountryRepository } from '../infrastructure/repository/CountryRepository'

interface UseCountryDetailsReturn {
  country: Country | null
  isLoading: boolean
  error: Error | null
}

export function useCountryDetails(countryId: number | null): UseCountryDetailsReturn {
  const [country, setCountry] = useState<Country | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<Error | null>(null)

  useEffect(() => {
    if (!countryId) {
      setCountry(null)
      setIsLoading(false)
      return
    }

    let isMounted = true

    const fetchCountryDetails = async () => {
      try {
        setIsLoading(true)
        setError(null)
        
        const data = await CountryRepository.getById(countryId)
        
        if (!isMounted) return
        
        if (data) {
          setCountry(data)
        } else {
          setError(new Error('Country not found'))
        }
        
        setIsLoading(false)
      } catch (err) {
        console.error(`❌ [useCountryDetails] Error:`, err)
        
        if (isMounted) {
          setError(err as Error)
          setIsLoading(false)
        }
      }
    }

    fetchCountryDetails()

    return () => {
      isMounted = false
    }
  }, [countryId])

  return { country, isLoading, error }
}
