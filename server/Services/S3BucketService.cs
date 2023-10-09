using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;

namespace Galleria.Services;

public class S3BucketService
{
    private readonly string accessKey;
    private readonly string secretKey;
    private readonly string bucketName;
    private RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
    private TransferUtility Transfer;

    private AmazonS3Client S3Client;
    public S3BucketService(IConfigurationSection JwtSettings)
    {
        accessKey = "AKIA27OYWTUEP2D4WJ6X";//wtSettings["AWS-Key"];
        secretKey = "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";
        bucketName = "galleria-storage";// JwtSettings["AWS-S3-Bucket"];

        S3Client = new AmazonS3Client(accessKey, secretKey, BucketRegion);

        Transfer = new TransferUtility(S3Client);
    }


    // upload
    public async Task<IFileInfo> Upload(IWebHostEnvironment env, IFormFile file)
    {
        string Key = GenerateRandomKey(8);
        string localFilePath = Path.Combine(env.ContentRootPath, "static", Key);

        // save the file in local static folder
        using (var localFileStream = new FileStream(localFilePath, FileMode.Create))
        {
            await file.CopyToAsync(localFileStream);
        }

        var localFile = env.ContentRootFileProvider.GetFileInfo(Path.Combine("static", Key));

        // Upload to s3
        await Transfer.UploadAsync(new TransferUtilityUploadRequest
        {
            BucketName = bucketName,
            FilePath = localFile.PhysicalPath,
            StorageClass = S3StorageClass.Standard,
            PartSize = localFile.Length,
            Key = Key,
        });

        // return the file
        return localFile;
    }

    // download
    public IFileInfo Download(IWebHostEnvironment env, string key)
    {
        var ContentRootFileProvider = env.ContentRootFileProvider;
        string filePath = Path.Combine("static", key);

        if (!ContentRootFileProvider.GetFileInfo(filePath).Exists)
        {
            // get from S3 bucket
            Transfer.Download(new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = key,
                FilePath = $"{env.ContentRootPath}/static/{key}"
            });
        }

        return env.ContentRootFileProvider.GetFileInfo($"static/{key}");
    }

    // delete
    public async void Delete(IWebHostEnvironment env, string key)
    {
        // Delete from S3
        await S3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        });

        // Delete from local static file
        File.Delete(Path.Combine(env.ContentRootPath, "static", key));
    }

    private static string GenerateRandomKey(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();

        // Use the random object to generate a sequence of random characters.
        string randomString = new string(Enumerable.Repeat(characters, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return randomString;
    }
}

public static class S3BucketServiceExtension
{
    public static IServiceCollection AddS3Bucket(this IServiceCollection serviceCollection, IConfigurationSection JwtSettings)
    {
        return serviceCollection.AddSingleton(_ => new S3BucketService(JwtSettings));
    }
}