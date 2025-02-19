using BaltBet.ReleaseService.Integration.Bitbucket.ApiContracts;
using BaltBet.ReleaseService.Integration.Bitbucket.Pull;
using System.Net.Http.Json;
using System.Text;
using static BaltBet.ReleaseService.Integration.Bitbucket.Constants.ApiConstants;

namespace BaltBet.ReleaseService.Integration.Bitbucket
{
    public class BitbucketApiService(HttpClient httpClient)
    {
        private static readonly Func<string, string> GetReposUrl = (key) => string.Format(API_PATH_TEMPLATE, key);

        private static readonly Func<string, string, string> GetPullRequestsUrl = (projectKey,repoSlug)
            => string.Format(PULL_REQUESTS_TEMPLATE, projectKey, repoSlug);

        private static readonly Func<string, string, int, string> GetPullRequestUrl = (projectKey, repoSlug, pullId)
            => string.Format(PULL_REQUEST_TEMPLATE, projectKey, repoSlug, pullId);

        private static readonly Func<string, string, string> GetBranchesUrl = (projectKey, repoSlug)
            => string.Format(BRANCHES_TEMPLATE, projectKey, repoSlug);

        private static readonly Func<string, string, int?, int, string> GetMergesUrl = (projectKey, repoSlug, version, pullId)
            => string.Format(MERGE_RELEASE_TEMPLATE, projectKey, repoSlug, pullId, version);

        private async Task<T?> GetResponse<T>(string url) where T : class, IHasStaticEmpty<T>, new()
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }

            return new T().Empty;
        }

        public Task<RepositoryResponse?> GetRepositories(string projectKey)
            => GetResponse<RepositoryResponse>(GetReposUrl(projectKey));

        public Task<PullRequestResponse?> GetPullRequests(string projectKey, string slug)
            => GetResponse<PullRequestResponse>(GetPullRequestsUrl(projectKey, slug));

        public Task<PullRequestResponse?> GetPullRequest(string projectKey, string slug, int id)
           => GetResponse<PullRequestResponse>(GetPullRequestUrl(projectKey, slug, id));

        public Task<BitbucketBranchResponse?> GetBranches(string projectKey, string slug)
            => GetResponse<BitbucketBranchResponse>(GetBranchesUrl(projectKey, slug));

        public async Task<int?> CreatePullRequest(string projectKey, string slug, string fromBranchName)
        {
            var branches = await GetBranches(projectKey, slug);

            var masterBranch = branches.Branches.FirstOrDefault(x => x.Id.ToLowerInvariant().Contains("master"));

            var repo = new CreatepullRequestBranchRepository()
            {
                Slug = slug,
                Project = new CreatepullRequestBranchProject()
                {
                    ProjectKey = projectKey,
                }
            };

            var request = new CreatePullRequest()
            {
               Title = fromBranchName,
               Source = new CreatepullRequestSourceBranch()
               {
                   Id = fromBranchName,
                   Repository = repo
               },
                Destination = new CreatepullRequestSourceBranch()
                {
                    Id = masterBranch.Id,
                    Repository = repo
                }
            };

            var response = await httpClient.PostAsJsonAsync(GetPullRequestsUrl(projectKey, slug), request);

            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadFromJsonAsync<PullRequestResponse>();

                if(cont.PullRequests?.Any() == true)
                    return cont.PullRequests[0].Id;

                return null;
            }

            var ctnt = await response.Content.ReadFromJsonAsync<BitbucketErrorResponse>();

            var alreadyClosed = ctnt.Errors.FirstOrDefault().ExceptionName.ToLowerInvariant().Contains(EmptyPullRequestExceptionText.ToLowerInvariant());

            if (!alreadyClosed)
                throw new ApplicationException(ctnt.Errors.Aggregate(new StringBuilder(), (sb, val) => sb.AppendLine(val.Message)).ToString());

            return null;
        }
        
        public async Task MergePullRequest(string projectKey, string slug, int pullRequestId, string fromBranchName)
        {
            var pullRequests = await GetPullRequests(projectKey, slug);
            var pRequest = pullRequests.PullRequests.FirstOrDefault(x => x.Id == pullRequestId);

            if (pRequest == null)
            {
                var createRes = await CreatePullRequest(projectKey, slug, fromBranchName);

                if(createRes.HasValue)
                {
                    var remoteRequest = await GetPullRequest(projectKey, slug, createRes.Value);
                    pRequest = remoteRequest.PullRequests[0];
                }
            }

            if (pRequest != null)
            {
                var check = await httpClient.GetAsync(GetMergesUrl(projectKey, slug, pRequest.Version, pullRequestId));

                if(check.IsSuccessStatusCode)
                {
                    var res = await httpClient.PostAsync(GetMergesUrl(projectKey, slug, pRequest.Version, pullRequestId), null);

                    if (res.IsSuccessStatusCode)
                    {
                        await Task.CompletedTask;
                    }
                    var ctnt = await res.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
