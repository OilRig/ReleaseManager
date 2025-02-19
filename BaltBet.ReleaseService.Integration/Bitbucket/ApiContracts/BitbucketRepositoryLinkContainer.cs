using System.Text.Json.Serialization;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketRepositoryLinkContainer(
        [property: JsonPropertyName("clone")] BitbucketRepositoryLink[]? LinksToClone
        );

}
