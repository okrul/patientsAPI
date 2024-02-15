using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePatients
{
    public class Patient
    {
        public Name? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
    }

    public class Name
    {
        public string? Use { get; set; }
        public string Family { get; set; }
        public string[]? Given { get; set; }
    }
}
