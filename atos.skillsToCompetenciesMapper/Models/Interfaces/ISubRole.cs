using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    public interface ISubRole
    {
        string Skill { get; set; }
     //   int SkillCount { get; set; }
        ICollection<IAbility> Abilities { get; set; }
    }
}