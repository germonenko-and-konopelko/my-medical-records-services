using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MMR.Common.Results;
using MMR.Patient.Common;
using MMR.Patient.Create;

namespace MMR.Patient;

public static class Endpoints
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("patients.createProfile",
            async Task<Results<Ok<Profile>, BadRequest<ProblemDetails>>> (
                [FromBody] CreateProfileModel createProfileModel,
                [FromServices] IPatientProfileCreator patientProfileCreator
            ) =>
            {
                var createResult = await patientProfileCreator.CreateAsync(createProfileModel);
                return CreateResponse(createResult);
            });
    }

    private static Results<Ok<TSuccessBody>, BadRequest<ProblemDetails>> CreateResponse<TSuccessBody>(
        Result<TSuccessBody, PatientError> source
    )
    {
        if (source.IsOk)
        {
            return TypedResults.Ok(source.Unwrap());
        }

        throw new NotImplementedException();
    }
}