using Dapper;
using FirstFrame.Helper.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FirstFrame.DapperEx
{
    public static class Dapper
    {
        private static IDbConnection GetDbConnection(this DbBase dbs, IDbTransaction transaction = null)
        {
            return transaction == null ? dbs.GetConnectionFromPool().dbConnecttion : dbs.GetConnectionFromPool(transaction);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Insert<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var sql = SqlQuery<T>.Builder(dbs);
            dbs.SQL = sql.InsertSql;
            var flag = db.Execute(sql.InsertSql, t, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return flag == 1;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="lastId"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Insert<T>(this DbBase dbs, T t, ref string lastId, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var sql = SqlQuery<T>.Builder(dbs);
            dbs.SQL = sql.InsertSql + " select @@IDENTITY LastId ";
            var flag = db.Query(dbs.SQL, t, transaction).SingleOrDefault();
            if (flag != null) lastId = flag.LastId.ToString();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return flag != null;
        }


        /// <summary>
        ///  批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool InsertBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var sql = SqlQuery<T>.Builder(dbs);
            dbs.SQL = sql.InsertSql;
            var flag = db.Execute(sql.InsertSql, lt, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return flag == lt.Count;
        }

        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            dbs.SQL = sql.DeleteSql;
            var f = db.Execute(sql.DeleteSql, sql.Param, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return f > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="updateProperties"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, T t, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.AppendParam<T>(t);
            dbs.SQL = sql.DeleteSql;
            var f = db.Execute(sql.DeleteSql, sql.Param, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return f > 0;
        }
        /// <summary>
        /// 指定主键ID删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="KeyValue"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool DeleteByID<T>(this DbBase dbs, string KeyValue, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            SqlQuery sql = SqlQuery<T>.Builder(dbs);
            sql.KeyValue = KeyValue;
            var f = db.Execute(sql.DeleteByIdSql, sql.Param, transaction);
            return f > 0;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool DeleteBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var sql = SqlQuery<T>.Builder(dbs);
            dbs.SQL = sql.DeleteSql;
            var flag = db.Execute(sql.DeleteSql, lt, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return flag >= lt.Count;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="sql">按条件修改</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Update<T>(this DbBase dbs, T t, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs, t);
            }
            sql = sql.AppendParam<T>(t);
            dbs.SQL = sql.UpdateSql;
            var f = db.Execute(sql.UpdateSql, sql.Param, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return f > 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="updateProperties">要修改的属性集合</param>
        /// <param name="sql">按条件修改</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Update<T>(this DbBase dbs, T t, IList<string> updateProperties, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.AppendParam<T>(t).SetExcProperties<T>(updateProperties);
            dbs.SQL = sql.UpdateSql;
            var f = db.Execute(sql.UpdateSql, sql.Param, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return f > 0;
        }

        /// <summary>
        /// 修改（批量）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt">修改列表</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool UpdateBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var sql = SqlQuery<T>.Builder(dbs);
            dbs.SQL = sql.UpdateSql;
            var flag = db.Execute(sql.UpdateSql, lt, transaction, commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return flag >= lt.Count;
        }

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.Top(1);
            dbs.SQL = sql.QuerySql;
            var result = db.Query<T>(sql.QuerySql, sql.Param, transaction);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="ID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        //public static bool Exists<T>(this DbBase dbs, string KeyValue, IDbTransaction transaction = null) where T : class
        //{
        //    var db = GetDbConnection(dbs, transaction);
        //    SqlQuery sql = SqlQuery<T>.Builder(dbs);
        //    sql.KeyValue = KeyValue;
        //    var f = db.Query(sql.ExistsSql, sql.Param, transaction).SingleOrDefault();
        //    return f.DataCount > 0; ;// f > 0;
        //}
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static IList<T> Page<T>(this DbBase dbs, int PageIndex, int PageSize, out long RecordCount, SqlQuery sqlQuery = null, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            var result = new List<T>();
            RecordCount = 0;
            if (sqlQuery == null)
            {
                sqlQuery = SqlQuery<T>.Builder(dbs);
            }
            sqlQuery = sqlQuery.Page(PageIndex, PageSize);
            var para = sqlQuery.Param;
            var cr = db.Query(sqlQuery.CountSql, para).SingleOrDefault();
            RecordCount = (long)cr.DataCount;
            dbs.SQL = sqlQuery.PageSql;
            result = db.Query<T>(sqlQuery.PageSql, para, transaction).ToList();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return result;
        }

        /// <summary>
        /// 数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Count<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            dbs.SQL = sql.CountSql;
            var cr = db.Query(sql.CountSql, sql.Param, transaction).SingleOrDefault();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return (int)cr.DataCount;
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxID"></param>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int Max<T>(this DbBase dbs, string maxID, SqlQuery sql = null, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.MaxID(maxID);
            dbs.SQL = sql.MaxSql;
            var cr = db.Query(sql.MaxSql, sql.Param, transaction).SingleOrDefault();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return (int)cr.MaxCount;
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static decimal Sum<T>(this DbBase dbs, string SumID, SqlQuery sql = null, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.SumID(SumID);
            dbs.SQL = sql.SumSql;
            var cr = db.Query(sql.SumSql, sql.Param, transaction).SingleOrDefault();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return (decimal)cr.SumCount;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static IList<T> Query<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null) where T : class
        {
            var db = GetDbConnection(dbs, transaction);
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            dbs.SQL = sql.QuerySql;
            var result = db.Query<T>(sql.QuerySql, sql.Param, transaction);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return result.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int Execute(this DbBase dbs, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            int _Result = SqlMapper.Execute(db, sql, param as object, transaction, commandTimeout: commandTimeout);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this DbBase dbs, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, CommandType? commandType = null, int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<T> _Result = SqlMapper.Query<T>(db, sql, param as object, transaction, buffered, commandTimeout, commandType);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this DbBase dbs, string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<TReturn> _Result = SqlMapper.Query(db, sql, map, param as object, transaction, buffered, splitOn);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this DbBase dbs, string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<TReturn> _Result = SqlMapper.Query(db, sql, map, param as object, transaction, buffered, splitOn);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this DbBase dbs, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<TReturn> _Result = SqlMapper.Query(db, sql, map, param as object, transaction, buffered, splitOn);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this DbBase dbs, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<TReturn> _Result = SqlMapper.Query(db, sql, map, param as object, transaction, buffered, splitOn);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(this DbBase dbs, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            IEnumerable<dynamic> _Result = SqlMapper.Query(db, sql, param as object, transaction, buffered);
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

        /// <summary>
        /// 多结果查询
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object QueryMultiple(this DbBase dbs, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            dbs.SQL = sql;
            var db = GetDbConnection(dbs, transaction);
            SqlMapper.GridReader _QueryResult = SqlMapper.QueryMultiple(db, sql, param, transaction, commandTimeout, commandType);
            object _Result = _QueryResult.Read().ToList();
            if (transaction == null) dbs.ReturnConnectionToPool(db);
            return _Result;
        }

    }
}
