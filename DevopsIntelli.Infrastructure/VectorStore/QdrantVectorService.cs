


using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace DevopsIntelli.Infrastructure.VectorStore;
/// <summary>
/// use qdrant client to store generated embedded vectors and also for searching , faster search with vectors compared 
/// to trad dbs and pulls results based on similarities
/// </summary>
/// 
public class QdrantOptions
{
    public string Host = "localhost";
    public int Port = 6334;
    public string CollectionName = "incident_logs";
    public int TVectorSize = 1536; //text embedding in 3 small dimensions


}
public class QdrantVectorService : IVectorService
{
    private readonly ILogger _logger;
    private readonly QdrantClient _client;
    private readonly QdrantOptions _qdrantOptions;

    public QdrantVectorService(ILogger<QdrantOptions> logger, IOptions<QdrantOptions> qdrantOptions, QdrantClient client)
    {
        _logger = logger;
        _qdrantOptions = qdrantOptions.Value;
        _client =  new QdrantClient(_qdrantOptions.Host,_qdrantOptions.Port);
        

    }

    private async Task InitializeCollectionAsync( string collection)
    {

        try {
            //create collection if it doesnt exist
            var collections = await  _client.ListCollectionsAsync();
            var exists = collections.Any(x => x == _qdrantOptions.CollectionName);
            _logger.LogDebug("collection  found :{exists}", exists);
            if (!exists)
            {
                var vectorParams = new VectorParams
                {
                    Size = (ulong)_qdrantOptions.TVectorSize,
                    Distance = Distance.Cosine
                };
                _client.CreateCollectionAsync(collectionName: _qdrantOptions.CollectionName, vectorsConfig:
               );
                _logger.LogInformation("Collection created successfully");
            }
            
        }
        catch( Exception ex)
        {

            _logger.LogDebug("failed to create collection {ex}", ex);
        }
    }

    public Task<List<VectorSearchResult>> SearchVectorAsync(float[] queryEmbedding, CancellationToken ct = default)
    {
       
        throw ();

    }

    public Task StoreVectorAsync(Guid id, float[] embedding, Dictionary<string, object> metadata, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
