
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> options) : IBlobStorageService
{
    private readonly BlobStorageSettings _settings = options.Value;

    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);

        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.LogosContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(data);

        var blobUrl = blobClient.Uri.ToString();
        return blobUrl;
    }

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null)
        {
            return null;
        }

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _settings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);

        var sasToken = sasBuilder
            .ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _settings.AccountKey))
            .ToString();

        return $"{blobUrl}?{sasToken}";
    }

    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}
