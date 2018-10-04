using atos.skillsToCompetenciesMapper.Core;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Models
{
    class SkillTab : BaseModel, ISkillTab, IDropTarget
    {

        string header = null;
        public string Header { get => header; set => base.SetProperty(ref header, value); }

        public int SkillCount => Role.SubRoles.Sum(sr => sr.Skills.Count);

        public IRole Role { get; set; } = new Role();

        public event EventHandler<SkillDropArgs> MappedAbilitiesToSkill;



        public SkillTab() { }

        public SkillTab(IRole role) : this() => AddRole(role);

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
                    if (!subRole.Skills.Contains(ability))
                        subRole.Skills.Add(ability);

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
                    var subrole = Role.SubRoles.FirstOrDefault(sr => sr.Skills?.Contains(ability) ?? false);

                    if (subrole != null)
                        subrole.Skills.Add(ability.Clone() as IAbility);
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
                    var subrole = Role.SubRoles.FirstOrDefault(sr => sr.Skills?.Contains(ability) ?? false);

                    if (subrole != null)
                        subrole.Skills.Remove(ability);

                }
            });
        }
    }
}

