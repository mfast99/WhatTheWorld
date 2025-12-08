import { JSX } from 'react'
import { Link } from 'react-router-dom'

export default function DatenschutzPage(): JSX.Element {
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
          <h1 className="text-4xl font-bold mb-2">Datenschutzerklärung</h1>
          <p className="text-sm opacity-60">Informationen gemäß DSGVO</p>
        </div>

        <div className="space-y-6">
          <section>
            <h2 className="text-xl font-semibold mb-3">1. Datenschutz auf einen Blick</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75 space-y-2">
              <h3 className="font-semibold opacity-100">Allgemeine Hinweise</h3>
              <p>
                Diese Website speichert keine personenbezogenen Daten über Cookies oder 
                Tracking-Technologien. Es erfolgt keine Analyse Ihres Nutzerverhaltens 
                zu Marketingzwecken.
              </p>
              <p className="mt-2">
                Lediglich technisch notwendige Server-Log-Dateien werden temporär gespeichert 
                (siehe Punkt 2).
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">2. Server-Log-Files</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75 space-y-3">
              <div>
                <h3 className="font-semibold mb-1">Was wird gespeichert?</h3>
                <p>
                  Der Webserver (Hetzner Online GmbH) erhebt und speichert automatisch 
                  folgende Informationen in Server-Log-Dateien:
                </p>
                <ul className="list-disc list-inside ml-2 mt-2 space-y-1">
                  <li>IP-Adresse des zugreifenden Rechners</li>
                  <li>Datum und Uhrzeit des Zugriffs</li>
                  <li>Name und URL der abgerufenen Datei</li>
                  <li>Übertragene Datenmenge</li>
                  <li>Meldung über erfolgreichen Abruf (HTTP-Status)</li>
                  <li>Browsertyp und Browserversion</li>
                  <li>Betriebssystem des Nutzers</li>
                  <li>Referrer URL (zuvor besuchte Seite)</li>
                </ul>
              </div>

              <div>
                <h3 className="font-semibold mb-1">Rechtsgrundlage</h3>
                <p>
                  Die Verarbeitung erfolgt gemäß Art. 6 Abs. 1 lit. f DSGVO auf Basis 
                  unseres berechtigten Interesses an der Gewährleistung der Systemsicherheit 
                  und Funktionsfähigkeit der Website.
                </p>
              </div>

              <div>
                <h3 className="font-semibold mb-1">Speicherdauer</h3>
                <p>
                  Diese Daten werden nach spätestens <strong>7 Tagen</strong> automatisch 
                  gelöscht. Eine Zusammenführung dieser Daten mit anderen Datenquellen 
                  wird nicht vorgenommen.
                </p>
              </div>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">3. Eingebundene externe Dienste</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75 space-y-4">
              <div>
                <h3 className="font-semibold mb-1">RestCountries.com</h3>
                <p>
                  Zur Bereitstellung von Länderdaten nutzen wir die API von RestCountries.com. 
                  Dabei wird Ihre IP-Adresse an RestCountries übertragen.
                </p>
                <p className="mt-1">
                  Anbieter: RestCountries<br />
                  Datenschutz: <a 
                    href="https://restcountries.com/" 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className="text-sky-500 hover:underline"
                  >
                    https://restcountries.com/
                  </a>
                </p>
              </div>

              <div>
                <h3 className="font-semibold mb-1">OpenStreetMap / Jawg Maps</h3>
                <p>
                  Zur Darstellung der interaktiven Karte nutzen wir OpenStreetMap-Kartendaten 
                  über den Dienst Jawg Maps. Beim Laden der Karte wird Ihre IP-Adresse an 
                  Jawg Maps übertragen.
                </p>
                <p className="mt-1">
                  Anbieter: Jawg Maps SAS, Frankreich<br />
                  Datenschutz: <a 
                    href="https://www.jawg.io/en/privacy/" 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className="text-sky-500 hover:underline"
                  >
                    https://www.jawg.io/en/privacy/
                  </a>
                </p>
                <p className="mt-1">
                  Rechtsgrundlage: Art. 6 Abs. 1 lit. f DSGVO (berechtigtes Interesse an 
                  funktionaler Kartendarstellung)
                </p>
              </div>

              <div>
                <h3 className="font-semibold mb-1">Perplexity AI</h3>
                <p>
                  News-Daten werden serverseitig über die Perplexity AI API aggregiert. 
                  Es werden <strong>keine</strong> personenbezogenen Daten von Ihnen 
                  an Perplexity übertragen.
                </p>
                <p className="mt-1">
                  Anbieter: Perplexity AI, Inc., USA<br />
                  Datenschutz: <a 
                    href="https://www.perplexity.ai/privacy" 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className="text-sky-500 hover:underline"
                  >
                    https://www.perplexity.ai/privacy
                  </a>
                </p>
              </div>

              <div>
                <h3 className="font-semibold mb-1">OpenWeather API</h3>
                <p>
                  Wetterdaten werden serverseitig über die OpenWeather API bezogen. 
                  Es werden <strong>keine</strong> personenbezogenen Daten von Ihnen 
                  an OpenWeather übertragen.
                </p>
                <p className="mt-1">
                  Anbieter: OpenWeather Ltd., UK<br />
                  Datenschutz: <a 
                    href="https://openweather.co.uk/privacy-policy" 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className="text-sky-500 hover:underline"
                  >
                    https://openweather.co.uk/privacy-policy
                  </a>
                </p>
              </div>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">4. Cookies</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75">
              <p>
                Diese Website verwendet <strong>keine Cookies</strong>. Die Einstellung 
                des Dark/Light-Modus wird lokal in Ihrem Browser (localStorage) gespeichert 
                und verlässt niemals Ihren Computer.
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">5. Hosting</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75">
              <p className="mb-2">
                Diese Website wird gehostet bei:
              </p>
              <p className="font-medium">
                Hetzner Online GmbH<br />
                Industriestr. 25<br />
                91710 Gunzenhausen<br />
                Deutschland
              </p>
              <p className="mt-2">
                Datenschutzerklärung: <a 
                  href="https://www.hetzner.com/de/rechtliches/datenschutz" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="text-sky-500 hover:underline"
                >
                  https://www.hetzner.com/de/rechtliches/datenschutz
                </a>
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">6. Ihre Rechte</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75">
              <p className="mb-2">
                Sie haben gegenüber uns folgende Rechte hinsichtlich der Sie betreffenden 
                personenbezogenen Daten:
              </p>
              <ul className="list-disc list-inside ml-2 space-y-1">
                <li>Recht auf Auskunft (Art. 15 DSGVO)</li>
                <li>Recht auf Berichtigung (Art. 16 DSGVO)</li>
                <li>Recht auf Löschung (Art. 17 DSGVO)</li>
                <li>Recht auf Einschränkung der Verarbeitung (Art. 18 DSGVO)</li>
                <li>Recht auf Widerspruch (Art. 21 DSGVO)</li>
                <li>Recht auf Datenübertragbarkeit (Art. 20 DSGVO)</li>
              </ul>
              <p className="mt-3">
                Sie haben zudem das Recht, sich bei einer Datenschutz-Aufsichtsbehörde 
                über die Verarbeitung Ihrer personenbezogenen Daten durch uns zu beschweren.
              </p>
              <p className="mt-3">
                Kontakt: <a 
                  href="mailto:mikefast@gmx.net" 
                  className="text-sky-500 hover:underline"
                >
                  mikefast@gmx.net
                </a>
              </p>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-semibold mb-3">7. SSL-Verschlüsselung</h2>
            <div className="bg-white/5 dark:bg-white/5 border border-[rgb(var(--color-border))] rounded-lg p-4 text-sm opacity-75">
              <p>
                Diese Seite nutzt aus Sicherheitsgründen und zum Schutz der Übertragung 
                vertraulicher Inhalte eine SSL-Verschlüsselung. Eine verschlüsselte 
                Verbindung erkennen Sie daran, dass die Adresszeile des Browsers von 
                "http://" auf "https://" wechselt.
              </p>
            </div>
          </section>
        </div>

        <div className="mt-12 pt-6 border-t border-[rgb(var(--color-border))]">
          <p className="text-sm opacity-50 text-center">
            Stand: {new Date().toLocaleDateString('de-DE', { 
              day: '2-digit', 
              month: '2-digit', 
              year: 'numeric' 
            })}
          </p>
        </div>
      </div>
    </div>
  )
}
