using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class IdentificationTypeBussines : IIdentificationTypeBussines
    {
        private readonly IRepository<IdentificationType> repository;

        public IdentificationTypeBussines(EsferaContext context)
        {
            this.repository = new IdentificationTypeRepository(context);
        }

        /// <summary>
        /// Busca todos los tipos de identificacion
        /// </summary>
        /// <returns></returns>
        public ICollection<IdentificationType> GetAllIdentificationTypes()
        {
            Task<List<IdentificationType>> task = this.repository.GetAsync();
            return task.Result;
        }

    }
}
