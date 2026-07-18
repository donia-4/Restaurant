using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Application.Common.Dtos;


namespace Restaurant.Application.Common.Interfaces.Services
{
    public interface IFileService
    {
        Task<UploadFileResponse> UploadAsync(
            FileUpload file,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            string publicId,
            CancellationToken cancellationToken = default);
    }
}
