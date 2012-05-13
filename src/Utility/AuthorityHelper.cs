using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class AuthorityHelper
    {
        public static int GetAuthority(params string[] roles)
        {
            var intRoles = roles.Select(r => int.Parse(r)).ToArray();
            return GetAuthority(intRoles);
        }
        public static int GetAuthority(params int[] roles)
        {
            int authority = 0;
            foreach (var r in roles)
            {
                authority = authority | r;
            }
            return authority;
        }
    }
}
