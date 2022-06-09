using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TheBlogProject.Services
{
    public interface IImageService
    {
        //Works with IForm file
        //User selection
        Task<byte[]> EncodeImageAsync(IFormFile file);
        //used for static images - stored in project - ref path
        Task<byte[]> EncodeImageAsync(string fileName);
        //Display the image
        string DeCodeImage(byte[] data,string type);
        //Type of file
        string ContentType(IFormFile file);
        //Records the size of file
        int Size(IFormFile file);
    }
}
