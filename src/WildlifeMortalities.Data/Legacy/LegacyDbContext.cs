using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Legacy
{
    public class LegacyDbContext
    {
        private static string connString = "Data Source=sql-apps2-prd.ynet.gov.yk.ca;Initial Catalog=HarvestDB;Integrated Security=True;";

        public static IDbConnection GetConnection()
        {
            var conn = new SqlConnection(connString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        public static void CloseConnection(IDbConnection conn)
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Broken)
            {
                conn.Close();
            }
        }
    }
}
