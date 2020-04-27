using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ExcelDataReader;
using Portal.ViewModels;


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

            string resul = @"C:\Users\User\Documents\doc";

            using (var stream = System.IO.File.Open(Path.Combine(resul, fileName), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        records.Add(new CustomerViewModel
                        {
                            FistName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            PhoneNumber = reader.GetString(2),
                            Address = reader.GetString(3)

                        });
                    }
                }
            }
            if (records.Any())
            {
                //db.Users.AddRange(records);
                //db.SaveChanges();
            }
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
                records.Add(new CustomerViewModel
                {
                    FistName = datos[0].ToString(),
                    LastName = datos[1].ToString(),
                    PhoneNumber = datos[2].ToString(),
                    Address = datos[3].ToString()
                });
            }
        }
    }

}