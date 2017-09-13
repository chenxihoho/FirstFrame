/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace Aliyun.OpenServices.OpenStorageService
{
    /// <summary>
    /// 阿里云开放存储服务（Open Storage Service， OSS）的访问接口。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 阿里云存储服务（Open Storage Service，简称OSS），是阿里云对外提供的海量，安全，低成本，
    /// 高可靠的云存储服务。用户可以通过简单的REST接口，在任何时间、任何地点上传和下载数据，
    /// 也可以使用WEB页面对数据进行管理。
    /// 基于OSS，用户可以搭建出各种多媒体分享网站、网盘、个人企业数据备份等基于大规模数据的服务。
    /// </para>
    /// <para>
    /// OSS的访问地址： http://oss.aliyuncs.com
    /// </para>
    /// <para>
    /// OSS的web体验地址：http://oss.aliyun.com/
    /// </para>
    /// </remarks>
    public interface IOss
    {
        #region Bucket Operations

        /// <summary>
        /// 在OSS中创建一个新的Bucket。
        /// </summary>
        /// <param name="bucketName">要创建的Bucket的名称。</param>
        /// <returns><see cref="Bucket" />对象。</returns>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.CreateBucketSample"]/*'/>
        Bucket CreateBucket(string bucketName);

        /// <summary>
        /// 在OSS中删除一个Bucket。
        /// </summary>
        /// <param name="bucketName">要删除的Bucket的名称。</param>
        void DeleteBucket(string bucketName);

        /// <summary>
        /// 返回请求者拥有的所有<see cref="Bucket" />的列表。
        /// </summary>
        /// <returns>请求者拥有的所有<see cref="Bucket" />的列表。</returns>
        IEnumerable<Bucket> ListBuckets();

        /// <summary>
        /// 分页返回请求者拥有的<see cref="Bucket" />的列表。
        /// </summary>
        /// <returns><see cref="ListBucketsResult" />。</returns>
        ListBucketsResult ListBuckets(ListBucketsRequest listBucketsRequest);

        /// <summary>
        /// 设置指定<see cref="Bucket" />的访问控制列表<see cref="AccessControlList" />。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <param name="acl"><see cref="CannedAccessControlList" />枚举中的访问控制列表。</param>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.SetBucketAclSample"]/*'/>
        void SetBucketAcl(string bucketName, CannedAccessControlList acl);

        /// <summary>
        /// 设置指定<see cref="Bucket" />的访问控制列表<see cref="AccessControlList" />。
        /// </summary>
        /// <param name="setBucketAclRequest"></param>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.SetBucketAclSample"]/*'/>
        void SetBucketAcl(SetBucketAclRequest setBucketAclRequest);

        /// <summary>
        /// 获取指定<see cref="Bucket" />的访问控制列表<see cref="AccessControlList" />。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <returns>访问控制列表<see cref="AccessControlList" />的实例。</returns>
        AccessControlList GetBucketAcl(string bucketName);


        /// <summary>
        /// 设置指定<see cref="Bucket" />的跨域资源共享(CORS)的规则，并覆盖原先所有的CORS规则。
        /// </summary>
        /// <param name="setBucketCorsRequest"></param>
        void SetBucketCors(SetBucketCorsRequest setBucketCorsRequest);

        /// <summary>
        /// 获取指定<see cref="Bucket" />的CORS规则。
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        IList<CORSRule> GetBucketCors(string bucketName);

        /// <summary>
        /// 关闭指定<see cref="Bucket" />对应的CORS功能并清空所有规则。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        void DeleteBucketCors(string bucketName);

        /// <summary>
        /// 设置<see cref="Bucket" />的访问日志记录功能。
        /// 这个功能开启后，OSS将自动记录访问这个<see cref="Bucket" />请求的详细信息，并按照用户指定的规则，
        /// 以小时为单位，将访问日志作为一个Object写入用户指定的<see cref="Bucket" />。
        /// </summary>
        /// <param name="setBucketLoggingRequest"></param>
        void SetBucketLogging(SetBucketLoggingRequest setBucketLoggingRequest);

        /// <summary>
        /// 查看<see cref="Bucket" />的访问日志配置。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <returns></returns>
        BucketLoggingResult GetBucketLogging(string bucketName);
        
        /// <summary>
        /// 关闭<see cref="Bucket" />的访问日志记录功能。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        void DeleteBucketLogging(string bucketName);

        /// <summary>
        /// 将一个<see cref="Bucket" />设置成静态网站托管模式。
        /// </summary>
        /// <param name="setBucketWebSiteRequest"></param>
        void SetBucketWebsite(SetBucketWebsiteRequest setBucketWebSiteRequest);
        

        /// <summary>
        /// 获取<see cref="Bucket" />的静态网站托管状态。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <returns></returns>
        BucketWebsiteResult GetBucketWebsite(string bucketName);


        /// <summary>
        /// 关闭<see cref="Bucket" />的静态网站托管模式。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        void DeleteBucketWebsite(string bucketName);


        /// <summary>
        /// 设置<see cref="Bucket" />的Referer访问白名单和是否允许referer字段为空。
        /// </summary>
        /// <param name="setBucketRefererRequest"></param>
        void SetBucketReferer(SetBucketRefererRequest setBucketRefererRequest);

        /// <summary>
        /// 查看<see cref="Bucket" />的Referer配置。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <returns></returns>
        RefererConfiguration GetBucketReferer(string bucketName);

        #endregion

        #region Object Operations

        /// <summary>
        /// 列出指定<see cref="Bucket" />下<see cref="OssObject" />的摘要信息<see cref="OssObjectSummary" />。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <returns><see cref="OssObject" />的列表信息。</returns>
        ObjectListing ListObjects(string bucketName);

        /// <summary>
        /// 列出指定<see cref="Bucket" />下其Key以prefix为前缀<see cref="OssObject" />
        /// 的摘要信息<see cref="OssObjectSummary" />。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <param name="prefix">限定返回的<see cref="OssObject.Key" />必须以此作为前缀。</param>
        /// <returns><see cref="OssObject" />的列表信息。</returns>
        ObjectListing ListObjects(string bucketName, string prefix);

        /// <summary>
        /// 列出指定<see cref="Bucket" />下<see cref="OssObject" />的摘要信息<see cref="OssObjectSummary" />。
        /// </summary>
        /// <param name="listObjectsRequest">请求参数。</param>
        /// <returns><see cref="OssObject" />的列表信息。</returns>
        ObjectListing ListObjects(ListObjectsRequest listObjectsRequest);

        /// <summary>
        /// 上传指定的<see cref="OssObject" />到指定的OSS<see cref="Bucket" />。
        /// </summary>
        /// <param name="bucketName">指定的<see cref="Bucket" />名称。</param>
        /// <param name="key"><see cref="OssObject" />的<see cref="OssObject.Key" />。</param>
        /// <param name="content"><see cref="OssObject" />的<see cref="OssObject.Content" />。</param>
        /// <returns><see cref="PutObjectResult" />实例。</returns>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.PutObjectSample"]/*'/>
        PutObjectResult PutObject(string bucketName, string key, Stream content);

        /// <summary>
        /// 上传指定的<see cref="OssObject" />到指定的OSS<see cref="Bucket" />。
        /// </summary>
        /// <param name="bucketName">指定的<see cref="Bucket" />名称。</param>
        /// <param name="key"><see cref="OssObject" />的<see cref="OssObject.Key" />。</param>
        /// <param name="content"><see cref="OssObject" />的<see cref="OssObject.Content" />。</param>
        /// <param name="metadata"><see cref="OssObject" />的元信息。</param>
        /// <returns><see cref="PutObjectResult" />实例。</returns>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.PutObjectSample"]/*'/>
        PutObjectResult PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata);

        /// <summary>
        /// 上传指定的<see cref="OssObject" />到指定的OSS<see cref="Bucket" />。
        /// </summary>
        /// <param name="bucketName">指定的<see cref="Bucket" />名称。</param>
        /// <param name="key"><see cref="OssObject" />的<see cref="OssObject.Key" />。</param>
        /// <param name="fileToUpload">指定上传文件的路径。</param>
        /// <returns><see cref="PutObjectResult" />实例。</returns>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.PutObjectSample"]/*'/>
        PutObjectResult PutObject(string bucketName, string key, string fileToUpload);

        /// <summary>
        /// 上传指定的<see cref="OssObject" />到指定的OSS<see cref="Bucket" />。
        /// </summary>
        /// <param name="bucketName">指定的<see cref="Bucket" />名称。</param>
        /// <param name="key"><see cref="OssObject" />的<see cref="OssObject.Key" />。</param>
        /// <param name="fileToUpload">指定上传文件的路径。</param>
        /// <param name="metadata"><see cref="OssObject" />的元信息。</param>
        /// <returns><see cref="PutObjectResult" />实例。</returns>
        /// <include file='oss_comments_include.xml' path='Comments/Member[@name="OSS.SampleCode.PutObjectSample"]/*'/>
        PutObjectResult PutObject(string bucketName, string key, string fileToUpload, ObjectMetadata metadata);

        /// <summary>
        /// 从指定的OSS<see cref="Bucket" />中获取指定的<see cref="OssObject" />。
        /// </summary>
        /// <param name="bucketName">要获取的<see cref="OssObject"/>所在的<see cref="Bucket" />的名称。</param>
        /// <param name="key">要获取的<see cref="OssObject"/>的<see cref="OssObject.Key"/>。</param>
        /// <returns><see cref="OssObject" />实例。</returns>
        OssObject GetObject(string bucketName, string key);

        /// <summary>
        /// 从指定的OSS<see cref="Bucket" />中获取满足请求参数<see cref="GetObjectRequest"/>的<see cref="OssObject" />。
        /// </summary>
        /// <param name="getObjectRequest">请求参数。</param>
        /// <returns><see cref="OssObject" />实例。使用后需要释放此对象以释放HTTP连接。</returns>
        OssObject GetObject(GetObjectRequest getObjectRequest);

        /// <summary>
        /// 从指定的OSS<see cref="Bucket" />中获取指定的<see cref="OssObject" />，
        /// 并导出到指定的输出流。
        /// </summary>
        /// <param name="getObjectRequest">请求参数。</param>
        /// <param name="output">输出流。</param>
        /// <returns>导出<see cref="OssObject" />的元信息。</returns>
        ObjectMetadata GetObject(GetObjectRequest getObjectRequest, Stream output);

        /// <summary>
        /// 获取<see cref="OssObject" />的元信息。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <param name="key"><see cref="OssObject.Key" />。</param>
        /// <returns><see cref="OssObject" />的元信息。</returns>
        ObjectMetadata GetObjectMetadata(string bucketName, string key);

        /// <summary>
        /// 删除指定的<see cref="OssObject" />。
        /// </summary>
        /// <param name="bucketName"><see cref="Bucket" />的名称。</param>
        /// <param name="key"><see cref="OssObject.Key" />。</param>
        void DeleteObject(string bucketName, string key);

        /// <summary>
        /// 批量删除指定的<see cref="OssObject" />。
        /// </summary>
        /// <param name="deleteObjectsRequest">请求参数</param>
        /// <returns>返回结果</returns>
        DeleteObjectsResult DeleteObjects(DeleteObjectsRequest deleteObjectsRequest);
        
        /// <summary>
        /// 复制一个Object
        /// </summary>
        /// <param name="copyObjectRequst">请求参数</param>
        /// <returns>返回的结果</returns>
        CopyObjectResult CopyObject(CopyObjectRequest copyObjectRequst);

        #endregion
        
        #region Generate URL
        
        /// <summary>
        /// 生成一个签名的Uri。
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>访问<see cref="OssObject" />的Uri。</returns>
        Uri GeneratePresignedUri(GeneratePresignedUriRequest request);
        
        /// <summary>
        /// 使用默认过期时间（自现在起15分钟后）生成一个用HTTP GET方法访问<see cref="OssObject" />的Uri。
        /// </summary>
        /// <param name="bucketName">Bucket名称。</param>
        /// <param name="key">Object的Key</param>
        /// <returns>访问<see cref="OssObject" />的Uri。</returns>
        Uri GeneratePresignedUri(string bucketName, string key);

        /// <summary>
        /// 使用指定过期时间生成一个用HTTP GET方法访问<see cref="OssObject" />的Uri。
        /// </summary>
        /// <param name="bucketName">Bucket名称。</param>
        /// <param name="key">Object的Key</param>
        /// <param name="expiration">Uri的超时时间。</param>
        /// <returns>访问<see cref="OssObject" />的Uri。</returns>
        Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration);
        
        
        /// <summary>
        /// 使用默认过期时间（自现在起15分钟后）生成一个用指定方法访问<see cref="OssObject" />的Uri。
        /// </summary>
        /// <param name="bucketName">Bucket名称。</param>
        /// <param name="key">Object的Key</param>
        /// <param name="method">访问Uri的方法</param>
        /// <returns>访问<see cref="OssObject" />的Uri。</returns>
        Uri GeneratePresignedUri(string bucketName, string key, SignHttpMethod method);

        /// <summary>
        /// 使用指定过期时间生成一个用指定方法访问<see cref="OssObject" />的Uri。
        /// </summary>
        /// <param name="bucketName">Bucket名称。</param>
        /// <param name="key">Object的Key</param>
        /// <param name="expiration">Uri的超时时间。</param>
        /// <param name="method">访问Uri的方法</param>
        /// <returns>访问<see cref="OssObject" />的Uri。</returns>
        Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration, SignHttpMethod method);
        
        #endregion
        
        #region Multipart Operations
        /// <summary>
        /// 列出所有执行中的Multipart Upload事件
        /// </summary>
        /// <param name="listMultipartUploadsRequest">请求参数</param>
        /// <returns><see cref="MultipartUploadListing" />的列表信息。</returns>
        MultipartUploadListing ListMultipartUploads(ListMultipartUploadsRequest listMultipartUploadsRequest);
        
        /// <summary>
        /// 初始化一个Multipart Upload事件
        /// </summary>
        /// <param name="initiateMultipartUploadRequest">请求参数</param>
        /// <returns>初始化结果</returns>
        InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest initiateMultipartUploadRequest);
        
        /// <summary>
        /// 中止一个Multipart Upload事件
        /// </summary>
        /// <param name="abortMultipartUploadRequest">请求参数</param>
        void AbortMultipartUpload(AbortMultipartUploadRequest abortMultipartUploadRequest);
        
        /// <summary>
        /// 上传某分块的数据
        /// </summary>
        /// <param name="uploadPartRequest">请求参数</param>
        /// <returns>分块上传结果</returns>
        UploadPartResult UploadPart(UploadPartRequest uploadPartRequest);

        /// <summary>
        /// 从某一已存在的Object中拷贝数据来上传某分块。
        /// </summary>
        /// <param name="uploadPartCopyRequest">请求参数</param>
        /// <returns>分块上传结果。</returns>
        UploadPartCopyResult UploadPartCopy(UploadPartCopyRequest uploadPartCopyRequest);
        
        /// <summary>
        /// 列出已经上传成功的Part
        /// </summary>
        /// <param name="listPartsRequest">请求参数</param>
        /// <returns><see cref="PartListing" />的列表信息。</returns>
        PartListing ListParts(ListPartsRequest listPartsRequest);
 
        /// <summary>
        /// 完成分块上传
        /// </summary>
        /// <param name="completeMultipartUploadRequest">请求参数</param>
        /// <returns><see cref="CompleteMultipartUploadResult" />的列表信息。</returns>        
        CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest completeMultipartUploadRequest);
        #endregion
    }
}
