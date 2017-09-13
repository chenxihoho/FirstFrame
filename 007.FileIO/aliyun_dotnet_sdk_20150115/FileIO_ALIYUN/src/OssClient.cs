/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Collections.Generic;

using Aliyun.OpenServices.Common.Communication;
using Aliyun.OpenServices.Common.Utilities;
using Aliyun.OpenServices.OpenStorageService.Commands;
using Aliyun.OpenServices.Properties;
using Aliyun.OpenServices.OpenStorageService.Utilities;
using Aliyun.OpenServices.Common.Authentication;
using Aliyun.OpenServices.Domain;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 访问阿里云开放存储服务（Open Storage Service， OSS）的入口类。
    /// </summary>
    public class OssClient : IOss
    {
        #region Fields & Properties

        private readonly Uri _endpoint;
        private readonly ServiceCredentials _credentials;
        private readonly IServiceClient _serviceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// 由默认的OSS访问地址(http://oss-cn-hangzhou.aliyuncs.com)构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="accessId">OSS的访问ID。</param>
        /// <param name="accessKey">OSS的访问密钥。</param>
        public OssClient(string accessId, string accessKey)
            : this(OssUtils.DefaultEndpoint, accessId, accessKey)
        {
        }

        /// <summary>
        /// <para>由用户指定的OSS访问地址构造一个新的<see cref="OssClient" />实例。</para>
        /// <para>OSS访问地址的生成规则为：http://region.aliyuncs.com ，其中region表示“数据中心“。</para>
        /// <para>目前OSS对外公开的数据中心的域名为： </para>
        /// <para>    杭州数据中心：oss-cn-hangzhou </para>
        /// <para>    青岛数据中心：oss-cn-qingdao     </para>
        /// <para>    北京数据中心：oss-cn-beijing       </para>
        /// <para>    香港数据中心：oss-cn-hongkong  </para>
        /// <para>    深圳数据中心：oss-cn-shenzhen   </para>
        /// <para>例如region表示北京数据中心，则OSS访问地址为：http://oss-cn-beijing.aliyuncs.com</para>
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessId">OSS的访问ID。</param>
        /// <param name="accessKey">OSS的访问密钥。</param>
        public OssClient(string endpoint, string accessId, string accessKey)
            : this(new Uri(endpoint), accessId, accessKey)
        {
        }

        /// <summary>
        /// <para>由用户指定的OSS访问地址构造一个新的<see cref="OssClient" />实例。</para>
        /// <para>OSS访问地址的生成规则为：http://region.aliyuncs.com ，其中region表示“数据中心“。</para>
        /// <para>目前OSS对外公开的数据中心的域名为： </para>
        /// <para>    杭州数据中心：oss-cn-hangzhou </para>
        /// <para>    青岛数据中心：oss-cn-qingdao     </para>
        /// <para>    北京数据中心：oss-cn-beijing       </para>
        /// <para>    香港数据中心：oss-cn-hongkong  </para>
        /// <para>    深圳数据中心：oss-cn-shenzhen   </para>
        /// <para>例如region表示北京数据中心，则OSS访问地址为：http://oss-cn-beijing.aliyuncs.com</para>
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessId">OSS的访问ID。</param>
        /// <param name="accessKey">OSS的访问密钥。</param>
        public OssClient(Uri endpoint, string accessId, string accessKey)
            : this(endpoint, accessId, accessKey, new ClientConfiguration())
        {
        }

        /// <summary>
        /// <para>由用户指定的OSS访问地址构造一个新的<see cref="OssClient" />实例。</para>
        /// <para>OSS访问地址的生成规则为：http://region.aliyuncs.com ，其中region表示“数据中心“。</para>
        /// <para>目前OSS对外公开的数据中心的域名为： </para>
        /// <para>    杭州数据中心：oss-cn-hangzhou </para>
        /// <para>    青岛数据中心：oss-cn-qingdao     </para>
        /// <para>    北京数据中心：oss-cn-beijing       </para>
        /// <para>    香港数据中心：oss-cn-hongkong  </para>
        /// <para>    深圳数据中心：oss-cn-shenzhen   </para>
        /// <para>例如region表示北京数据中心，则OSS访问地址为：http://oss-cn-beijing.aliyuncs.com</para>
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessId">OSS的访问ID。</param>
        /// <param name="accessKey">OSS的访问密钥。</param>
        /// <param name="configuration">客户端配置。</param>
        public OssClient(Uri endpoint, string accessId, string accessKey, ClientConfiguration configuration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            if (string.IsNullOrEmpty(accessId))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");
            if (string.IsNullOrEmpty(accessKey))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessKey");

            if (!endpoint.ToString().StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(OssResources.BucketNameInvalid, "endpoint");

            _endpoint = endpoint;
            _credentials = new ServiceCredentials(accessId, accessKey);
            _serviceClient = ServiceClientFactory.CreateServiceClient(configuration ?? new ClientConfiguration());
        }

        #endregion

        #region Bucket Operations

        /// <inheritdoc/>
        public Bucket CreateBucket(string bucketName)
        {
            var cmd = CreateBucketCommand.Create(GetServiceClient(),
                                                 _endpoint,
                                                 CreateContext(HttpMethod.Put, bucketName, null),
                                                 bucketName);
            using(cmd.Execute())
            {
                // Do nothing
            }

            return new Bucket(bucketName);
        }

        /// <inheritdoc/>
        public void DeleteBucket(string bucketName)
        {
            var cmd = DeleteBucketCommand.Create(GetServiceClient(),
                                                 _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, null),
                                                 bucketName);
            using(cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Bucket> ListBuckets()
        {
            var cmd = ListBucketsCommand.Create(GetServiceClient(),
                                                _endpoint,
                                                CreateContext(HttpMethod.Get, null, null), null);
            var result = cmd.Execute();
            return result.Buckets;
        }

        /// <inheritdoc/>
        public ListBucketsResult ListBuckets(ListBucketsRequest listBucketsRequest)
        {
            var cmd = ListBucketsCommand.Create(GetServiceClient(),
                                                _endpoint,
                                                CreateContext(HttpMethod.Get, null, null), listBucketsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void SetBucketAcl(string bucketName, CannedAccessControlList acl)
        {
            var setBucketAclRequest = new SetBucketAclRequest(bucketName, acl);
            SetBucketAcl(setBucketAclRequest);
        }

        /// <inheritdoc/>
        public void SetBucketAcl(SetBucketAclRequest setBucketAclRequest)
        {
            ThrowIfNullRequest(setBucketAclRequest);
            var cmd = SetBucketAclCommand.Create(GetServiceClient(),
                                                _endpoint,
                                                CreateContext(HttpMethod.Put, setBucketAclRequest.BucketName, null),
                                                setBucketAclRequest.BucketName, setBucketAclRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public AccessControlList GetBucketAcl(string bucketName)
        {
            var cmd = GetBucketAclCommand.Create(GetServiceClient(),
                                                 _endpoint,
                                                 CreateContext(HttpMethod.Get, bucketName, null),
                                                 bucketName);
            return cmd.Execute();
        }



        /// <inheritdoc/>
        public void SetBucketCors(SetBucketCorsRequest setBucketCorsRequest)
        {
            ThrowIfNullRequest(setBucketCorsRequest);
            var cmd = SetBucketCorsCommand.Create(GetServiceClient()
                                                 , _endpoint,
                                                 CreateContext(HttpMethod.Put, setBucketCorsRequest.BucketName, null),
                                                 setBucketCorsRequest.BucketName,
                                                 setBucketCorsRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public IList<CORSRule> GetBucketCors(string bucketName)
        {
            var cmd = GetBucketCorsCommand.Create(GetServiceClient(),
                                                 _endpoint,
                                                 CreateContext(HttpMethod.Get, bucketName, null),
                                                 bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketCors(string bucketName)
        {
            var cmd = DeleteBucketCorsCommand.Create(GetServiceClient()
                                                 , _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, null),
                                                 bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }
        
        /// <inheritdoc/>
        public void SetBucketLogging(SetBucketLoggingRequest setBucketLoggingRequest)
        {
            ThrowIfNullRequest(setBucketLoggingRequest);
            var cmd = SetBucketLoggingCommand.Create(GetServiceClient()
                                                    , _endpoint,
                                                    CreateContext(HttpMethod.Put, setBucketLoggingRequest.BucketName, null),
                                                    setBucketLoggingRequest.BucketName,
                                                    setBucketLoggingRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public BucketLoggingResult GetBucketLogging(string bucketName)
        {
            var cmd = GetBucketLoggingCommand.Create(GetServiceClient(),
                                     _endpoint,
                                     CreateContext(HttpMethod.Get, bucketName, null),
                                     bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketLogging(string bucketName)
        {
            var cmd = DeleteBucketLoggingCommand.Create(GetServiceClient()
                                                 , _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, null),
                                                 bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public void SetBucketWebsite(SetBucketWebsiteRequest setBucketWebSiteRequest)
        {
            ThrowIfNullRequest(setBucketWebSiteRequest);
            var cmd = SetBucketWebsiteCommand.Create(GetServiceClient()
                                               , _endpoint,
                                               CreateContext(HttpMethod.Put, setBucketWebSiteRequest.BucketName, null),
                                               setBucketWebSiteRequest.BucketName,
                                               setBucketWebSiteRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public BucketWebsiteResult GetBucketWebsite(string bucketName)
        {
            var cmd = GetBucketWebsiteCommand.Create(GetServiceClient(),
                                 _endpoint,
                                 CreateContext(HttpMethod.Get, bucketName, null),
                                 bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketWebsite(string bucketName)
        {
            var cmd = DeleteBucketWebsiteCommand.Create(GetServiceClient()
                                                 , _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, null),
                                                 bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }


        /// <inheritdoc/>
        public void SetBucketReferer(SetBucketRefererRequest setBucketRefererRequest)
        {
            ThrowIfNullRequest(setBucketRefererRequest);
            var cmd = SetBucketRefererCommand.Create(GetServiceClient()
                                               , _endpoint,
                                               CreateContext(HttpMethod.Put, setBucketRefererRequest.BucketName, null),
                                               setBucketRefererRequest.BucketName,
                                               setBucketRefererRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public RefererConfiguration GetBucketReferer(string bucketName)
        {
            var cmd = GetBucketRefererCommand.Create(GetServiceClient(),
                                 _endpoint,
                                 CreateContext(HttpMethod.Get, bucketName, null),
                                 bucketName);
            return cmd.Execute();
        }

        #endregion

        #region Object Operations

        /// <inheritdoc/>
        public ObjectListing ListObjects(string bucketName)
        {
            return ListObjects(bucketName, null);
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects(string bucketName, string prefix)
        {
            return ListObjects(
                new ListObjectsRequest(bucketName)
                {
                    Prefix = prefix
                });
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects(ListObjectsRequest listObjectsRequest)
        {
            ThrowIfNullRequest(listObjectsRequest);
            var cmd = ListObjectsCommand.Create(GetServiceClient(), _endpoint,
                                                CreateContext(HttpMethod.Get, listObjectsRequest.BucketName, null),
                                                listObjectsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, Stream content)
        {
            return PutObject(bucketName, key, content, null);
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata)
        {
            var cmd = PutObjectCommand.Create(GetServiceClient(), _endpoint,
                                              CreateContext(HttpMethod.Put, bucketName, key),
                                              bucketName, key, content, metadata);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, string fileToUpload)
        {
            return PutObject(bucketName, key, fileToUpload, null);
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, string fileToUpload, ObjectMetadata metadata)
        {
            if (!File.Exists(fileToUpload))
                throw new ArgumentException("The specified file to upload not exists");

            PutObjectResult result = null;
            using (Stream content = File.OpenRead(fileToUpload))
            {
                result = PutObject(bucketName, key, content, metadata);
            }
            return result;
        }

        /// <inheritdoc/>
        public OssObject GetObject(string bucketName, string key)
        {
            return GetObject(new GetObjectRequest(bucketName, key));
        }

        /// <inheritdoc/>
        public OssObject GetObject(GetObjectRequest getObjectRequest)
        {
            ThrowIfNullRequest(getObjectRequest);
            var cmd = GetObjectCommand.Create(GetServiceClient(),
                                              _endpoint,
                                              CreateContext(HttpMethod.Get,
                                                            getObjectRequest.BucketName,
                                                            getObjectRequest.Key),
                                              getObjectRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public ObjectMetadata GetObject(GetObjectRequest getObjectRequest, Stream output)
        {
            var ossObject = GetObject(getObjectRequest);
            using(ossObject.Content)
            {
                ossObject.Content.WriteTo(output);
            }
            return ossObject.Metadata;
        }

        /// <inheritdoc/>
        public ObjectMetadata GetObjectMetadata(string bucketName, string key)
        {
            var cmd = GetObjectMetadataCommand.Create(GetServiceClient(), _endpoint,
                                                      CreateContext(HttpMethod.Head, bucketName, key),
                                                      bucketName, key);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteObject(string bucketName, string key)
        {
            var cmd = DeleteObjectCommand.Create(GetServiceClient(), _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, key),
                                                 bucketName, key);
            using(cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public DeleteObjectsResult DeleteObjects(DeleteObjectsRequest deleteObjectsRequest)
        {
            ThrowIfNullRequest(deleteObjectsRequest);
            var cmd = DeleteObjectsCommand.Create(GetServiceClient(), _endpoint,
                                                  CreateContext(HttpMethod.Put, deleteObjectsRequest.BucketName, null),
                                                  deleteObjectsRequest);
            return cmd.Execute();
        }
        
        /// <inheritdoc/>
        public CopyObjectResult CopyObject(CopyObjectRequest copyObjectRequst)
        {
            ThrowIfNullRequest(copyObjectRequst);
            var cmd = CopyObjectCommand.Create(GetServiceClient(), _endpoint,
                                                 CreateContext(HttpMethod.Delete, copyObjectRequst.DestinationBucketName, copyObjectRequst.DestinationKey),
                                                 copyObjectRequst);
            return cmd.Execute();
        }
        #endregion
        
        #region Generate URL
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(string bucketName, string key)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get);
            return GeneratePresignedUri(request);
        }

        /// <inheritdoc/> 
        public Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get) {Expiration = expiration};
            return GeneratePresignedUri(request);
        }
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(string bucketName, string key, SignHttpMethod method)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, method);
            return GeneratePresignedUri(request);            
        }

        /// <inheritdoc/>  
        public Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration, SignHttpMethod method)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, method) {Expiration = expiration};
            return GeneratePresignedUri(request);
        }
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(GeneratePresignedUriRequest generatePresignedUriRequest)
        {
            ThrowIfNullRequest(generatePresignedUriRequest);

            var accessID = _credentials.AccessId;
            var accessKey = _credentials.AccessKey;
            var bucketName = generatePresignedUriRequest.BucketName;
            var key = generatePresignedUriRequest.Key;
            
            const long ticksOf1970 = 621355968000000000;
            var expires = ((generatePresignedUriRequest.Expiration.ToUniversalTime().Ticks - ticksOf1970) / 10000000L).ToString();
            var resourcePath = OssUtils.MakeResourcePath(key);
                
            var request = new ServiceRequest();
            var cc = OssUtils.GetClientConfiguration(_serviceClient);
            request.Endpoint = OssUtils.MakeBucketEndpoint(_endpoint, bucketName, cc);
            request.ResourcePath = resourcePath;

            switch (generatePresignedUriRequest.Method)
            {
                case SignHttpMethod.Get:
                    request.Method = HttpMethod.Get;
                    break;
                case SignHttpMethod.Put:
                    request.Method = HttpMethod.Put;
                    break;
                default:
                    throw new ArgumentException("Unsupported http method.");
            }
            
            request.Headers.Add(HttpHeaders.Date, expires);
            if (!string.IsNullOrEmpty(generatePresignedUriRequest.ContentType))
                request.Headers.Add(HttpHeaders.ContentType, generatePresignedUriRequest.ContentType);
            if (!string.IsNullOrEmpty(generatePresignedUriRequest.ContentMd5))
                request.Headers.Add(HttpHeaders.ContentMd5, generatePresignedUriRequest.ContentMd5);

            foreach (var pair in generatePresignedUriRequest.UserMetadata)
            {
                request.Headers.Add(OssHeaders.OssUserMetaPrefix + pair.Key, pair.Value);
            }
            
            if (generatePresignedUriRequest.ResponseHeaders != null)
                generatePresignedUriRequest.ResponseHeaders.Populate(request.Parameters);

            foreach (var param in generatePresignedUriRequest.QueryParams)
            {
                request.Parameters.Add(param.Key, param.Value);
            }
            
            var canonicalResource = "/" + (bucketName ?? "") + ((key != null ? "/" + key : ""));
            var httpMethod = generatePresignedUriRequest.Method.ToString().ToUpperInvariant();
            
            var canonicalString =
                SignUtils.BuildCanonicalString(httpMethod, canonicalResource, request/*, expires*/);
            var signature = ServiceSignature.Create().ComputeSignature(accessKey, canonicalString);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("Expires", expires);
            queryParams.Add("OSSAccessKeyId", accessID);
            queryParams.Add("Signature", signature);
            foreach (var param in request.Parameters)
            {
                queryParams.Add(param.Key, param.Value);
            }

            // Generate uri
            var queryString = HttpUtils.GetRequestParameterString(queryParams);
            var uriString = request.Endpoint.ToString();
            if (!uriString.EndsWith("/"))
            {
                uriString += "/";
            }
            uriString += resourcePath + "?" + queryString;
            
            return new Uri(uriString);
        }
        
        #endregion
        
        #region Multipart Operations
        /// <inheritdoc/>
        public MultipartUploadListing ListMultipartUploads(ListMultipartUploadsRequest listMultipartUploadsRequest)
        {
            ThrowIfNullRequest(listMultipartUploadsRequest);
            var cmd = ListMultipartUploadsCommand.Create(GetServiceClient(), _endpoint, 
                                                         CreateContext(HttpMethod.Get, listMultipartUploadsRequest.BucketName, null),
                                                         listMultipartUploadsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest initiateMultipartUploadRequest)
        {
            ThrowIfNullRequest(initiateMultipartUploadRequest);
            var cmd = InitiateMultipartUploadCommand.Create(GetServiceClient(), _endpoint, 
                                                         CreateContext(HttpMethod.Post, initiateMultipartUploadRequest.BucketName, initiateMultipartUploadRequest.Key),
                                                         initiateMultipartUploadRequest);
            return cmd.Execute();
        }
        
        /// <inheritdoc/>
        public void AbortMultipartUpload(AbortMultipartUploadRequest abortMultipartUploadRequest)
        {
            ThrowIfNullRequest(abortMultipartUploadRequest);
            var cmd = AbortMultipartUploadCommand.Create(GetServiceClient(), _endpoint, 
                                                         CreateContext(HttpMethod.Delete, abortMultipartUploadRequest.BucketName, abortMultipartUploadRequest.Key),
                                                         abortMultipartUploadRequest);
            using(cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>        
        public UploadPartResult UploadPart(UploadPartRequest uploadPartRequest)
        {
            ThrowIfNullRequest(uploadPartRequest);
            var cmd = UploadPartCommand.Create(GetServiceClient(), _endpoint,
                                                         CreateContext(HttpMethod.Put, uploadPartRequest.BucketName, uploadPartRequest.Key),
                                                         uploadPartRequest);   
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public UploadPartCopyResult UploadPartCopy(UploadPartCopyRequest uploadPartCopyRequest)
        {
            ThrowIfNullRequest(uploadPartCopyRequest);
            var cmd = UploadPartCopyCommand.Create(GetServiceClient(), _endpoint,
                CreateContext(HttpMethod.Put, uploadPartCopyRequest.TargetBucket, uploadPartCopyRequest.TargetKey),
                uploadPartCopyRequest);
            return cmd.Execute();
        }
        
        /// <inheritdoc/>                
        public PartListing ListParts(ListPartsRequest listPartsRequest)
        {
            ThrowIfNullRequest(listPartsRequest);
            var cmd = ListPartsCommand.Create(GetServiceClient(), _endpoint,
                                                          CreateContext(HttpMethod.Get, listPartsRequest.BucketName, listPartsRequest.Key),
                                                         listPartsRequest);   
            return cmd.Execute();            
        }
        
        /// <inheritdoc/>                
        public CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest completeMultipartUploadRequest)
        {
            ThrowIfNullRequest(completeMultipartUploadRequest);
            var cmd = CompleteMultipartUploadCommand.Create(GetServiceClient(), _endpoint,
                                                          CreateContext(HttpMethod.Post, completeMultipartUploadRequest.BucketName, completeMultipartUploadRequest.Key),
                                                          completeMultipartUploadRequest);
            return cmd.Execute();
        }
        
        #endregion

        #region Private Methods

        private IServiceClient GetServiceClient()
        {
            return _serviceClient;
        } 

        private ExecutionContext CreateContext(HttpMethod method, string bucket, string key)
        {
            var builder = new ExecutionContextBuilder();
            builder.Bucket = bucket;
            builder.Key = key;
            builder.Method = method;
            builder.Credentials = _credentials;
            builder.ResponseHandlers.Add(new ErrorResponseHandler());
            return builder.Build();
        }

        private void ThrowIfNullRequest<TRequestType> (TRequestType request)
        {
            if (request == null)
                throw new ArgumentNullException("request should not be null");
        }

        #endregion
    }
}
