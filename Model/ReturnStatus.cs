using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public enum ReturnStatus
{
    Success = 0,
    Fail = 1,
    Retry = 2,
    Exception = 3
}
