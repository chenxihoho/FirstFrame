using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Text;

using ybzf.Storage.Dapper;

namespace ybzf.Storage.DapperEx
{
    public static class Database<TDatabase> : IDisposable where TDatabase : Database<TDatabase>, new()
    {
        public static class Table
        {
            internal Database<TDatabase> database;
            internal string tableName;
            internal string likelyTableName;
            //public virtual int? Insert1(DbBase dbs, IDbTransaction transaction = null, int? commandTimeout = null)
            //{
            //    return 1;
            //}
            //public Table(Database<TDatabase> database, string likelyTableName)
            //{
            //    this.database = database;
            //    this.likelyTableName = likelyTableName;
            //}

            //public string TableName
            //{
            //    get
            //    {
            //        tableName = tableName ?? database.DetermineTableName<T>(likelyTableName);
            //        return tableName;
            //    }
            //}
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dbs"></param>
            /// <param name="t"></param>
            /// <param name="transaction"></param>
            /// <param name="commandTimeout"></param>
            /// <returns></returns>
            public static bool Insert<T>(DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) //where T : class
            {
                var db = dbs.DbConnecttion;
                var sql = SqlQuery<T>.Builder(dbs);
                var flag = db.Execute(sql.InsertSql, t, transaction, commandTimeout);

                return flag == 1;
            }

            ///// <summary>
            /////  批量插入数据
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="lt"></param>
            ///// <param name="transaction"></param>
            ///// <param name="commandTimeout"></param>
            ///// <returns></returns>
            //public bool InsertBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    var sql = SqlQuery<T>.Builder(dbs);
            //    var flag = db.Execute(sql.InsertSql, lt, transaction, commandTimeout);

            //    return flag == lt.Count;
            //}

            ///// <summary>
            ///// 按条件删除
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="sql"></param>
            ///// <returns></returns>
            //public bool Delete<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    var f = db.Execute(sql.DeleteSql, sql.Param);

            //    return f > 0;
            //}

            ///// <summary>
            ///// 修改
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="t">如果sql为null，则根据t的主键进行修改</param>
            ///// <param name="sql">按条件修改</param>
            ///// <returns></returns>
            //public bool Update<T>(this DbBase dbs, T t, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    sql = sql.AppendParam<T>(t);
            //    var f = db.Execute(sql.UpdateSql, sql.Param);

            //    return f > 0;
            //}

            ///// <summary>
            ///// 修改
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="t">如果sql为null，则根据t的主键进行修改</param>
            ///// <param name="updateProperties">要修改的属性集合</param>
            ///// <param name="sql">按条件修改</param>
            ///// <returns></returns>
            //public bool Update<T>(this DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    sql = sql.AppendParam<T>(t).SetExcProperties<T>(updateProperties);
            //    var f = db.Execute(sql.UpdateSql, sql.Param);

            //    return f > 0;
            //}

            ///// <summary>
            ///// 获取默认一条数据，没有则为NULL
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="sql"></param>
            ///// <returns></returns>
            //public T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    sql = sql.Top(1);
            //    var result = db.Query<T>(sql.QuerySql, sql.Param);

            //    return result.FirstOrDefault();
            //}

            ///// <summary>
            ///// 分页查询
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="pageIndex"></param>
            ///// <param name="pageSize"></param>
            ///// <param name="dataCount"></param>
            ///// <param name="sqlQuery"></param>
            ///// <returns></returns>
            //public IList<T> Page<T>(this DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    var result = new List<T>();
            //    dataCount = 0;
            //    if (sqlQuery == null)
            //    {
            //        sqlQuery = SqlQuery<T>.Builder(dbs);
            //    }
            //    sqlQuery = sqlQuery.Page(pageIndex, pageSize);
            //    var para = sqlQuery.Param;
            //    var cr = db.Query(sqlQuery.CountSql, para).SingleOrDefault();
            //    dataCount = (long)cr.DataCount;
            //    result = db.Query<T>(sqlQuery.PageSql, para).ToList();

            //    return result;
            //}

            ///// <summary>
            ///// 查询
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="sql"></param>
            ///// <returns></returns>
            //public IList<T> Query<T>(this DbBase dbs, SqlQuery sql = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    var result = db.Query<T>(sql.QuerySql, sql.Param);

            //    return result.ToList();
            //}

            ///// <summary>
            ///// 数据数量
            ///// </summary>
            ///// <typeparam name="T"></typeparam>
            ///// <param name="dbs"></param>
            ///// <param name="sql"></param>
            ///// <returns></returns>
            //public long Count<T>(this DbBase dbs, SqlQuery sql = null) where T : class
            //{
            //    var db = dbs.DbConnecttion;
            //    if (sql == null)
            //    {
            //        sql = SqlQuery<T>.Builder(dbs);
            //    }
            //    var cr = db.Query(sql.CountSql, sql.Param).SingleOrDefault();

            //    return (long)cr.DataCount;
            //}
        }
        //public class Table<T> 
        //{
        //    public Table(Database<TDatabase> database, string likelyTableName)
        //        //: base(database, likelyTableName)
        //    {
        //    }
        //}

        DbConnection connection;
        int commandTimeout;
        DbTransaction transaction;

        private readonly static string[] iar_ConnectString = new string[30];
        static Database()
        {
            iar_ConnectString[0] = "server=WIN-6OIJFRVD95B;database=D_Log;User Id=sa;password=123456;connection reset=false;connection lifetime=5;min pool size=1;max pool size=5";
            //iar_ConnectString[0] = "server=192.168.1.130;database=DB_Log;User Id=sa;password=123456;connection reset=false;connection lifetime=5;min pool size=1;max pool size=5";
            iar_ConnectString[1] = "server=WIN-6OIJFRVD95B;database=D_Log;User Id=sa;password=123456;connection reset=false;connection lifetime=5;min pool size=1;max pool size=5";
        }

        //public static TDatabase Init(int iConnectID)
        //{
        //    return Init(new SqlConnection(iar_ConnectString[iConnectID]), 2);
        //}
        //public static TDatabase Init(DbConnection connection, int commandTimeout)
        //{
        //    TDatabase db = new TDatabase();
        //    db.InitDatabase(connection, commandTimeout);
        //    return db;
        //}

        //internal static Action<TDatabase> tableConstructor;
        //internal void InitDatabase(DbConnection connection, int commandTimeout)
        //{
        //    this.connection = connection;
        //    this.commandTimeout = commandTimeout;
        //    if (tableConstructor == null)
        //    {
        //        tableConstructor = CreateTableConstructorForTable();
        //    }

        //    tableConstructor(this as TDatabase);
        //}

        //internal virtual Action<TDatabase> CreateTableConstructorForTable()
        //{
        //    return CreateTableConstructor(typeof(Table<>));
        //}

        public void BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            transaction = connection.BeginTransaction(isolation);
        }

        public void CommitTransaction()
        {
            transaction.Commit();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
            transaction = null;
        }
        protected Action<TDatabase> CreateTableConstructor(Type tableType)
        {
            var dm = new DynamicMethod("ConstructInstances", null, new Type[] { typeof(TDatabase) }, true);
            var il = dm.GetILGenerator();

            var setters = GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == tableType)
                .Select(p => Tuple.Create(
                        p.GetSetMethod(true),
                        p.PropertyType.GetConstructor(new Type[] { typeof(TDatabase), typeof(string) }),
                        p.Name,
                        p.DeclaringType
                 ));

            foreach (var setter in setters)
            {
                il.Emit(OpCodes.Ldarg_0);
                // [db]

                il.Emit(OpCodes.Ldstr, setter.Item3);
                // [db, likelyname]

                il.Emit(OpCodes.Newobj, setter.Item2);
                // [table]

                var table = il.DeclareLocal(setter.Item2.DeclaringType);
                il.Emit(OpCodes.Stloc, table);
                // []

                il.Emit(OpCodes.Ldarg_0);
                // [db]

                il.Emit(OpCodes.Castclass, setter.Item4);
                // [db cast to container]

                il.Emit(OpCodes.Ldloc, table);
                // [db cast to container, table]

                il.Emit(OpCodes.Callvirt, setter.Item1);
                // []
            }

            il.Emit(OpCodes.Ret);
            return (Action<TDatabase>)dm.CreateDelegate(typeof(Action<TDatabase>));
        }

        static ConcurrentDictionary<Type, string> tableNameMap = new ConcurrentDictionary<Type, string>();
        private string DetermineTableName<T>(string likelyTableName)
        {
            string name;

            if (!tableNameMap.TryGetValue(typeof(T), out name))
            {
                name = likelyTableName;
                if (!TableExists(name))
                {
                    name = "[" + typeof(T).Name + "]";
                }

                tableNameMap[typeof(T)] = name;
            }
            return name;
        }

        private bool TableExists(string name)
        {
            string schemaName = null;

            name = name.Replace("[", "");
            name = name.Replace("]", "");

            if (name.Contains("."))
            {
                var parts = name.Split('.');
                if (parts.Count() == 2)
                {
                    schemaName = parts[0];
                    name = parts[1];
                }
            }

            var builder = new StringBuilder("select 1 from INFORMATION_SCHEMA.TABLES where ");
            if (!String.IsNullOrEmpty(schemaName)) builder.Append("TABLE_SCHEMA = @schemaName AND ");
            builder.Append("TABLE_NAME = @name");

            return connection.Query(builder.ToString(), new { schemaName, name }, transaction: transaction).Count() == 1;
        }
        public int Execute(string sql, dynamic param = null)
        {
            return SqlMapper.Execute(connection, sql, param as object, transaction, commandTimeout: this.commandTimeout);
        }

        public IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true)
        {
            return SqlMapper.Query<T>(connection, sql, param as object, transaction, buffered, commandTimeout);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(connection, sql, map, param as object, transaction, buffered, splitOn);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(connection, sql, map, param as object, transaction, buffered, splitOn);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(connection, sql, map, param as object, transaction, buffered, splitOn);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(connection, sql, map, param as object, transaction, buffered, splitOn);
        }

        public IEnumerable<dynamic> Query(string sql, dynamic param = null, bool buffered = true)
        {
            return SqlMapper.Query(connection, sql, param as object, transaction, buffered);
        }

        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(connection, sql, param, transaction, commandTimeout, commandType);
        }


        public void Dispose()
        {
            if (connection.State != ConnectionState.Closed)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                connection.Close();
                connection = null;
            }
        }

    }
}
