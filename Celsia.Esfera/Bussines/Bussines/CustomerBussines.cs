using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public Customer GetCustomer(int code, byte systemId)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x=>x.Code == code && x.ExternalSystemId == systemId , includeProperties: "IdentificationType,ExternalSystem");
            task.Wait();

            return task.Result.FirstOrDefault();
        }

    }
}
