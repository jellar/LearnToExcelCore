using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{
    public class ContactType
    {
        public int ContactTypeId { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
