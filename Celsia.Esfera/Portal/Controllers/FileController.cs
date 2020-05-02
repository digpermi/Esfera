using System.Collections.Generic;
using System.IO;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.ViewModels;
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
            FileViewModel viewModel = new FileViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(IFormFile file)
        {
            FileViewModel viewModel = new FileViewModel();

            if (file == null)
            {
                ModelState.AddModelError("File", "Campo requerido");
            }
            else 
            {
                string tempPath = Path.GetTempFileName();

                using (FileStream stream = System.IO.File.Create(tempPath))
                {
                    file.CopyToAsync(stream);
                }

                List<ApplicationMessage> processMessages = this.personBussines.UploadVinculatedPersons(tempPath);

                viewModel.Messages = processMessages;
                viewModel.TotalRows = processMessages.Count;
            }

            return this.View(viewModel);
        }

    }


}
