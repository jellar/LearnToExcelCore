using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ContactTypeId { get; set; }
        public virtual ContactType ContactType { get; set; }
    }
}
