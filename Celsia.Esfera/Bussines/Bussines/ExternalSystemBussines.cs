using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class ExternalSystemBussines : IExternalSystemBussines
    {
        private readonly IRepository<ExternalSystem> repository;

        public ExternalSystemBussines(EsferaContext context)
        {
            this.repository = new ExternalSystemRepository(context);
        }

        /// <summary>
        /// Busca todas los external system
        /// </summary>
        /// <returns></returns>
        public ICollection<ExternalSystem> GetAllExternalSystems()
        {
            Task<List<ExternalSystem>> task = this.repository.GetAsync();
            task.Wait();

            return task.Result;
        }

    }
}
