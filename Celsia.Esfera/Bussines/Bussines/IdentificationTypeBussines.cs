using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.Bussines
{
    public class IdentificationTypeBussines : Repository<IdentificationType, EsferaContext>, IIdentificationTypeBussines
    {
        public IdentificationTypeBussines(EsferaContext context) : base(context)
        {

        }

        /// <summary>
        /// Busca todos los tipos de identificacion
        /// </summary>
        /// <returns></returns>
        public ICollection<IdentificationType> GetAllIdentificationTypes()
        {
            Task<List<IdentificationType>> task = this.GetAsync();
            task.Wait();
            return task.Result;
        }

        public IdentificationType GetIdentificationTypeById(byte id)
        {
            Task<IdentificationType> task = this.GetAsyncByte(id);
            task.Wait();

            return task.Result;
        }

    }
}
