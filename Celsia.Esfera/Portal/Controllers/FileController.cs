using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPersonBussines personBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<FileController> logger;

        public FileController(IWebHostEnvironment hostingEnvironment, EsferaContext context, ICacheUtility cache, ILogger<FileController> logger)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.logger = logger;
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

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

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

                    List<ApplicationMessage> processMessages = this.personBussines.UploadVinculatedPersons(tempPath, userName);

                    viewModel.Messages = processMessages;
                    viewModel.TotalRows = processMessages.Count;
                }
            }
            catch (Exception exec)
            {
                viewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, viewModel.UserMesage.Text);
            }

            return this.View(viewModel);
        }

    }


}
