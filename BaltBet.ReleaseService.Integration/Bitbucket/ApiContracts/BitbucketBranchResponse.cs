using BaltBet.ReleaseService.Integration.Bitbucket.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts
{
    public record BitbucketBranchResponse(
        [property: JsonPropertyName("values")] BitbucketBranch[]? Branches
        ) : IHasStaticEmpty<BitbucketBranchResponse>
    {
        public BitbucketBranchResponse() : this([]) { }

        private static readonly BitbucketBranchResponse EMPTY = new([]);
        public BitbucketBranchResponse Empty => EMPTY;
    }
}
