import axios, {
  AxiosInstance,
  AxiosResponse,
  AxiosError
} from 'axios'

interface ErrorResponse {
  message: string
  statusCode: number
  timestamp?: string
}

const BASE_URL = 'http://localhost:5230'

const http: AxiosInstance = axios.create({
  baseURL: BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})

http.interceptors.response.use(
  (response: AxiosResponse): AxiosResponse => {
    return response
  },

  (error: AxiosError<ErrorResponse>): Promise<never> => {
    const status = error.response?.status
    const message = error.response?.data?.message || error.message

    console.error(`HTTP Error [${status}]:`, message)

    return Promise.reject(error)
  }
)

export default http

export type { AxiosResponse, AxiosError, ErrorResponse }
