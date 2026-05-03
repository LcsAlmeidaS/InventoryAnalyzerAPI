import axios, { isAxiosError } from 'axios'
import type { AnalysisResult } from '../types'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? '/api',
})

export async function analyzeInventory(file: File): Promise<AnalysisResult> {
  const formData = new FormData()
  formData.append('file', file)

  try {
    const response = await api.post<AnalysisResult>('/inventory/analyze', formData)
    return response.data
  } catch (err) {
    if (isAxiosError(err) && err.response?.data?.detail) {
      throw new Error(err.response.data.detail as string)
    }
    throw err
  }
}
