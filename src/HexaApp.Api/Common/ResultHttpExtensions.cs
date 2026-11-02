using HexaApp.SharedKernel;

namespace HexaApp.Api.Common;

/// <summary>Traduction unique des erreurs métier en ProblemDetails (RFC 9457).</summary>
public static class ResultHttpExtensions
{
    public static IResult ToHttpResult(this Result result) =>
        result.IsSuccess ? Results.NoContent() : result.ToProblem();

    public static IResult ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccess ? Results.Ok(result.Value) : result.ToProblem();

    public static IResult ToProblem(this Result result)
    {
        var error = result.Error ?? throw new InvalidOperationException("Un succès n'a pas de ProblemDetails.");
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(
            statusCode: status,
            title: error.Code,
            detail: error.Message,
            extensions: new Dictionary<string, object?> { ["code"] = error.Code });
    }
}
