using System;

namespace atos.skillsToCompetenciesMapper.Models.Interfaces
{
    /// <summary>
    /// abilities are the input that comes form Nessie
    /// they are the source of data for our transformation
    /// </summary>
    public interface IAbility : IComparable<IAbility>, ICloneable
    {
        /// <summary>
        /// the name of the abilitie's group
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// the name of the ability
        /// </summary>
        string Name { get; set; }
    }
}
