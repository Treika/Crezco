using Cqrs.Model;

namespace Application.Queries
{
    public record GetIpQuery(string IpAddress) : Command;
}
