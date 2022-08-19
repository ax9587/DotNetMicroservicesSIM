namespace PaymentSIMService.Jobs
{
    public class BackgroundJobsConfig
    {
        public string HangfireConnectionStringName { get; set; }
        public string InPaymentFileFolder { get; set; }
    }
}