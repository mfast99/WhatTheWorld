import { createContext } from 'react'

export interface ThemeContextType {
  colorScheme: 'light' | 'dark'
  toggleTheme: () => void
}

export const ThemeContext = createContext<ThemeContextType | undefined>(undefined)
