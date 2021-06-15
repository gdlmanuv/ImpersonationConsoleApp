using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpersonationConsoleApp.Core
{
    public static class Persistance
    {
        public static IntPtr ImpersonationToken;

        static Persistance()
        {
            ImpersonationToken = IntPtr.Zero;
        }
    }
}
