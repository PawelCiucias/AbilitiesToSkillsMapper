using atos.skillsToCompetenciesMapper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class Employee : IEmployee, IEquatable<IEmployee>
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Subarea { get; set; }
        public string CostCenter { get; set; }
        public bool Direct { get; set; }
        public string FunctionLong { get; set; }
        public int GCM { get; set; }
        public string NameOfSuperior { get; set; }
        public int Utilization { get; set; }
        public IList<IAbility> Abilities { get; set; } = new List<IAbility>();

        public bool Equals(IEmployee other) => Id == other.Id;

        private string DebuggerDisplay() => $"{FirstName} {LastName} abilities:({Abilities?.Count() ?? 0})";
    }
}
