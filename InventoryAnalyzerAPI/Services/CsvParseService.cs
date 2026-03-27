using CsvHelper;
using CsvHelper.Configuration;
using InventoryAnalyzerAPI.DTOs;
using InventoryAnalyzerAPI.Enums;
using InventoryAnalyzerAPI.Services.Interfaces;
using System.Globalization;

namespace InventoryAnalyzerAPI.Services;

public class CsvParseService : ICsvParseService
{
    public async Task<List<InventoryRecordDto>> ParseAsync(IFormFile file)
    {
        var records = new List<InventoryRecordDto>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
        };

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, config);

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            if (!long.TryParse(csv.GetField(0), out var timestamp))
                continue;

            var productId = csv.GetField(1);
            var productName = csv.GetField(2);

            if (!Enum.TryParse<MovementType>(csv.GetField(3), true, out var type))
                continue;

            if (!int.TryParse(csv.GetField(4), out var quantity))
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