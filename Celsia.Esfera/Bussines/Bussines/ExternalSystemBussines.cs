using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.Bussines
{
    public class ExternalSystemBussines : Repository<ExternalSystem, EsferaContext>, IExternalSystemBussines
    {
        public ExternalSystemBussines(EsferaContext context) : base(context)
        {

        }

        /// <summary>
        /// Busca todos los sistemas externos
        /// </summary>
        /// <returns></returns>
        public ICollection<ExternalSystem> GetAllExternalSystems()
        {
            Task<List<ExternalSystem>> task = this.GetAsync();
            return task.Result;
        }

    }
}
