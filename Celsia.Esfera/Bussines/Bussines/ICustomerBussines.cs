using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface ICustomerBussines
    {
        Customer GetCustomerById(int id);
        Customer GetCustomer(int cod, byte externalSystemId);
        Customer GetCustomerByCode(int code);

    }
}