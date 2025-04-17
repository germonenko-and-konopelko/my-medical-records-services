using System.Net.Mime;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using MMR.Common.Api;
using MMR.Common.Api.Responses;
using MMR.Common.Api.Validation;
using MMR.Common.Optional;
using MMR.Common.Results;
using MMR.Patient.Common;
using MMR.Patient.Create;
using MMR.Patient.Resources;
using MMR.Patient.Update;
using Unauthorized = Microsoft.AspNetCore.Http.HttpResults.UnauthorizedHttpResult;
using Problem = Microsoft.AspNetCore.Http.HttpResults.BadRequest<MMR.Common.Api.Responses.ProblemResponse>;
using ServerError = Microsoft.AspNetCore.Http.HttpResults.InternalServerError<MMR.Common.Api.Responses.ProblemResponse>;

namespace MMR.Patient;

public static class Endpoints
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("patients.me.lookupProfile",
            async Task<Results<Ok<Profile>, NotFound, Problem, Unauthorized, ServerError>> (
                [FromServices] UserContext userContext,
                [FromServices] IPatientProfileRepository profileRepository
            ) =>
            {
                if (userContext.CurrentUserId is null)
                {
                    return TypedResults.Unauthorized();
                }

                Result<Option<Profile>, Exception> profile = await profileRepository.GetProfileAsync(userContext.CurrentUserId);
                if (profile.IsError)
                {
                    return TypedResults.InternalServerError(ProblemResponse.ServerError);
                }

                if (profile.Value.IsNone)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(profile.Value.Value);
            })
            .Produces<CreateProfileModel>()
            .Produces<Profile>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ServerError>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson);

        app.MapPost("patients.me.createProfile",
                async Task<Results<Ok<Profile>, Problem, Unauthorized, ServerError>> (
                    [FromBody] CreateProfileModel createProfileModel,
                    [FromServices] UserContext userContext,
                    [FromServices] IStringLocalizer<ErrorMessages> localizer,
                    [FromServices] IValidator<CreateProfileModel> validator,
                    [FromServices] IPatientProfileCreator patientProfileCreator
                ) =>
                {
                    if (userContext.CurrentUserId is null)
                    {
                        return TypedResults.Unauthorized();
                    }

                    ValidationResult? validationResult = validator.Validate(createProfileModel);
                    if (!validationResult.IsValid)
                    {
                        return TypedResults.BadRequest(validationResult.ToProblemResponse());
                    }

                    Result<Profile, PatientError> createResult = await patientProfileCreator.CreateAsync(
                        userContext.CurrentUserId,
                        createProfileModel
                    );
                    if (createResult.IsOk)
                    {
                        return TypedResults.Ok(createResult.Value);
                    }

                    if (createResult.Error.IsInternal)
                    {
                        return TypedResults.InternalServerError(ProblemResponse.ServerError);
                    }

                    return TypedResults.BadRequest(new ProblemResponse
                    {
                        Title = localizer["CannotCreatePatientProfileTitle"],
                        Detail = localizer[$"{createResult.Error.Code:G}ErrorDetails"],
                        Status = StatusCodes.Status400BadRequest,
                    });
                })
            .Accepts<CreateProfileModel>(MediaTypeNames.Application.Json)
            .Produces<Profile>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces<Problem>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)
            .Produces<ServerError>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson);

        app.MapPatch("patients.me.updateProfile",
            async Task<Results<Ok<Profile>, Problem, Unauthorized, ServerError>> (
                [FromBody] UpdateProfileModel updateProfileModel,
                [FromServices] UserContext userContext,
                [FromServices] IStringLocalizer<ErrorMessages> localizer,
                [FromServices] IValidator<UpdateProfileModel> validator,
                [FromServices] IPatientProfileUpdater patientProfileUpdater
            ) =>
            {
                if (userContext.CurrentUserId is null)
                {
                    return TypedResults.Unauthorized();
                }

                ValidationResult? validationResult = validator.Validate(updateProfileModel);
                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(validationResult.ToProblemResponse());
                }

                Result<Profile, PatientError> updateResult = await patientProfileUpdater.CreateAsync(
                    userContext.CurrentUserId,
                    updateProfileModel
                );
                if (updateResult.IsOk)
                {
                    return TypedResults.Ok(updateResult.Value);
                }

                if (updateResult.Error.IsInternal)
                {
                    return TypedResults.InternalServerError(ProblemResponse.ServerError);
                }

                return TypedResults.BadRequest(new ProblemResponse
                {
                    Title = localizer["CannotUpdatePatientProfileTitle"],
                    Detail = localizer[$"{updateResult.Error.Code:G}ErrorDetails"],
                    Status = StatusCodes.Status400BadRequest,
                });
            })
            .Accepts<UpdateProfileModel>(MediaTypeNames.Application.Json)
            .Produces<Profile>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces<Problem>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)
            .Produces<ServerError>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson);
    }
}