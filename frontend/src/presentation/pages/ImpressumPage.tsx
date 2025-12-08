import { JSX } from 'react'
import { Link } from 'react-router-dom'

export default function ImpressumPage(): JSX.Element {
  return (
    <div className="min-h-screen pt-16 bg-[rgb(var(--color-bg))] text-[rgb(var(--color-text))]">
      <div className="py-12 px-4 max-w-3xl mx-auto">
        <Link 
          to="/"
          className="inline-flex items-center gap-2 text-sm text-sky-500 hover:text-sky-600 mb-6"
        >
          <span>←</span>
          <span>Back to Map</span>
        </Link>

        <div className="mb-8">
          <h1 className="text-4xl font-bold mb-2">Impressum</h1>
          <p className="text-sm opacity-60">Angaben gemäß § 5 TMG</p>
        </div>

        <div className="space-y-6">
          <section>
            <h2 className="text-xl font-semibold mb-3">Verantwortlich für den Inhalt</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4">
              <p className="font-medium">Mike Fast</p>
              <p className="text-sm opacity-75">Helmstedter Straße 75</p>
              <p className="text-sm opacity-75">38126 Braunschweig</p>
              <p className="text-sm opacity-75 mt-2">Deutschland</p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">Kontakt</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 space-y-1">
              <p className="text-sm">
                <span className="opacity-60">E-Mail:</span>{' '}
                <a 
                  href="mailto:mikefast@gmx.net" 
                  className="text-sky-500 hover:underline"
                >
                  mikefast@gmx.net
                </a>
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">Hinweis</h2>
            <div className="bg-sky-500/10 border border-sky-500/30 rounded-lg p-4">
              <p className="text-sm">
                Dies ist ein <strong>persönliches Projekt</strong>, 
                das ausschließlich zu Lern- und Übungszwecken erstellt wurde
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">Haftungsausschluss</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 space-y-3 text-sm opacity-75">
              <div>
                <h3 className="font-semibold mb-1">Haftung für Inhalte</h3>
                <p>
                  Die Inhalte unserer Seiten wurden mit größter Sorgfalt erstellt. 
                  Für die Richtigkeit, Vollständigkeit und Aktualität der Inhalte 
                  können wir jedoch keine Gewähr übernehmen.
                </p>
              </div>
              
              <div>
                <h3 className="font-semibold mb-1">Haftung für Links</h3>
                <p>
                  Das Angebot enthält Links zu externen Webseiten Dritter, auf deren 
                  Inhalte wir keinen Einfluss haben. Für die Inhalte der verlinkten 
                  Seiten ist stets der jeweilige Anbieter oder Betreiber der Seiten verantwortlich.
                </p>
              </div>

              <div>
                <h3 className="font-semibold mb-1">Urheberrecht</h3>
                <p>
                  Die durch die Seitenbetreiber erstellten Inhalte und Werke auf diesen 
                  Seiten unterliegen dem deutschen Urheberrecht.
                </p>
              </div>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">Verwendete Dienste</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 space-y-2 text-sm">
              <p>Diese Website nutzt folgende externe Dienste:</p>
              <ul className="list-disc list-inside space-y-1 opacity-75 ml-2">
                <li>OpenStreetMap (Kartendaten)</li>
                <li>Jawg Maps (Kartenvisualisierung)</li>
                <li>Perplexity AI (News-Aggregation)</li>
                <li>OpenWeather API (Wetterdaten)</li>
                <li>RestCountries.com (Länderdaten)</li>
              </ul>
            </div>
          </section>
        </div>

        <div className="mt-12 pt-6 border-t border-[rgb(var(--color-border))]">
          <p className="text-sm opacity-50 text-center">
            Stand: {new Date().toLocaleDateString('de-DE')}
          </p>
        </div>
      </div>
    </div>
  )
}
