using Cqrs.Model;

namespace Cqrs.Handlers
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        public Task<HandlerResponse> Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
