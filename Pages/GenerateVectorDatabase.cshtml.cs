using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.AI;
using MyProject.Models;

namespace MyProject.Pages
{
    public class GenerateVectorDatabaseModel(IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator) : PageModel
    {
        [TempData]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetGenerateVectorDatabaseAsync()
        {
            await QuotesByGreatAuthorsVector.GenerateSqliteVectorCollectionAsync(_embeddingGenerator);

            Message = "Generated/upserted successfully!";

            return RedirectToPage();
        }
    }
}
