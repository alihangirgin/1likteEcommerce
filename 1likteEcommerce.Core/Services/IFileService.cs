using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Services
{
    public interface IFileService
    {
        Task UploadFileAsync(byte[] bytes, string fileName);
        Task<byte[]?> GetFileBytesAsync(string fileName);
    }
}
