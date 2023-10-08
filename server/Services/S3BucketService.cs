using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galleria.Services;

public class S3BucketService
{
    private readonly IConfigurationSection JwtSettings;

    public S3BucketService(IConfigurationSection JwtSettings)
    {
        this.JwtSettings = JwtSettings;
    }

    // upload

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