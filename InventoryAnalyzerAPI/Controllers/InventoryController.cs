using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly CsvParseService _parser;
    private readonly InventoryService _inventoryService;

    public InventoryController(CsvParseService parser, InventoryService inventoryService)
    {
        _parser = parser;
        _inventoryService = inventoryService;
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(IFormFile file)
    {
        if (!IsCsvFile(file))
            return BadRequest("Only CSV files are allowed.");

        else if (file == null || file.Length == 0)
            return BadRequest("Invalid CSV file.");

        var records = await _parser.ParseAsync(file);

        var result = _inventoryService.Analyze(records);

        return Ok(result);
    }

    private bool IsCsvFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);

        return extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);
    }
}