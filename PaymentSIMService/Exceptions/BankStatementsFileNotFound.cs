using System;

namespace PaymentSIMService.Exceptions
{
    public class BankStatementsFileNotFound : BussinesExceptions
    {
        public BankStatementsFileNotFound(Exception ex) :
            base("Bank statements file not found.", ex)
        {
        }
    }
}
