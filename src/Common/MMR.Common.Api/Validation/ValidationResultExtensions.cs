using FluentValidation.Results;
using MMR.Common.Api.Resources;
using MMR.Common.Api.Responses;

namespace MMR.Common.Api.Validation;

public static class ValidationResultExtensions
{
    public static ProblemResponse ToProblemResponse(this ValidationResult validationResult)
    {
        var problemResponse = new ProblemResponse
        {
            Title = Errors.ProblemResponseTitle,
            Detail = Errors.ProblemResponseDetail,
            Status = 400,
        };

        return problemResponse;
    }
}