using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Custom;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    class AbilityTab : BaseModel, IAbilityTab
    {
        string header;
        public string Header
        {
            get { return header; }
            set { base.SetProperty(ref header, value); }
        }

        public int AbilityCount { get => Abilities?.Count ?? 0; }

        public ICollection<IAbility> Abilities { get; set; } = new ObservableCollection<IAbility>();

        public AbilityTab() { }
        public AbilityTab(IEnumerable<IAbility> abilities) : this()
        {
            abilities = abilities.OrderBy<IAbility, string>(x => x.Name);

            this.Header = abilities?.First()?.Category;

            foreach (var ability in abilities)
                this.Abilities.Add(ability);
            base.NotifyPropertyChanged(nameof(Abilities));
        }

        public void RemoveAbility(IAbility ability)
        {
            if (Abilities.Remove(ability))
                base.NotifyPropertyChanged(nameof(AbilityCount));
        }

        private string DebuggerDisplay() => $"{Header} ({Abilities?.Count ?? -1})";

        public static JsonConverter GetConverter() => new AbilityTabConverter();
        
        private class AbilityTabConverter : JsonConverter<IAbilityTab>
        {
            public override IAbilityTab ReadJson(JsonReader reader, Type objectType, IAbilityTab existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                try
                {
                    return serializer.Deserialize<AbilityTab>(reader);
                }
                catch (Exception ex)
                {
                    var sdf = ex;
                }

                return null;
            }

            public override void WriteJson(JsonWriter writer, IAbilityTab value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
                serializer.Serialize(writer, value);
            }
        }
    }
}
