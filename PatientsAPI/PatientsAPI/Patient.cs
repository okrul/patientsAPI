using System.Xml.Linq;

namespace PatientsAPI
{
    public class Patient
    {
        public Name? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
    }

    public class PatientWithId: Patient
    {
        public NameWithId? Name { get; set; }
    }

    public class Name
    {
        public string? Use { get; set; }
        public string Family { get; set; }
        public string[]? Given { get; set; }
    }

    public class NameWithId: Name
    {
        public Guid Id { get; set; }
    }
}
