using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.Pull
{
    public class CreatePullRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; } = "OPEN";

        [JsonPropertyName("fromRef")]
        public CreatepullRequestSourceBranch Source { get; set; }

        [JsonPropertyName("toRef")]
        public CreatepullRequestSourceBranch Destination { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; } = false;
    }

    public class CreatepullRequestBranchRepository
    {
        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("project")]
        public CreatepullRequestBranchProject Project { get; set; }
    }

    public class CreatepullRequestBranchProject
    {
        [JsonPropertyName("key")]
        public string ProjectKey { get; set; }
    }

    public class CreatepullRequestSourceBranch
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("repository")]
        public CreatepullRequestBranchRepository Repository { get; set; }

    }
}
