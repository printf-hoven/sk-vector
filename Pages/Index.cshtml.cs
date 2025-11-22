// NuGet\Install-Package Microsoft.SemanticKernel.Connectors.Google -Version 1.67.1-alpha
// NuGet\Install-Package Microsoft.SemanticKernel.Process.Core -Version 1.67.1-alpha
// NuGet\Install-Package Microsoft.SemanticKernel.Process.LocalRuntime -Version 1.67.1-alpha
// dotnet add package Microsoft.SemanticKernel.Connectors.SqliteVec --prerelease

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.AI;
using MyProject.Models;
using MyProject.Pages.Shared;

namespace MyProject.Pages;

public class IndexModel(IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator) : PageModel
{
    [BindProperty]
    public required InputModel Input { get; set; } = new InputModel();

    public List<string>? Quotes;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Quotes = [];

        // Get and create collection if it doesn't exist.
        var collection = QuotesByGreatAuthorsVector.GetCollection();

        var searchString = Input.Name;

        var searchVector = (await _embeddingGenerator.GenerateAsync(searchString)).Vector;

        var resultRecords = collection.SearchAsync(searchVector, top: 3);

        await foreach (var rec in resultRecords)
        {
            Quotes.Add($"{rec.Record.Quote} [Score: {rec.Score}]");
        }

        return Page();
    }   
}
