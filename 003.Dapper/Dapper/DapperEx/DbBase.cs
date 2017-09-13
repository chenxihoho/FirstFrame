using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading;
using FirstFrame.Helper.Log;

namespace FirstFrame.DapperEx
{
    public interface IDBService
    {
        string[] GetConnectionStrings();
    }
    public class ConnectionSettings
    {
        public int ThreadId = 0;
        public bool InUse = false;
        public IDbConnection dbConnecttion = null;
        public List<IDbTransaction> TransactionList = new List<IDbTransaction>();
        public int ExecutedTime = 0;
        public int TransactionTimeOut = 0; //事务最大允许执行时间
    }
    /// <summary>
    /// DbBase
    /// </summary>
    public class DbBase : IDisposable
    {
        private IDBService _DBService;
        private object Locker = new object();
        public Dictionary<IDbConnection, ConnectionSettings> ConnectionPool = new Dictionary<IDbConnection, ConnectionSettings>();
        private int ConnectionPoolSize = 5;
        public bool KeepConnect = true;
        public Timer MonitorTimer;
        private int _ConnID;
        private string paramPrefix = "@";
        private string _ProviderName = "System.Data.SqlClient";
        private DbProviderFactory dbFactory = null;
        private DBType _dbType = DBType.SqlServer;
        //private readonly static string[] _connStr = new string[30];
        private string ConnString = string.Empty;
        #if DEBUG
            public const int ConfigTransactionTimeOut = 500000;
        #endif
        #if !DEBUG
            public const int ConfigTransactionTimeOut = 5000;
        #endif
        public string SQL = string.Empty; // 返回上一个执行操作的sql语句

        #region 初始化
        public DbBase(IDBService DBService, int iConnID, string providerName = "System.Data.SqlClient")
        {
            _DBService = DBService;
            _ConnID = iConnID;
            _ProviderName = providerName;

            MonitorTimer = new Timer(new TimerCallback(TimeOutCheck), null, 0, 1000);
            dbFactory = DbProviderFactories.GetFactory(_ProviderName);
            InitConnectionPool();
        }
        #endregion
        #region 初始化连接池
        private void InitConnectionPool()
        {
            lock (Locker)
            {
                if (ConnectionPool.Count >= ConnectionPoolSize) return;

                for (int i = 0; i < ConnectionPoolSize; i++)
                {
                    IDbConnection _dbConnecttion = dbFactory.CreateConnection();
                    ConnString = _DBService.GetConnectionStrings()[_ConnID].ToString();
                    _dbConnecttion.ConnectionString = ConnString;
                    if (KeepConnect) _dbConnecttion.Open();
                    SetParamPrefix(_dbConnecttion);
                    ConnectionSettings _Setting = new ConnectionSettings()
                    {
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        InUse = false,
                        dbConnecttion = _dbConnecttion,
                        ExecutedTime = 0
                    };
                    ConnectionPool.Add(_dbConnecttion, _Setting);
                }
            }
        }
        #endregion
        #region 从连接池中获取连接信息对象
        public ConnectionSettings GetConnectionFromPool()
        {
            lock (Locker)
            {
                try
                {
                    foreach (var item in ConnectionPool)
                    {
                        if (!item.Value.InUse)
                        {
                            if (item.Key.State == ConnectionState.Closed) item.Key.Open();
                            item.Value.InUse = true;
                            return item.Value;
                        }
                    }
                    //如果连接池中的对象全部都在使用中
                    dbFactory = DbProviderFactories.GetFactory(_ProviderName);
                    IDbConnection _dbConnection = dbFactory.CreateConnection();
                    _dbConnection.ConnectionString = _DBService.GetConnectionStrings()[_ConnID].ToString();
                    if (KeepConnect) _dbConnection.Open();
                    ConnectionSettings _Setting = new ConnectionSettings()
                    {
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        InUse = true,
                        dbConnecttion = _dbConnection,
                        ExecutedTime = 0
                    };
                    ConnectionPool.Add(_dbConnection, _Setting);
                    return _Setting;
                }
                catch (Exception e)
                {
                    LogHelper.Error(string.Format("GetConnectionFromPool {0}", e.Message));
                }
                return null;
            }
        }
        #endregion
        #region 从连接池中获取一个连接
        public IDbConnection GetConnectionFromPool(IDbTransaction _Trans)
        {
            lock (Locker)
            {
                try
                {
                    foreach (var item in ConnectionPool)
                    {
                        if (item.Value.TransactionList.Contains(_Trans))  //如果指定线程要求获取连接（此行为出现在事务中）
                        {
                            if (item.Key.State == ConnectionState.Closed) item.Key.Open();
                            item.Value.InUse = true;
                            return item.Key;
                        }
                    }
                }
                catch (Exception)
                {
                    //SystemLog.Error(string.Format("GetConnectionFromPool {0}", e.Message));
                }
                return null;
            }
        }
        #endregion
        #region 向连接池中归还连接
        public void ReturnConnectionToPool(IDbConnection dbConnecttion)
        {
            lock (Locker)
            {
                foreach (var item in ConnectionPool)
                {
                    if (item.Key == dbConnecttion)
                    {
                        if (!KeepConnect) item.Key.Close();
                        item.Value.InUse = false;
                        item.Value.TransactionList.Clear();
                        return;
                    }
                }
            }
        }
        public void ReturnConnectionToPool(IDbTransaction _dbTransaction)
        {
            lock (Locker)
            {
                foreach (var item in ConnectionPool)
                {
                    if (item.Value.TransactionList.Contains(_dbTransaction))
                    {
                        item.Value.TransactionList.Remove(_dbTransaction);
                        if (item.Value.TransactionList.Count == 0) //所有事务已经执行完成才可退回连接池
                        {
                            if (!KeepConnect) item.Key.Close();
                            item.Value.InUse = false;
                        }
                        return;
                    }
                }
            }
        }
        #endregion
        #region 向连接上绑定事务
        private void BindTransaction(IDbConnection dbConnecttion, IDbTransaction dbTransaction)
        {
            lock (Locker)
            {
                foreach (var item in ConnectionPool)
                {
                    if (item.Key == dbConnecttion)
                    {
                        item.Value.ThreadId = Thread.CurrentThread.ManagedThreadId;
                        item.Value.TransactionList.Add(dbTransaction);
                        return;
                    }
                }
            }
        }
        #endregion
        #region 启动事务
        public IDbTransaction BeginTransaction(int _TransactionTimeOut = ConfigTransactionTimeOut, IsolationLevel _IsolationLevel = IsolationLevel.ReadUncommitted)
        {
            ConnectionSettings _ConnectionSettings = GetConnectionFromPool();           
            if (_ConnectionSettings.dbConnecttion.State == ConnectionState.Closed) _ConnectionSettings.dbConnecttion.Open();
            IDbTransaction dbTransaction = _ConnectionSettings.dbConnecttion.BeginTransaction(_IsolationLevel);
            BindTransaction(_ConnectionSettings.dbConnecttion, dbTransaction);
            _ConnectionSettings.TransactionTimeOut = _TransactionTimeOut;
            return dbTransaction;
        }
        #endregion
        #region 提交事务
        public void Commit(IDbTransaction _Trans)
        {
            if (_Trans == null) return;

            try
            {
                _Trans.Commit();
            }
            finally
            {
                ReturnConnectionToPool(_Trans);
            }
        }
        #endregion
        #region 回滚事务
        public void Rollback(IDbTransaction _Trans)
        {
            if (_Trans == null) return;

            try
            {
                _Trans.Rollback();
            }
            finally
            {
                ReturnConnectionToPool(_Trans);
            }
        }
        #endregion
        #region 事务超时检测
        private void TimeOutCheck(object state)
        {
            lock (Locker)
            {
                for (int i = ConnectionPool.Count - 1; i >= 0; i--)
                {
                    ConnectionSettings _ConnectionSettings = ConnectionPool.ElementAt(i).Value;
                    if ((!_ConnectionSettings.InUse) || (_ConnectionSettings.TransactionTimeOut == 0)) continue; //不检查事务超时
                    if (_ConnectionSettings.TransactionList.Count > 0) _ConnectionSettings.ExecutedTime += 1000; //如果有事务在执行，则进行计时
                    if (_ConnectionSettings.ExecutedTime >= _ConnectionSettings.TransactionTimeOut)
                    {
                        //强制回滚事务，并收回连接
                        foreach (IDbTransaction Trans in _ConnectionSettings.TransactionList)
	                    {
                            Trans.Rollback();
	                    }
                        ReturnConnectionToPool(_ConnectionSettings.dbConnecttion);
                        LogHelper.Error("事务执行超时，已被强制回滚。");
                    }
                }
            }
        }
        #endregion
        #region 释放连接池
        private void FreeConnectionPool()
        {
            lock (Locker)
            {
                foreach (var item in ConnectionPool)
                {
                    item.Value.dbConnecttion.Dispose();
                }
            }
        }
        #endregion
        /// <summary>
        /// 参数前缀
        /// </summary>
        public string ParamPrefix
        {
            get
            {
                return paramPrefix;
            }
        }

        /// <summary>
        /// ProviderName
        /// </summary>
        public string ProviderName
        {
            get
            {
                return _ProviderName;
            }
        }

        /// <summary>
        /// DbType
        /// </summary>
        public DBType DbType
        {
            get
            {
                return _dbType;
            }
        }

        private void SetParamPrefix(IDbConnection _dbConnection)
        {
            string dbtype = (dbFactory == null ? _dbConnection.GetType() : dbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _dbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _dbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _dbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _dbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _dbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient.")) _dbType = DBType.SqlServer;
            // else try with provider name
            else if (_ProviderName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.MySql;
            else if (_ProviderName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SqlServerCE;
            else if (_ProviderName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.PostgreSQL;
            else if (_ProviderName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.Oracle;
            else if (_ProviderName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SQLite;

            if (_dbType == DBType.MySql && _dbConnection != null && _dbConnection.ConnectionString != null && _dbConnection.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                paramPrefix = "?";

            if (_dbType == DBType.Oracle)
                paramPrefix = ":";
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            FreeConnectionPool();
        }
    }

    /// <summary>
    /// DBType
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// SqlServer
        /// </summary>
        SqlServer,
        /// <summary>
        /// SqlServerCE
        /// </summary>
        SqlServerCE,
        /// <summary>
        /// MySql
        /// </summary>
        MySql,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSQL,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// SQLite
        /// </summary>
        SQLite
    }

}