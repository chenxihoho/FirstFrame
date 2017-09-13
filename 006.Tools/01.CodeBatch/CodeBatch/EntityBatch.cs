using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace codeBatch
{
    public partial class EntityBatch : MasterForm
    {
        public EntityBatch()
        {
            InitializeComponent();
        }

        string DBname = string.Empty;

        private void EntityBatch_Load(object sender, EventArgs e)
        {
            try
            {
                txtdb.Text = ConString;
                tbxNameSpace.Text = NamespaceConfig;
                tbxpath.Text = PathConfig;



                //获取数据库所有表名
                //comboBox1.DataSource = GetDBTable();
                //comboBox1.DisplayMember = "TableName";
                //comboBox1.ValueMember = "TableName";
            }
            catch (Exception ex)
            {
                txtmsg.Text = ex.Message;
                throw;
            }

        }


        private void rbtbcreate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtbcreate.Checked)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //批量生成
                if (rbtbcreate.Checked)
                {
                    DataTable dtValue = GetDBTable();

                    foreach (DataRow row in dtValue.Rows)
                    {
                        string tableName = ObjToStr(row["TableName"]);
                        DataTable dtBatch = GetTableColumn(tableName);
                        if (!CreateEntity(tableName))
                        {
                            break;
                        }
                    }
                }
                //单个生成
                else
                {
                    string tableName = ObjToStr(comboBox1.SelectedValue);
                    CreateEntity(tableName);
                }
            }
            catch (Exception ex)
            {
                string msg = txtmsg.Text.Trim();
                txtmsg.Text = msg + "\r\n" + ex.Message;
                throw;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtmsg.Text = string.Empty;
        }

        /// <summary>
        /// 生成单个表Entity
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private bool CreateEntity(string tableName)
        {
            string modelStr = @"using System;" + System.Environment.NewLine +
                              @"using System.Collections.Generic;" + System.Environment.NewLine +
                              @"using System.Text;" + System.Environment.NewLine +
                              @"using ybzf.Storage.Dapper.Common;" + System.Environment.NewLine + System.Environment.NewLine +

                                "namespace namespaceName" + System.Environment.NewLine +
                                "{" + System.Environment.NewLine +
                                "    [Database(Name = 'DBname')]" + System.Environment.NewLine +
                                "    [Owner(Name = 'dbo')]" + System.Environment.NewLine +
                                "    [Table(Name = 'Tname')]" + System.Environment.NewLine +

                                "    public class className" + System.Environment.NewLine +
                                "    {" + System.Environment.NewLine +
                                "columnStr" + System.Environment.NewLine +
                                "    }" + System.Environment.NewLine +
                                "}";
            string namespacestr = tbxNameSpace.Text.Trim();
            string savepath = tbxpath.Text.Trim();
            if (string.IsNullOrEmpty(namespacestr) || string.IsNullOrEmpty(savepath))
            {
                string msg = txtmsg.Text.Trim();
                msg += "\r\n" + "命名空间、保存路径和数据库链接都不能为空！";
                txtmsg.Text = msg.Trim();
                return false;
            }

            StreamWriter sw = null;
            DataTable dtValue = GetTableColumn(tableName);
            string columnStr = string.Empty;

            modelStr = modelStr.Replace("Tname", tableName).Replace("DBname", DBname).Replace("'", "\"");

            string subTablename = string.Empty;
            if (tableName.ToLower().IndexOf("tb") == 0)
            {
                subTablename = tableName.Substring(2);
            }

            string csname = subTablename + "_Model.cs";
            string filename = subTablename + "_Model";

            // 保存路劲是否存在如果不存在就创建
            if (!System.IO.Directory.Exists(savepath))
            {
                System.IO.Directory.CreateDirectory(savepath);
            }

            try
            {
                if (!DataTableIsNullOrEmpty(dtValue))
                {


                    foreach (DataRow row in dtValue.Rows)
                    {
                        string addColumn = string.Empty;
                        string columnEntity = @"public columnType columnName { get; set; }";

                        string columnMasterEntity = @"[ID]" + System.Environment.NewLine +
                                                    @"        public columnType columnName { get; set; }";
                        string columnMasterAddEntity = @"[ID(true)]" + System.Environment.NewLine +
                                                       @"         public columnType columnName { get; set; }";
                        string columnAddEntity = @"[Ignore]" + System.Environment.NewLine +
                                                 @"        public columnType columnName { get; set; }";
                        string columnName = ObjToStr(row["columnName"]);
                        string columnType = ObjToStr(row["columnType"]);
                        string masterKey = ObjToStr(row["masterkey"]);
                        string addKey = ObjToStr(row["systemAdd"]);
                        string addnull = ObjToStr(row["cnull"]);
                        string adddesc = ObjToStr(row["cdesc"]).Replace("\r\n", " ");
                        columnType = GetColumnType(columnType.ToLower());

                        if (masterKey.Equals("1") && addKey.Equals("1"))
                        {
                            addColumn = columnMasterAddEntity.Replace("columnName", columnName).Replace("columnType", columnType).Replace("cdesc", adddesc);
                        }

                        else
                        {
                            if (masterKey.Equals("1") && !addKey.Equals("1"))
                            {
                                addColumn = columnMasterEntity.Replace("columnName", columnName).Replace("columnType", columnType).Replace("cdesc", adddesc);
                            }
                            else if (!masterKey.Equals("1") && addKey.Equals("1"))
                            {
                                addColumn = columnAddEntity.Replace("columnName", columnName).Replace("columnType", columnType).Replace("cdesc", adddesc);
                            }
                            else
                            {
                                //加入为空的标识
                                if (addnull.Equals("1") && !columnType.Equals("string"))
                                {
                                    columnName = " ? " + columnName;
                                }
                                addColumn = columnEntity.Replace("columnName", columnName).Replace("columnType", columnType).Replace("cdesc", adddesc);
                            }
                        }

                        columnStr += "        " + addColumn + " \r\n";
                    }

                    if (!string.IsNullOrEmpty(columnStr))
                    {
                        modelStr = modelStr.Replace("namespaceName", namespacestr).Replace("className", filename).Replace("columnStr", columnStr);
                        Encoding code = Encoding.GetEncoding("UTF-8");
                        //判断文件路劲是否以\结束
                        int index = savepath.LastIndexOf(@"\");

                        if (index < savepath.Length - 1)
                        {
                            savepath += @"\";
                        }

                        string path = savepath + csname;
                        string msg = txtmsg.Text.Trim();
                        // 生成文件是否存在 不存在就创建 否者提示文件已存在
                        if (!File.Exists(path))
                        {
                            sw = new StreamWriter(path, false, code);
                            sw.Write(modelStr);
                            sw.Flush();
                            msg += "\r\n" + path + " 文件生成成功!";
                        }
                        else
                        {
                            msg += "\r\n" + path + " 文件生成失败：文件已存在!";
                        }
                        txtmsg.Text = msg.Trim();
                    }
                }
                else
                {
                    string msg = txtmsg.Text.Trim();
                    msg += "\r\n 表：" + tableName + "字段为空或者不存在!";
                }
            }
            catch (Exception ex)
            {
                string msg = txtmsg.Text.Trim();
                txtmsg.Text = msg + "\r\n" + ex.Message;
                throw;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 表字段类型转换成Asp.Net数据类型
        /// </summary>
        /// <param name="name">数据库字段类型名称</param>
        /// <returns></returns>
        private string GetColumnType(string name)
        {
            string outstr = string.Empty;
            if (name == "int" || name == "bigint" || name == "tinyint" || name == "smallint")
            {
                return " int ";
            }
            else if (name == "nvarchar" || name == "char" || name == "nchar" || name == "varchar" || name == "ntext" || name == "text")
            {
                return "string";
            }
            else if (name == "decimal" || name == "numeric" || name == "smallmoney" || name == "smallmoney" || name == "money")
            {

                return "decimal";
            }
            else if (name == "float")
            {
                return "float";
            }
            else if (name == "datetime")
            {
                return "DateTime";

            }
            else if (name == "binary" || name == "varbinary" || name == "image")
            {
                return "byte[]";

            }
            else if (name == "bit")
            {
                return "Boolean";
            }
            return outstr;

        }

        /// <summary>
        /// 获取单表生所有字段名称和类型
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        private DataTable GetTableColumn(string TableName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(txtdb.Text))
            {
                con.Open();
                string sql = @"
 	            SELECT (case when a.colorder=1 then d.name else null end) 表名,  
                a.colorder 字段序号,
                a.name columnName,
                (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 1 else 0 end) systemAdd, 
                (case when (SELECT count(*) FROM sysobjects  
                WHERE (name in (SELECT name FROM sysindexes  
                WHERE (id = a.id) AND (indid in  
                (SELECT indid FROM sysindexkeys  
                WHERE (id = a.id) AND (colid in  
                (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  
                AND (xtype = 'PK'))>0 then 1 else 0 end) masterkey,
                b.name columnType,
                a.length 占用字节数,  
                COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,  
                isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,
                (case when a.isnullable=1 then 1 else 0 end) cnull,  
                isnull(e.text,'') 默认值,
                isnull(g.[value], ' ') AS  cdesc
                FROM  syscolumns a 
                left join systypes b on a.xtype=b.xusertype  
                inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' 
                left join syscomments e on a.cdefault=e.id  
                left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
                left join sys.extended_properties f on d.id=f.class and f.minor_id=0
                WHERE d.name='{0}' 
                order by a.id,a.colorder
                ";
                sql = string.Format(sql, TableName);
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 获取数据库所有的表名
        /// </summary>
        /// <returns></returns>
        private DataTable GetDBTable()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(txtdb.Text))
            {
                con.Open();
                string sql = "SELECT Name as TableName FROM SysObjects Where XType='U' ORDER BY Name";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            return dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //获取数据库所有表名
            comboBox1.DataSource = GetDBTable();
            comboBox1.DisplayMember = "TableName";
            comboBox1.ValueMember = "TableName";
            if (!string.IsNullOrEmpty(txtdb.Text))
            {
                string sbustr = txtdb.Text.Substring(txtdb.Text.ToLower().IndexOf("Catalog=".ToLower())).Replace("Catalog=", "");
                DBname = sbustr.Substring(0, sbustr.IndexOf(";".ToLower()));
            }
        }

    }
}
