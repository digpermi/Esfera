using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.ViewModels;
using Utilities.File;

namespace Portal.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
            {
                if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                {
                    return View();
                }
                else
                {
                    var fileName = file.FileName;
                    UploadRecordsToDataBase(fileName);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var fileName = file.FileName;
                UploadTxt(fileName);
                return RedirectToAction("Index");
            }

            return View();
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

        public void UploadTxt(string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var records = new List<CustomerViewModel>();
            var datos = new List<string>();

            string resul = @"C:\Users\User\Documents\doc\prueba.txt";

            using (StreamReader leer = new StreamReader(resul))
            {
                string x;
                while ((x = leer.ReadLine()) != null)
                {
                    datos.Add(x);
                }
                Customer customerRow = new Customer
                {
                    FirstName = datos[0].ToString(),
                    LastName = datos[1].ToString(),
                    PhoneNumber = datos[2].ToString(),
                    Address = datos[3].ToString()
                };
                records.Add(new CustomerViewModel
                {
                    Customer = customerRow
                });
            }
        }
    }

}