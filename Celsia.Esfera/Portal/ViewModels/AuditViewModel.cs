using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels
{
    public class AuditViewModel
    {
        public DateTime dateLog { get; set; }
        public String usser { get; set; }
        public string operation { get; set; }
    }
}
