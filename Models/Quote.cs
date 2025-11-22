using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyProject.Models;

public class QuotesByGreatAuthors
{
    [JsonPropertyName("quote")]
    public required string Quote { get; set; }

    [JsonPropertyName("author")]
    public required string Author { get; set; }

    [JsonPropertyName("tags")]
    public required string[] Tags { get; set; }
}

public class QuotesByGreatAuthorsVector
{
    [VectorStoreKey]
    public long Id { get; set; }

    [VectorStoreData]
    public required string Quote { get; set; }

    [VectorStoreData]
    public required string Author { get; set; }

    [VectorStoreVector(1536)]
    public ReadOnlyMemory<float> DefinitionEmbedding { get; set; }

    public static VectorStoreCollection<string, QuotesByGreatAuthorsVector> GetCollection()
    {
        string path = Path.Combine("Database", "Vectors");

        Directory.CreateDirectory(path);

        path = Path.Combine(path, "db_vectors.sqlite");

        string connectionString = $"Data Source=\"{path}\"";

        return new SqliteCollection<string, QuotesByGreatAuthorsVector>(connectionString, "skquotes");
    }
    public static async Task GenerateSqliteVectorCollectionAsync(IEmbeddingGenerator<string, Embedding<float>> _generator)
    {
        VectorStoreCollection<string, QuotesByGreatAuthorsVector> collection = GetCollection();

        await collection.EnsureCollectionExistsAsync();

        QuotesByGreatAuthors[]? ls = JsonSerializer.Deserialize<QuotesByGreatAuthors[]>(File.ReadAllText("quotes.json"));

        Debug.Assert(null != ls, "JSON file could not be de-serialized.");

        List<QuotesByGreatAuthorsVector> list = [];

        int id = 1;

        foreach (var quote in ls)
        {
            QuotesByGreatAuthorsVector vec = new() { Author = quote.Author, Quote = quote.Quote, Id = id++ };

            vec.DefinitionEmbedding = (await _generator.GenerateAsync(vec.Quote)).Vector;

            list.Add(vec);
        }

        await collection.UpsertAsync(list);
    }
}



