using Microsoft.AspNetCore.Http;
using System;
using System.IO;


namespace DemoPL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //1. Get Located Folder Path
            // var folderPath = "C:\\Users\\moham\\OneDrive\\Desktop\\ASP MVC\\Demo\\DemoPL\\wwwroot\\Imgs\\";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);


            //2. Get File Name
            var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";

            //3. Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            //4. Save File As Streams
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DelteFile(string folderName, string fileName) 
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName, fileName);

            if(File.Exists(filePath)) 
               File.Delete(filePath);
            

        }
    }
}

