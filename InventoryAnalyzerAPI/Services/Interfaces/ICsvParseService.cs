using InventoryAnalyzerAPI.DTOs;

namespace InventoryAnalyzerAPI.Services.Interfaces;

public interface ICsvParseService
{
    Task<IReadOnlyList<InventoryRecordDto>> ParseAsync(IFormFile file);
}