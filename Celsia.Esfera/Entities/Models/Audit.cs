using System;


namespace Entities.Models
{
    public class Audit
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public OperationAudit OperationAudit { get; set; }
    }
}
