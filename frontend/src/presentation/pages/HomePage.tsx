import { JSX, useState } from 'react'
import { useCountries } from '../../hooks/useCountries'
import { useCountryDetails } from '../../hooks/useCountryDetails'
import { CountryListItem } from '../../domain/models/Country'
import CountrySelector from '../components/CountrySelector'
import CountryDetail from '../components/CountryDetail'
import { useResponsive } from '../../hooks/useResponsive'

export default function HomePage(): JSX.Element {
  const { countries, isLoading, error } = useCountries()
  const [selectedCountryId, setSelectedCountryId] = useState<number | null>(null)
  const { country: selectedCountry, isLoading: isLoadingDetails } = useCountryDetails(selectedCountryId)
  const { isMobile } = useResponsive()

  const handleSelectCountry = (country: CountryListItem) => {
    setSelectedCountryId(country.id)
  }

  if (error) {
    return (
      <div className="h-screen flex items-center justify-center bg-[rgb(var(--color-bg))]">
        <div className="text-red-500 text-xl">❌ Error loading data!</div>
      </div>
    )
  }

  return (
    <div 
      style={{
        position: 'fixed',
        top: '64px',
        left: 0,
        right: 0,
        bottom: 0,
        overflow: 'hidden'
      }}
    >
      {!isMobile ? (
        <CountrySelector
          countries={countries}
          selectedCountry={selectedCountry}
          isLoadingDetails={isLoadingDetails}
          onSelectCountry={handleSelectCountry}
          isLoading={isLoading}
        />
      ) : selectedCountry ? (
        <div className="h-full flex flex-col bg-[rgb(var(--color-bg))]">
          <button
            onClick={() => setSelectedCountryId(null)}
            className="p-4 font-semibold border-b border-[rgb(var(--color-border))] shrink-0"
          >
            ← Back to list
          </button>
          <div className="flex-1 overflow-y-auto">
            {isLoadingDetails ? (
              <div className="p-6">
                <div className="animate-pulse space-y-4">
                  <div className="h-8 bg-black/10 dark:bg-white/10 rounded w-3/4" />
                  <div className="h-4 bg-black/10 dark:bg-white/10 rounded w-1/2" />
                  <div className="h-32 bg-black/10 dark:bg-white/10 rounded" />
                </div>
              </div>
            ) : (
              <CountryDetail country={selectedCountry} />
            )}
          </div>
        </div>
      ) : (
        <CountrySelector
          countries={countries}
          selectedCountry={null}
          isLoadingDetails={false}
          onSelectCountry={handleSelectCountry}
          isLoading={isLoading}
        />
      )}
    </div>
  )
}
