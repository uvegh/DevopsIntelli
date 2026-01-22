using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.common.Interface;
/// <summary>
/// Service for vector database operations
/// WHY: Enables semantic search across millions of log entries
/// INTERVIEW: "I use Qdrant for fast semantic similarity search with <100ms response times"
/// </summary>
public interface IVectorService
{

    Task StoreVectorAsync(
        Guid id,
        float[] embedding,
        Dictionary<string, object> metadata,
        CancellationToken ct = default
        );
    Task<List<VectorSearchResult>> SearchVectorAsync(float[] queryEmbedding, CancellationToken ct = default);
   

}
public class VectorSearchResult
{
    public Guid Id;
    public double SimilaryScore { get; init; }
    public Dictionary<string, object> Metadata { get; init; } = new();


            
            } 
