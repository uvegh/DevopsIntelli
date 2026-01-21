using DevopsIntelli.Application.common.Interface;
using DevopsIntelli.Application.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace DevopsIntelli.Infrastructure.Ollama;

public class OllamaEmbeddingService() : IEmbeddingService
{
    private readonly HttpClient _httpClient;

    public OllamaEmbeddingService(HttpClient httpClient)
    {

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:11434");

    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var request = new
        {
            model = "nomic-embed-test",
            prompt = text
        };
        var response = await _httpClient.PostAsJsonAsync("/api/embeddings", request, ct);
        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        return result!.Embedding;
    }
}
    
