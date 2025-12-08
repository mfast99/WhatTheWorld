import { JSX, useState } from 'react'
import { Link, useLocation } from 'react-router-dom'
import ThemeToggle from './ThemeToggle'

export default function Navbar(): JSX.Element {
  const location = useLocation()
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false)

  const isActive = (path: string) => location.pathname === path

  return (
    <nav className="fixed top-0 left-0 right-0 z-10000 bg-[rgb(var(--color-bg))]/90 backdrop-blur-md border-b border-[rgb(var(--color-border))] shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo / Brand */}
          <Link 
            to="/" 
            className="flex items-center gap-2 font-bold text-lg hover:opacity-80 transition-opacity"
          >
            <span className="text-2xl">🌍</span>
            <span className="hidden sm:inline">WhatTheWorld</span>
          </Link>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center gap-6">
            <Link
              to="/"
              className={`text-sm font-medium transition-colors ${
                isActive('/') 
                  ? 'text-sky-500' 
                  : 'opacity-70 hover:opacity-100'
              }`}
            >
              Map
            </Link>
            
            <Link
              to="/impressum"
              className={`text-sm font-medium transition-colors ${
                isActive('/impressum') 
                  ? 'text-sky-500' 
                  : 'opacity-70 hover:opacity-100'
              }`}
            >
              Impressum
            </Link>
            
            <Link
              to="/datenschutz"
              className={`text-sm font-medium transition-colors ${
                isActive('/datenschutz') 
                  ? 'text-sky-500' 
                  : 'opacity-70 hover:opacity-100'
              }`}
            >
              Privacy
            </Link>

            <div className="ml-2">
              <ThemeToggle />
            </div>
          </div>

          {/* Mobile Menu Button */}
          <div className="flex items-center gap-2 md:hidden">
            <ThemeToggle />
            <button
              onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
              className="p-2 rounded-lg hover:bg-black/5 dark:hover:bg-white/5 transition-colors"
              aria-label="Toggle menu"
            >
              <svg
                className="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                {isMobileMenuOpen ? (
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M6 18L18 6M6 6l12 12"
                  />
                ) : (
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M4 6h16M4 12h16M4 18h16"
                  />
                )}
              </svg>
            </button>
          </div>
        </div>
      </div>

      {/* Mobile Menu */}
      {isMobileMenuOpen && (
        <div className="md:hidden border-t border-[rgb(var(--color-border))] bg-[rgb(var(--color-bg))]">
          <div className="px-4 py-3 space-y-2">
            <Link
              to="/"
              onClick={() => setIsMobileMenuOpen(false)}
              className={`block px-3 py-2 rounded-lg text-sm font-medium transition-colors ${
                isActive('/')
                  ? 'bg-sky-500/10 text-sky-500'
                  : 'hover:bg-black/5 dark:hover:bg-white/5'
              }`}
            >
              Map
            </Link>
            
            <Link
              to="/impressum"
              onClick={() => setIsMobileMenuOpen(false)}
              className={`block px-3 py-2 rounded-lg text-sm font-medium transition-colors ${
                isActive('/impressum')
                  ? 'bg-sky-500/10 text-sky-500'
                  : 'hover:bg-black/5 dark:hover:bg-white/5'
              }`}
            >
              Impressum
            </Link>
            
            <Link
              to="/datenschutz"
              onClick={() => setIsMobileMenuOpen(false)}
              className={`block px-3 py-2 rounded-lg text-sm font-medium transition-colors ${
                isActive('/datenschutz')
                  ? 'bg-sky-500/10 text-sky-500'
                  : 'hover:bg-black/5 dark:hover:bg-white/5'
              }`}
            >
              Privacy
            </Link>
          </div>
        </div>
      )}
    </nav>
  )
}
