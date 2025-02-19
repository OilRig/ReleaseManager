using System.Text.Json.Serialization;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketBranch(
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonPropertyName("displayId")] string? DisplayId,
        [property: JsonPropertyName("type")] string? Type,
        [property: JsonPropertyName("latestCommit")] string? LatestCommit,
        [property: JsonPropertyName("latestChangeset")] string? LatestChangeset,
        [property: JsonPropertyName("isDefault")] bool IsDefault
        );


}
