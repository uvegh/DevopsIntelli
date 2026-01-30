using DevopsIntelli.Domain.Common.Entities;

public interface IIncidentRepository
{
    Task<Incident?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Incident>> GetAllAsync(CancellationToken ct);
    Task<List<Incident>> GetByTenantAsync(string tenantId, CancellationToken ct);
    Task AddAsync(Incident incident, CancellationToken ct);
    Task UpdateAsync(Incident incident, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}