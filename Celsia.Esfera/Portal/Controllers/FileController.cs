using System.IO;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPersonBussines personBussines;

        public FileController(IWebHostEnvironment hostingEnvironment, EsferaContext context)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.personBussines = new PersonBussines(context);
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

            using (FileStream stream = System.IO.File.Create(tempPath))
            {
                file.CopyToAsync(stream);
            }

            this.personBussines.UploadVinculatedPersons(tempPath);

            return this.RedirectToAction("Index");
        }

        // GET: File/Details/5
        public ActionResult Details(int id)
        {
            return this.View();
        }

        // GET: File/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: File/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }

        // GET: File/Edit/5
        public ActionResult Edit(int id)
        {
            return this.View();
        }

        // POST: File/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }

        // GET: File/Delete/5
        public ActionResult Delete(int id)
        {
            return this.View();
        }

        // POST: File/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }
    }


}
