﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Model
{
    public enum HandlerStatus
    {
        Success,
        NotFound,
        Error
    }
}