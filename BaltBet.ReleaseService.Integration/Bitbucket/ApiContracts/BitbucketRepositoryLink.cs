using System.Text.Json.Serialization;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketRepositoryLink(
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("href")] string? Url
        );

}
