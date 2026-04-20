import { useState } from 'react'
import { UploadZone } from '../components/UploadZone'
import { StockTable } from '../components/StockTable'
import { AnomaliesList } from '../components/AnomaliesList'
import { analyzeInventory } from '../services/inventoryService'
import type { AnalysisResult } from '../types'

export function HomePage() {
  const [result, setResult] = useState<AnalysisResult | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function handleFileSelect(file: File) {
    setIsLoading(true)
    setError(null)
    setResult(null)

    try {
      const data = await analyzeInventory(file)
      setResult(data)
    } catch (err: unknown) {
      if (err instanceof Error) {
        setError(err.message)
      } else {
        setError('Erro ao analisar o arquivo. Verifique se o CSV está no formato correto.')
      }
    } finally {
      setIsLoading(false)
    }
  }

  function handleReset() {
    setResult(null)
    setError(null)
  }

  return (
    <div className="page">
      <header className="header">
        <div className="header-inner">
          <div className="logo">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
              <rect x="2" y="3" width="20" height="14" rx="2" />
              <path d="M8 21h8M12 17v4" />
              <path d="M7 8h.01M7 12h.01M11 8h6M11 12h6" />
            </svg>
            <span>Inventory Analyzer</span>
          </div>
          {result && (
            <button className="btn-reset" onClick={handleReset}>
              Nova análise
            </button>
          )}
        </div>
      </header>

      <main className="main">
        {!result && !isLoading && (
          <div className="hero">
            <h1 className="hero-title">
              Analise seu <span className="accent">estoque</span>
            </h1>
            <p className="hero-subtitle">
              Faça upload de um arquivo CSV com as movimentações e veja o saldo atual de cada produto e possíveis anomalias.
            </p>
          </div>
        )}

        <UploadZone onFileSelect={handleFileSelect} isLoading={isLoading} />

        {error && (
          <div className="error-box">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <circle cx="12" cy="12" r="10" />
              <line x1="12" y1="8" x2="12" y2="12" />
              <line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
            {error}
          </div>
        )}

        {result && (
          <div className="results">
            <div className="results-summary">
              <div className="summary-item">
                <span className="summary-value">{result.stock.length}</span>
                <span className="summary-label">Produtos</span>
              </div>
              <div className="summary-divider" />
              <div className="summary-item">
                <span className={`summary-value ${result.anomalies.length > 0 ? 'danger' : 'success'}`}>
                  {result.anomalies.length}
                </span>
                <span className="summary-label">Anomalias</span>
              </div>
              <div className="summary-divider" />
              <div className="summary-item">
                <span className="summary-value">
                  {result.stock.filter(s => s.quantity > 0).length}
                </span>
                <span className="summary-label">Em estoque</span>
              </div>
            </div>

            <div className="results-grid">
              <StockTable items={result.stock} />
              <AnomaliesList anomalies={result.anomalies} />
            </div>
          </div>
        )}
      </main>
    </div>
  )
}
