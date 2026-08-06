using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file);

        Task DeleteAsync(string blobName);

        Task<Stream> DownloadAsync(string blobName);
    }
}
