using DevopsIntelli.Domain.Common.Entities;
using DevopsIntelli.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Infrastructure.Repository;


public class IncidentRepository : IIncidentRepository
{
    private readonly DevopsIntelliDBContext _dbContext;
    private readonly DbSet<Incident> _incidentContext;
    public IncidentRepository( DevopsIntelliDBContext dbContext )
    {
        _dbContext = dbContext;
     _incidentContext=   _dbContext.Set<Incident>();

    }
    public  async Task AddAsync(Incident incident, CancellationToken ct)
    {
        await _incidentContext.AddAsync(incident);
        await _dbContext.SaveChangesAsync();

    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
     var incident=    await _incidentContext.FirstOrDefaultAsync(i => i.Id == id,ct);
        if (incident != null)
        {
            _incidentContext.Remove(incident);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await _incidentContext.FindAsync(id) != null;
    }

    public async Task<List<Incident>> GetAllAsync(CancellationToken ct)
    {
        var incidents = _incidentContext.AsNoTracking().ToList();
        return incidents;
    }

    public async Task<Incident?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var incident =  await _incidentContext.FindAsync(new object?[] { id, ct }, cancellationToken: ct);
        return incident ?? null;
    }

    public async Task<List<Incident>> GetByTenantAsync(string tenantId, CancellationToken ct)
    {
        var incidents =  _incidentContext.Where(i => i.TenantId == tenantId).ToList();
        return incidents;
      
    }

    public async Task UpdateAsync(Incident incident, CancellationToken ct)
    {
        //attach updated incident
        _incidentContext.Attach(incident);
        // Mark all properties as potentially modified for simplicity here
        _incidentContext.Entry(incident).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(ct);


      
    }
}
