using BaltBet.ReleaseService.Integration.Bitbucket;
using BaltBet.ReleaseService.Integration.Models;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Git
{
    public class GitRepoInfoService
    {
        private readonly BitbucketApiService _bitbucketRepositoryService;
        public GitRepoInfoService(BitbucketApiService bitbucketRepositoryService)
        {
            _bitbucketRepositoryService = bitbucketRepositoryService;
        }

        public async Task<ProjectModel> GetProjectRepositoryInfo(string projectKey)
        {
            var repos = await _bitbucketRepositoryService.GetRepositories(projectKey);

            List<ProjectRepository> result = new List<ProjectRepository>();

            foreach (var repo in repos.Repositories!)
            {
                using var libRepo = new Repository(repo.Links.LinksToClone[0].Url, new RepositoryOptions());

                result.Add(new ProjectRepository()
                {
                    Links = repo.Links.LinksToClone.Select(x => x.Url).ToArray(),
                    Name = repo.Name,
                    ReleaseBranches = libRepo.Branches
                    .Where(x => x.RemoteName.ToLowerInvariant().Contains("release"))
                    .Select(x => new Models.Branch()
                    {
                        Name = x.RemoteName
                    }).ToArray()
                });
            }

            return new ProjectModel()
            {
                Repositories = result.ToArray()
            };
        }
    }
}
