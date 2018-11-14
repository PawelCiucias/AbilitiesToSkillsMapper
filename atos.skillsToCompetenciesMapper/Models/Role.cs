using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace atos.skillsToCompetenciesMapper.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    class Role : IRole
    {
        [JsonProperty(Constants.CATEGORY_KEY)]
        public string Category { get; set; }

        [JsonProperty(Constants.SKILLS_KEY)]
        public ICollection<ISubRole> SubRoles { get; set; } = new ObservableCollection<ISubRole>();

        public Role() { }
        public Role(string category) : this() => Category = category;

        public static JsonConverter<IRole> GetConverter() => new roleConverter();

        private class roleConverter : JsonConverter<IRole>
        {
            public override IRole ReadJson(JsonReader reader, Type objectType, IRole existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                JObject jRole = JObject.Load(reader);
                var category = jRole["category"].ToString();
                var subroles = jRole["subroles"].ToObject<IEnumerable<string>>();

                return new Role
                {
                    Category = category,
                    SubRoles = new ObservableCollection<ISubRole>(subroles.Select(s => new SubRole(s)))
                };
            }

            public override void WriteJson(JsonWriter writer, IRole value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        private string DebuggerDisplay() => $"{Category} ({SubRoles?.Count ?? 0})";
    }

    [DebuggerDisplay("{DebuggerDisplay()}")]
    class SubRole : BaseModel,  ISubRole
    {
        public string Skill { get; set; }
        public ICollection<IAbility> Abilities { get; set; } = new ObservableCollection<IAbility>();

        // public int SkillCount { get => Skills?.Count() ?? 0; set { } }

        public SubRole() { }

        public SubRole(string category) : this()
        {
            this.Skill = category;
            base.NotifyPropertyChanged(nameof(Skill));
        }

        public SubRole(string category, IEnumerable<IAbility> skills) : this(category)
        {
            foreach (var skill in skills)
                this.Abilities.Add(skill);
           // base.NotifyPropertyChanged(nameof(SkillCount));
        }

        private string DebuggerDisplay() => $"{Skill} ({Abilities?.Count ?? 0})";
    }
}
