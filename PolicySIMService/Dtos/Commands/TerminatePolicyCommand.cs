using System;


namespace PolicySIMService.Dtos.Commands
{
    public class TerminatePolicyCommand 
    {
        public string PolicyNumber { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}