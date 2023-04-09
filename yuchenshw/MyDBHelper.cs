using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuchenshw
{
    internal class MyDBHelper
    {
        //請出連線字串
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private SqlCommandBuilder builder;
        private DataTable dt;

        //連線字串、Table名稱
        public MyDBHelper(string connstr, string initDataTableName)
        {
            conn = new SqlConnection(connstr);
            adapter = new SqlDataAdapter($"select * from {initDataTableName}", conn);
            builder = new SqlCommandBuilder(adapter);
        }
        //填入table值至欄位(無條件)
        public DataTable Select()
        {
            dt = new DataTable();
            adapter.FillSchema(dt, SchemaType.Mapped);
            adapter.Fill(dt);
            return dt;
        }


        //填入table值至欄位(依條件)
        public DataTable Select(string sql)
        {
            adapter.SelectCommand = new SqlCommand(sql, conn);
            dt = new DataTable();
            adapter.FillSchema(dt, SchemaType.Mapped);
            adapter.Fill(dt);
            return dt;
        }

        //獲得Builder所建構Insert所需的集合
        public string[] GetInsertCommandParameters()
        {

            SqlParameterCollection temp = builder.GetInsertCommand(true).Parameters;

            string[] paramList = new string[temp.Count];

            for (int i = 0; i < temp.Count; i++)
            {
                paramList[i] = temp[i].ParameterName;
            }
            return paramList;
        }
        //新增資料
        public int Insert(Dictionary<string, string> data)
        {
            if (data.Count == GetInsertCommandParameters().Length)
            {
                adapter.InsertCommand = builder.GetInsertCommand();
                foreach (KeyValuePair<string, string> item in data)
                {
                    adapter.InsertCommand.Parameters[item.Key].Value = item.Value;
                }
                conn.Open();
                int i = adapter.InsertCommand.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            else
            {
                return -1;
            }
        }
        //修改資料
        public string[] GetUpdateCommandParameters()
        {

            builder.ConflictOption = ConflictOption.OverwriteChanges;
            SqlParameterCollection temp = builder.GetUpdateCommand(true).Parameters;
            //{@Original_empid}
            string[] paramList = new string[temp.Count];

            for (int i = 0; i < temp.Count; i++)
            {
                paramList[i] = temp[i].ParameterName;
            }
            return paramList;
        }
        public int Update(Dictionary<string, string> data)
        {
            if (data.Count == GetUpdateCommandParameters().Length)
            {
                adapter.UpdateCommand = builder.GetUpdateCommand();
                foreach (KeyValuePair<string, string> item in data)
                {
                    adapter.UpdateCommand.Parameters[item.Key].Value = item.Value;
                }
                conn.Open();
                int i = adapter.UpdateCommand.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            else
            {
                return -1;
            }
        }
        //刪除資料
        public string[] GetDeleteCommandParameters()
        {
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            SqlParameterCollection temp = builder.GetDeleteCommand(true).Parameters;
            string[] paramList = new string[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                paramList[i] += temp[i].ParameterName;
            }
            return paramList;
        }
        public int Delete(Dictionary<string, string> data)
        {
            if (data.Count == GetDeleteCommandParameters().Length)
            {
                adapter.DeleteCommand = builder.GetDeleteCommand();
                foreach (KeyValuePair<string, string> item in data)
                {
                    adapter.DeleteCommand.Parameters[item.Key].Value = item.Value;
                }
                conn.Open();
                int i = adapter.DeleteCommand.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            else
            {
                return -1;
            }
        }

    }
}
