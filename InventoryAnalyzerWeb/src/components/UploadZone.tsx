import { useRef, useState, DragEvent, ChangeEvent } from 'react'

interface UploadZoneProps {
  onFileSelect: (file: File) => void
  isLoading: boolean
}

export function UploadZone({ onFileSelect, isLoading }: UploadZoneProps) {
  const inputRef = useRef<HTMLInputElement>(null)
  const [isDragging, setIsDragging] = useState(false)

  function handleDrop(e: DragEvent<HTMLDivElement>) {
    e.preventDefault()
    setIsDragging(false)
    const file = e.dataTransfer.files[0]
    if (file) onFileSelect(file)
  }

  function handleChange(e: ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0]
    if (file) {
      onFileSelect(file)
      e.target.value = ''
    }
  }

  return (
    <div
      className={`upload-zone ${isDragging ? 'dragging' : ''} ${isLoading ? 'loading' : ''}`}
      onClick={() => !isLoading && inputRef.current?.click()}
      onDragOver={(e) => { e.preventDefault(); setIsDragging(true) }}
      onDragLeave={() => setIsDragging(false)}
      onDrop={handleDrop}
    >
      <input
        ref={inputRef}
        type="file"
        accept=".csv"
        style={{ display: 'none' }}
        onChange={handleChange}
      />
      <div className="upload-icon">
        {isLoading ? (
          <div className="spinner" />
        ) : (
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
            <path d="M21 15v4a2 2 0 01-2 2H5a2 2 0 01-2-2v-4" />
            <polyline points="17 8 12 3 7 8" />
            <line x1="12" y1="3" x2="12" y2="15" />
          </svg>
        )}
      </div>
      <p className="upload-title">
        {isLoading ? 'Analisando...' : 'Solte o CSV aqui'}
      </p>
      <p className="upload-subtitle">
        {isLoading ? 'Aguarde um momento' : 'ou clique para selecionar o arquivo'}
      </p>
    </div>
  )
}
