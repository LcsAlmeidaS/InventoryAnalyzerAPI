using InventoryAnalyzerAPI.DTOs;

namespace InventoryAnalyzerAPI.Services.Interfaces;

public interface IInventoryService
{
    InventoryAnalysisResultDto Analyze(IReadOnlyList<InventoryRecordDto> records);
}