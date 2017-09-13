using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace codeBatch
{
    public class MasterForm : Form
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        private string _conString;

        public string ConString
        {
            get { return _conString = System.Configuration.ConfigurationManager.AppSettings["DefaultConnection"].Trim(); }

        }

        private string _namespaceConfig;

        public string NamespaceConfig
        {
            get { return _namespaceConfig = System.Configuration.ConfigurationManager.AppSettings["namespace"].Trim(); } 
        }

        private string _pathConfig;

        public string PathConfig
        {
            get { return _pathConfig = System.Configuration.ConfigurationManager.AppSettings["path"].Trim(); } 
        }

        /// <summary>
        /// 类型转换（可以将NULL的object类型直接转换成string不报错）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ObjToStr(object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断DataTable是否为空
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool DataTableIsNullOrEmpty(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
