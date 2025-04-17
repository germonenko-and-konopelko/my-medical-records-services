using System.Text.Json;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using MMR.Common.Api.Resources;
using MMR.Common.Api.Responses;

namespace MMR.Common.Api.Validation;

public static class ValidationResultExtensions
{
    public static ProblemResponse ToProblemResponse(this ValidationResult validationResult)
    {
        var error = ProblemResponse.ValidationError;
        error.InvalidFields = validationResult.Errors
            .GroupBy(err => err.PropertyName)
            .ToDictionary(
                err => JsonNamingPolicy.CamelCase.ConvertName(err.Key),
                err => err.Select(e => new FieldError { Code = e.ErrorCode, Message = e.ErrorMessage })
            );

        return error;
    }
}