using Cqrs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Handlers
{
    public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest
    {
        public Task<HandlerResponse<TResult>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
