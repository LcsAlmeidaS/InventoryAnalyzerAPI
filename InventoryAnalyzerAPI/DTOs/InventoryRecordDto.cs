using InventoryAnalyzerAPI.Enums;

namespace InventoryAnalyzerAPI.DTOs;

public class InventoryRecordDto
{
    public long Timestamp { get; set; }
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public MovementType Type { get; set; }
    public int Quantity { get; set; }
}