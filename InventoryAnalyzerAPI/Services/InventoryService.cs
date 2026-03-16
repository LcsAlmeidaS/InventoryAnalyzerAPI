public class InventoryService
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
        var anomalies = new HashSet<string>();

        foreach (var record in records.OrderBy(r => r.Timestamp))
        {
            names[record.ProductId!] = record.ProductName ?? "";

            stock.TryAdd(record.ProductId!, 0);

            if (!MovementFactor.TryGetValue(record.Type, out var factor))
                continue;

            stock[record.ProductId!] += factor * record.Quantity;

            if (stock[record.ProductId!] < 0)
                anomalies.Add(record.ProductId!);
        }

        return BuildResult(stock, names, anomalies);
    }


    private InventoryAnalysisResultDto BuildResult(
    Dictionary<string, int> stock,
    Dictionary<string, string> names,
    HashSet<string> anomalies)
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

        foreach (var id in anomalies)
        {
            result.Anomalies.Add(new AnomalyDto
            {
                ProductId = id,
                ProductName = names[id],
                Message = "Stock went negative"
            });
        }

        return result;
    }
}