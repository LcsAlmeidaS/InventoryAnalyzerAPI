using InventoryAnalyzerAPI.DTOs;
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
    [ProducesResponseType(typeof(InventoryAnalysisResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Analyze(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Problem(
                title: "Invalid file.",
                detail: "No file was provided or the file is empty.",
                statusCode: StatusCodes.Status400BadRequest);

        if (!IsCsvFile(file))
            return Problem(
                title: "Invalid file type.",
                detail: "Only CSV files are allowed. Accepted content types: text/csv, application/csv, text/plain.",
                statusCode: StatusCodes.Status400BadRequest);

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