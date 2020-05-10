using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.Bussines
{
    public class InterestBussines : Repository<Interest, EsferaContext>, IInterestBussines
    {
        public InterestBussines(EsferaContext context) : base(context)
        {

        }

        /// <summary>
        /// Busca todos los intereses
        /// </summary>
        /// <returns></returns>
        public ICollection<Interest> GetAllInterests()
        {
            Task<List<Interest>> task = this.GetAsync();
            task.Wait();
            return task.Result;
        }

        public Interest GetInterestById(byte id)
        {
            Task<Interest> task = this.GetAsyncByte(id);
            task.Wait();

            return task.Result;
        }

    }
}
