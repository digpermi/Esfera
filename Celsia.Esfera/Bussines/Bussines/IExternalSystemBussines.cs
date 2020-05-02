using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IExternalSystemBussines
    {
        ICollection<ExternalSystem> GetAllExternalSystems();

        ExternalSystem GetExternalSystemById(byte id);

    }
}