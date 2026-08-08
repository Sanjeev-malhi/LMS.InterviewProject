using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Enums
{
    public enum EmailStatus : byte
    {
        Pending = 1,

        Send = 2,

        Failed = 3
    }
}
