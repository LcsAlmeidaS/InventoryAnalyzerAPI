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
            names[record.ProductId!] = record.ProductName ?? "";
            stock.TryAdd(record.ProductId!, 0);

            if (!MovementFactor.TryGetValue(record.Type, out var factor))
                continue;

            stock[record.ProductId!] += factor * record.Quantity;

            if (stock[record.ProductId!] < 0)
            {
                var current = stock[record.ProductId!];
                if (!anomalies.TryGetValue(record.ProductId!, out var min) || current < min)
                    anomalies[record.ProductId!] = current;
            }
        }

        return BuildResult(stock, names, anomalies);
    }

    private static InventoryAnalysisResultDto BuildResult(
        Dictionary<string, int> stock,
        Dictionary<string, string> names,
        Dictionary<string, int> anomalies)
    {
        var result = new InventoryAnalysisResultDto();

        foreach (var item in stock)
        {
            result.Stock.Add(new StockItemDto
            {
                ProductId = item.Key,
                ProductName = names[item.Key],
                Quantity = item.Value
            });
        }

        foreach (var (id, minStock) in anomalies)
        {
            result.Anomalies.Add(new AnomalyDto
            {
                ProductId = id,
                ProductName = names[id],
                Message = $"Stock went negative (reached {minStock})"
            });
        }

        return result;
    }
}