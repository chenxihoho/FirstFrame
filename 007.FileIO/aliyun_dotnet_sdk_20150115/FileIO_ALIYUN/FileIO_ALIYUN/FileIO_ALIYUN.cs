using Aliyun.OpenServices.OpenStorageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIO_ALIYUN
{
    public static class FileIO
    {
        public static string PutOjbect(string accessId, string accessKey, string endpoint, string BucketName, string FilePath)
        {
            try
            {
                string FolderName = DateTime.Now.ToString("yyyyMM");
                CreateEmptyFolder.CreateFolder(accessId, accessKey, endpoint, BucketName, FolderName);
                
                string bucketName = BucketName;
                string objectName = DateTime.Now.ToString("yyyyMMddhhmmssfffffff") + Path.GetExtension(FilePath);
                int partSize = 5 * 1024 * 1024;  // 5MB
                MultipartUploadSample._ossClient = new OssClient(endpoint, accessId, accessKey);
                return MultipartUploadSample.UploadMultipart(bucketName, FolderName + "/" + objectName, FilePath, partSize);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Exception: " + ex.Message + ", Errorcode: " + ex.ErrorCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception message: " + ex.Message);
            }
            return string.Empty;
        }
    }
}
