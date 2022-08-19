using PolicySIMService.Dtos.Commands.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySIMService.Messaging
{
    public interface IMessageBusClient
    {
        void PublishPolicyCreated(PolicyCreated policyCreated);
        void PublishPolicyTerminated(PolicyTerminated policyTerminated);
    }
}
