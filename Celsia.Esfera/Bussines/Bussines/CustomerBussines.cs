using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class CustomerBussines : ICustomerBussines
    {
        private readonly IRepository<Customer> repository;

        public CustomerBussines(EsferaContext context)
        {
            this.repository = new CustomerRepository(context);
        }

        public List<Customer> GetAllCustomersByFilter(string name)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x => x.Name.StartsWith(name));
            task.Wait();

            return task.Result;
        }

        public Customer GetCustomerByName(string name)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x => x.Name.StartsWith(name));
            task.Wait();

            return task.Result.FirstOrDefault();
        }
    }
}
