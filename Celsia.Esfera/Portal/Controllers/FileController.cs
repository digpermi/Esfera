using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPersonBussines personBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<FileController> logger;

        public FileController(EsferaContext context, ICacheUtility cache, ILogger<FileController> logger)
        {
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
        public ActionResult Index(FileViewModel fileViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                    string tempPath = Path.GetTempFileName();

                    using (FileStream stream = System.IO.File.Create(tempPath))
                    {
                        fileViewModel.UploadFile.CopyTo(stream);
                    }

                    List<ApplicationMessage> processMessages = this.personBussines.UploadVinculatedPersons(tempPath, userName);

                    fileViewModel.Messages = processMessages;
                    fileViewModel.TotalRows = processMessages.Count;        
                }
            }
            catch (Exception exec)
            {
                fileViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, fileViewModel.UserMesage.Text);
            }

            return this.View(fileViewModel);
        }
    }
}
