import { JSX } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Navbar from './presentation/components/Navbar'
import HomePage from './presentation/pages/HomePage'
import ImpressumPage from './presentation/pages/ImpressumPage'
import DatenschutzPage from './presentation/pages/DatenschutzPage'

export default function App(): JSX.Element {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-[rgb(var(--color-bg))]">
        <Navbar />
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/impressum" element={<ImpressumPage />} />
          <Route path="/datenschutz" element={<DatenschutzPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}
