using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface ICustomerBussines
    {
        Customer GetAllCustomersById(int Id);

        //List<Customer> GetAllCustomersByFilter(string name);
    }
}