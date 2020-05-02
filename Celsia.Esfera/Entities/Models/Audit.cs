using System;


namespace Entities.Models
{
    public class Audit
    {
        public int id { get; set; }
        public DateTime dateAudit { get; set; }
        public String usser { get; set; }
        public string operation { get; set; }
    }
}
