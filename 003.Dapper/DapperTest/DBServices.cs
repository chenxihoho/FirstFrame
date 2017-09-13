using FirstFrame.DapperEx;
using System;
using System.Configuration;

namespace DapperTest
{
    public enum DbIndex { DB_MALL = 0, DB_Maintenance = 1 }; 
    public class DBTools : IDBService
    {        
        private readonly static string[] _connStr = new string[30];
        private const string _Conn = "server={0};database={1};User Id={2};password={3};connection reset=false;connection lifetime=5;min pool size=3;max pool size=150;MultipleActiveResultSets=true";        
        public string[] GetConnectionStrings()
        {
            string environment = ConfigurationManager.AppSettings["Environment"];
            string ip = "127.0.0.1";
            string userid = "sa";
            string pwd = "9999$9999";
            if (!string.IsNullOrEmpty(environment))
            {
                switch (environment.ToUpper())
                {
                    case "DEV":
                        ip = "127.0.0.1";
                        userid = "sa";
                        pwd = "9999$9999";
                        break;
                    case "TEST":
                        ip = "192.168.1.130";
                        userid = "sa";
                        pwd = "123456";
                        break;
                    case "ONLINE":
                        ip = "113.10.243.102,2000";
                        userid = "sa";
                        pwd = "";
                        break;
                }
            }
            _connStr[0] = string.Format(_Conn, ip, "yh_mall", userid, pwd);
            _connStr[1] = string.Format(_Conn, ip, "02.Gains_Maintenance", userid, pwd);
            return _connStr;
        }
    }

}
