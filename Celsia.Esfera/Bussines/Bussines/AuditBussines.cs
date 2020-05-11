using System;
using System.Threading.Tasks;
using Entities.Models;

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
        public Audit Add(string userName, OperationAudit operationAudit)
        {
            Audit auditoria = new Audit
            {
                Date = DateTime.Now,
                OperationAudit = operationAudit,
                UserName = userName
            };

            Task<Audit> task = this.AddAsync(auditoria);
            task.Wait();
            return task.Result;
        }
    }
}
