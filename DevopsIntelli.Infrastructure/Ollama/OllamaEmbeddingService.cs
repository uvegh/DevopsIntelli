


using Microsoft.Extensions.Options;

namespace DevopsIntelli.Infrastructure.Ollama;

public  class OllamaOptions{
    public string BaseUrl = "http://localhost:11434";
    public string model = "nomic-embed-text";
    public int timeoutSecs = 30;
};
public class OllamaEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly  OllamaOptions _options;

    public OllamaEmbeddingService(HttpClient httpClient, ILogger<OllamaEmbeddingService> logger, IOptions<OllamaOptions> options)
    {
        _options = options.Value;//extract value thanks to ioptions
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.timeoutSecs);
            
        _logger = logger;

    }

    public async Task<float[]> GenerateSingleEmbeddingAsync(string text, CancellationToken ct = default)
    {
        try {
            var request = new
            {
                model = "nomic-embed-test",
                prompt = text
            };
            var response = await _httpClient.PostAsJsonAsync("/api/embeddings", request, ct);
            response.EnsureSuccessStatusCode();
            //must be successful be for converting
            OllamaResponse? ollamaResponse = await response.Content.ReadFromJsonAsync<OllamaResponse>(ct);
            OllamaResponse? result = ollamaResponse;
            _logger.LogDebug("return embeddings of length: {text.Length} ", text);
            _logger.LogDebug("Generated dimensions :{dimensions}", result!.Embedding.Length);
            return result!.Embedding;
        }
        catch(Exception ex)
        {
            _logger.LogError("failed to embed: {ex}", ex);
            _logger.LogError("failed to embed for {text.Length}", text);
            throw;

        }
        
    }

    public async Task<List<float[]>> GenerateBatchEmbeddingAsync(List< string> text, CancellationToken ct = default)
    {
         var  embeddings = new List<float[]>( );
        var throttler = new SemaphoreSlim(3);
        var tasks = text.Select(async (txt, index) =>
        {
            //wait for till theres a freed up space
            await throttler.WaitAsync(ct);
            try
            {
                
                embeddings.Add(await GenerateSingleEmbeddingAsync(txt,ct));
            }
            catch (  Exception ex)
           
            {
                _logger.LogError("error occured when generating embeding  {ex}", ex);
                throw;

            }
            finally
            {
                throttler.Release();
                _logger.LogDebug("throttler released at  {index}", index);

            }

        });

       await Task.WhenAll(tasks);
        return embeddings;

        //foreach ( var val in text)
        //{
        //  embeddings.Add(  await  GenerateSingleEmbeddingAsync(val,ct));
        //}
        //return embeddings;
    }

    
}
    
