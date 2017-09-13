using System;

namespace Aliyun.OpenServices.OpenStorageService.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Aliyun SDK for .NET Samples!");

            try
            {
                string FileAbsUrl = FileIO_ALIYUN.FileIO.PutOjbect("gains-usercenter-test", @"D:\WorkRoot\svnRoot\FirstFrame\07.FileIO\aliyun_dotnet_sdk_20150115\sample\bin\Debug\test.jpg");
                Console.WriteLine(FileAbsUrl);
                // 1. Create bucket sample
                //string bucketName = "create-bucket-sample";
                //CreateBucketSample.CreateBucket(bucketName);

                // 2. List bucket sample
                //ListBucketsSample.ListBuckets();

                // 3. Set bucket cors sample
                //SetBucketCorsSample.SetBucketCors();

                // 4. Set bucket acl sample
                //SetBucketAclSample.SetBucketAcl();

                // 5. CName sample
                //CNameSample.SetRootDomains();

                // 6. Get/Set bucket referer list sample
                //SetBucketRefererSample.SetBucketReferer();

                // 7. Put object sample
                //PutObjectSample.PutObject();

                // 8. Delete objects sample
                //DeleteObjectsSample.DeleteObjects();

                // 9. Upload multipart sample
                //string bucketName = "gains-usercenter-test";
                //string objectName = "test.jpg";
                //string fileToUpload = @"D:\WorkRoot\svnRoot\FirstFrame\07.FileIO\aliyun_dotnet_sdk_20150115\sample\bin\Debug\test.jpg";
                //int partSize = 5 * 1024 * 1024;  // 5MB
                //MultipartUploadSample.UploadMultipart(bucketName, objectName, fileToUpload, partSize);

                // 10. Upload multipart copy sample 
                //string targetName = "<target bucket>";
                //string targetKey = "<target key>";
                //string sourceBucket = "<source bucket>";
                //string sourceKey = "<source key>";
                //int partSize = 5 * 1024 * 1024;  // 5MB
                //MultipartUploadSample.UploadMultipartCopy(targetName, targetKey, sourceBucket, sourceKey, partSize);

                // 11. Get object by range sample
                //GetObjectByRangeSample.GetObjectPartly();

                // 12. Get object once sample.
                //GetObjectSample.GetObject();

                // 13. Url signature sample
                //UrlSignatureSample.genUrl();

                // 14. Copy object sample
                //CopyObjectSample.CopyObject();

                // 15. Create empty folder
                //CreateEmptyFolderSample.CreateEmptyFolder();

            }
            catch (OssException ex)
            {
                Console.WriteLine("Exception: " + ex.Message + ", Errorcode: " + ex.ErrorCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception message: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}