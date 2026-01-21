using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.common.Interface;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default);
}
