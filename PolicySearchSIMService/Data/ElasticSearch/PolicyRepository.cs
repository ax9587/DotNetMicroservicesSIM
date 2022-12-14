using Nest;
using PolicySearchSIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Policy = PolicySearchSIMService.Model.Policy;

namespace PolicySearchSIMService.Data.ElasticSearch
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ElasticClient elasticClient;

        public PolicyRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task Add(Policy policy)
        {
            var response=await elasticClient.IndexDocumentAsync<Policy>(policy);
            if (!response.IsValid)
            {
                Console.WriteLine("Invalid response received: {0}", response.ServerError);
                Console.WriteLine("\n\n===\n\nDebug information: {0}", response.DebugInformation);
            }
        }

        public async Task<List<Policy>> Find(string queryText)
        {
            var result = await elasticClient
                .SearchAsync<Policy>(
                    s =>
                        s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.MultiMatch(mm =>
                                mm.Query(queryText)
                                .Fields(f => f.Fields(p => p.PolicyNumber, p => p.PolicyHolder))
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(Fuzziness.Auto)
                            )
                    ));

           return result.Documents.ToList();
        }
    }
}
