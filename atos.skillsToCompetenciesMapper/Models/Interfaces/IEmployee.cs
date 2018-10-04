using System.Collections.Generic;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    public interface IEmployee
    {
        string Id { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Subarea { get; set; }
        string CostCenter { get; set; }
        bool Direct { get; set; }
        string FunctionLong { get; set; }
        int GCM { get; set; }
        string NameOfSuperior { get; set; }
        int Utilization { get; set; }
        
        IList<IAbility> Abilities { get; set; }
    }
}