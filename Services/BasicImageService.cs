using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TheBlogProject.Services
{
    public class BasicImageService : IImageService
    {
        public string ContentType(IFormFile file)
        {
            //return null or empty string
            return file?.ContentType;
        }

        public string DeCodeImage(byte[] data, string type)
        {
            if (data == null || type == null)
            {
                return null;
            }
            //Get out of the database
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();


        }

        public async Task<byte[]> EncodeImageAsync(string fileName)
        {
            //handles the path
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{fileName}";
            return await File.ReadAllBytesAsync(file);
        }

        public int Size(IFormFile file)
        {
            return (int)file?.Length;
        }
    }
}
