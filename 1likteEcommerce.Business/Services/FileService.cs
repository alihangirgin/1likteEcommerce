using _1likteEcommerce.Core.Dtos;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Business.Services
{
    public class FileService : IFileService
    {
        public async Task UploadFileAsync(byte[] bytes, string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, fileName);
            await File.WriteAllBytesAsync(filePath, bytes);
        }

        public async Task<byte[]?> GetFileBytesAsync(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            return null;
        }
    }
}
