using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Helpers;
using atos.skillsToCompetenciesMapper.Models;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using atos.skillsToCompetenciesMapper.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public ICollection<IAbilityTab> AbilityTabs { get; } = new ObservableCollection<IAbilityTab>();
        public ICollection<ISkillTab> SkillTabs { get; } = new ObservableCollection<ISkillTab>();


        private bool _activeAbilities;
        public bool ActiveAbilitites
        {
            get => _activeAbilities;
            set => base.SetValue(ref _activeAbilities, value);
        }


        public IList<IEmployee> Employees { get; set; } = new List<IEmployee>();

        public ICommand ImportCommand => new RelayCommand(async () =>
            {
                var mapping = new Dictionary<string, HashSet<IAbility>>();
                var csvData = await IOHelper.OpenFileDialogAsync(Core.Constants.CSV_FILTER);
                if (!String.IsNullOrEmpty(csvData))
                {
                    IEmployee employee = null;
                    string skillGroup = null;
                    foreach (var line in csvData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (Regex.IsMatch(line, Core.Constants.DASIDREGEX_LINE))
                        {
                            employee = new Employee { Id = Regex.Match(line, Core.Constants.DASIDREGEX_FIELD).Value };
                            Employees.Add(employee);
                        }
                        else if (Regex.IsMatch(line, Core.Constants.EMPLOYEENAME_LINE))
                        {
                            var name = Regex.Match(line, Core.Constants.Lastvalueindoublequotes).Value.Trim('"');
                            var firstNames = Regex.Matches(name, Core.Constants.CAPITALIZEDWORDS);
                            employee.FirstName = firstNames.Cast<Match>().Select(m => m.Value).Aggregate((x, y) => $"{x} {y}");

                            var lastNames = Regex.Matches(name, Core.Constants.UPPERCASEWORDS);
                            employee.LastName = lastNames.Cast<Match>().Select(m => m.Value).Aggregate((x, y) => $"{x} {y}");
                        }
                        else if (Regex.IsMatch(line, Core.Constants.GSMLEVEL_LINE))
                        {
                            employee.GCM = int.Parse(Regex.Match(line, Core.Constants.Lastvalueindoublequotes).Value.Trim('"'));
                        }
                        else if (Regex.IsMatch(line, Core.Constants.SUPOERVISOR_LINE))
                        {
                            var supervisorName = Regex.Match(line, Core.Constants.Lastvalueindoublequotes).Value.Trim('"');
                            if (supervisorName.Trim() == "Overload" || supervisorName.Trim() == "CO CH_empty")
                                continue;

                            employee.NameOfSuperior = Regex.Match(supervisorName, Constants.SUPERVISOR_NAME).Value;
                        }
                        else if (Regex.IsMatch(line, Core.Constants.SKILLGROUP))
                        {
                            skillGroup = Regex.Match(line, Core.Constants.SKILLGROUP).Value.Trim('"');
                        }
                        else if (Regex.IsMatch(line, Core.Constants.SKILL))
                        {
                            var ability = new Ability(skillGroup, line);
                            employee.Abilities.Add(ability);

                            if (!mapping.ContainsKey(ability.Category))
                                mapping.Add(ability.Category, new HashSet<IAbility>(Ability.GetEqualityComparer()));
                            mapping[ability.Category].Add(ability);
                        }
                    }

                    foreach (var abilityGroup in mapping)
                        AbilityTabs.Add(new AbilityTab(abilityGroup.Value));
                }
            });

        public ICommand SaveMapCommand => new RelayCommand(() =>
        {
            var dataToSave = new MapData
            {
                AbilityTabs = this.AbilityTabs,
                SkillTabs = this.SkillTabs,
                Employees = this.Employees
            };
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string jsonData = JsonConvert.SerializeObject(dataToSave, settings);
            IOHelper.SaveFileDialog(jsonData, Core.Constants.MAP_FILTER);
        });

        public ICommand OpenMapCommand => new RelayCommand(async () =>
        {
            var jsonData = await IOHelper.OpenFileDialogAsync(Core.Constants.MAP_FILTER);
            if (!String.IsNullOrEmpty(jsonData))
            {
                var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                var data = JsonConvert.DeserializeObject<MapData>(jsonData, settings);

                this.AbilityTabs.Clear();
                foreach (var tab in data.AbilityTabs)
                    this.AbilityTabs.Add(tab);

                this.SkillTabs.Clear();
                foreach (var tab in data.SkillTabs)
                    this.SkillTabs.Add(tab);

                this.Employees.Clear();
                foreach (var employee in data.Employees)
                    this.Employees.Add(employee);
            }
        });

        public ICommand ExpoertXLSVCommand => new RelayCommand(() =>
        {
            var abilityToSkillMap = new Dictionary<string, IList<string>>();

            foreach (var r in SkillTabs.Select(st => st.Role))
                foreach (var sr in r.SubRoles)
                    foreach (var s in sr.Abilities)
                    {
                        var key = $"{s.Category}:{s.Name}";
                        var value = $"{r.Category}:{sr.Skill}";

                        if (abilityToSkillMap.ContainsKey(key))
                            abilityToSkillMap[key].Add(value);
                        else
                            abilityToSkillMap.Add(key, new List<string>(new string[] { value }));
                    }

            var tabs = new[] { "Languages" };
            //var sdf = Employees.SelectMany(e => e.Abilities.Where(a => tabs.Contains(a.Category)).Select(a => ((Ability)a).Level)).Distinct();
            //var names = Employees.SelectMany(e => e.Abilities).Select(a => a.Category).Distinct();

            new ExcelHelper(SkillTabs.Select(st => st.Role), Employees, abilityToSkillMap);

            //using (new ExcelHelper(SkillTabs.Select(st => st.Role), Employees, abilityToSkillMap))




        });

        public ICommand ActiveAbilitiesCommand
        {
            get => new RelayCommand(() => ActiveAbilitites = !ActiveAbilitites);
        }

        public MainWindowViewModel()
        {
            var Roles = Task.Run(() => IOHelper.ReadDataFileAsync<IEnumerable<IRole>>(DataFile.SkillMatrix)).Result;

            foreach (var role in Roles)
            {
                var st = new SkillTab(role, SkillTabs, AbilityTabs);
                st.MappedAbilitiesToSkill += SkillTab_MappedAbilities;
                SkillTabs.Add(st);
            }
        }

        private void SkillTab_MappedAbilities(object sender, SkillDropArgs e)
        {
            if (e.Abilities != null && e.Abilities.Count() > 0)
            {
                AbilityTab abilityTab = AbilityTabs.First(tab => String.Equals(tab.Header, e.Abilities.First().Category)) as AbilityTab;

                foreach (var ability in e.Abilities)
                    if (ability.Active)
                        abilityTab.Abilities.Remove(ability);

                abilityTab.NotifyPropertyChanged(nameof(abilityTab.AbilityCount));
            }
        }
    }
}
