export interface StockItem {
  productId: string
  productName: string
  quantity: number
}

export interface Anomaly {
  productId: string
  productName: string
  message: string
}

export interface AnalysisResult {
  stock: StockItem[]
  anomalies: Anomaly[]
}
