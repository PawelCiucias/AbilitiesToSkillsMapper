using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    public interface IRole
    {
        /// <summary>
        /// The category name of the skill
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Name of the skill
        /// </summary>
        ICollection<ISubRole> SubRoles { get; set; }
    }
}
