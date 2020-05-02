using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IIdentificationTypeBussines
    {
        ICollection<IdentificationType> GetAllIdentificationTypes();

        IdentificationType GetIdentificationTypeById(byte id);

    }
}