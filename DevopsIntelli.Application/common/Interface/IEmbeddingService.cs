using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.common.Interface;

public interface IEmbeddingService
{
    Task<float[]> GenerateSingleEmbeddingAsync(string text, CancellationToken ct = default);
    Task<List<float[]>> GenerateBatchEmbeddingAsync(string text, CancellationToken ct = default);

}
