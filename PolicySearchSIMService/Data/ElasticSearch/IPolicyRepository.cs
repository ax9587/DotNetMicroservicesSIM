using PolicySearchSIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchSIMService.Data.ElasticSearch
{
    public interface IPolicyRepository
    {
        Task Add(Policy policy);

        Task<List<Policy>> Find(string queryText);
    }
}
