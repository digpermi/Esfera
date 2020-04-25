using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface ICustomerBussines
    {
        Customer GetCustomerByName(string name);

        List<Customer> GetAllCustomersByFilter(string name);
    }
}