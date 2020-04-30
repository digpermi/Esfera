﻿using System.Collections.Generic;
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
        /// Busca el cliente con el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer GetCustomer(int cod, byte externalSystemId)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x=>x.Code == cod && x.ExternalSystemId == externalSystemId, null, "IdentificationType,ExternalSystem");
            task.Wait();

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerById(int cod)
        {
            Task<List<Customer>> task = this.repository.GetAsync(x => x.Code == cod, null, "IdentificationType,ExternalSystem");
            task.Wait();

            return task.Result.FirstOrDefault();
        }
    }
}
