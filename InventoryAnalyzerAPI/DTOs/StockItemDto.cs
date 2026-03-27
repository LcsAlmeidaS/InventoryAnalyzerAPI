namespace InventoryAnalyzerAPI.DTOs;

public class StockItemDto
{
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
}