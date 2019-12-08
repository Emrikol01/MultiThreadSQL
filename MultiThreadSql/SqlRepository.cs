using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadSql
{
    public class SqlRepository
    {
        public string _conStr { get; set; }
        public SqlRepository(string conStr)
        {
            _conStr = conStr;
        }
        

        public List<string> GetDMV(string query)
        {
            List<string> list = new List<string>();

            using (var con = new SqlConnection(_conStr))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = query;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetValue(0).ToString());
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<ExecConnections>> GetDmvExecConnectionsAsync()
        {
            List<ExecConnections> listCon = new List<ExecConnections>();
            ExecConnections execCon = new ExecConnections();
            string query = "Select session_id, most_recent_session_id, connect_time, net_transport, protocol_type from sys.dm_exec_connections";
            
            try
            {
                using (var con = new SqlConnection(_conStr))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = query;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                execCon.sessionId = reader.GetInt32(reader.GetOrdinal("session_id"));
                                execCon.mostRecentSessionId = reader.GetInt32(reader.GetOrdinal("most_recent_session_id"));
                                execCon.connectTime = reader.GetDateTime(reader.GetOrdinal("connect_time"));
                                execCon.netTransport = reader.GetString(reader.GetOrdinal("net_transport"));
                                execCon.protocolType = reader.GetString(reader.GetOrdinal("protocol_type"));
                                listCon.Add(execCon);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("error thrown in GetDmvExecConnectionsAsync");
            }

            return listCon;
        }

        public async Task<List<ExecSessions>> GetDmvExecSessionsAsync()
        {
            List<ExecSessions> listSession = new List<ExecSessions>();
            ExecSessions execSess = new ExecSessions();
            string query = "select session_id, login_name, login_time, status from sys.dm_exec_sessions;";

            try
            {
                using (var con = new SqlConnection(_conStr))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = query;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                execSess.sessionId = reader.GetInt16(reader.GetOrdinal("session_id"));
                                execSess.loginName = reader.GetString(reader.GetOrdinal("login_name"));
                                execSess.loginTime = reader.GetDateTime(reader.GetOrdinal("login_time"));
                                execSess.status = reader.GetString(reader.GetOrdinal("status"));

                                listSession.Add(execSess);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Error thrown in GetDmvExecSessionsAsync");
            }


            return listSession;
        }

    }
}
