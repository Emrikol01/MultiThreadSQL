

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreadSql
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("welcome to running async tasks");

            string serverName = "";
            string userId = "";
            string password = "";

            if (serverName == string.Empty || userId == string.Empty || password == string.Empty)
            {
                Console.WriteLine("server, username & password must be filled in.");
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            string conStr = string.Format("Server= {0}; Database=Master;User ID = {1}; Password = {2}", serverName, userId, password);

            SqlRepository repo = new SqlRepository(conStr);

            List<ExecConnections> lRun01 = new List<ExecConnections>();
            List<ExecSessions> lRun02 = new List<ExecSessions>();
            DateTime startTime = DateTime.Now;

            var Run01 = Task.Run(async () => lRun01 = await repo.GetDmvExecConnectionsAsync());
            var Run02 = Task.Run(async () => lRun02 = await repo.GetDmvExecSessionsAsync());

            Task.WhenAll(Run01, Run02).Wait();

            DateTime endTime = DateTime.Now;
            Console.WriteLine(endTime.Millisecond - startTime.Millisecond);
            Console.WriteLine("finished running the Tasks.");
            Console.WriteLine(" - - - - - - ");
            Console.WriteLine(" ");

            foreach (var item1 in lRun01)
            {
                Console.WriteLine(string.Format("{0} ; {1} ; {2} ; {3} ; {4}", item1.sessionId, item1.netTransport, item1.protocolType, item1.connectTime, item1.mostRecentSessionId));
            }
            foreach (var item2 in lRun02)
            {
                Console.WriteLine(string.Format("{0} ; {1} ; {2} ; {3}", item2.sessionId, item2.loginName, item2.loginTime, item2.status));
            }
            Console.Read();
        }
    }
}
