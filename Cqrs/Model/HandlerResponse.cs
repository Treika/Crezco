using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Model
{
    public record HandlerResponse(
        Type Type,
        HandlerStatus HandlerStatus,
        string Message = "")
    { }

    public record HandlerResponse<TResult>(
        Type Type,
        HandlerStatus Status,
        TResult Result,
        string Message = "")
        : HandlerResponse(Type, Status, Message)
    { }
}
