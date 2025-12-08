import { useContext } from 'react'
import { ThemeContext, ThemeContextType } from '../core/state/theme'

export function useTheme(): ThemeContextType {
  const context = useContext(ThemeContext)
  if (!context) {
    throw new Error('useTheme muss innerhalb ThemeProvider verwendet werden')
  }
  return context
}
