using InventoryAnalyzerAPI.DTOs;

namespace InventoryAnalyzerAPI.Services.Interfaces;

public interface IInventoryService
{
    InventoryAnalysisResultDto Analyze(IEnumerable<InventoryRecordDto> records);
}