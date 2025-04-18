using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MMR.Patient.Common;
using MMR.Patient.Create;
using MMR.Patient.Update;

namespace MMR.Patient;

public static class Services
{
    public static void AddPatientModule(this IServiceCollection services)
    {
        services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
        services.AddScoped<IPatientProfileUpdater, PatientProfileUpdater>();
        services.AddScoped<IPatientProfileCreator, PatientProfileCreator>();
        services.AddSingleton<IValidator<CreateProfileModel>, CreateProfileModelValidator>();
        services.AddSingleton<IValidator<UpdateProfileModel>, UpdateProfileModelValidator>();
    }
}