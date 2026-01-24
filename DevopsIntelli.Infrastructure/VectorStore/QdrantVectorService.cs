


using DevopsIntelli.Application.common.Exceptions;
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

    public async Task StoreVectorAsync(
        Guid id,
        
        float[] queryEmbedding,
        Dictionary<string, object> metadata,
        CancellationToken ct = default)
    {
        try
        {
            var point = new PointStruct
            {
                Id = new PointId { Uuid = id.ToString() },
                Vectors = queryEmbedding,
                Payload =
                {
                    {
                        "incident_id",id.ToString()
                    },
                    {
                        "timestamp",DateTime.UtcNow.ToString("0")
                    }
                }


            };

            foreach (var (key,value) in metadata)
            {
                point.Payload[key] = value?.ToString() ?? string.Empty;
                
            }

            await _client.UpsertAsync(collectionName:_qdrantOptions.CollectionName,points: new[] { point }, cancellationToken:ct )
             _logger.LogDebug("Stored vector for ID: {Id}", id);
        }

        catch(Exception ex)
        {
            _logger.LogError("failed to create new pointstruct {ex}", ex); _logger.LogError("failed to create new pointstruct {ex}", ex);

        }

    }

    

    public async Task<List<VectorSearchResult>> SearchVectorAsync(double minScore=0.7,float[] queryEmbedding, CancellationToken ct = default)
    {
        try
        {
            var searchRes = await _client.SearchAsync(collectionName: _qdrantOptions.CollectionName, vector: queryEmbedding,
                limit: (ulong)_qdrantOptions.TVectorSize, scoreThreshold: (float)minScore,
                cancellationToken: ct);

            //map search result to vectorsearchresult
            _logger.LogDebug("Found {Count} similar vectors (min score: {MinScore})", searchRes.Count, minScore);
            var res=  searchRes.Select(r => new VectorSearchResult
            {
                Id = Guid.Parse(r.Id.Uuid),
                SimilaryScore = r.Score,
                //convert to dictionary
                Metadata = r.Payload.ToDictionary(kvp => kvp.Key,

             kvp => (object)kvp.Value.StringValue)//convert to object

            }).ToList();

            return res;
           
        }
        catch(NotFoundException ex)
        {
            throw new NotFoundException($"No similarties for vector found,{ex}", nameof(SearchVectorAsync))
;        }
    }

  
}
