import type { Anomaly } from '../types'

interface AnomaliesListProps {
  anomalies: Anomaly[]
}

export function AnomaliesList({ anomalies }: AnomaliesListProps) {
  if (anomalies.length === 0) {
    return (
      <div className="card">
        <div className="card-header">
          <h2 className="card-title">Anomalias</h2>
          <span className="badge badge-success">Nenhuma</span>
        </div>
        <div className="empty-state">
          <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
            <path d="M22 11.08V12a10 10 0 11-5.93-9.14" />
            <polyline points="22 4 12 14.01 9 11.01" />
          </svg>
          <p>Nenhuma anomalia detectada</p>
        </div>
      </div>
    )
  }

  return (
    <div className="card card-danger">
      <div className="card-header">
        <h2 className="card-title">Anomalias</h2>
        <span className="badge badge-danger">{anomalies.length} encontrada{anomalies.length > 1 ? 's' : ''}</span>
      </div>
      <div className="anomalies-list">
        {anomalies.map((anomaly) => (
          <div key={anomaly.productId} className="anomaly-item">
            <div className="anomaly-icon">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z" />
                <line x1="12" y1="9" x2="12" y2="13" />
                <line x1="12" y1="17" x2="12.01" y2="17" />
              </svg>
            </div>
            <div className="anomaly-content">
              <span className="anomaly-name">{anomaly.productName}</span>
              <span className="anomaly-id mono">{anomaly.productId}</span>
              <span className="anomaly-message">{anomaly.message}</span>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
