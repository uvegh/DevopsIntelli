using DevopsIntelli.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Infrastructure.Data;

public class DevopsIntelliDBContext : DbContext
{

    public DevopsIntelliDBContext(DbContextOptions<DevopsIntelliDBContext> options) : base(options){ }
   public DbSet<Incident> Incident { get; set; }

   
}
