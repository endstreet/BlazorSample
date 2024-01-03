using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace Trevali.Reports
{
    [Route("/")]
    public class HomeController : Controller
    {
        public object Index() => Resource("index.html");

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{file}")]

        public object Resource(string file)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, file);

            if (!System.IO.File.Exists(filePath))
                return new NotFoundResult();

            if (Path.GetExtension(file) == ".html")
                return new ContentResult() { Content = System.IO.File.ReadAllText(filePath), ContentType = "text/html" };

            var resFile = System.IO.File.ReadAllBytes(filePath);

            if (Path.GetExtension(file) == ".ico")
                return new FileContentResult(resFile, "image/x-icon") { FileDownloadName = file };

            return new FileContentResult(resFile, GetType(file)) { FileDownloadName = file };
        }

        private string GetType(string file)
        {
            if (file.EndsWith(".css"))
                return "text/css";

            if (file.EndsWith(".js"))
                return "text/javascript";

            return "text/html";
        }

        [HttpGet("reports")]
        public ActionResult Reports()
        {
            string[] validExtensions = { ".rdl", ".rdlx", ".rdlx-master" };

            string pathToReport = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            var reportsList = Directory.GetFiles(pathToReport);

            return new ObjectResult(reportsList
                .Where(x => validExtensions.Any(ext => x.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
                .Select(x => x)
                .ToArray());
        }
    }
}
