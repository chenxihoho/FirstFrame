/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using Aliyun.OpenServices.OpenStorageService.Utilities;

namespace Aliyun.OpenServices
{
    /// <summary>
    /// 表示访问阿里云服务的配置信息。
    /// </summary>
    public class ClientConfiguration : ICloneable
    {
        private static string _userAgent = "aliyun-sdk-dotnet/"
                                                    + typeof (ClientConfiguration).Assembly.GetName().Version.ToString()
                                                    + "(" 
                                                    + OssUtils.GetOSVersion() + "/" + Environment.OSVersion.Version + "/" 
                                                    + OssUtils.GetSystemArchitecture() + ";" + Environment.Version
                                                    + ")";

        private int _proxyPort = -1;
        // A temporary solution for undesired abortion when putting/getting large objects,
        // asynchronous implementions of putobject/getobject in next version can be a better 
        // solution to solve such problems.  
        private int _connectionTimeout = -1;
        private int _maxErrorRetry = 3;
        private const string _defRootDomains = ".aliyuncs.com,.aliyun-inc.com,localhost";
        private IList<string> _rootDomains = new List<string>();

        /// <summary>
        /// HttpWebRequest最大的并发连接数目。
        /// </summary>
        public const int ConnectionLimit = 512;

        /// <summary>
        /// 获取访问请求的User-Agent。
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
        }

        /// <summary>
        /// 获取或设置代理服务器的地址。
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// 获取或设置代理服务器的端口。
        /// </summary>
        public int ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// 获取或设置密码。
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// 获取或设置代理服务器授权用户所在的域。
        /// </summary>
        public string ProxyDomain { get; set; }

        /// <summary>
        /// 获取或设置连接超时时间，单位为毫秒。
        /// </summary>
        public int ConnectionTimeout
        {
            get { return _connectionTimeout; }
            set { _connectionTimeout = value; }
        }

        /// <summary>
        /// 获取或设置请求发生错误时最大的重试次数。
        /// </summary>
        public int MaxErrorRetry
        {
            get { return _maxErrorRetry; }
            set { _maxErrorRetry = value; }
        }

        /// <summary>
        /// 获取或设置根域名列表，如果用户请求的Host未包含在该列表内，
        /// 则表明采用了自定义域名绑定功能，否则未开启该功能。
        /// </summary>
        public IList<string> RootDomainList
        {
            get
            {
                if (_rootDomains.Count == 0)
                {
                    var domains = _defRootDomains.Split(',');
                    foreach (var domain in domains)
                    {
                        _rootDomains.Add(domain);
                    }
                }
                return ((List<string>)_rootDomains).AsReadOnly();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Root domain list should not be null.");
                }
                _rootDomains.Clear();
                foreach (var domain in value)
                {
                    _rootDomains.Add(domain);
                }
                var domains = _defRootDomains.Split(',');
                foreach (var domain in domains)
                {
                    if (!_rootDomains.Contains(domain))
                    {
                        _rootDomains.Add(domain);
                    }
                }
            }
        }

        /// <summary>
        /// 获取该实例的拷贝。
        /// </summary>
        /// <returns>该实例的拷贝。</returns>
        public object Clone()
        {
            var config = new ClientConfiguration();
            config.ConnectionTimeout = ConnectionTimeout;
            config.MaxErrorRetry = MaxErrorRetry;
            config.ProxyDomain = ProxyDomain;
            config.ProxyHost = ProxyHost;
            config.ProxyPassword = ProxyPassword;
            config.ProxyPort = ProxyPort;
            config.ProxyUserName = ProxyUserName;
            config.RootDomainList = RootDomainList;
            return config;
        }
    }
}
