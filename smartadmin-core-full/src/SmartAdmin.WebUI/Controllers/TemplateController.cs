using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace SmartAdmin.WebUI.Controllers
{
    public class TemplateController : Controller
    {
        public IActionResult TemplateFiles()
        {
            return View();
        }

        public IActionResult GetCompanyTemplateFile(string fileName)
        {
            string fileTypePath = GetFileTypePath(fileName);
            var filePath = Path.Combine(System.Environment.CurrentDirectory, @"OtherResources\Template files\"+ $"{fileTypePath}", fileName);
            byte[] thisFileBytes = System.IO.File.ReadAllBytes(filePath);
            FileContentResult file = new FileContentResult(thisFileBytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return file;
        }
        private string GetFileTypePath(string fileName)
        {
            if (fileName.EndsWith(".dotx"))
            {
                return @"Word\";
            }
            if (fileName.EndsWith(".potx"))
            {
                return @"Powerpoint\";
            }
            if (fileName.EndsWith(".xltx"))
            {
                return @"Excel\";
            }
            return "";
        }
    }
}
