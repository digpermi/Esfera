using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IExternalSystemBussines
    {
        ICollection<ExternalSystem> GetAllExternalSystems();

    }
}