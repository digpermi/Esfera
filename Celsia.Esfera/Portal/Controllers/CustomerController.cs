using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class CustomerController : Controller
    {
        //private readonly TRepository repository;

        //public CustomerController (TRepository repository)
        //{
        //    this.repository = repository;
        //}

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }



        // GET: Customer/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Details(int id)
        {
            // Customer customer = await this.repository.GetAsync(id);

            var customer = new Customer()
            {
                Name = "Prueba",
                LastName = "Prueba1",
                Identification = "343345656",
                IdentificationType = new IdentificationType()
                {
                    Id = 1,
                    Name = "cedula"
                },
                Address = "calle 58 # 45-09",
                PhoneNumber = "2020100",
                CellPhone = "3048064611",
                Email = "prueba@hoitmail.com",
                PolicyData = 1,
                System = new ExternalSystem()
                {
                    Id = 1,
                    Name = "Open"
                }
            };
            
            if (customer == null)
            {
                this.NotFound();
            }
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
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

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
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

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
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
    }
}