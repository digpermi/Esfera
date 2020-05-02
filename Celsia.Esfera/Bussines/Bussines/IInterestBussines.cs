using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IInterestBussines
    {
        ICollection<Interest> GetAllInterests();
    }
}