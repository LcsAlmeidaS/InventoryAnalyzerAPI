using InventoryAnalyzerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAnalyzerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly ICsvParseService _parser;
    private readonly IInventoryService _inventoryService;

    private static readonly string[] AllowedContentTypes =
        ["text/csv", "application/csv", "text/plain"];

    public InventoryController(ICsvParseService parser, IInventoryService inventoryService)
    {
        _parser = parser;
        _inventoryService = inventoryService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid CSV file.");

        if (!IsCsvFile(file))
            return BadRequest("Only CSV files are allowed.");

        var records = await _parser.ParseAsync(file);
        var result = _inventoryService.Analyze(records);

        return Ok(result);
    }

    private static bool IsCsvFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        return extension.Equals(".csv", StringComparison.OrdinalIgnoreCase)
               && AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase);
    }
}