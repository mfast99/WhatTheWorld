import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import AppLayout from './presentation/layout/AppLayout'
import './styles/globals.css'

const container = document.getElementById('root')
if (!container) {
  throw new Error('Root container not found')
}

ReactDOM.createRoot(container).render(
  <React.StrictMode>
    <AppLayout>
      <App />
    </AppLayout>
  </React.StrictMode>
)
