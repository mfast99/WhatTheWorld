import { JSX } from 'react'
import { Country, CountryListItem } from '../../domain/models/Country'
import { useResponsive } from '../../hooks/useResponsive'
import CountryListMobile from './CountryListMobile'
import CountryMapDesktop from './CountryMapDesktop'

interface CountrySelectorProps {
  countries: CountryListItem[]
  selectedCountry: Country | null
  isLoadingDetails: boolean
  onSelectCountry: (country: CountryListItem) => void
  isLoading: boolean
}

export default function CountrySelector({
  countries,
  selectedCountry,
  isLoadingDetails,
  onSelectCountry,
  isLoading,
}: CountrySelectorProps): JSX.Element {
  const { isMobile } = useResponsive()

  if (isLoading) {
    return (
      <div className="fixed inset-0 flex items-center justify-center bg-[rgb(var(--color-bg))]">
        <div className="text-2xl">⏳ Loading...</div>
      </div>
    )
  }

  return isMobile ? (
    <CountryListMobile
      countries={countries}
      onSelectCountry={onSelectCountry}
    />
  ) : (
    <CountryMapDesktop
      countries={countries}
      selectedCountry={selectedCountry}
      isLoadingDetails={isLoadingDetails}
      onSelectCountry={onSelectCountry}
    />
  )
}
