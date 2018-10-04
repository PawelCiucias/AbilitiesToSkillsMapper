using atos.skillsToCompetenciesMapper.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace atos.skillsToCompetenciesMapper.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class Ability : BaseModel, IAbility, IEquatable<IAbility>
    {
        #region Properties
        string category;
        [JsonProperty("category")]
        public string Category { get => category; set => base.SetProperty(ref category, value); }

        string name;
        [JsonProperty("name")]
        public string Name { get => name; set => base.SetProperty(ref name, value); }

        [JsonProperty("level")]
        public string Level { get; set; }
        #endregion

        #region Constructors
        public Ability() { }

        public Ability(string category, string line) : this()
        {
            this.Category = category;
            var values = line.Split(',');
            this.Name = values[0].Trim('"');
            this.Level = values[2].Trim('"');
        }
        #endregion

        #region Interface implementations
        public bool Equals(IAbility other)
           => Category == other.Category && Name == other.Name;

        public int CompareTo(IAbility other)
            => Name == other.Name ? 0 : Name.CompareTo(other.Name);

        public object Clone() => new Ability
        {
            Category = this.Category,
            Name = this.Name,
            Level = this.Level
        }; 
        #endregion

        private string DebuggerDisplay() => $"{Category}:{Name} ({Level})";
        

        public static IEqualityComparer<IAbility> GetEqualityComparer() => new EqualityComparer();

        private class EqualityComparer : IEqualityComparer<IAbility>
        {
            public bool Equals(IAbility x, IAbility y) => ((Ability)x).Equals((Ability)y);
            public int GetHashCode(IAbility obj) => $"{obj.Category}{obj.Name}".GetHashCode();
        }
    }
}
