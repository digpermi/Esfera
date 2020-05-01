using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IRelationshipBussines
    {
        ICollection<Relationship> GetAllRelationships();

        Relationship GetRelationshipById(byte id);

    }
}