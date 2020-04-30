using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Entities.Models;
using Utilities.File;
using Bussines.Bussines;
using Bussines;

namespace Portal.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileBussines fileBussines;

        public FileController(IWebHostEnvironment hostingEnvironment, EsferaContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            this.fileBussines = new FileBussines(context);
        }

        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            var fileName = file.FileName;
            
            string filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
            }

            UploadRecordsToDataBase(filePath);

            return RedirectToAction("Index");
        }

        // GET: File/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: File/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: File/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: File/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: File/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: File/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: File/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void UploadRecordsToDataBase(string fileName)
        {
            var records = new List<CustomerViewModel>();
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string resul = @"C:\Users\User\Documents\doc\Andromeda.csv";

            CsvFile<Customer> csvFile = new CsvFile<Customer>(new CsvCustomerMapper());
            List<Customer> Customers = csvFile.ParseCSVFile().ToList();

        }


}
