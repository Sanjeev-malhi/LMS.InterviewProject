using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Enums
{
    public enum EnrollmentStatus : byte
    {
        Pending = 0,

        Active = 1,

        Failed = 2
    }
}
