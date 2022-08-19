using PaymentSIMService.Data;
using PaymentSIMService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentSIMService.Jobs.Services
{
    public class InPaymentRegistrationService
    {
        private readonly IPolicyAccountRepository dataStore;

        public InPaymentRegistrationService(IPolicyAccountRepository dataStore)
        {
            this.dataStore = dataStore;
        }
        
        public async Task RegisterInPayments(string directory, DateTimeOffset date)
        {
            var fileToImport = new BankStatementFile(directory, date);

            if (!fileToImport.Exists())
            {
                return;
            }

            using (dataStore)
            {
                foreach (var txLine in fileToImport.Read())
                {
                    var account = await dataStore.FindByNumber(txLine.AccountNumber);
                    account?.InPayment(txLine.Amount, txLine.AccountingDate);
                    
                    dataStore.Update(account);
                }
                
                fileToImport.MarkProcessed();
                
                dataStore.SaveChanges();
            }
        }

        
    }
}
