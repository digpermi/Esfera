using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface ICustomerBussines
    {
        Customer GetCustomer(int cod, byte externalSystemId);
        Customer GetCustomerByCode(int code);
    }
}