using Microsoft.EntityFrameworkCore;
using MMR.Common.Data;
using MMR.Common.Data.Entities;
using MMR.Common.Optional;
using MMR.Common.Results;

namespace MMR.Patient.Common;

internal interface IPatientProfileRepository
{
    internal Task<Result<Option<Profile>, Exception>> GetProfileAsync(string userId);

    internal Task<Result<bool, Exception>> PatientHasProfileAsync(string userId);

    internal Task<Result<Exception>> SaveAsync(Profile profile);
}

public class PatientProfileRepository(MmrDatabaseContext db) : IPatientProfileRepository
{
    public Task<Result<Option<Profile>, Exception>> GetProfileAsync(string userId)
    {
        return Result.TryAsync(async () =>
        {
            Profile? profile = await db.PatientProfiles
                .Select(p => new Profile
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    Sex = p.Sex,
                })
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return profile is null ? Option<Profile>.None() : Option<Profile>.Some(profile);
        });
    }

    public Task<Result<bool, Exception>> PatientHasProfileAsync(string userId)
    {
        return Result.TryAsync(() => db.PatientProfiles.AnyAsync(pp => pp.UserId == userId));
    }

    public Task<Result<Exception>> SaveAsync(Profile profile)
    {
        return Result.TryAsync(async () =>
        {
            PatientProfile? profileEntity = await db.PatientProfiles.FindAsync(profile.Id) ?? new()
            {
                UserId = profile.UserId
            };

            profileEntity.FirstName = profile.FirstName;
            profileEntity.LastName = profile.LastName;
            profileEntity.BirthDate = profile.BirthDate;
            profileEntity.Sex = profile.Sex;

            db.PatientProfiles.Update(profileEntity);
            await db.SaveChangesAsync();
        });
    }
}