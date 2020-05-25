using Entities.Models;

namespace Bussines.Bussines
{
    public interface ICustomerBussines
    {
        Customer GetCustomerById(int id);

        Customer GetCustomer(int cod, byte externalSystemId, string userName);

        Customer GetCustomerByCode(int? code);

        int GetCustomerIdByPersonId(int personId);

        void EditSystemUdateDate(int? custormerId);
    }
}