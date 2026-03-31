using InventoryAnalyzerAPI.DTOs;
using InventoryAnalyzerAPI.Enums;
using InventoryAnalyzerAPI.Services.Interfaces;

namespace InventoryAnalyzerAPI.Services;

public class InventoryService : IInventoryService
{
    private static readonly Dictionary<MovementType, int> MovementFactor = new()
    {
        { MovementType.In, 1 },
        { MovementType.Out, -1 }
    };

    public InventoryAnalysisResultDto Analyze(IEnumerable<InventoryRecordDto> records)
    {
        var stock = new Dictionary<string, int>();
        var names = new Dictionary<string, string>();
        var anomalies = new Dictionary<string, int>();

        foreach (var record in records.OrderBy(r => r.Timestamp))
        {
            RegisterProduct(record, stock, names);
            ApplyMovement(record, stock, anomalies);
        }

        return BuildResult(stock, names, anomalies);
    }

    private static void RegisterProduct(
        InventoryRecordDto record,
        Dictionary<string, int> stock,
        Dictionary<string, string> names)
    {
        names[record.ProductId!] = record.ProductName ?? "";
        stock.TryAdd(record.ProductId!, 0);
    }

    private static void ApplyMovement(
        InventoryRecordDto record,
        Dictionary<string, int> stock,
        Dictionary<string, int> anomalies)
    {
        if (!MovementFactor.TryGetValue(record.Type, out var factor))
            return;

        stock[record.ProductId!] += factor * record.Quantity;

        CheckAnomaly(record.ProductId!, stock, anomalies);
    }

    private static void CheckAnomaly(
        string productId,
        Dictionary<string, int> stock,
        Dictionary<string, int> anomalies)
    {
        if (stock[productId] >= 0)
            return;

        var current = stock[productId];

        if (!anomalies.TryGetValue(productId, out var min) || current < min)
            anomalies[productId] = current;
    }

    private static InventoryAnalysisResultDto BuildResult(
        Dictionary<string, int> stock,
        Dictionary<string, string> names,
        Dictionary<string, int> anomalies)
    {
        var result = new InventoryAnalysisResultDto();

        foreach (var item in stock)
            result.Stock.Add(MapToStockItem(item.Key, item.Value, names));

        foreach (var (id, minStock) in anomalies)
            result.Anomalies.Add(MapToAnomaly(id, minStock, names));

        return result;
    }

    private static StockItemDto MapToStockItem(
        string productId,
        int quantity,
        Dictionary<string, string> names)
    {
        return new StockItemDto
        {
            ProductId = productId,
            ProductName = names[productId],
            Quantity = quantity
        };
    }

    private static AnomalyDto MapToAnomaly(
        string productId,
        int minStock,
        Dictionary<string, string> names)
    {
        return new AnomalyDto
        {
            ProductId = productId,
            ProductName = names[productId],
            Message = $"Stock went negative (reached {minStock})"
        };
    }
}