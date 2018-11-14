using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Models
{
    class SkillTab : BaseModel, ISkillTab, IDropTarget
    {

        string header = null;
        private ICollection<ISkillTab> skillTabs;
        private ICollection<IAbilityTab> abilityTabs;

        public string Header { get => header; set => base.SetProperty(ref header, value); }

        public int SkillCount => Role.SubRoles.Sum(sr => sr.Abilities.Count);

        public IRole Role { get; set; } = new Role();

        public event EventHandler<SkillDropArgs> MappedAbilitiesToSkill;



        public SkillTab() { }

        public SkillTab(IRole role) : this() => AddRole(role);

        public SkillTab(IRole role, ICollection<ISkillTab> skillTabs) : this(role)
        {
            this.skillTabs = skillTabs;
        }

        public SkillTab(IRole role, ICollection<ISkillTab> skillTabs, ICollection<IAbilityTab> abilityTabs) : this(role, skillTabs)
        {
            this.abilityTabs = abilityTabs;
        }

        public bool AddRole(IRole role)
        {
            if ((Header = Header ?? role.Category) == role.Category)
            {
                this.Role = role;
                return true;
            }
            return false;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.TargetItem is ISubRole && (dropInfo.Data is IAbility || dropInfo.Data is IEnumerable<IAbility>))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var abilities = dropInfo.Data as IEnumerable<IAbility> ?? new[] { dropInfo.Data as IAbility };
            var subRole = dropInfo.TargetItem as ISubRole;

            if (!(subRole == null || abilities == null))
                foreach (var ability in abilities)
                    if (!subRole.Abilities.Contains(ability) && ability.Active)
                        subRole.Abilities.Add(ability);

            base.NotifyPropertyChanged(nameof(SkillCount));

            if (MappedAbilitiesToSkill != null)
                MappedAbilitiesToSkill.Invoke(this, new SkillDropArgs(abilities));
        }


        public ICommand DuplicateCommand
        {
            get => new RelayCommand(a =>
            {
                var ability = a as IAbility;
                if (ability != null)
                {
                    var subrole = Role.SubRoles.FirstOrDefault(sr => sr.Abilities?.Contains(ability) ?? false);

                    if (subrole != null)
                        subrole.Abilities.Add(ability.Clone() as IAbility);
                }
            });
        }


        public ICommand DeleteCommand
        {
            get => new RelayCommand(a =>
            {
                var ability = a as IAbility;
                if (ability != null)
                {
                    
                    var subrole = Role.SubRoles.FirstOrDefault(sr => sr.Abilities?.Contains(ability) ?? false);


                    if (subrole != null)
                        subrole.Abilities.Remove(ability);

                    var result = skillTabs.SelectMany(t => t.Role.SubRoles.SelectMany(sr=> sr.Abilities.Where(s=> s.Name == ability.Name)));

                    if (!result.Any())
                    {
                        IAbilityTab tab = abilityTabs.FirstOrDefault(t => t.Header == ability.Category) ;
                        if(tab.Abilities.FirstOrDefault(t=> t.Name == ability.Name) == null)
                            tab.Abilities.Add(ability);
                    }
                }
            });
        }
    }
}

