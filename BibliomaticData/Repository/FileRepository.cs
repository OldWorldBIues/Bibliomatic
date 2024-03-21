using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Models;
using BibliomaticData.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO.Compression;
using BibliomaticData.Models.DTOs;
using Azure;

namespace BibliomaticData.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient clientContainer;

        public FileRepository(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
            clientContainer = this.blobServiceClient.GetBlobContainerClient("filestorage");
        }

        public async Task<BlobDTO> DeleteFile(string blobFilename)
        {
            BlobClient file = clientContainer.GetBlobClient(blobFilename);

            await file.DeleteIfExistsAsync();

            return new BlobDTO { Error = false, Status = $"File: {blobFilename} was deleted successfully" };
        }

        public async Task<List<BlobDTO>> DeleteFiles(IEnumerable<string> blobFilenames)
        {
            List<BlobDTO> blobDTOs = new List<BlobDTO>();

            foreach(string blobFilename in blobFilenames)
            {
                var dto = await DeleteFile(blobFilename);
                blobDTOs.Add(dto);
            }

            return blobDTOs;
        }

        public async Task<Blob?> DownloadFile(string blobFilename)
        {
            BlobClient file = clientContainer.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;
                string contentType = content.Value.Details.ContentType;

                return new Blob { Content = blobContent, Name = name, ContentType = contentType };
            }

            return null;
        }

        public async Task<Blob?> DownloadFiles(IEnumerable<string> blobFilenames)
        {
            var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var blobFilename in blobFilenames)
                {
                    var fileInArchive = zipArchive.CreateEntry(blobFilename, CompressionLevel.Optimal);
                    var file = clientContainer.GetBlobClient(blobFilename);

                    using (var entryStream = fileInArchive.Open())
                    {
                        await file.DownloadToAsync(entryStream);
                    }
                }
            }

            memoryStream.Position = 0;
            return new Blob { Content = memoryStream, Name = "files.zip", ContentType = "application/zip" };
        }

        public async Task<List<Blob>> FilesList()
        {
            List<Blob> blobsList = new List<Blob>();

            await foreach (var file in clientContainer.GetBlobsAsync())
            {
                string uri = clientContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = Path.Combine(uri, name);

                blobsList.Add(new Blob
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            return blobsList;
        }

        private string GenerateSasUriForFile(BlobClient blobClient)
        {
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTime.UtcNow.AddDays(1))
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
            };

            return blobClient.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task<List<Blob>> FilesList(IEnumerable<string> filenames)
        {
            List<Blob> blobsList = new List<Blob>();

            foreach (var filename in filenames)
            {
                var file = clientContainer.GetBlobClient(filename);

                if(await file.ExistsAsync())
                {                   
                    blobsList.Add(new Blob
                    {
                        Uri = GenerateSasUriForFile(file),                        
                        Name = file.Name
                    });
                }
            }

            return blobsList;
        }

        public async Task<BlobDTO> UploadFile(IFormFile blob)
        {
            BlobDTO response = new();

            try
            {                
                BlobClient client = clientContainer.GetBlobClient(blob.FileName);                
                
                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data, true);
                }

                response.Status = $"File {blob.FileName} uploaded successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.ToString();
                response.Blob.Name = client.Name;               
            }
            catch(Exception)
            {
                response.Status = $"File {blob.FileName} upload was failed";
                response.Error = true;   
                response.Blob.Name = blob.FileName;
            }

            return response;
        }

        public async Task<List<BlobDTO>> UploadFiles(IEnumerable<IFormFile> files)
        {
            List<BlobDTO> blobDTOs = new List<BlobDTO>();

            foreach (var file in files)
            {
                var dto = await UploadFile(file);
                blobDTOs.Add(dto);
            }

            return blobDTOs;
        }
    }
}
