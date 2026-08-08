using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Enums
{
    public enum EmailType : byte
    {
        Welcome = 1,

        ForgetPassword = 2,

        CourseEnrollment = 3,

        Certificate = 4,

        Invoice = 5, 

        otp = 6
    }
}
