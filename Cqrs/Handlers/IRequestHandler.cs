using Cqrs.Model;

namespace Cqrs.Handlers
{
    public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest
    {
        public Task<HandlerResponse<TResult>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
