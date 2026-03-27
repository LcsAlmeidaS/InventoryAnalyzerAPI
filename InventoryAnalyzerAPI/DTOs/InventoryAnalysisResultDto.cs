namespace InventoryAnalyzerAPI.DTOs;

public class InventoryAnalysisResultDto
{
    public List<StockItemDto> Stock { get; set; } = new();
    public List<AnomalyDto> Anomalies { get; set; } = new();
}