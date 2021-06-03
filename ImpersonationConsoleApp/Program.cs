using ImpersonationConsoleApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpersonationConsoleApp
{
    class Program
    {
        private const string USER_NAME = "";
        private const string PASSWORD = "";
        private const string DOMAIN = "AMERICAS";

        static void Main(string[] args)
        {
            Task[] tasks =
            {
                Task.Run(() =>
                {
                    using (Impersonator impersonator = new Impersonator(USER_NAME, PASSWORD, DOMAIN))
                    {

                    }
                }),
            };
        }
    }
}
