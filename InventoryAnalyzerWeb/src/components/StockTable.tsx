import { useMemo } from 'react'
import type { StockItem } from '../types'

interface StockTableProps {
  items: StockItem[]
}

export function StockTable({ items }: StockTableProps) {
  const sorted = useMemo(
    () => [...items].sort((a, b) => b.quantity - a.quantity),
    [items]
  )

  if (items.length === 0) {
    return (
      <div className="card">
        <div className="card-header">
          <h2 className="card-title">Estoque Atual</h2>
          <span className="badge">0 produtos</span>
        </div>
        <div className="empty-state">
          <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
            <path d="M5 8h14M5 8a2 2 0 110-4h14a2 2 0 110 4M5 8v10a2 2 0 002 2h10a2 2 0 002-2V8" />
          </svg>
          <p>Nenhum produto encontrado no arquivo</p>
        </div>
      </div>
    )
  }

  return (
    <div className="card">
      <div className="card-header">
        <h2 className="card-title">Estoque Atual</h2>
        <span className="badge">{items.length} produtos</span>
      </div>
      <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Produto</th>
            <th>Quantidade</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {sorted.map((item) => (
            <tr key={item.productId}>
              <td className="mono">{item.productId}</td>
              <td>{item.productName}</td>
              <td className={`mono quantity ${item.quantity < 0 ? 'negative' : ''}`}>
                {item.quantity}
              </td>
              <td>
                <span className={`tag ${item.quantity < 0 ? 'tag-danger' : 'tag-ok'}`}>
                  {item.quantity < 0 ? 'Negativo' : 'OK'}
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
