using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class InterestBussines : IInterestBussines
    {
        private readonly IRepository<Interest> repository;

        public InterestBussines(EsferaContext context)
        {
            this.repository = new InterestRepository(context);
        }

        /// <summary>
        /// Busca todos los intereses
        /// </summary>
        /// <returns></returns>
        public ICollection<Interest> GetAllInterests()
        {
            Task<List<Interest>> task = this.repository.GetAsync();
            return task.Result;
        }

        public Interest GetInterestById(byte id)
        {
            Task<Interest> task = this.repository.GetAsync(id);
            task.Wait();

            return task.Result;
        }

    }
}
