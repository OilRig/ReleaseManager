using BaltBet.ReleaseService.Integration.Options;
using LibGit2Sharp;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Git
{
    public class GitReleaseService
    {
        private readonly BitbucketOptions _options;

        public GitReleaseService(IOptions<BitbucketOptions> options)
        {
            _options = options.Value;
        }

        public void CloseRelease(string releaseBranchName, string repoDirectory)
        {
            using var repo = new Repository(repoDirectory);
            var masterBranch = repo.Branches["master"];
            var releaseBranch = repo.Branches[releaseBranchName];
            var merger = new Signature(_options.UserName, string.Empty, DateTime.Now);

            Commands.Checkout(repo, masterBranch);
            repo.Merge(releaseBranch, merger, new MergeOptions() { CommitOnSuccess = true });

            repo.Network.Push(masterBranch, new PushOptions()
            {
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = _options.UserName,
                    Password = _options.Password
                }
            });
        }
    }
}
