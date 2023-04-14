namespace Cqrs.Model
{
    public record Command : INotification, IRequest;
}
