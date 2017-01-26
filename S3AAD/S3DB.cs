using System;
using System.IO;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3AAD
{
    public class S3DB
    {
        private static AmazonS3Client client;

        public static void Init(string accessKey, string accessSecret, string regionName)
        {
            client = new AmazonS3Client(
                accessKey, accessSecret, RegionEndpoint.GetBySystemName(regionName));
        }

        internal static bool SaveDocument(string bucketName, string key, RemoteDocument document, bool publicRead = false)
        {
            if (client == null)
                throw new InvalidOperationException("not initialized");

            var resp = client.PutObject(new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                ContentBody = document.AsJson(),

                CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.NoACL
            });

            return resp.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static RemoteDocument CreateDocument(string bucketName, string key, bool publicRead = false)
        {
            if (client == null)
                throw new InvalidOperationException("not initialized");

            try
            {
                var document = new RemoteDocument(bucketName, key)
                {
                    publicRead = publicRead
                };

                SaveDocument(bucketName, key, document);

                return document;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public static RemoteDocument FindDocument(string bucketName, string key)
        {
            if (client == null)
                throw new InvalidOperationException("not initialized");

            try
            { 
                var resp = client.GetObject(bucketName, key);
                var reader = new StreamReader(resp.ResponseStream);

                return new RemoteDocument(reader.ReadToEnd());
            }
            catch (JsonFx.Serialization.DeserializationException e)
            {
                throw new InvalidOperationException(bucketName + "::" + key + " is not a S3AAD document.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
