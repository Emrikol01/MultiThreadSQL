using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreadSql
{
    public class ExecConnections
    {
        public int sessionId { get; set; }
        public int mostRecentSessionId { get; set; }
        public DateTime connectTime { get; set; }
        public string netTransport { get; set; }
        public string protocolType { get; set; }
    }

    public class ExecSessions
    {
        public int sessionId { get; set; }
        public DateTime loginTime { get; set; }
        public string status { get; set; }

        public string loginName { get; set; }
    }
}
