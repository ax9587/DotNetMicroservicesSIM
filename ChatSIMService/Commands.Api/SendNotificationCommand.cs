using MediatR;

namespace ChatSIMService.Commands.Api
{
    public class SendNotificationCommand : IRequest
    {
        public string Message { get; set; }
    }
}