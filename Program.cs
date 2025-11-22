// Install-Package Microsoft.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.Sqlite
// Install-Package Microsoft.EntityFrameworkCore.Tools
// Install-Package Microsoft.SemanticKernel.Connectors.Google -Version 1.67.1-alpha
// Install-Package Microsoft.SemanticKernel.Process.Core -Version 1.67.1-alpha
// Install-Package Microsoft.SemanticKernel.Process.LocalRuntime -Version 1.67.1-alpha
// Install-Package Microsoft.SemanticKernel.Connectors.SqliteVec -prerelease

// https://aistudio.google.com/

using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;

var builder = WebApplication.CreateBuilder(args);

//-- Razor Pages
builder.Services.AddRazorPages();

// GENERATE KEY ->> https://hoven.in/cs-lang/gemini-keys-for-sk.html
const string GEMINIAPIKEY = __your_key__;

//--- Register AI Services
// get key here: https://hoven.in/cs-lang/gemini-keys-for-sk.html
builder.Services.AddGoogleAIGeminiChatCompletion(
modelId: "gemini-2.5-flash",
apiKey: GEMINIAPIKEY
);

builder.Services.AddTransient((sp) => new Kernel(sp));


builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>((sp) =>
  new GoogleAIEmbeddingGenerator(
    modelId: "gemini-embedding-001",
    apiKey: GEMINIAPIKEY,
    dimensions: 1536
));

var app = builder.Build();

app.MapRazorPages();

app.Run();
