using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;

namespace Galleria.Services;

public class S3BucketService
{
    private readonly string accessKey;
    private readonly string secretKey;
    private readonly string bucketName;
    private RegionEndpoint BucketRegion = RegionEndpoint.USEast1;

    private AmazonS3Client S3Client;
    public S3BucketService(IConfigurationSection JwtSettings)
    {
        accessKey = "AKIA27OYWTUEP2D4WJ6X";//wtSettings["AWS-Key"];
        secretKey = "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";
        bucketName = "galleria-storage";// JwtSettings["AWS-S3-Bucket"];

        S3Client = new AmazonS3Client(accessKey, secretKey, BucketRegion);
    }


    // upload
    public void upload()
    {
        Console.WriteLine($"Upload function fired, bucket: {bucketName}");

        TransferUtility fileTransferUtility = new TransferUtility(S3Client);
        fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
        {
            BucketName = bucketName,
            FilePath = "C:\\Users\\TG Chipoyera\\Pictures\\Screenshots\\Screenshot 2023-10-06 104049.png",
            StorageClass = S3StorageClass.Standard,
            PartSize = 74109,
            Key = "key",
        }).GetAwaiter().GetResult();

        Console.WriteLine("Uploaded");
    }

    // download

    // delete
}

public static class S3BucketServiceExtension
{
    public static IServiceCollection AddS3Bucket(this IServiceCollection serviceCollection, IConfigurationSection JwtSettings)
    {
        return serviceCollection.AddSingleton(_ => new S3BucketService(JwtSettings));
    }
}