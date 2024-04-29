using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Layout;
using System.Windows.Forms;
using ST.Core;

namespace ST.DataModeler
{
    [ListBindable(false)]
    [ComVisible(false)]
    public class RelationControlCollection : IEnumerable
    {
        public IRelationControlParent Owner => _Owner;
        private IRelationControlParent _Owner;

        private List<RelationControl> InnerList = new List<RelationControl>();

        public int Count => InnerList.Count;

        public RelationControl this[int index]
        {
            get
            {
                if (InnerList == null || index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException("index", "IndexOutOfRange " + index.ToString(CultureInfo.CurrentCulture));
                }

                return InnerList[index];
            }
        }

        public RelationControlCollection(IRelationControlParent owner)
        {
            _Owner = owner;
        }

        public void Add(RelationControl value)
        {
            DataModeler parent = _Owner as DataModeler;

            if (value == null || InnerList.Contains(value))
            {
                return;
            }
            else if (parent.ContainsRelation(value.Model))
            {
                return;
            }
            else if (!parent.CanBeAddedRelation(value))
            {
                return;
            }

            if (value.Parent != null)
            {
                value.Parent.Relations.Remove(value);
            }

            value.Parent = parent;

            string valueStringForSorting = value.Model.GetSortingString();
            int insertIndex = -1;
            for(int i = 0; i < InnerList.Count; i++)
            {
                if (valueStringForSorting.CompareTo(InnerList[i].Model.GetSortingString()) < 0 && i == 0)
                {
                    insertIndex = 0;
                    break;
                }
                else if (valueStringForSorting.CompareTo(InnerList[i].Model.GetSortingString()) > 0)
                {
                    if (i < InnerList.Count - 1 && valueStringForSorting.CompareTo(InnerList[i + 1].Model.GetSortingString()) < 0)
                    {
                        insertIndex = i + 1;
                        break;
                    }
                }
            }

            InnerList.Insert(insertIndex < 0 ? InnerList.Count : insertIndex, value);
            value.SetDrawInfo(parent.ScaleValue);

            _Owner.OnRelationAdded(new RelationControlEventArgs(value));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual void AddRange(RelationControl[] controls)
        {
            if (controls == null)
            {
                throw new ArgumentNullException("controls");
            }

            if (controls.Length == 0)
            {
                return;
            }

            // owner.SuspendLayout();
            try
            {
                for (int i = 0; i < controls.Length; i++)
                {
                    Add(controls[i]);
                }
            }
            finally
            {
                // owner.ResumeLayout(performLayout: true);
            }
        }

        public bool Contains(RelationControl control)
        {
            return InnerList.Contains(control);
        }

        public int IndexOf(RelationControl control)
        {
            return InnerList.IndexOf(control);
        }

        private bool IsValidIndex(int index)
        {
            if (index >= 0)
            {
                return index < Count;
            }

            return false;
        }

        public virtual void Remove(RelationControl value)
        {
            if (value != null && value.Parent == _Owner)
            {
                InnerList.Remove(value);
                _Owner.OnRelationRemoved(new RelationControlEventArgs(value));
            }
        }

        public void RemoveAt(int index)
        {
            Remove(this[index]);
        }

        public virtual void Clear()
        {
            // owner.SuspendLayout();
            try
            {
                while (Count != 0)
                {
                    RemoveAt(Count - 1);
                }
            }
            finally
            {
                // owner.ResumeLayout();
            }
        }

        public int GetChildIndex(RelationControl child)
        {
            return GetChildIndex(child, throwException: true);
        }

        public virtual int GetChildIndex(RelationControl child, bool throwException)
        {
            int num = IndexOf(child);
            if (num == -1 && throwException)
            {
                throw new ArgumentException("ControlNotChild");
            }

            return num;
        }

        public virtual void SetChildIndex(RelationControl child, int newIndex)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }

            int childIndex = GetChildIndex(child);
            if (childIndex != newIndex)
            {
                if (newIndex >= Count || newIndex == -1)
                {
                    newIndex = Count - 1;
                }

                MoveElement(child, childIndex, newIndex);
            }
        }

        private void MoveElement(RelationControl child, int childIndex, int newIndex)
        {
            if (childIndex != newIndex)
            {
                InnerList.Remove(child);
                InnerList.Insert(newIndex, child);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ControlCollectionEnumerator(this);
        }

        public List<RelationControl> ToList()
        {
            return InnerList;
        }

        private class ControlCollectionEnumerator : IEnumerator
        {
            private RelationControlCollection controls;

            private int current;

            private int originalCount;

            public object Current
            {
                get
                {
                    if (current == -1)
                    {
                        return null;
                    }

                    return controls[current];
                }
            }

            public ControlCollectionEnumerator(RelationControlCollection controls)
            {
                this.controls = controls;
                originalCount = controls.Count;
                current = -1;
            }

            public bool MoveNext()
            {
                if (current < controls.Count - 1 && current < originalCount - 1)
                {
                    current++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                current = -1;
            }
        }
    }
}
