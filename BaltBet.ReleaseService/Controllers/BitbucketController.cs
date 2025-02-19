using BaltBet.ReleaseService.Integration.Bitbucket;
using BaltBet.ReleaseService.Integration.Models;
using BaltBet.ReleaseService.Integration.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BaltBet.ReleaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitbucketController(BitbucketApiService bitbucketApiService,
        IOptions<BitbucketOptions> options) : ControllerBase
    {
        private readonly BitbucketOptions _bitbucketOptions = options.Value;

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var res = await _bitbucketOptions.Projects.ToAsyncEnumerable()
                .SelectAwait(async x =>
                {
                    var response = await bitbucketApiService.GetRepositories(x);

                    var projectRepo = response.Repositories.FirstOrDefault(r => r.Project?.Key == x);
                    
                    return new ProjectModel()
                    {
                        Name = projectRepo?.Project?.Name,
                        ProjectKey = x
                    };
                })
                .ToArrayAsync();

            return Ok(res);
        }

        [HttpGet("project/{projectKey}")]
        public async Task<IActionResult> GetProject(string projectKey)
        {
            var repos = await bitbucketApiService.GetRepositories(projectKey);
           
            return Ok(new ProjectModel()
            {
                Name = repos.Repositories[0].Project.Name,
                ProjectKey = projectKey,
                Repositories = repos.Repositories.Select(x => new ProjectRepository()
                {
                    Name = x.Name,
                    Id= x.Id,
                    Links = x.Links.LinksToClone.Select(l => l.Url).ToArray(),
                    Slug = x.Slug
                }).ToArray()
            });
        }

        [HttpGet("repository/{projectKey}/{slug}")]
        public async Task<IActionResult> GetRepoDetails(string projectKey, string slug)
        {
            var branches = await bitbucketApiService.GetBranches(projectKey, slug);
            var repos = await bitbucketApiService.GetRepositories(projectKey);
            var pulls = await bitbucketApiService.GetPullRequests(projectKey, slug);

            var repo = repos.Repositories.FirstOrDefault(x => x.Slug == slug);

            if(repo != null)
            {
                ProjectRepository projectRepository = new()
                {
                    Id = repo.Id,
                    Name = repo.Name,
                    Links = repo.Links.LinksToClone.Select(l => l.Url).ToArray(),
                    Slug = repo.Slug,
                    ReleaseBranches = await branches.Branches
                    .ToAsyncEnumerable()
                    .Where(x => x.DisplayId.ToLowerInvariant().Contains("release"))
                    .SelectAwait(async x =>
                    {
                        var branch = new Branch()
                        {
                            Name = x.DisplayId,
                            LastCommitHash = x.LatestCommit
                        };

                        var foundReq = pulls.PullRequests.FirstOrDefault(p => p.FromRef.DisplayId == x.DisplayId);
                        branch.IsClosedToMaster = foundReq != null && foundReq.State.ToLowerInvariant() == "merged";

                        if (foundReq == null)
                        {
                            var pullResult = await bitbucketApiService.CreatePullRequest(projectKey, slug, branch.Name);
                            branch.IsClosedToMaster = !pullResult.HasValue;
                        }
                        return branch;
                    }).ToArrayAsync()
                };

                return Ok(projectRepository);
            }

            return Problem();
        }

        [HttpPost("pullrequest/create")]
        public async Task<IActionResult> CreatePullrequest([FromBody] PullRequestRequest pullRequestRequest)
            => Ok(await bitbucketApiService.CreatePullRequest(pullRequestRequest.ProjectKey, pullRequestRequest.Slug, pullRequestRequest.BranchName));


        [HttpPost("pullrequest/merge")]
        public async Task<IActionResult> MergePullRequest([FromBody] PullRequestRequest pullRequestRequest)
        {
            await bitbucketApiService.MergePullRequest(pullRequestRequest.ProjectKey, pullRequestRequest.Slug, pullRequestRequest.Id ?? 0, pullRequestRequest.BranchName);

            return Ok();
        }
    }
}
