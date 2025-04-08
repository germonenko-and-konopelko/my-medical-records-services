using MMR.Common.Data.Entities;

namespace MMR.Common.Data;

public class MmrDatabaseContext(DbContextOptions<MmrDatabaseContext> options) : DbContext(options)
{
    public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("core");
    }
}