using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithSociety.Data
{
    public class ValidationHelper
    {
        public static int code = 0;

        public static string role = "";

        public static int codeForUserRole = 0;

        public static string user = "";

        public static int codeForUser = 0;

        public static int codeRemoval = 0;

        public static IEnumerable<IdentityError> errors = new List<IdentityError>();
    }
}
