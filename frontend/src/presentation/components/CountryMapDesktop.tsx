import { JSX, useEffect, useRef } from 'react'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import CountryDetail from './CountryDetail'
import { Country, CountryListItem } from '../../domain/models/Country'
import { useTheme } from '../../hooks/useTheme'

interface CountryMapDesktopProps {
  countries: CountryListItem[]
  selectedCountry: Country | null
  isLoadingDetails: boolean
  onSelectCountry: (country: CountryListItem) => void
}

export default function CountryMapDesktop({
  countries,
  selectedCountry,
  isLoadingDetails,
  onSelectCountry,
}: CountryMapDesktopProps): JSX.Element {
  const mapRef = useRef<HTMLDivElement>(null)
  const mapInstanceRef = useRef<L.Map | null>(null)
  const tileLayerRef = useRef<L.TileLayer | null>(null)
  const markersRef = useRef<L.Marker[]>([])
  const { colorScheme } = useTheme()

  // Map Initialization
  useEffect(() => {
    if (!mapRef.current || mapInstanceRef.current) return

    const extendedBounds = L.latLngBounds(
      [-90, -360],
      [90, 360]
    )

    mapInstanceRef.current = L.map(mapRef.current, {
      center: [20, 0],
      zoom: 2,
      minZoom: 2,
      maxZoom: 18,
      maxBounds: extendedBounds,
      maxBoundsViscosity: 1.0,
      zoomControl: true,
      attributionControl: true,
      scrollWheelZoom: true,
      doubleClickZoom: true,
      touchZoom: true,
      dragging: true,
      inertia: false,
      worldCopyJump: false,
      zoomSnap: 1,
      zoomDelta: 1
    })

    return () => {
      if (mapInstanceRef.current) {
        mapInstanceRef.current.remove()
        mapInstanceRef.current = null
      }
    }
  }, [])

  // Tile Layer
  useEffect(() => {
    if (!mapInstanceRef.current) return

    if (tileLayerRef.current) {
      mapInstanceRef.current.removeLayer(tileLayerRef.current)
    }

    const tileUrl = colorScheme === 'light'
      ? `http://localhost:5230/api/Map/tiles/light/{z}/{x}/{y}.png`
      : `http://localhost:5230/api/Map/tiles/dark/{z}/{x}/{y}.png`

    tileLayerRef.current = L.tileLayer(tileUrl, {
      attribution: 
        '<a href="https://jawg.io" target="_blank">&copy; Jawg Maps</a> ' +
        '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
      minZoom: 2,
      maxZoom: 18,
      noWrap: false,
      keepBuffer: 2
    }).addTo(mapInstanceRef.current)
  }, [colorScheme])

  // Markers
  useEffect(() => {
    if (!mapInstanceRef.current) return

    markersRef.current.forEach(marker => {
      mapInstanceRef.current?.removeLayer(marker)
    })
    markersRef.current = []

    const createCustomIcon = (isSelected: boolean = false) => {
      const isDark = colorScheme === 'dark'
      
      return L.divIcon({
        html: `
          <div class="pin-marker" style="
            position: relative;
            width: 40px;
            height: 56px;
            cursor: pointer;
          ">
            <div class="pin-head" style="
              position: absolute;
              top: 0;
              left: 50%;
              transform: translateX(-50%);
              width: 40px;
              height: 40px;
              background: ${isSelected 
                ? '#0ea5e9'
                : isDark 
                  ? '#1e293b'
                  : '#ffffff'
              };
              border: 2px solid ${isSelected 
                ? '#0284c7'
                : isDark 
                  ? '#475569'
                  : '#cbd5e1'
              };
              border-radius: 50%;
              box-shadow: ${isSelected
                ? '0 4px 12px rgba(14, 165, 233, 0.4)'
                : isDark
                  ? '0 2px 8px rgba(0, 0, 0, 0.6)'
                  : '0 2px 8px rgba(0, 0, 0, 0.15)'
              };
              display: flex;
              align-items: center;
              justify-content: center;
              transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            ">
              <span style="
                font-size: 20px;
                font-weight: 700;
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', system-ui, sans-serif;
                color: ${isSelected 
                  ? '#ffffff'
                  : isDark 
                    ? '#e2e8f0'
                    : '#334155'
                };
                line-height: 1;
                transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
              ">i</span>
            </div>
            
            <div class="pin-needle" style="
              position: absolute;
              top: 38px;
              left: 50%;
              transform: translateX(-50%);
              width: 2px;
              height: 18px;
              background: ${isSelected 
                ? '#0ea5e9'
                : isDark 
                  ? '#475569'
                  : '#94a3b8'
              };
              transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            "></div>
          </div>
          
          <style>
            .pin-marker:hover .pin-head {
              transform: translateX(-50%) scale(1.15);
              box-shadow: ${isSelected
                ? '0 6px 16px rgba(14, 165, 233, 0.6)'
                : isDark
                  ? '0 4px 12px rgba(0, 0, 0, 0.7)'
                  : '0 4px 12px rgba(0, 0, 0, 0.25)'
              };
            }
            
            .pin-marker:hover .pin-head span {
              transform: scale(1.1);
            }
            
            .pin-marker:active .pin-head {
              transform: translateX(-50%) scale(1.05);
            }
          </style>
        `,
        className: '',
        iconSize: [40, 56],
        iconAnchor: [20, 56],
        popupAnchor: [0, -56]
      })
    }

    countries.forEach((country) => {
      const isSelected = selectedCountry?.id === country.id
      
      const marker = L.marker([country.lat, country.lon], {
        icon: createCustomIcon(isSelected),
        interactive: true,
        bubblingMouseEvents: false
      })
        .addTo(mapInstanceRef.current!)

      marker.on('click', () => {
        onSelectCountry(country)
      })

      markersRef.current.push(marker)
    })

    console.log(`✅ [Map] ${markersRef.current.length} markers added`)
  }, [countries, selectedCountry, onSelectCountry, colorScheme])

  return (
    <div 
        className="fixed inset-0 flex"
        style={{
        width: '100vw',
        height: '100vh',
        top: 0,
        left: 0,
        background: colorScheme === 'light' 
            ? 'linear-gradient(to bottom, #e0f2fe, #bae6fd)'
            : 'linear-gradient(to bottom, #0f172a, #1e293b)'
        }}
    >
    <div 
      ref={mapRef}
      className="flex-1"
      style={{
        position: 'relative',
        width: selectedCountry ? 'calc(100vw - 24rem)' : '100vw',
        height: '100vh'
      }}
    />
      
      {selectedCountry && (
        <div 
          className="bg-[rgb(var(--color-bg))] border-l border-[rgb(var(--color-border))]"
          style={{
            width: '24rem',
            height: '100vh',
            position: 'relative',
            overflow: 'hidden',
            display: 'flex',
            flexDirection: 'column'
          }}
        >
          <div 
            style={{
              flex: 1,
              overflowY: 'auto',
              overflowX: 'hidden'
            }}
          >
            {isLoadingDetails ? (
              <div className="p-6">
                <div className="animate-pulse space-y-4">
                  <div className="h-12 bg-black/10 dark:bg-white/10 rounded w-3/4" />
                  <div className="h-6 bg-black/10 dark:bg-white/10 rounded w-1/2" />
                  <div className="space-y-3 mt-6">
                    <div className="h-32 bg-black/10 dark:bg-white/10 rounded" />
                    <div className="h-32 bg-black/10 dark:bg-white/10 rounded" />
                    <div className="h-32 bg-black/10 dark:bg-white/10 rounded" />
                  </div>
                </div>
              </div>
            ) : (
              <CountryDetail country={selectedCountry} />
            )}
          </div>
        </div>
      )}
    </div>
  )
}
