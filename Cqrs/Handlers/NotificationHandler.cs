using Cqrs.Model;

namespace Cqrs.Handlers
{
    public abstract class RequestHandler<TNotification> : INotificationHandler<TNotification> where TNotification : INotification
    {
        public async Task<HandlerResponse> Handle(TNotification notification, CancellationToken cancellationToken)
        {
            var response = await Notify(notification, cancellationToken);

            // add error handling and better support

            return response;
        }

        protected HandlerResponse Failure(HandlerStatus status, string errorMessage) => new(GetType(), status, errorMessage);
        protected HandlerResponse Success => new(GetType(), HandlerStatus.Success);

        protected abstract Task<HandlerResponse> Notify(TNotification notification, CancellationToken cancellationToken);
    }
}
