import { JSX, ReactNode } from 'react'
import { ThemeProvider } from '../../core/state/ThemeContext'
import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from '../../core/config/queryClient'
import ThemeToggle from '../components/ThemeToggle'

interface AppLayoutProps {
  children: ReactNode
}

export default function AppLayout({ children }: AppLayoutProps): JSX.Element {
  return (
    <ThemeProvider>
      <QueryClientProvider client={queryClient}>
        <ThemeToggle />
        {children}
      </QueryClientProvider>
    </ThemeProvider>
  )
}
