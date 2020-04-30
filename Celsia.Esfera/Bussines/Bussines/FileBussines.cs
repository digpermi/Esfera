using Entities.Models;
using System.Collections.Generic;


namespace Bussines.Bussines
{
    public class FileBussines : IFileBussines
    {
        private readonly IPersonBussines personBussines;
        private readonly ICustomerBussines customerBussines;
        

        public FileBussines(EsferaContext context)
        {
            this.personBussines =  new PersonBussines(context);
            this.customerBussines = new CustomerBussines(context);
        }

        public bool AddPersonVinculed(List<Person> person)
        {
            foreach (var item in person)
            {
                var customer = customerBussines.GetCustomerById(item.Code.Value);
              
                if (customer != null)
                {
                    var task = personBussines.AddAsync(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
