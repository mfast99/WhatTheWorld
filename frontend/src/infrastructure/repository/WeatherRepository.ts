import http from '../api/http'
import { Weather } from '@domain/models/Weather'

export class WeatherRepository {
  /**
   * Fetch current weather for a specific country
   * @param countryId - The ID of the country
   * @returns Promise with Weather data or null
   * @throws Error if API request fails
   */
  static async getByCountryId(countryId: number): Promise<Weather | null> {
    try {
      const response = await http.get<Weather>('/api/Weather', {
        params: { countryId }
      })
      return response.data
    } catch (error) {
      console.error(`Error fetching weather for country ${countryId}:`, error)
      return null
    }
  }
}
