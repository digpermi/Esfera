

using Entities.Models;

namespace Bussines.Bussines
{
    public interface IAuditBussines
    {
        Audit Add(string userName, OperationAudit operationAudit);
    }
}
