using Microsoft.AspNetCore.Http;

namespace MyReservations.Interfaces
{
    public interface IFileUploadService
    {       
        string? SaveImage(IFormFile eventImages1, string? eventImages2);
    }
}
        


  