using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    public interface ISubRole
    {
        string Category { get; set; }
     //   int SkillCount { get; set; }
        ICollection<IAbility> Skills { get; set; }
    }
}