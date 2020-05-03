using Entities.Models;
using System.Threading.Tasks;

namespace Bussines.Bussines
{
   public class AuditBussines : Repository<Audit, EsferaContext>, IAuditBussines
    {
        public AuditBussines(EsferaContext context) : base(context)
        {
            
        }
        /// <summary>
        /// almacenara los moviemintos que se hacen dentro de la aplicacion 
        /// </summary>
        /// <returns></returns>
        public Audit Add(Audit auditoria)
        {
            Task<Audit> task = this.AddAsync(auditoria);
            task.Wait();
            return task.Result;
        }
    }
}
