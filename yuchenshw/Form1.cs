using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yuchenshw
{
    public partial class Form1 : Form
    {
        MyDBHelper MyDB;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyDB = new MyDBHelper(Properties.Settings.Default.yuchenshwConnectionString, "phonebook");
            DataTable dt = MyDB.Select();
            dataGridView1.DataSource = dt;

        }
        //txt各欄位值


        private void btn_search_Click(object sender, EventArgs e)
        {
            string sql = "";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@empid", txt_empid.Text.Trim());
            dic.Add("@empname", txt_empname.Text.Trim());
            dic.Add("@empphone", txt_empphone.Text.Trim());

            sql = $"select * from phonebook where empid like '%{dic["@empid"]}%' and empname like '%{dic["@empname"]}%' and empphone like '%{dic["@empphone"]}%'";

            DataTable dt = MyDB.Select(sql);
            dataGridView1.DataSource = dt;
        }



        private void btn_delete_Click(object sender, EventArgs e)
        {

        }


    }
}
