import http from '../api/http'
import { News } from '../../domain/models/News'

export interface NewsRefreshResult {
  wasRefreshed: boolean
  news: News[]
}

export class NewsRepository {
  static async getCached(countryId: number): Promise<News[]> {
    try {
      const response = await http.get<News[]>('/News/cached', {
        params: { countryId }
      })
      
      return response.data || []
    } catch (error) {
      console.error('[NewsRepository] getCached error:', error)
      return []
    }
  }

  static async refresh(countryId: number): Promise<NewsRefreshResult | null> {
    try {
      const response = await http.post<NewsRefreshResult>(
        '/News/refresh',
        null,
        {
          params: { countryId },
          timeout: 20000
        }
      )

      return response.data
    } catch (error) {
      console.error('[NewsRepository] refresh error:', error)
      return null
    }
  }
}
