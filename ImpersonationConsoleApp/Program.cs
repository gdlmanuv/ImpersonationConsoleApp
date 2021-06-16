using ImpersonationConsoleApp.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpersonationConsoleApp
{
    class Program
    {
        private const string USER_NAME = "";
        private const string PASSWORD = "";
        private const string DOMAIN = "";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            DateTime overallStartedTime;
            DateTime overallEndedTime;
            DateTime impersonationStarted;
            DateTime impersonationEnded;
            TimeSpan diff;

            try
            {
                if (int.TryParse(ConfigurationManager.AppSettings["totalImpersonationsByExecution"], out int totalImpersonations))
                {
                    overallStartedTime = DateTime.Now;

                    for (int i = 0; i < totalImpersonations; i++)
                    {
                        impersonationStarted = DateTime.Now;

                        using (Impersonator impersonator = new Impersonator(USER_NAME, DOMAIN, PASSWORD))
                        {
                            impersonationEnded = DateTime.Now;
                            ExecuteDatabaseConnection();
                        }

                        diff = impersonationEnded - impersonationStarted;

                        Logger.Info(string.Concat(
                            $"Impersonation started at: {impersonationStarted.ToString("hh:mm:ss.fff")}, ",
                            $"ended at: {impersonationEnded.ToString("hh:mm:ss.fff")}. ",
                            $"Process took {Math.Round(diff.TotalMilliseconds)} miliseconds."));
                    }

                    overallEndedTime = DateTime.Now;

                    diff = overallEndedTime - overallStartedTime;

                    Logger.Info(string.Concat($"Process completed in {Math.Round(diff.TotalMilliseconds)} miliseconds."));
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
            finally
            {
                Console.ReadKey();
            }
        }

        private static void ExecuteDatabaseConnection()
        {
            using (var dbConn = new SqlConnection("Server=<SERVER>;Database=<DATABASE>;Integrated Security=SSPI;"))
            {
                using (var cmd = new SqlCommand("SELECT TOP 10 * FROM ffSerialNumber", dbConn))
                {
                    dbConn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Console.WriteLine(reader["Value"].ToString());
                        }
                    }
                }
            }
        }
    }
}
