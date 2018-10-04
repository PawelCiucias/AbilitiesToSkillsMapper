using atos.skillsToCompetenciesMapper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Core
{
    class SkillDropArgs : EventArgs
    {
        public IEnumerable<IAbility> Abilities { get; private set; }
        public SkillDropArgs() : base() { }

        public SkillDropArgs(IEnumerable<IAbility> abilities) : this()
        {
            this.Abilities = abilities;
        }
    }
}
