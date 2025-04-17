using MMR.Common.Data.Entities;

namespace MMR.Common.Data;

public class MmrDatabaseContext(DbContextOptions<MmrDatabaseContext> options, TimeProvider timeProvider)
    : DbContext(options)
{
    public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("core");
    }

    public override int SaveChanges()
    {
        SetCreatedAndUpdateDate();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetCreatedAndUpdateDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetCreatedAndUpdateDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedAndUpdateDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedAndUpdateDate()
    {
        var affectedEntities = ChangeTracker
            .Entries<MmrEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entity in affectedEntities)
        {
            switch (entity.State)
            {
                case EntityState.Added:
                    var now = timeProvider.GetUtcNow();
                    entity.Entity.CreatedAt = now;
                    entity.Entity.UpdatedAt = now;
                    break;
                case EntityState.Modified:
                    entity.Entity.UpdatedAt = timeProvider.GetUtcNow();
                    break;
            }
        }
    }
}