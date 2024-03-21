using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyReservations.Interfaces;
using System;
using System.IO;

namespace MyReservations.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private const string imagesFolder = "uploads";

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string SaveImage(IFormFile newImage, string existingImagePath)
        {
            if (newImage == null) return existingImagePath;
          
            string uploadDir = Path.Combine(_environment.WebRootPath, imagesFolder);

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string filePath = Path.Combine(uploadDir, newImage.FileName);


            if (!Path.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newImage.CopyTo(stream);
                }
            }

            return $"/{imagesFolder}/{newImage.FileName}";
        }
    }
}

    

       