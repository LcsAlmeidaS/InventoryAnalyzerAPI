using InventoryAnalyzerAPI.DTOs;

namespace InventoryAnalyzerAPI.Services.Interfaces;

public interface ICsvParseService
{
    Task<List<InventoryRecordDto>> ParseAsync(IFormFile file);
}