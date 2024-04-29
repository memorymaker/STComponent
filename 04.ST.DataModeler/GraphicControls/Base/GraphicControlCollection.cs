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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.DataModeler
{
    [ListBindable(false)]
    [ComVisible(false)]
    public class GraphicControlCollection : IEnumerable
    {
        public IGraphicControlParent Owner => _Owner;
        private IGraphicControlParent _Owner;

        private List<GraphicControl> InnerList = new List<GraphicControl>();

        public int Count => InnerList.Count;

        public GraphicControl this[int index]
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

        public GraphicControlCollection(IGraphicControlParent owner)
        {
            this._Owner = owner;
        }

        public void Add(GraphicControl value)
        {
            if (value == null || InnerList.Contains(value))
            {
                return;
            }

            if (value.Parent != null)
            {
                value.Parent.Controls.Remove(value);
            }

            value.Parent = _Owner;
            InnerList.Add(value);

            _Owner.OnControlAdded(new GraphicControlEventArgs(value));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual void AddRange(GraphicControl[] controls)
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

        public bool Contains(GraphicControl control)
        {
            return InnerList.Contains(control);
        }

        public int IndexOf(GraphicControl control)
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

        public virtual void Remove(GraphicControl value)
        {
            if (value != null && value.Parent == _Owner)
            {
                InnerList.Remove(value);
                _Owner.OnControlRemoved(new GraphicControlEventArgs(value));
                value.Parent = null;
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

        public int GetChildIndex(GraphicControl child)
        {
            return GetChildIndex(child, throwException: true);
        }

        public virtual int GetChildIndex(GraphicControl child, bool throwException)
        {
            int num = IndexOf(child);
            if (num == -1 && throwException)
            {
                throw new ArgumentException("ControlNotChild");
            }

            return num;
        }

        public virtual void SetChildIndex(GraphicControl child, int newIndex)
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

        private void MoveElement(GraphicControl child, int childIndex, int newIndex)
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

        private class ControlCollectionEnumerator : IEnumerator
        {
            private GraphicControlCollection controls;

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

            public ControlCollectionEnumerator(GraphicControlCollection controls)
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
