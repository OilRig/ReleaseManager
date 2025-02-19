using BaltBet.ReleaseService.Integration.Bitbucket.Pull;
using System.Text.Json.Serialization;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketProject(
        [property: JsonPropertyName("key")] string? Key,
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("public")] bool Public,
        [property: JsonPropertyName("type")] string? Type,
        [property: JsonPropertyName("links")] Links? Links
        );

}
