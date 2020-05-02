using Entities.Models;
using System;
using System.Collections.Generic;
using Utilities.Messages;

namespace Portal.ViewModels
{
    public class FileViewModel : BaseViewModel
    {
        public List<ApplicationMessage> Messages { get; set; }

        public string TotalRows { get; set; }

        public string File { get; set; }

    }
}
