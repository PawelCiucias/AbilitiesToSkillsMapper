using atos.skillsToCompetenciesMapper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Models
{
    class MapData : IMapData
    {
        public ICollection<IAbilityTab> AbilityTabs { get; set; }
        public ICollection<ISkillTab> SkillTabs { get; set; }
        public IList<IEmployee> Employees { get; set; }
        
    }
}
