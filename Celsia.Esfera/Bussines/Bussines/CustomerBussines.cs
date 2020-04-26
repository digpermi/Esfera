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

        /// <summary>
        /// Busca el cliente con el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer GetAllCustomersById(int Id)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x=>x.Id.Equals(Id));
            task.Wait();

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerByName(string name)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x => x.FistName.StartsWith(name));
            task.Wait();

            return task.Result.FirstOrDefault();
        }
    }
}
