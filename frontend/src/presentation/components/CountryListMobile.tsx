import { JSX } from 'react'
import { CountryListItem } from '../../domain/models/Country'

interface CountryListMobileProps {
  countries: CountryListItem[]
  onSelectCountry: (country: CountryListItem) => void
}

export default function CountryListMobile({
  countries,
  onSelectCountry,
}: CountryListMobileProps): JSX.Element {
  return (
    <div className="h-screen overflow-y-auto bg-[rgb(var(--color-bg))]">
      <div className="p-4">
        <h1 className="text-2xl font-bold mb-4 text-[rgb(var(--color-text))]">Countries</h1>
        
        <div className="space-y-2">
          {countries.map((country) => (
            <button
              key={country.id}
              onClick={() => onSelectCountry(country)}
              className="w-full p-4 rounded-lg shadow hover:shadow-md transition-shadow text-left"
              style={{
                backgroundColor: 'rgb(var(--color-bg))',
                border: '1px solid rgb(var(--color-border))',
                color: 'rgb(var(--color-text))'
              }}
            >
              <div className="flex items-center gap-3">
                <div 
                  className="w-10 h-10 rounded-full flex items-center justify-center"
                  style={{
                    backgroundColor: 'rgba(14, 165, 233, 0.1)',
                  }}
                >
                  <span className="text-lg font-bold text-sky-500">i</span>
                </div>
                <div className="flex-1">
                  <h3 className="font-semibold text-[rgb(var(--color-text))]">{country.name}</h3>
                  <p className="text-sm opacity-60">
                    {country.lat.toFixed(2)}°, {country.lon.toFixed(2)}°
                  </p>
                </div>
                <div className="opacity-40">→</div>
              </div>
            </button>
          ))}
        </div>
      </div>
    </div>
  )
}
