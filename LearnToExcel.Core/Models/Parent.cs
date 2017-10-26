using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnToExcel.Core.Models
{
    public enum Title
    {
        Mr, Mrs, Ms
    }
    public class Parent
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public ICollection<Contact> Contacts { get; set; }

    }
}
