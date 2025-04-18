using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMR.Common.Enums;

namespace MMR.Common.Data.Entities;

public class PatientProfile : MmrEntity
{
    [Required, StringLength(50, MinimumLength = 1)]
    public string UserId { get; set; } = string.Empty;

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Sex? Sex { get; set; }
}

public class PatientProfileConfiguration : IEntityTypeConfiguration<PatientProfile>
{
    public void Configure(EntityTypeBuilder<PatientProfile> builder)
    {
        builder.ToTable("patient_profiles");
        builder.HasKey(x => x.Id);
        builder.HasIndex(pp => pp.UserId).IsUnique();
    }
}
