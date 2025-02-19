using BaltBet.ReleaseService.Integration.Bitbucket;
using BaltBet.ReleaseService.Integration.Bitbucket.Pull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Models
{
    public class ProjectModel
    {
        public string? Name { get; set; }
        public string? ProjectKey { get; set; }
        public ProjectRepository[]? Repositories { get; set; }
    }

    public class ProjectRepository
    {
        public string? Name { get; set; }

        public int Id { get; set; }

        public string?[]? Links { get; set; }

        public string? Slug { get; set; }

        public Branch[]? ReleaseBranches { get; set; }
    }

    public class Branch
    {
        public string? Name { get; set; }
        public string? LastCommitHash { get; set; }
        public bool HasPullRequest => PullRequest != null;
        public DateTime LastCommitDate { get; set; }

        public PullRequest? PullRequest { get; set; }
        public bool IsClosedToMaster { get; set; }
    }

    public class PullRequestRequest
    {
        public string? Slug { get; set; }
        public string? ProjectKey { get; set; }
        public string? BranchName { get; set; }
        public int? Id { get; set; }
        public int? Version { get; set; }
    }
}
