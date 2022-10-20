using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;

namespace WordPress2Blob
{
    class Program
    {
        internal enum CM_Environment
        {
            DEV = 0, PROD = 1
        }


        static void Main(string[] args)
        {
            string clientID = string.Empty;
            string clientSecret = string.Empty;
            string orgName = string.Empty;
            bool isTest = false;
            int testSelection = 1;

            //secure TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("======SELECT ACTION======", Environment.NewLine);
            Console.WriteLine("0) Performance Test{0}1) Get mapping{0}2) Full run{0}", Environment.NewLine);
            int userChoise = Convert.ToInt32(Console.ReadLine());

            //Provision text update
            Console.WriteLine("======SELECT ENVIRONMENT======");
            string[] envs = ConfigurationManager.AppSettings.AllKeys;

            //display list of connections
            for (int i = 0; i < envs.Length; i++)
            {
                Console.Write($"\n  {envs[i]}  - {i}");
            }
            Console.Write("\n\n");

            int envSelection;
            bool invalidRange;
            do
            {
                envSelection = Convert.ToInt32(Console.ReadLine());

                invalidRange = envSelection < 0 || envSelection > envs.Length - 1;
                if (invalidRange)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Where do you think you're going?");
                    Console.ResetColor();
                }
            }
            while (invalidRange);

            CM_Environment romtdgEnvironment = (CM_Environment)envSelection;

            orgName = ConfigurationManager.AppSettings[romtdgEnvironment.ToString()];

            //Provision Clent ID
            Console.WriteLine("======Insert client id======");
            clientID = Console.ReadLine();

            //Provision Clent Secret
            Console.WriteLine("======Insert client secret======");
            clientSecret = Console.ReadLine();

            switch (userChoise)
            {
                #region Performance test
                case 0:
                    {

                        int maxParallels = 3;
                        int maxCounter = 2;

                        //start timer
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        Console.WriteLine($"Started test");

                        //call test method
                        //var connector = getConnector(orgName, clientID, clientSecret);
                        ExecuteMySQLquery();
                        //time end marker
                        watch.Stop();
                        Console.WriteLine($"Total time in milliseconds: {watch.ElapsedMilliseconds}");
                        Console.ReadLine();
                        break;

                    }
                #endregion Performance test

                #region Get Mapping

                case 1:
                    {
                        //is test
                        Console.WriteLine("For real run select 0, to test (100 files) select 1: ");
                        testSelection = Convert.ToInt32(Console.ReadLine());
                        if (testSelection == 0 || testSelection == 1)
                        {
                            isTest = testSelection == 0 ? false : true;

                        }

                        var connector = getConnector(orgName, clientID, clientSecret);

                        #region Start Provision Update

                        Console.WriteLine("Console report: Started the execution " + DateTime.Now.ToLocalTime().ToString());
                        IOrganizationService service = connector.OrganizationServiceProxy;

                        //execute the method here
                        
                        Console.ReadLine();
                        break;

                        #endregion Start Provision Update
                    }
                #endregion Provision Update

                #region Full run

                case 2:
                    {
                        var connector = getConnector(orgName, clientID, clientSecret);
                        Console.ForegroundColor = ConsoleColor.Red;
                        




                        Console.WriteLine("Press enter to exit");
                        Console.ReadLine();
                        break;
                    }


                #endregion


                default:
                    Console.WriteLine("Option is not defined");
                    break;
            }

        }

        static bool isValidString(string source)
        {
            return !string.IsNullOrEmpty(source) && !string.IsNullOrWhiteSpace(source) && source != "&nbsp";
        }

        internal static CrmServiceClient getConnector(string environment, string clientId, string clientSecret)
        {
            CrmServiceClient connector = new CrmServiceClient($"AuthType=ClientSecret; url=https://{environment}.crm3.dynamics.com; ClientId={clientId}; ClientSecret={clientSecret}");


            string error = connector.LastCrmError;
            Exception ex = connector.LastCrmException;

            if (connector.IsReady)
            {
                return connector;

            }

            throw new Exception($"Connection error:  {error} \n Last server exception: {ex}");
        }

        static void ExecuteMySQLquery() {


            
            string connStr = "server=107.161.74.26;user id=cambrid6_michaelk;persistsecurityinfo=True;database=cambrid6_cforums;pwd=";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {


                string sql = "SELECT * FROM cambrid6_cforums.wp_postmeta where meta_value like '%069-Marly-Ohlsson-Director-Legal%'";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();                

                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.WriteLine(rdr[i]);
                    }
                }
                rdr.Close();

            }
            catch (MySqlException ex)
            {
                
            }
            conn.Close();
        }
    }
}
