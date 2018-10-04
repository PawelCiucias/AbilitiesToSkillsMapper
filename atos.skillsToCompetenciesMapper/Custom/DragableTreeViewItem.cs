using atos.skillsToCompetenciesMapper.Models.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace atos.skillsToCompetenciesMapper.Custom
{
    public class DragableTreeViewItem : TreeViewItem
    {
        public IAbility EmployeeAbility { get; set; }
        public bool IsSkillTree { get; set; } = false;
        DragableTreeViewItem previousNode = null;
        DragableTreeViewItem nextNode = null;

        public event EventHandler LastNodeRemoved;

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this.Parent, new DataObject("DragableTreeViewItem", this, true), DragDropEffects.Move);
            base.OnMouseDown(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (IsSkillTree)
            {
                var skillTreeViewItem = ((DragableTreeViewItem)e.Source).Parent as TreeViewItem;
                var clone = new DragableTreeViewItem { EmployeeAbility = this.EmployeeAbility, IsSkillTree = true, Header = this.Header };
                this.nextNode = clone;
                clone.previousNode = this;
                skillTreeViewItem.Items.Add(clone);
            }
            base.OnMouseDoubleClick(e);
        }
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (IsSkillTree)
            {
                var skillTreeViewItem = ((DragableTreeViewItem)e.Source).Parent as TreeViewItem;

                if (previousNode == null && nextNode == null)
                {
                    //only node
                    LastNodeRemoved?.Invoke(this, new EventArgs());
                    return;
                }
                else if (nextNode == null)
                {
                    //last node
                    this.previousNode.nextNode = null;
                }
                else if (previousNode == null)
                {
                    //first node
                    nextNode.previousNode = null;
                }
                else
                {
                    nextNode.previousNode = this.previousNode;
                    previousNode.nextNode = this.nextNode;
                }

                skillTreeViewItem.Items.Remove(this);

            }
            base.OnMouseRightButtonDown(e);
        }


    }

    public class DragableListViewItem : ListViewItem
    {
        public IAbility Ability
        {
            get { return (IAbility)GetValue(AbilityProperty); }
            set { SetValue(AbilityProperty, value); }
        }

        public DragableListViewItem(IAbility ability)
        {
            this.Ability = ability;
            this.Content = ability.Name;
        }
        
        public static readonly DependencyProperty AbilityProperty = DependencyProperty.Register("Ability", typeof(IAbility), typeof(DragableListViewItem));
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            try
            {
                DragDrop.DoDragDrop(this, new DataObject("DragableListViewItem", this, true), DragDropEffects.Move);
            }
            finally
            {
                base.OnMouseDown(e);
            }
        }
    }
}