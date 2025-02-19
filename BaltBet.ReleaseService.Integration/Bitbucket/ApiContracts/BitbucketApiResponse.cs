using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record RepositoryResponse(
       [property: JsonPropertyName("values")] BitbucketRepository[]? Repositories
    ) : IHasStaticEmpty<RepositoryResponse>
    {
        public RepositoryResponse() : this([]) { }

        private static readonly RepositoryResponse EMPTY = new([]);

        public RepositoryResponse Empty => EMPTY;
    };

}
