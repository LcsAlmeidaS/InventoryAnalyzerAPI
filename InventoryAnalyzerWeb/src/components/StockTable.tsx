import type { StockItem } from '../types'

interface StockTableProps {
  items: StockItem[]
}

export function StockTable({ items }: StockTableProps) {
  const sorted = [...items].sort((a, b) => b.quantity - a.quantity)

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
