using FirstFrame.DapperEx;
using System;
using System.Configuration;

namespace TaskTester
{
    public enum DbIndex { DB_MALL = 0 };
    public class DBTools : IDBService
    {
        private readonly static string[] _connStr = new string[30];
        private const string _Conn = "server={0};database={1};User Id={2};password={3};connection reset=false;connection lifetime=5;min pool size=3;max pool size=150;MultipleActiveResultSets=true";
        public string[] GetConnectionStrings()
        {
            string environment = ConfigurationManager.AppSettings["Environment"];
            string ip = "101.201.145.13,2000";
            string userid = "yhAzure.Mall";
            string pwd = "mY4Gd:a3oBye8=o5Hzjp";
            if (!string.IsNullOrEmpty(environment))
            {
                switch (environment.ToUpper())
                {
                    case "DEV":
                        ip = "HAIER-PC\\SQL2008";
                        userid = "sa";
                        pwd = "sa";
                        break;
                    case "ALPHA":
                        ip = "111.202.122.117,2000";
                        userid = "sa";
                        pwd = "InYtoJRwVX55A38SdLyT";
                        break;
                    case "ONLINE":
                        ip = "101.201.145.13,2000";
                        userid = "yhAzure.Mall";
                        pwd = "mY4Gd:a3oBye8=o5Hzjp";
                        break;
                }
            }
            _connStr[0] = string.Format(_Conn, ip, "yh_mall", userid, pwd);
            return _connStr;
        }
    }
}
