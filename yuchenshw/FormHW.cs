using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace yuchenshw
{
    public partial class FormHW : Form
    {
        MyDBHelper MyDB;

        public FormHW()
        {
            InitializeComponent();
        }

        private void FormHW_Load(object sender, EventArgs e)
        {
            MyDB = new MyDBHelper(Properties.Settings.Default.yuchenshwConnectionString, "phonebook");
            DataTable dt = MyDB.Select();
            dataGridView1.DataSource = dt;
            MyControlBinding(dt);
        }
        //模糊查詢資料
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
            
            MyControlBinding(dt);
        }

        //新增、修改、刪除那邊的資料綁定
        private void MyControlBinding(DataTable flower)
        {
            txtempid.DataBindings.Clear();
            txtempid.DataBindings.Add("Text", flower, "empid", true, DataSourceUpdateMode.OnPropertyChanged);
            txtempname.DataBindings.Clear();
            txtempname.DataBindings.Add("Text", flower, "empname", true, DataSourceUpdateMode.OnPropertyChanged);
            txtempphone.DataBindings.Clear();
            txtempphone.DataBindings.Add("Text", flower, "empphone", true, DataSourceUpdateMode.OnPropertyChanged);

        }

        //新增資料
        private void btn_add_Click(object sender, EventArgs e)
        {
            
            if (txtempid.Text.Length != 0 && txtempname.Text.Length != 0 && txtempphone.Text.Length != 0)
            {
                string[] temp = MyDB.GetInsertCommandParameters();

                Dictionary<string, string> dicadd = new Dictionary<string, string>();
                for (int index = 0; index < temp.Length; index++)
                {
                    string controlName = temp[index].Replace("@", "txt");
                    Control[] apple = this.Controls.Find(controlName,true);
                    dicadd.Add(temp[index], apple[0].Text);

                }
                int i = MyDB.Insert(dicadd);
                if (i==1)
                {
                    MessageBox.Show($"新增成功");
                    DataTable dt = MyDB.Select();
                    dataGridView1.DataSource = dt;
                    MyControlBinding(dt);
                }
                else
                {
                    MessageBox.Show($"新增失敗");
                }

            }
            else { MessageBox.Show($"資料請填寫完整"); }


        }


        //修改資料
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtempid.Text.Length != 0 && txtempname.Text.Length != 0 && txtempphone.Text.Length != 0)
            {
                string[] temp = MyDB.GetUpdateCommandParameters();
                Dictionary<string, string> dicchange = new Dictionary<string, string>();
                for (int index = 0; index < temp.Length; index++)
                {
                    string controlName = temp[index].StartsWith("@Original_") ? temp[index].Replace("@Original_", "txt") : temp[index].Replace("@", "txt");
                    Control[] apple = this.Controls.Find(controlName, true);
                    dicchange.Add(temp[index], apple[0].Text);
                }

                int i = MyDB.Update(dicchange);
                if (i == 1)
                {
                    MessageBox.Show($"更新成功");
                    DataTable dt = MyDB.Select();
                    dataGridView1.DataSource = dt;
                    MyControlBinding(dt);
                }
                else
                {
                    MessageBox.Show($"更新失敗");
                }

            }
            else { MessageBox.Show($"資料請填寫完整"); }

        }


        //刪除資料
        private void btn_delete_Click(object sender, EventArgs e)
        {

            if (txtempid.Text.Length != 0 && txtempname.Text.Length != 0 && txtempphone.Text.Length != 0)
            {
                string[] temp = MyDB.GetDeleteCommandParameters();
                //@Original_empid
                Dictionary<string, string> dicdelete = new Dictionary<string, string>();
                for (int index = 0; index < temp.Length; index++)
                {
                    string controlName = (temp[index].StartsWith("@Original") ? temp[index].Replace("@Original_", "txt") : temp[index].Replace("@", "txt"));
                    Control[] apple = this.Controls.Find(controlName, true);
                    dicdelete.Add(temp[index], apple[0].Text);
                }

                int i = MyDB.Delete(dicdelete);

                if (i == 1)
                {
                    MessageBox.Show($"刪除成功");
                    DataTable dt = MyDB.Select();
                    dataGridView1.DataSource = dt;
                    MyControlBinding(dt);
                }
                else
                {
                    MessageBox.Show($"刪除失敗");
                }

            }
            else { MessageBox.Show($"請指定刪除項目"); }
        }

    }
}
