import { useQuery, UseQueryResult } from '@tanstack/react-query'
import { Weather } from '../domain/models/Weather'
import { WeatherRepository } from '../infrastructure/repository/WeatherRepository'

interface UseWeatherOptions {
  enabled?: boolean | undefined
}

export function useWeather(
  countryId: number,
  options?: UseWeatherOptions 
): UseQueryResult<Weather | null, Error> {
  return useQuery<Weather | null, Error>({
    queryKey: ['weather', countryId],
    
    queryFn: async (): Promise<Weather | null> => {
      return await WeatherRepository.getByCountryId(countryId)
    },
    
    enabled: options?.enabled ?? true,
    staleTime: 1000 * 60 * 60 * 3,
    gcTime: 1000 * 60 * 60 * 6,
  })
}
