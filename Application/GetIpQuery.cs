using Cqrs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public record GetIpQuery(string ipAddress) : Command;
}
