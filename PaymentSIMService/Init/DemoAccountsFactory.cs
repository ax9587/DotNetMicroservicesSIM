
using PaymentSIMService.Model;
using System.Collections.Generic;

namespace PaymentSIMService.Init
{
    internal static class DemoAccountsFactory
    {
        internal static List<PolicyAccount> DemoAccounts()
        {
            return new List<PolicyAccount>() {
                    new PolicyAccount("POLICY_1", "231232132131","Tim","Jones"),
                    new PolicyAccount("POLICY_2", "389hfswjfrh2032r","Mike","Zorn"),
                    new PolicyAccount("POLICY_3", "0rju130fhj20","Judith", "Powell")
            };
        }
    }
}
