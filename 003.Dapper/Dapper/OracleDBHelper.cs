using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;


namespace Web.Utility.sqlHelper
{


    #region Data Access Base Class
    /// <summary>	 
    /// Data Access Base Class
    /// </summary>
    public abstract class OracleDbHelper
    {
        protected static string connectionString = ConfigHelper.DBConnection;

        #region const
        protected const string str_isExistJBRWPackage = @"
        select  count(*)
        from   user_objects  
        where  object_type   = 'PACKAGE' and object_name= 'RASEI' 
        ";
        protected const string str_jbrwpackage = @"
            create or replace package RASEI is
              procedure GetPager
              (
                     row_from int,          /*行开始*/
                     row_to int,            /*行结束*/
                     p_SqlSelect varchar2,    /*查询语句,含排序部分*/
                     p_OutRecordCount out int,/*返回总记录数*/
                     p_OutCursor out sys_refcursor
              );
              procedure PROC_INSERT_DATA(
              TABLE_NAME nVARCHAR2,
              EXISTS_CONDITIONS nVARCHAR2,
              UPDATE_SQL nVARCHAR2,
              INSERT_SQL nVARCHAR2
              );

            end;";
        protected const string str_jbrwpackage_body = @"
            create or replace package body RASEI
                as
                /*分页查询*/
                  procedure GetPager
                  (
                         row_from int,          /*行开始*/
                         row_to int,            /*行结束*/
                         p_SqlSelect varchar2,    /*查询语句,含排序部分*/
                         p_OutRecordCount out int,/*返回总记录数*/
                         p_OutCursor out sys_refcursor
                  )
                as
                    v_sql varchar2(4000);
                    v_count int;
                begin
                  /*取记录总数*/
                  v_sql := 'select count(1) from (' || p_SqlSelect || ')';
                  execute immediate v_sql into v_count;
                  p_OutRecordCount := v_count;
                  /*执行分页查询*/
                  

                  v_sql := 'SELECT *
                            FROM (
                                  SELECT A.*, rownum rn
                                  FROM  ('|| p_SqlSelect ||') A
                                  WHERE rownum <= '|| to_char(row_to) || '
                                 ) B
                            WHERE rn >= ' || to_char(row_from) ;
                            /*注意对rownum别名的使用,第一次直接用rownum,第二次一定要用别名rn*/

                  OPEN p_OutCursor FOR  v_sql;
                end ;

                /*基础数据增删改*/
                procedure PROC_INSERT_DATA(
                TABLE_NAME nVARCHAR2,
                EXISTS_CONDITIONS nVARCHAR2,
                UPDATE_SQL nVARCHAR2,
                INSERT_SQL nVARCHAR2)
                is
                  ROW_CN NUMBER;
                  STR_SQL VARCHAR2(1000);
                begin
                  if(length(EXISTS_CONDITIONS)>0) then
                    STR_SQL := 'SELECT COUNT(1)  FROM '||TABLE_NAME||' WHERE  '||EXISTS_CONDITIONS ;
                    DBMS_OUTPUT.put_line(STR_SQL);
                    EXECUTE IMMEDIATE STR_SQL into ROW_CN;
                    IF(ROW_CN>0) THEN
                      STR_SQL := 'UPDATE '|| TABLE_NAME||' '||UPDATE_SQL || ' WHERE  '||EXISTS_CONDITIONS ;
                    ELSE
                      STR_SQL :='INSERT INTO '|| TABLE_NAME||INSERT_SQL;
                    END IF;
                  elsif(length(UPDATE_SQL)>0) then
                     STR_SQL := 'UPDATE '|| TABLE_NAME||' '||UPDATE_SQL;
                  elsif(length(INSERT_SQL)>0) then
                     STR_SQL := 'INSERT INTO '|| TABLE_NAME||INSERT_SQL;
                  end if;
                   DBMS_OUTPUT.put_line(STR_SQL);
                   EXECUTE IMMEDIATE STR_SQL;
                end;
               end ;
            ";
        private static void check_JBRWPackage()
        {
            if (GetSingle(str_isExistJBRWPackage).ToString() == "0")
            {
                ExecuteSql(str_jbrwpackage);
                ExecuteSql(str_jbrwpackage_body);
            }
        }
        #endregion

        public OracleDbHelper()
        {

        }

        #region 参数处理
        /// <summary>
        /// 参数替换:得数参数时,各参数不得相互包涵
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pas">参数</param>
        /// <returns></returns>
        public static string INIT_PARAMS_SQL(List<DParam> pas, string sql)
        {
            if (string.IsNullOrEmpty(sql.Trim())) return "";

            sql = sqlLeach(sql.Trim()) + " ";
            foreach (DParam pa in pas)
            {
                string paramName = pa.paramName.Trim();
                string cEnd = " ";
                int pos = sql.IndexOf(":" + paramName + cEnd);
                if (pos == -1)
                {
                    cEnd = ",";
                    pos = sql.IndexOf(":" + paramName + cEnd);
                }
                if (pos == -1)
                {
                    cEnd = ")";
                    pos = sql.IndexOf(":" + paramName + cEnd);
                }
                if (pos == -1)
                {
                    cEnd = "'";
                    pos = sql.IndexOf(":" + paramName + cEnd);
                }

                if (pos != -1)
                {
                    string v = "";
                    switch ((OracleType)pa.fieldType)
                    {
                        case OracleType.Double:
                        case OracleType.Int16:
                        case OracleType.Int32:
                        case OracleType.Float:
                        case OracleType.Number:
                        case OracleType.UInt16:
                        case OracleType.UInt32:
                            v = DBHelper.setNumFiled(pa.paramValue);
                            break;
                        case OracleType.DateTime:
                            v = DBHelper.setOracleDateTime(pa.paramValue);
                            break;
                        default:
                            v = DBHelper.setVarcharFiled(pa.paramValue);
                            break;
                    }
                    sql = sql.Replace(":" + paramName + cEnd, v + cEnd) + " ";
                }

            }
            return sql.Trim();
        }
        /// <summary>
        /// 参数替换:得数参数时,各参数不得相互包涵
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pas">参数</param>
        /// <returns></returns>
        public static PARAMS_SQL INIT_PARAMS_SQL(string sql, List<DParam> pas)
        {
            if (string.IsNullOrEmpty(sql.Trim())) return null;

            PARAMS_SQL ps = new PARAMS_SQL();
            ps.strSQl = sqlLeach(sql.Trim()) + " ";
            List<OracleParameter> dps = new List<OracleParameter>();

            foreach (DParam pa in pas)
            {
                pa.paramName = pa.paramName.Trim();
                OracleParameter np = new OracleParameter(pa.paramName, pa.fieldType);
                np.Value = pa.paramValue;
                dps.Add(np);

            }
            ps.strSQl = ps.strSQl.Trim();
            ps.Params = dps.ToArray();
            return ps;
        }
        #endregion

        #region 公用方法

        private static string sqlLeach(string strSql)
        {
            return strSql.Replace("\r\n", " ").Replace("\n", " ").Replace("&", "\\&").TrimEnd();//.TrimEnd(';');
        }
        /// <summary>
        /// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            return BuildQueryCommand(null, storedProcName, parameters);
        }
        /// <summary>
        /// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleTransaction sqlTrans, OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection, sqlTrans);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters) command.Parameters.Add(parameter);
            return command;
        }

        /// <summary>
        /// 创建 OracleCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand 对象实例</returns>
        private static OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new OracleParameter("ReturnValue",
                OracleType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = sqlLeach(cmdText);
                if (trans != null) cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;//cmdType;
                if (cmdParms != null)
                {
                    foreach (OracleParameter parm in cmdParms)
                    {
                        if((parm.Value==null|| parm.Value.ToString()=="") && (parm.DbType == DbType.Date || parm.DbType == DbType.DateTime || parm.DbType == DbType.DateTime2))
                        { 
                            parm.DbType= DbType.String;
                        }
                        if ((parm.Value==null|| parm.Value.ToString().Trim().Replace("'","")=="")&& (parm.DbType == DbType.Decimal || parm.DbType == DbType.Double || parm.DbType == DbType.Int16 || parm.DbType == DbType.Int32 || parm.DbType == DbType.Int64 || parm.DbType == DbType.UInt16|| parm.DbType == DbType.UInt32|| parm.DbType == DbType.UInt64))
                        {
                            parm.Value = 0;
                        }
                        cmd.Parameters.Add(parm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static OracleConnection initDBConnect(string connectionString)
        {
            return new OracleConnection(connectionString); ;
        }
        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public static OracleConnection initDBConnect()
        {
            return new OracleConnection(connectionString); ;
        }
        /// <summary>
        /// 初始化事务,并打开数据连接
        /// </summary>
        /// <returns></returns>
        public static OracleTransaction initTranc()
        {
            OracleConnection connection = initDBConnect();
            if (connection.State == System.Data.ConnectionState.Closed) connection.Open();
            return connection.BeginTransaction();
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="connection"></param>
        public static void ConnectionClose(OracleConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Open) connection.Close();
        }
        #endregion

        #region Exists
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <param name="cmdParms">脚本参数</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(OracleTransaction sqlTrans, string SQLString, params OracleParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, hasInputTransaction == true ? sqlTrans.Connection : initDBConnect(), sqlTrans, sqlLeach(SQLString), cmdParms);
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false) ConnectionClose(cmd.Connection);
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            return GetSingle(null, SQLString, new OracleParameter[] { });
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="SQLString">脚本</param>
        /// <returns></returns>
        public static object GetSingle(OracleTransaction sqlTrans, string SQLString)
        {
            return GetSingle(sqlTrans, SQLString, new OracleParameter[] { });
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="SQLString">脚本</param>
        /// <param name="sqlParams">脚本参数</param>
        /// <returns></returns>
        public static object GetSingle(string SQLString, List<OracleParameter> sqlParams)
        {
            return GetSingle(null, SQLString, sqlParams);
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="SQLString">脚本</param>
        /// <param name="sqlParams">脚本参数</param>
        /// <returns></returns>
        public static object GetSingle(OracleTransaction sqlTrans, string SQLString, List<OracleParameter> sqlParams)
        {
            return GetSingle(sqlTrans, SQLString, sqlParams.ToArray());
        }


        public static bool Exists(string strSql)
        {
            return Exists(null, strSql, new OracleParameter[] { });
        }
        public static bool Exists(string strSql, List<OracleParameter> cmdParms)
        {
            return Exists(null, strSql, cmdParms.ToArray());
        }
        public static bool Exists(OracleTransaction sqlTrans, string strSql, List<OracleParameter> cmdParms)
        {
            return Exists(sqlTrans, strSql, cmdParms.ToArray());
        }
        public static bool Exists(OracleTransaction sqlTrans, string strSql, params OracleParameter[] cmdParms)
        {
            object obj = GetSingle(sqlTrans, strSql, cmdParms);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) return false;
            else return true;
        }

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static string ExecuteSqlTran(ArrayList SQLStringList)
        {
            string RetrunValue = "";
            string strsql = "";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                OracleTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = sqlLeach(strsql);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    tx.Rollback();
                    RetrunValue = strsql + "\n" + E.ToString();
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
            return RetrunValue;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static string ExecuteSqlTran_ReTrim(ArrayList SQLStringList)
        {
            string RetrunValue = "";
            string strsql = "";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                try
                {

                    cmd.Connection = connection;
                    OracleTransaction tx = connection.BeginTransaction();
                    cmd.Transaction = tx;
                    try
                    {
                        for (int n = 0; n < SQLStringList.Count; n++)
                        {
                            strsql = SQLStringList[n].ToString();
                            if (strsql.Trim().Length > 1)
                            {
                                cmd.CommandText = sqlLeach(strsql);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        tx.Rollback();
                        RetrunValue = E.ToString();
                    }
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    RetrunValue = strsql + "\n" + E.ToString();
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
            return RetrunValue;
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                try
                {
                    cmd = new OracleCommand(sqlLeach(SQLString), connection);
                    System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@content", OracleType.NVarChar);
                    myParameter.Value = content;
                    cmd.Parameters.Add(myParameter);
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入BLOB的字段
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">字节,数据库的字段类型为BLOB的情况</param>
        /// <returns></returns>
        public static int ExecuteSqlInsertBLOB(string strSQL, byte[] fs)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                OracleTransaction tx = connection.BeginTransaction();
                try
                {
                    //cmd = new OracleCommand(strSQL, connection);
                    //启动一个事务
                    cmd = connection.CreateCommand();
                    cmd.Transaction = tx;
                    //这里是关键，他定义了一个命令对象的t-sql语句，通过dmbs_lob来创建一个零时对象，这个对象的类型为blob，并存放在变量xx中，然后将xx的值付给外传参数tmpblob
                    cmd.CommandText = "declare xx blob; begin dbms_lob.createtemporary(xx, false, 0); :tempblob := xx; end;";
                    //构造外传参数对象，并加入到命令对象的参数集合中
                    cmd.Parameters.Add(new OracleParameter("tempblob", OracleType.Blob)).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    //构造OracleLob对象，他的值为tmpblob外传参数的值
                    OracleLob tempLob = (OracleLob)cmd.Parameters[0].Value;
                    //指定tempLob的访问模式，并开始操作二进制数据
                    tempLob.BeginBatch(OracleLobOpenMode.ReadWrite);
                    //将二进制流byte数组集合写入到tmpLob里
                    tempLob.Write(fs, 0, fs.Length);
                    tempLob.EndBatch();
                    cmd.Parameters.Clear();
                    //
                    cmd.CommandText = sqlLeach(strSQL);
                    cmd.CommandType = CommandType.Text;
                    //创建存储过程的Blob参数，并指定Blob参数的值为tempLob（表示服务器上大型对象二进制数据类型），并将Blob参数加入到command对象的参数集合里
                    OracleParameter tmp_blob = new OracleParameter(":fs", OracleType.Blob);
                    tmp_blob.Value = tempLob;
                    cmd.Parameters.Add(tmp_blob);
                    int rows = cmd.ExecuteNonQuery();
                    tx.Commit();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    tx.Rollback();
                    cmd.Dispose();
                    connection.Close();
                    throw new Exception(E.Message + strSQL);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strSQL)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand(sqlLeach(strSQL), connection);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }
        }

        #endregion


        #region 存储过程操作
        /// <summary>
        /// 执行存储过程 返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="cmdParms">存储过程参数</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ReaderProcedure(OracleTransaction sqlTrans, string storedProcName, IDataParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            OracleDataReader returnReader;
            OracleCommand cmd = BuildQueryCommand(sqlTrans, hasInputTransaction == true ? sqlTrans.Connection : initDBConnect(), sqlLeach(storedProcName), cmdParms);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (hasInputTransaction)
                    returnReader = cmd.ExecuteReader();
                else
                    returnReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Dispose();
                return returnReader;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (hasInputTransaction == false) ConnectionClose(cmd.Connection);
                cmd.Dispose();
            }
        }
        /// <summary>
        /// 执行存储过程 返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="cmdParms">存储过程参数</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ReaderProcedure(string storedProcName, IDataParameter[] cmdParms)
        {
            return ReaderProcedure(null, storedProcName, cmdParms);
        }

        public static bool RunProcedure(OracleTransaction sqlTrans, string SQLString)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            if (hasInputTransaction == false) sqlTrans = initTranc();
            OracleCommand cmd = new OracleCommand();
            cmd.Transaction = sqlTrans;
            cmd.Connection = sqlTrans.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            bool isSuccess = false;
            try
            {
                cmd.CommandText = sqlLeach(SQLString);
                cmd.ExecuteNonQuery();
                isSuccess = true;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false)
                {
                    if (isSuccess == false) sqlTrans.Rollback();
                    else sqlTrans.Commit();
                    ConnectionClose(cmd.Connection);
                }
                cmd.Dispose();
            }
            return isSuccess;
        }
        public static bool RunProcedure(string SQLString)
        {
            return RunProcedure(null, SQLString);
        }
        public static bool RunProcedure(ArrayList SQLStrings)
        {
            OracleTransaction tranc = initTranc();
            bool isSuccess = false;
            try
            {
                foreach (string sql in SQLStrings)
                {
                    isSuccess = RunProcedure(tranc, sql);
                    if (isSuccess == false) break;
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            finally
            {
                if (isSuccess) tranc.Commit();
                else tranc.Rollback();
                ConnectionClose(tranc.Connection);
            }
            return isSuccess;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="procName">存储过程名</param>
        /// <param name="cmdParms">存储过程参数</param>
        /// <returns>执行结果:成功,失败</returns>
        public static bool RunProcedure(string procName, List<OracleParameter> cmdParms)
        {
            return RunProcedure(null, procName, cmdParms.ToArray());
        }
        public static bool RunProcedure(OracleTransaction sqlTrans, string procName, IDataParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            if (hasInputTransaction == false) sqlTrans = initTranc();
            bool isSuccess = false;
            try
            {
                using (OracleCommand cmd = BuildQueryCommand(sqlTrans, sqlTrans.Connection, procName, cmdParms))
                {
                    cmd.ExecuteNonQuery();
                    isSuccess = true;
                }
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false)
                {
                    if (isSuccess == false) sqlTrans.Rollback();
                    else sqlTrans.Commit();
                    ConnectionClose(sqlTrans.Connection);
                }
            }
            return isSuccess;
        }

        public bool RunProcedure(OracleTransaction sqlTrans, List<PARAMS_SQL> params_sql)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            if (hasInputTransaction == false) sqlTrans = initTranc();
            bool isSuccess = false;
            try
            {
                foreach (PARAMS_SQL ps in params_sql)
                {
                    isSuccess = RunProcedure(sqlTrans, ps.strSQl, ps.SqlParams);
                    if (isSuccess == false) break;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (hasInputTransaction == false)
                {
                    if (isSuccess) sqlTrans.Commit();
                    else sqlTrans.Rollback();
                    ConnectionClose(sqlTrans.Connection);
                }
            }
            return isSuccess;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sqlTrans">事务</param>
        /// <param name="procName">存储过程名</param>
        /// <param name="cmdParms">存储过程参数</param>
        /// <returns>执行结果:成功,失败</returns>
        public static DataSet RunProcedure(OracleTransaction sqlTrans, string storedProcName, IDataParameter[] cmdParms, string tableName)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            if (hasInputTransaction == false) sqlTrans = initTranc();
            DataSet Dset = new DataSet();
            bool isSuccess = false;
            try
            {
                using (OracleCommand cmd = BuildQueryCommand(sqlTrans, sqlTrans.Connection, storedProcName, cmdParms))
                {
                    OracleDataAdapter ODAdp = new OracleDataAdapter(cmd);
                    if (string.IsNullOrEmpty(tableName)) ODAdp.Fill(Dset, "PROJECT");
                    else ODAdp.Fill(Dset, tableName);
                    ODAdp.Dispose();
                }
                isSuccess = true;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false)
                {
                    if (isSuccess == false) sqlTrans.Rollback();
                    else sqlTrans.Commit();
                    ConnectionClose(sqlTrans.Connection);
                }
            }
            return Dset;
        }

        public static DataSet RunProcedure(string storedProcName, List<OracleParameter> parameters, string tableName)
        {
            return RunProcedure(null, storedProcName, parameters.ToArray(), tableName);
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            return RunProcedure(null, storedProcName, parameters, tableName);
        }
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            return RunProcedure(null, storedProcName, parameters, null);
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    int result;
                    connection.Open();
                    OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    connection.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OracleParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleTransaction trans = conn.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, sqlLeach(cmdText), cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                        trans.Commit();

                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }
        public static void ExecuteSqlTran(Hashtable SQLStringList1, Hashtable SQLStringList2)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleTransaction trans = conn.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList1)
                        {
                            //同一个model多条记录的批量操作,sql重复
                            string cmdText = myDE.Value.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])myDE.Key;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList2)
                        {
                            //同一个model多条记录的批量操作,sql重复
                            string cmdText = myDE.Value.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])myDE.Key;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }
        public static void ExecuteSqlTran(Hashtable SQLStringList, bool isReversed)
        {
            if (isReversed)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleTransaction trans = conn.BeginTransaction())
                    {
                        OracleCommand cmd = new OracleCommand();
                        try
                        {
                            //循环
                            foreach (DictionaryEntry myDE in SQLStringList)
                            {
                                //同一个model多条记录的批量操作,sql重复
                                string cmdText = myDE.Value.ToString();
                                OracleParameter[] cmdParms = (OracleParameter[])myDE.Key;
                                PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            cmd.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
            else
                ExecuteSqlTran(SQLStringList);
        }
        #endregion

        #region Query 执行查询语句，返回DataSet
        public static DataSet Query(OracleTransaction sqlTrans, string SQLString, params OracleParameter[] cmdParms)
        {
            return Query(sqlTrans, SQLString, "Project", cmdParms);
        }
        public static DataSet Query(OracleTransaction sqlTrans, string SQLString, string strTableName, params OracleParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            try
            {
                PrepareCommand(cmd, hasInputTransaction == true ? sqlTrans.Connection : initDBConnect(), sqlTrans, sqlLeach(SQLString), cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    da.Fill(ds, strTableName);
                    cmd.Parameters.Clear();
                }

            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false) ConnectionClose(cmd.Connection);
                cmd.Dispose();
            }
            return ds;
        }
        public static DataSet Query(string SQLString)
        {
            return Query(null, SQLString, new OracleParameter[] { });
        }
        public static DataSet Query(OracleTransaction sqlTrans, string SQLString)
        {
            return Query(sqlTrans, SQLString, new OracleParameter[] { });
        }
        public static DataSet Query(string SQLString, List<OracleParameter> cmdParms)
        {
            return Query(null, SQLString, cmdParms.ToArray());
        }
        public static DataSet Query(string SQLString, params OracleParameter[] cmdParms)
        {
            return Query(null, SQLString, cmdParms);
        }
        public static DataSet Query(OracleTransaction sqlTrans, string SQLString, List<OracleParameter> cmdParms)
        {
            return Query(sqlTrans, SQLString, cmdParms.ToArray());
        }
        public static DataSet Query(OracleTransaction sqlTrans, string SQLString, string strTableName, List<OracleParameter> cmdParms)
        {
            return Query(sqlTrans, SQLString, strTableName, cmdParms.ToArray());
        }
        #endregion

        #region QueryTable 执行查询语句，返回DataTable
        public static DataTable QueryTable(OracleTransaction sqlTrans, string SQLString, params OracleParameter[] cmdParms)
        {
            return QueryTable(sqlTrans, SQLString, "", cmdParms);

        }
        public static DataTable QueryTable(OracleTransaction sqlTrans, string SQLString, string strTableName, params OracleParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            OracleCommand cmd = new OracleCommand();
            DataTable dt = new DataTable();
            try
            {
                PrepareCommand(cmd, hasInputTransaction == true ? sqlTrans.Connection : initDBConnect(), sqlTrans, sqlLeach(SQLString), cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    da.Fill(dt);
                    if (!string.IsNullOrEmpty(strTableName) && dt != null) dt.TableName = strTableName;
                }

            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false) ConnectionClose(cmd.Connection);
                cmd.Dispose();
            }
            return dt;
        }
        public static DataTable QueryTable(string SQLString)
        {
            return QueryTable(null, SQLString, new OracleParameter[] { });
        }
        public static DataTable QueryTable(OracleTransaction sqlTrans, string SQLString)
        {
            return QueryTable(sqlTrans, SQLString, new OracleParameter[] { });
        }
        public static DataTable QueryTable(string SQLString, List<OracleParameter> cmdParms)
        {
            return QueryTable(null, SQLString, cmdParms.ToArray());
        }
        public static DataTable QueryTable(string SQLString, params OracleParameter[] cmdParms)
        {
            return QueryTable(null, SQLString, cmdParms);
        }
        public static DataTable QueryTable(OracleTransaction sqlTrans, string SQLString, List<OracleParameter> cmdParms)
        {
            return QueryTable(sqlTrans, SQLString, cmdParms.ToArray());
        }
        public static DataTable QueryTable(OracleTransaction sqlTrans, string SQLString, string strTableName, List<OracleParameter> cmdParms)
        {
            return QueryTable(sqlTrans, SQLString, strTableName, cmdParms.ToArray());
        }
        #endregion

        #region PagingQuery 执行查询语句，返回DataTable
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="page_size">每页显示行数</param>
        /// <param name="page_no">当前第几页</param>
        /// <returns>DataSet</returns>
        public static DataTable PagingQuery(string strSql, int page_size, int page_no)
        {
            int row_from = (page_no - 1) * page_size + 1, row_to = page_no * page_size;
            return PagingQuery(row_from, row_to, strSql);

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="page_size">开始行</param>
        /// <param name="page_no">结束行</param>
        /// <returns>DataSet</returns>
        public static DataTable PagingQuery(int row_from, int row_to, string strSql)
        {
            if (row_to == 0)
            {
                DataTable dt = QueryTable(strSql);
                if (dt != null) dt.TableName = dt.Rows.Count.ToString();
                return dt;
            }
            else
            {
                OracleParameter[] param = new OracleParameter[]{
                    new OracleParameter("row_from", OracleType.Number),
                    new OracleParameter("row_to", OracleType.Number),
                    new OracleParameter("p_SqlSelect", OracleType.VarChar),
                    new OracleParameter("p_OutRecordCount", OracleType.Number),
                    new OracleParameter("p_OutCursor", OracleType.Cursor)
                };
                param[0].Value = row_from;
                param[1].Value = row_to;
                param[2].Value = strSql;
                param[3].Direction = ParameterDirection.Output;
                param[4].Direction = ParameterDirection.Output;
                check_JBRWPackage();
                DataSet ds = RunProcedure("rasei.GetPager", param, "searcher");
                string rowCount = param[3].Value.ToString();
                if (rowCount == "0") return null;
                else
                {
                    ds.Tables[0].TableName = rowCount;
                    return ds.Tables[0];
                }
            }

        }
        #endregion

        #region ExecuteSql 执行增删改脚本方法
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(OracleTransaction sqlTrans, string SQLString, params OracleParameter[] cmdParms)
        {
            bool hasInputTransaction = sqlTrans != null;//是否传入事务
            OracleCommand cmd = new OracleCommand();
            DataTable dt = new DataTable();
            int rows = -1;
            try
            {
                PrepareCommand(cmd, hasInputTransaction == true ? sqlTrans.Connection : initDBConnect(), sqlTrans, sqlLeach(SQLString), cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }

            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (hasInputTransaction == false) ConnectionClose(cmd.Connection);
                cmd.Dispose();
            }
            return rows;

        }
        public static int ExecuteSql(string SQLString)
        {
            return ExecuteSql(null, SQLString, new OracleParameter[] { });
        }
        public static int ExecuteSql(OracleTransaction sqlTrans, string SQLString)
        {
            return ExecuteSql(sqlTrans, SQLString, new OracleParameter[] { });
        }
        public static int ExecuteSql(string SQLString, List<OracleParameter> cmdParms)
        {
            return ExecuteSql(null, SQLString, cmdParms.ToArray());
        }
        public static int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {
            return ExecuteSql(null, SQLString, cmdParms);
        }
        public static int ExecuteSql(OracleTransaction sqlTrans, string SQLString, List<OracleParameter> cmdParms)
        {
            return ExecuteSql(sqlTrans, SQLString, cmdParms.ToArray());
        }
        #endregion

        //---------单表操作-------------------
        #region 获取表所有列信息
        /// <summary>
        /// 获取表所有列信息
        /// </summary>
        /// <param name="strTbName">表名</param>
        /// <returns>COLUMN_NAME,DATA_TYPE,DATA_LENGTH,NULLABLE,constraint_type</returns>
        public static DataTable getTabelColsInfos(string strTbName)
        {
            try
            {
                string cacheKey = "TCOLS_" + strTbName;
                object obj = DataCache.GetCache(cacheKey);
                DataTable rtn = null;
                if (obj == null)
                {
                    //获取表所有列
                    string strSql = string.Format(@"
                    select 
                       T0.COLUMN_NAME,T0.DATA_TYPE,T0.DATA_LENGTH,T0.NULLABLE,to_char(substr(wmsys.wm_concat(T2.constraint_type),0,4000)) constraint_type
                    from user_tab_columns T0
                    LEFT JOIN user_cons_columns T1
                    ON T1.column_name =T0.column_name 
                    AND T1.table_name=T0.table_name
                    left join user_constraints T2
                    ON T1.constraint_name =T2.constraint_name 
                    AND T1.table_name=T2.table_name
                    AND T1.owner=T2.owner
                    where T0.table_name={0})
                    group by T0.COLUMN_NAME,T0.DATA_TYPE,T0.DATA_LENGTH,T0.NULLABLE",
                        DBHelper.setVarcharFiled(strTbName.ToUpper().Trim())
                        );
                    rtn = QueryTable(strSql);
                    DataCache.SetCache(cacheKey, rtn);
                }
                else rtn = (DataTable)obj;
                return rtn;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取表所有列信息
        /// </summary>
        /// <param name="strTbName">表名</param>
        /// <returns>所有列名字符串,以逗号分隔</returns>
        public static string getTabelCols(string strTbName)
        {
            try
            {
                string cacheKey = "SCOLS_" + strTbName;
                object obj = DataCache.GetCache(cacheKey);
                if (obj == null)
                {
                    //获取表所有列
                    string strSql = string.Format("select to_char(substr(wmsys.wm_concat(T.column_name),0,4000)) from user_tab_columns T where T.table_name={0}"
                         , DBHelper.setVarcharFiled(strTbName.ToUpper().Trim())
                        );
                    obj = OracleDbHelper.GetSingle(strSql);
                    DataCache.SetCache(cacheKey, obj);
                }
                return (obj == null ? "" : obj.ToString());
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region GetData 获取表或视图数据
        private static string getCValue(ConditionType CType, object CValue)
        {
            switch (CType)
            {
                case ConditionType.num:
                    return DBHelper.setNumFiled(CValue);
                case ConditionType.date:
                    return DBHelper.setOracleDateTime(CValue);
                default:
                    return DBHelper.setVarcharFiled(CValue);
            }
        }
        /// <summary>
        /// 查询数据表或View数据,支持分页
        /// </summary>
        /// <param name="CQuery">查询条件</param>
        /// <param name="strTbName">表或View名称(不区分大小写)</param>
        /// <returns>查询结果</returns>
        public static DataTable getData(List<CSearchCondition> CQuery, string strTbName)
        {
            return getData(CQuery, strTbName, "T.*");
        }
        /// <summary>
        /// 解析查询条件
        /// </summary>
        /// <param name="CQuery">查询条件</param>
        /// <param name="strTbName">表名</param>
        /// <returns>查询条件</returns>
        public static string getCSearchCondition(List<CSearchCondition> CQuery, string strTbName)
        { 
            int page_size=0,   page_no=0;
            string strOrderBy = "";
            return getCSearchCondition(CQuery, strTbName, out page_size, out page_no,out strOrderBy);
        }
        /// <summary>
        /// 解析查询条件
        /// </summary>
        /// <param name="CQuery">查询条件</param>
        /// <param name="strTbName">表名</param>
        /// <param name="page_size">分页行数</param>
        /// <param name="page_no">页码</param>
        /// <param name="strOrderBy">排序</param>
        /// <returns>查询条件</returns>
        public static string getCSearchCondition(List<CSearchCondition> CQuery, string strTbName, out int page_size, out int page_no, out string strOrderBy)
        {
            string strCols = getTabelCols(strTbName).ToUpper();
            string strWhere = "";
            strOrderBy = "";
            page_size = 0;
            page_no = 0;//分页查询
            string cKey, cValue = "";
            if (CQuery != null)
            {
                foreach (CSearchCondition condition in CQuery)
                {

                    cKey = string.IsNullOrEmpty(condition.Key) ? "" : condition.Key.ToUpper();
                    if (!string.IsNullOrEmpty(cKey) && Common.inStr(strCols, cKey, ',') && condition.value != null)
                    {
                        if (condition.qCond != queryCondition.exists && condition.qCond != queryCondition.nexists)
                        {
                            cValue = getCValue(condition.type, condition.value);
                        }

                        #region 添加查询条件
                        switch (condition.qCond)
                        {
                            case queryCondition.neq:
                                strWhere += string.Format(" and T.{0}!={1}", cKey, cValue);
                                break;
                            case queryCondition.eq:
                                strWhere += string.Format(" and T.{0}={1}", cKey, cValue);
                                break;

                            case queryCondition.gt:
                                strWhere += string.Format(" and T.{0}>{1}", cKey, cValue);
                                break;
                            case queryCondition.gte:
                                strWhere += string.Format(" and T.{0}>={1}", cKey, cValue);
                                break;

                            case queryCondition.lt:
                                strWhere += string.Format(" and T.{0}<{1}", cKey, cValue);
                                break;
                            case queryCondition.lte:
                                strWhere += string.Format(" and T.{0}<={1}", cKey, cValue);
                                break;

                            case queryCondition.Like:
                                strWhere += string.Format(" and T.{0} like '%'||{1}||'%'", cKey, cValue);
                                break;
                            case queryCondition.LF_Like:
                                strWhere += string.Format(" and T.{0} like '%'||{1}", cKey, cValue);
                                break;
                            case queryCondition.RT_Like:
                                strWhere += string.Format(" and T.{0} like {1}||'%'", cKey, cValue);
                                break;

                            case queryCondition.IN:
                                strWhere += string.Format(" and T.{0} in( '{1}')", cKey, string.Join("','",cValue.Trim(new char[]{',','\''}).Split(',')));
                                break;
                            case queryCondition.NIN:
                                strWhere += string.Format(" and T.{0} not in( {1})", cKey, cValue);
                                break;

                            case queryCondition.exists:
                                strWhere += string.Format(" and exists({0})", condition.value);
                                break;
                            case queryCondition.nexists:
                                strWhere += string.Format(" and not exists({0})", condition.value);
                                break;

                            default:
                                strWhere += string.Format("T.{0}={1}", cKey, cValue); ;
                                break;
                        }
                        #endregion
                    }
                    else if (string.IsNullOrEmpty(cKey))
                    {
                        if (!string.IsNullOrEmpty(condition.conditionSql))
                        {
                            strWhere += condition.conditionSql;
                        }

                        if (condition.comMod != null)
                        {
                            page_no = condition.comMod.PAGE_NO;
                            page_size = condition.comMod.PAGE_SIZE;
                            strOrderBy = string.IsNullOrEmpty(condition.comMod.SORT.Trim()) ? "" : " order by " + condition.comMod.SORT;
                        }

                    }
                }
            }
            return strWhere;
        
        }
        /// <summary>
        /// 查询数据表或View数据,支持分页
        /// </summary>
        /// <param name="CQuery">查询条件</param>
        /// <param name="strTbName">表或View名称(不区分大小写)</param>
        /// <param name="selectFields">查询字段</param>
        /// <returns>查询结果</returns>
        public static DataTable getData(List<CSearchCondition> CQuery, string strTbName, string selectFields)
        {
            string strSql = "";
            try
            {
                if (string.IsNullOrEmpty(selectFields)) selectFields = "T.*";
                
                string strOrderBy = "";
                int page_size = 0, page_no = 0;//分页查询

                string strWhere =" where 1=1 "+ getCSearchCondition(CQuery, strTbName, out page_size, out page_no, out strOrderBy);
                strSql = "select " + selectFields + " from " + strTbName + " T " + strWhere + strOrderBy;

                //如果是分页,则返回的table表名为查询条件的总行数
                return OracleDbHelper.PagingQuery(strSql, page_size, page_no);
            }
            catch (OracleException OExp)
            {
                throw new Exception(OExp.Message + "\n SQL is:" + strSql);
            }
        }

        #endregion

    }
    #endregion

}
