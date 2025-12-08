import { JSX } from 'react'
import { useTheme } from '../../hooks/useTheme'

export default function ThemeToggle(): JSX.Element {
  const { colorScheme, toggleTheme } = useTheme()

  return (
    <button
      onClick={toggleTheme}
      className="px-3 py-2 bg-[rgb(var(--color-bg))] border border-[rgb(var(--color-border))] rounded-lg hover:bg-sky-500 hover:text-white hover:border-sky-500 transition-all duration-200"
      aria-label={`Switch to ${colorScheme === 'light' ? 'dark' : 'light'} mode`}
    >
      <span className="flex items-center gap-2">
        <span className="text-lg">
          {colorScheme === 'light' ? '🌙' : '☀️'}
        </span>
        <span className="hidden sm:inline text-sm font-medium">
          {colorScheme === 'light' ? 'Dark' : 'Light'}
        </span>
      </span>
    </button>
  )
}
