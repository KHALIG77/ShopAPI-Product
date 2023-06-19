using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Shop.Data.FileManager
{
    public static class FileManager
    {
        public static string SaveImage(string rootPath,string folder,IFormFile file)
        { 

            string newPath=Guid.NewGuid().ToString()+(file.FileName.Length<=64?file.FileName:(file.FileName.Substring(file.FileName.Length-64)));
            string path=Path.Combine(rootPath,folder,newPath);
            using(FileStream stream =new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newPath;
        }
        public static bool Delete(string rootPath,string folder,string fileName)
        {
            string path =Path.Combine(rootPath,folder,fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;

        }
    }
}
