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
      return
    }

    let isMounted = true
    
    setCountry(null)
    setError(null)

    const fetchCountryDetails = async () => {
      try {
        setIsLoading(true)
        
        const data = await CountryRepository.getById(countryId)
        
        if (!isMounted) return
        
        if (data) {
          setCountry(data)
        } else {
          setError(new Error('Country not found'))
        }
      } catch (err) {
        if (isMounted) {
          setError(err as Error)
        }
      } finally {
        if (isMounted) {
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
