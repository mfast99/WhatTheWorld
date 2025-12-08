import { JSX, ReactNode, useState } from 'react'
import { ThemeContext } from './theme'

interface ThemeProviderProps {
  children: ReactNode
}

export function ThemeProvider({ children }: ThemeProviderProps): JSX.Element {
  const [colorScheme, setColorScheme] = useState<'light' | 'dark'>('light')

  const toggleTheme = (): void => {
    const newScheme = colorScheme === 'light' ? 'dark' : 'light'
    setColorScheme(newScheme)
    document.documentElement.setAttribute('data-color-scheme', newScheme)
  }

  return (
    <ThemeContext.Provider value={{ colorScheme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  )
}
