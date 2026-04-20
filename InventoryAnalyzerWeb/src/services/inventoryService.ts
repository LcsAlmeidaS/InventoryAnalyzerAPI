import axios from 'axios'
import type { AnalysisResult } from '../types'

const api = axios.create({
  baseURL: '/api',
})

export async function analyzeInventory(file: File): Promise<AnalysisResult> {
  const formData = new FormData()
  formData.append('file', file)

  const response = await api.post<AnalysisResult>('/inventory/analyze', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  })

  return response.data
}
