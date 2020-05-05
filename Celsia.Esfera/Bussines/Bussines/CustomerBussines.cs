using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.Bussines
{
    public class CustomerBussines : Repository<Customer, EsferaContext>, ICustomerBussines
    {
        private readonly IAuditBussines auditBussines;

        public CustomerBussines(EsferaContext context) : base(context)
        {
            this.auditBussines = new AuditBussines(context);
        }

        /// <summary>
        /// Busca el cliente con el código y sistema
        /// </summary>
        /// <param name="code"></param>
        /// <param name="system"></param>
        /// <returns></returns>

        public Customer GetCustomer(int code, byte externalSystemId, string userName)
        {
            Task<List<Customer>> task = this.GetAsync(x => x.Code == code && x.ExternalSystemId == externalSystemId, null, "IdentificationType,ExternalSystem,Persons,Persons.Relationship,Persons.Interest,Persons.IdentificationType,Persons.ExternalSystem");
            task.Wait();

            Audit auditoria = new Audit()
            {
                dateAudit = DateTime.Now,
                usser = userName,
                operation = "Consultar Cliente"
            };

            this.auditBussines.Add(auditoria);

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerByCode(int? code)
        {
            Task<List<Customer>> task = this.GetAsync(x => x.Code == code, null, "IdentificationType,ExternalSystem");
            task.Wait();

            return task.Result.FirstOrDefault();
        }

        public Customer GetCustomerById(int id)
        {
            Task<Customer> task = this.GetAsync(id);
            task.Wait();

            return task.Result;
        }
    }
}
