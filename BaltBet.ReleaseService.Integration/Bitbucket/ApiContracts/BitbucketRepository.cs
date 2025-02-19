using System.Text.Json.Serialization;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketRepository(
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("slug")] string? Slug,
        [property: JsonPropertyName("project")] BitbucketProject? Project,
        [property: JsonPropertyName("links")] BitbucketRepositoryLinkContainer? Links
        );

}
