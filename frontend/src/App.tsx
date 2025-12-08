import { JSX } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Navbar from './presentation/components/Navbar'
import HomePage from './presentation/pages/HomePage'
import ImpressumPage from './presentation/pages/ImpressumPage'
import DatenschutzPage from './presentation/pages/DatenschutzPage'
import NotFoundPage from './presentation/pages/NotFoundPage'
import { ErrorBoundary } from 'react-error-boundary'

function ErrorFallback({ error }: { error: Error }) {
  return (
    <div className="min-h-screen flex items-center justify-center bg-[rgb(var(--color-bg))] text-[rgb(var(--color-text))]">
      <div className="text-center px-4">
        <h1 className="text-6xl font-bold text-red-500">⚠️</h1>
        <h2 className="text-2xl font-semibold mt-4">Something went wrong</h2>
        <p className="text-sm opacity-60 mt-2">{error.message}</p>
        <button
          onClick={() => window.location.reload()}
          className="mt-6 px-6 py-3 bg-sky-500 text-white rounded-lg hover:bg-sky-600"
        >
          Reload Page
        </button>
      </div>
    </div>
  )
}

export default function App(): JSX.Element {
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
        <BrowserRouter>
        <div className="min-h-screen bg-[rgb(var(--color-bg))]">
            <Navbar />
                <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/impressum" element={<ImpressumPage />} />
                <Route path="/datenschutz" element={<DatenschutzPage />} />
                <Route path="*" element={<NotFoundPage />} />  {/* ✅ Catch-all */}
                </Routes>
        </div>
        </BrowserRouter>
    </ErrorBoundary>
  )
}
