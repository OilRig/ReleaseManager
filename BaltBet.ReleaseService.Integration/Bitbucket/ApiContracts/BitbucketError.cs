using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketErrorResponse(
        [property: JsonPropertyName("errors")] BitbucketError[]? Errors
    );

    public record BitbucketError(string? Message, string? ExceptionName);
}
