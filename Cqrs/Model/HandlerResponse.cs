namespace Cqrs.Model
{
    public record HandlerResponse(
        Type Type,
        HandlerStatus Status,
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
