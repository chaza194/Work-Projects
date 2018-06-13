using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataMapper.Resources.AppCode;

namespace DataMapper.Resources.AppCode
{
    class SQLHandler
    {
        public List<string> GetSQlSchemaFromSQLItem(SQLTreeItem item, string connectionstring)
        {
            List<string> result = GetSQLSchema(item.SchemaSQL, connectionstring);
            return result;
        }

        public DataTable GetDataFromSQLItem(SQLTreeItem item, string connectionstring, string[] globalParams)
        {
            string sql = item.SQL;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string str in globalParams)
            {
                string[] tmp = str.Split('=');
                dict.Add(tmp[0], tmp[1]);
            }

            foreach (KeyValuePair<string, string> pair in dict)
            {
                sql = sql.ToLower().Replace("[" + pair.Key.ToLower().Trim() + "]", "'"+pair.Value.Trim()+"'");
            }

            DataTable result = GetData(sql, connectionstring);

            return result;
        }

        public List<string> GetSQLSchema(string sql, string connectionstring)
        {
            DataTable dataTable = GetData(sql, connectionstring);
            List<string> result = new List<string>();
            foreach(DataColumn col in dataTable.Columns)
            {
                result.Add(col.ColumnName);
            }

            return result;
        }

        public DataTable GetData(string sql, string connectionstring)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dataTable);

                        conn.Close();
                        da.Dispose();
                    }
                }
            }

            return dataTable;
        }
    }
}
