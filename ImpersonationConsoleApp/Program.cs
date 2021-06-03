using ImpersonationConsoleApp.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpersonationConsoleApp
{
    class Program
    {
        private const string USER_NAME = "sac_fa_ffown";
        private const string PASSWORD = "6H#$9re2W$$W#__wW_#r";
        private const string DOMAIN = "AMERICAS";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            DateTime impersonationStarted;
            DateTime impersonationEnded;
            TimeSpan diff;

            try
            {
                if (int.TryParse(ConfigurationManager.AppSettings["totalImpersonationsByExecution"], out int totalImpersonations))
                {
                    for (int i = 0; i < totalImpersonations; i++)
                    {
                        impersonationStarted = DateTime.Now;

                        using (Impersonator impersonator = new Impersonator(USER_NAME, DOMAIN, PASSWORD))
                        {
                            impersonationEnded = DateTime.Now;
                        }

                        diff = impersonationEnded - impersonationStarted;

                        Logger.Info(string.Concat(
                            $"Impersonation started at: {impersonationStarted.ToString("hh:mm:ss.fff")}, ",
                            $"ended at: {impersonationEnded.ToString("hh:mm:ss.fff")}. ",
                            $"Process took {Math.Round(diff.TotalMilliseconds)} miliseconds."));
                    }
                }
                else
                {
                    throw new Exception("Invalid setting value for totalImpersonationsByExecution");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
