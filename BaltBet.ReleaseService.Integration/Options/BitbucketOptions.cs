using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Options
{
    public class BitbucketOptions
    {
        public static string Position = "Bitbucket";
        public string BaseUrl { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string[] Projects { get; set; }
    }
}
