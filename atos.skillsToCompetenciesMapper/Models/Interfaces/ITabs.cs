using atos.skillsToCompetenciesMapper.Custom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    interface ITab
    {
        string Header { get; set; }
    }

    interface IAbilityTab : ITab
    {
        int AbilityCount { get; }
        ICollection<IAbility> Abilities { get; set; }
    }

    interface ISkillTab : ITab
    {
        int SkillCount { get; }
        IRole Role { get; set; }
        bool AddRole(IRole skill);
        ICommand DuplicateCommand { get; }
    }
}
