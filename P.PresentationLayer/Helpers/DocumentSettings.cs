using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace P.PresentationLayer.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {

            //Get Located Folder Path

            string FolderPath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            //Get FileName and Make It Uniq

            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            //Get FilePath[FolderPath+FileName]

            string FilePath=Path.Combine(FolderPath,FileName);

            //Save File As Streams

            using var fs=new FileStream(FilePath,FileMode.Create);
            file.CopyTo(fs);

            //Return FileName

            return FileName;
            
        }
        public static void DeleteFile(string FileName,string FolderName)
        {
            //Get File Path
            string FilePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",FolderName,FileName);
            //Check 
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
