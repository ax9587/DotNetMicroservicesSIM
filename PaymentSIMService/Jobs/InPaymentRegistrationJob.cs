using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentSIMService.Data;
using PaymentSIMService.Jobs.Services;

namespace PaymentSIMService.Jobs
{
    public class InPaymentRegistrationJob
    {
        private readonly IPolicyAccountRepository dataStore;
        private readonly BackgroundJobsConfig jobConfig;

        public InPaymentRegistrationJob(IPolicyAccountRepository dataStore, BackgroundJobsConfig jobConfig)
        {
            this.dataStore = dataStore;
            this.jobConfig = jobConfig;
        }

        public async Task Run()
        {
            Console.WriteLine($"InPayment import started. Looking for file in {jobConfig.InPaymentFileFolder}");

            var importService = new InPaymentRegistrationService(dataStore);
            await importService.RegisterInPayments(jobConfig.InPaymentFileFolder, DateTimeOffset.Now);
            
            Console.WriteLine("InPayment import finished.");

        }
    }
}