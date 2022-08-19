using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentSIMService.Model;

namespace PaymentSIMService.Data
{
    public class HangfireContext : DbContext
    {
        public HangfireContext (DbContextOptions<HangfireContext> options)
            : base(options)
        {
        }
    }
}