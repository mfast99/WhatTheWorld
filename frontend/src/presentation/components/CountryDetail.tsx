import { JSX } from 'react'
import NewsSection from './NewsSection'
import WeatherSection from './WeatherSection'
import CountryInfo from './CountryInfo'
import { Country } from '../../domain/models/Country'

interface CountryDetailProps {
  country: Country
}

export default function CountryDetail({
  country,
}: CountryDetailProps): JSX.Element {
  return (
    <div className="p-6 space-y-6">
      <div>
        <br/><br/>
        <h1 className="text-3xl font-bold">{country.name}</h1>
        <p className="text-sm opacity-75">{country.region}</p>
      </div>

      <CountryInfo country={country} />

      <WeatherSection
        countryId={country.id}
        countryName={country.name}
        lat={country.lat}
        lon={country.lon}
      />

      <NewsSection 
        countryId={country.id} 
        countryName={country.name} 
      />
    </div>
  )
}
