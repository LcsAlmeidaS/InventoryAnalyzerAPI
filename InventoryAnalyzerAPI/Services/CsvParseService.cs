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
        using var csv = CreateCsvReader(file);
        return await ReadRecordsAsync(csv);
    }

    private CsvReader CreateCsvReader(IFormFile file)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
        };

        var stream = file.OpenReadStream();
        var reader = new StreamReader(stream);
        return new CsvReader(reader, config);
    }

    private async Task<List<InventoryRecordDto>> ReadRecordsAsync(CsvReader csv)
    {
        var records = new List<InventoryRecordDto>();

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            var record = ParseLine(csv);
            if (record is not null)
                records.Add(record);
        }

        return records;
    }

    private InventoryRecordDto? ParseLine(CsvReader csv)
    {
        if (!long.TryParse(csv.GetField(0), out var timestamp))
            return null;

        var productId = csv.GetField(1);
        var productName = csv.GetField(2);

        if (!Enum.TryParse<MovementType>(csv.GetField(3), true, out var type))
            return null;

        if (!int.TryParse(csv.GetField(4), out var quantity))
            return null;

        return new InventoryRecordDto
        {
            Timestamp = timestamp,
            ProductId = productId,
            ProductName = productName,
            Type = type,
            Quantity = quantity
        };
    }
}
