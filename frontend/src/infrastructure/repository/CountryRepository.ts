import http from '../api/http'
import { Country, CountryListItem } from '../../domain/models/Country'

export class CountryRepository {
  static async getList(): Promise<CountryListItem[]> {
    try {
      const response = await http.get<CountryListItem[]>('/api/Countries')
      return response.data
    } catch (error) {
      console.error('❌ [CountryRepository] Error fetching list:', error)
      return []
    }
  }

  static async getById(id: number): Promise<Country | null> {
    try {
      const response = await http.get<Country>(`/api/Countries/${id}`)
      return response.data
    } catch (error) {
      console.error(`❌ [CountryRepository] Error fetching country ${id}:`, error)
      return null
    }
  }
}