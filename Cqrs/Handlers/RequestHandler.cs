using Cqrs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Handlers
{
    public abstract class RequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest
    {
        public async Task<HandlerResponse<TResult>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return await Request(request, cancellationToken);
        }
        protected HandlerResponse<TResult> Failure(HandlerStatus status, TResult result, string errorMessage) => new(GetType(), status, result, errorMessage);
        protected HandlerResponse<TResult> Success(TResult result) => new(GetType(), HandlerStatus.Success, result);
        protected abstract Task<HandlerResponse<TResult>> Request(TRequest request, CancellationToken cancellationToken);
    }

}
