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
        /// Busca el cliente con el código y sistema
        /// </summary>
        /// <param name="code"></param>
        /// <param name="system"></param>
        /// <returns></returns>

        public Customer GetCustomer(int code, byte externalSystemId)
        {
            string IncludeProperties = "IdentificationType,ExternalSystem,Persons";

            Task<List<Customer>> task = this.repository.GetAsync(x => x.Code == code && x.ExternalSystemId == externalSystemId, null, IncludeProperties);
            task.Wait();

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerByCode(int? code)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x => x.Code == code, null, "IdentificationType,ExternalSystem");
            task.Wait();

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerById(int id)
        {
            Task<Customer> task = this.repository.GetAsync(id);
            task.Wait();

            return task.Result;
        }
    }
}
