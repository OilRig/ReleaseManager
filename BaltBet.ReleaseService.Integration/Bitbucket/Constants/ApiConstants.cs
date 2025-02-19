using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket.Constants
{
    internal class ApiConstants
    {
        public const string API_PATH_TEMPLATE = "{0}/repos";
        public const string PULL_REQUESTS_TEMPLATE = "{0}/repos/{1}/pull-requests?state=ALL";
        public const string PULL_REQUEST_TEMPLATE = "{0}/repos/{1}/pull-requests/{2}";
        public const string BRANCHES_TEMPLATE = "{0}/repos/{1}/branches";
        public const string MERGE_RELEASE_TEMPLATE = "{0}/repos/{1}/pull-requests/{2}/merge?version={3}";

        public const string EmptyPullRequestExceptionText = "EmptyPullRequestException";
    }
}
