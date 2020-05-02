using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPersonBussines personBussines;
        private readonly ICacheUtility cache;

        public FileController(IWebHostEnvironment hostingEnvironment, EsferaContext context, ICacheUtility cache)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.cache = cache;

            this.personBussines = new PersonBussines(context, this.cache);
        }

        // GET: File
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            string tempPath = Path.GetTempFileName();
            var userName = User.FindFirst(ClaimTypes.Name).Value;

            using (FileStream stream = System.IO.File.Create(tempPath))
            {
                file.CopyToAsync(stream);
            }

            List<ApplicationMessage> processMessages = this.personBussines.UploadVinculatedPersons(tempPath,userName);

            return this.RedirectToAction("Index");
        }

    }


}
