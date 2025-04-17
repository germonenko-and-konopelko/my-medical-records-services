using Microsoft.Extensions.Localization;

namespace MMR.Patient.Resources;

internal interface IResponseFactory
{
}

internal class ResponseFactory(IStringLocalizer<ErrorMessages> localizer)
{
}