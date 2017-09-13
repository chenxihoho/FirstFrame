using FirstFrame.DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.DBHelper
{
    public class SQLHelper
    {
        #region 将List类转换为SQL的IN查询条件值
        public static string ConvertToInSyntax<T>(IList<T> List, string KeyName)
        {
            string NullParamString = "''";
            try
            {
                if (List.Count == 0) return NullParamString;

                var _StringBuilder = new StringBuilder();
                foreach (dynamic _T in List)
                {
                    _StringBuilder.Append("'");
                    _StringBuilder.Append(_T.GetType().GetProperty(KeyName).GetValue(_T, null).ToString());
                    _StringBuilder.Append("'");
                    _StringBuilder.Append(",");
                }
                if (_StringBuilder.Length > 0) _StringBuilder.Remove(_StringBuilder.Length - 1, 1);
                return _StringBuilder.ToString();
            }
            catch (Exception)
            {
                return NullParamString;
            }
        }
        #endregion
        #region 为指定的表创建字段
        public static int AddField(DbBase _DbBase, string TableName, string FieldName, string FieldType)
        {
            //首先判断字段是否存在
            string SqlString = string.Format(@"if NOT EXISTS(select * From sysobjects,syscolumns where sysobjects.id = syscolumns.id and sysobjects.name = '{0}' and syscolumns.name = '{1}')
                                               ALTER TABLE {0} ADD {1} {2}", TableName, FieldName, FieldType);
            return _DbBase.Execute(SqlString);
        }
        #endregion
        #region 创建表
        protected void CreateTable()
        {
            string sqlconn = "server=.; database=dbName; uid=sa; pwd=sa";//连接字
            //首先判断表是否存在，在创建表
            string strSql = string.Format(@"if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[表名]') 
                                            and OBJECTPROPERTY(id, N'IsUserTable') = 1)
                                            drop table [dbo].[表名]
                                            create table 表名 (id int primary key ,name varchar(100) not null )");
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(sqlconn))
            {
                conn.Open();
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSql, conn))
                {
                    cmd.ExecuteNonQuery();//执行
                }
            }
        }
        #endregion
    }
}
