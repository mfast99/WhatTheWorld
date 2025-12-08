// src/presentation/pages/NotFoundPage.tsx
import { JSX } from 'react'
import { Link } from 'react-router-dom'

export default function NotFoundPage(): JSX.Element {
  return (
    <div className="min-h-screen pt-16 bg-[rgb(var(--color-bg))] text-[rgb(var(--color-text))] flex items-center justify-center">
      <div className="text-center px-4">
        <h1 className="text-9xl font-bold text-sky-500">404</h1>
        <h2 className="text-3xl font-semibold mt-4">Page Not Found</h2>
        <p className="text-lg opacity-60 mt-2">
          The page you're looking for doesn't exist.
        </p>
        <Link 
          to="/"
          className="inline-block mt-8 px-6 py-3 bg-sky-500 text-white rounded-lg font-medium hover:bg-sky-600 transition-colors"
        >
          ← Back to Map
        </Link>
      </div>
    </div>
  )
}
