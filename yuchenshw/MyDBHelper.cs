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

        //update該table可能會用到的參數
        public string[] GetUpdateCommandParameters()
        {

            builder.ConflictOption = ConflictOption.OverwriteChanges;
            SqlParameterCollection temp = builder.GetUpdateCommand(true).Parameters;
            string[] paramList = new string[temp.Count];

            for (int i = 0; i < temp.Count; i++)
            {
                paramList[i] = temp[i].ParameterName;
            }
            return paramList;
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


    }
}
