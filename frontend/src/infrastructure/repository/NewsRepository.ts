import http from '../api/http'
import { News } from '../../domain/models/News'

export interface NewsRefreshResult {
  wasRefreshed: boolean
  news: News[]
  lastFetchTime: string | null
  nextFetchAllowed: string
}

export class NewsRepository {
  static async getCached(countryId: number): Promise<News[]> {
    try {
      const response = await http.get<News[]>('/News/cached', {
        params: { countryId }
      })
      
      return response.data
    } catch (error) {
      console.error('❌ [NewsRepository] Error fetching cached news:', error)
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
          timeout: 20000,
          validateStatus: (status) => status === 200 || status === 304
        }
      )

      if (response.status === 304) {
        const nextRefresh = response.headers['x-next-refresh']
        
        return {
          wasRefreshed: false,
          news: [],
          lastFetchTime: null,
          nextFetchAllowed: nextRefresh || new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString()
        }
      }
      return response.data
    } catch (error) {
      console.error('❌ [NewsRepository] Error refreshing news:', error)
      return null
    }
  }
}
