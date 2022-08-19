using System;

namespace PaymentSIMService.Model
{
    public class PolicyAccountNumberGenerator
    {
        public string Generate() {
            return Guid.NewGuid().ToString();
        }
    }
}
