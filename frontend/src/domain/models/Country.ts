export interface CountryListItem {
  id: number
  name: string
  lat: number
  lon: number
}

export interface Country{
    "id": number,
    "code": string,
    "name": string,
    "capital": string,
    "flagEmoji": string,
    "lat": number,
    "lon": number,
    "region": string,
    "subregion": string,
    "population": number,
    "areaKm2": string,
    "timezones": string,
    "currencies": string,
    "languages": string
}