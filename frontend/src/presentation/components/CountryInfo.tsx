import { JSX } from 'react'
import { Country } from '@/domain/models/Country'

interface CountryInfoProps {
  country: Country
}

export default function CountryInfo({
  country,
}: CountryInfoProps): JSX.Element {
  return (
    <div className="glass-card">
      <h3 className="text-lg font-semibold mb-4">📊 Information</h3>

      <div className="grid grid-cols-2 gap-4 text-sm">
        <div>
          <p className="opacity-75">Capital</p>
          <p className="font-semibold">{country.capital}</p>
        </div>
        <div>
          <p className="opacity-75">Population</p>
          <p className="font-semibold">{country.population}</p>
        </div>
        <div>
          <p className="opacity-75">Area</p>
          <p className="font-semibold">{country.areaKm2} km²</p>
        </div>
        <div>
          <p className="opacity-75">Languages</p>
          <p className="font-semibold">{country.languages}</p>
        </div>
        <div>
          <p className="opacity-75">Currency</p>
          <p className="font-semibold">{country.currencies}</p>
        </div>
        <div>
          <p className="opacity-75">Timezones</p>
          <p className="font-semibold">{country.timezones}</p>
        </div>
      </div>
    </div>
  )
}
