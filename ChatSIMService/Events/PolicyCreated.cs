using ChatSIMService.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatSIMService.Events
{
    public class PolicyCreated : INotification
    {
        public string PolicyNumber { get; set; }
        public string ProductCode { get; set; }
        public DateTime PolicyFrom { get; set; }
        public DateTime PolicyTo { get; set; }
        public PersonDto PolicyHolder { get; set; }
        public decimal TotalPremium { get; set; }
        public string AgentLogin { get; set; }
    }
}
