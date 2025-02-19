using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltBet.ReleaseService.Integration.Bitbucket
{
    public interface IHasStaticEmpty<T> where T : class
    {
        public T Empty { get; }
    }
}
