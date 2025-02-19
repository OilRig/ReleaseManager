using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.Pull
{
    public record PullRequestAuthor(
        [property: JsonPropertyName("user")] User? User,
        [property: JsonPropertyName("role")] string? Role,
        [property: JsonPropertyName("approved")] bool IsApproved,
        [property: JsonPropertyName("status")] string? Status
        );

    public class Clone
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class FromRef
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayId")]
        public string DisplayId { get; set; }

        [JsonPropertyName("latestCommit")]
        public string LatestCommit { get; set; }
    }

    public class Links
    {
        [JsonPropertyName("self")]
        public List<Self> Self { get; set; }

        [JsonPropertyName("clone")]
        public List<Clone> Clone { get; set; }
    }

    public class MergeResult
    {
        [JsonPropertyName("outcome")]
        public string Outcome { get; set; }

        [JsonPropertyName("current")]
        public bool? Current { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("mergeResult")]
        public MergeResult MergeResult { get; set; }

        [JsonPropertyName("resolvedTaskCount")]
        public int? ResolvedTaskCount { get; set; }

        [JsonPropertyName("openTaskCount")]
        public int? OpenTaskCount { get; set; }
    }

    public class PullRequest
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("open")]
        public bool? Open { get; set; }

        [JsonPropertyName("closed")]
        public bool? Closed { get; set; }

        [JsonPropertyName("createdDate")]
        public long? CreatedDate { get; set; }

        [JsonPropertyName("updatedDate")]
        public long? UpdatedDate { get; set; }

        [JsonPropertyName("fromRef")]
        public FromRef FromRef { get; set; }

        [JsonPropertyName("toRef")]
        public ToRef ToRef { get; set; }

        [JsonPropertyName("locked")]
        public bool? Locked { get; set; }

        [JsonPropertyName("author")]
        public PullRequestAuthor Author { get; set; }

        [JsonPropertyName("reviewers")]
        public List<object> Reviewers { get; set; }

        [JsonPropertyName("participants")]
        public List<object> Participants { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }
    }
    public class PullRequestResponse : IHasStaticEmpty<PullRequestResponse>
    {
        [JsonPropertyName("values")]
        public PullRequest[] PullRequests { get; set; }

        private static readonly PullRequestResponse EMPTY = new() { PullRequests = Array.Empty<PullRequest>() };
        public PullRequestResponse Empty => EMPTY;
    }
    public class Self
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class ToRef
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayId")]
        public string DisplayId { get; set; }

        [JsonPropertyName("latestCommit")]
        public string LatestCommit { get; set; }
    }

    public class User
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("active")]
        public bool? Active { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }
    }


}
