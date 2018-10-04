using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    interface IMapData
    {
        ICollection<IAbilityTab> AbilityTabs { get; set; }
        ICollection<ISkillTab> SkillTabs { get; set; }
        IList<IEmployee> Employees { get; set; }
    }
}
