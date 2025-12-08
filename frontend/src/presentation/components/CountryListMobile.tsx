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
        <h1 className="text-2xl font-bold mb-4">Countries</h1>
        
        <div className="space-y-2">
          {countries.map((country) => (
            <button
              key={country.id}
              onClick={() => onSelectCountry(country)}
              className="w-full p-4 bg-white dark:bg-gray-800 rounded-lg shadow hover:shadow-md transition-shadow text-left"
            >
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 bg-sky-100 dark:bg-sky-900 rounded-full flex items-center justify-center">
                  <span className="text-lg font-bold text-sky-600 dark:text-sky-400">i</span>
                </div>
                <div className="flex-1">
                  <h3 className="font-semibold">{country.name}</h3>
                  <p className="text-sm opacity-60">
                    {country.lat.toFixed(2)}°, {country.lon.toFixed(2)}°
                  </p>
                </div>
                <div className="text-gray-400">→</div>
              </div>
            </button>
          ))}
        </div>
      </div>
    </div>
  )
}
