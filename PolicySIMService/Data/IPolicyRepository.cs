using PolicySIMService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Data
{
    public interface IPolicyRepository
    {
        void Add(Policy policy);

        Task<Policy> WithNumber(string number);

        bool SaveChanges();
    }
}
