public class CsvParseService
{
    public async Task<List<InventoryRecordDto>> ParseAsync(IFormFile file)
    {
        var records = new List<InventoryRecordDto>();

        using var reader = new StreamReader(file.OpenReadStream());

        bool isHeader = true;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (isHeader)
            {
                isHeader = false;
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var columns = line.Split(',');

            if (columns.Length != 5)
                continue;

            if (!long.TryParse(columns[0], out var timestamp))
                continue;

            var productId = columns[1];
            var productName = columns[2];

            if (!Enum.TryParse<MovementType>(columns[3], true, out var type))
                continue;

            if (!int.TryParse(columns[4], out var quantity))
                continue;

            records.Add(new InventoryRecordDto
            {
                Timestamp = timestamp,
                ProductId = productId,
                ProductName = productName,
                Type = type,
                Quantity = quantity
            });
        }

        return records;
    }
}