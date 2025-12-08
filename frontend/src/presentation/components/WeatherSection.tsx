import { JSX } from 'react'
import { useWeather } from '../../hooks/useWeather'

interface WeatherSectionProps {
  countryId: number
  countryName: string
  lat: number
  lon: number
}

export default function WeatherSection({
  countryId,
  countryName,
  lat,
  lon,
}: WeatherSectionProps): JSX.Element {
  const { data: weather, isLoading, isError } = useWeather(countryId)

  if (isLoading) {
    return (
      <div className="glass-card">
        <h3 className="text-lg font-semibold mb-4">🌤️ Wetter</h3>
        <div className="text-center animate-pulse">
          <div className="w-16 h-16 bg-black/20 rounded-full mx-auto mb-2" />
          <div className="h-8 bg-black/20 rounded w-24 mx-auto mb-2" />
          <div className="h-4 bg-black/20 rounded w-32 mx-auto" />
        </div>
      </div>
    )
  }

  if (isError) {
    return (
      <div className="glass-card">
        <h3 className="text-lg font-semibold mb-4">🌤️ Weather</h3>
        <div className="text-center py-4 text-red-500">
          ❌ Error during loading
        </div>
      </div>
    )
  }

  if (!weather) {
    return (
      <div className="glass-card">
        <h3 className="text-lg font-semibold mb-4">🌤️ Weather</h3>
        <div className="text-center py-4 opacity-75">
          No weatherdata for {countryName} available
        </div>
      </div>
    )
  }

  return (
    <div className="glass-card">
      <h3 className="text-lg font-semibold mb-4">🌤️ Weather</h3>
      <div className="text-center">
        <img
          src={weather.iconUrl}
          alt={weather.description}
          className="w-20 h-20 mx-auto mb-2"
        />
        <p className="text-4xl font-bold mb-1">
          {Math.round(weather.tempCelsius)}°C
        </p>
        <p className="text-sm opacity-75 mb-3">{weather.description}</p>
        <p className="text-sm opacity-60">{countryName}</p>
        <p className="text-xs opacity-50 mt-2">
          {lat.toFixed(2)}, {lon.toFixed(2)}
        </p>
        <p className="text-xs opacity-40 mt-3">
          Updated: {new Date(weather.time).toLocaleTimeString('de-DE')}
        </p>
      </div>
    </div>
  )
}
