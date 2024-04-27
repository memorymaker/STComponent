using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class NodeBase : GraphicPanel
    {
        #region Value & Propertise
        public virtual NodeType NodeType { get; set; } = NodeType.None;

        public virtual string NodeOption { get; set; } = string.Empty;

        public virtual string NodeNote { get; set; } = string.Empty;
        
        public bool AutoSize
        {
            get
            {
                return _AutoSize;
            }
        }
        protected bool _AutoSize = false;

        public int AbsoluteLeft
        {
            get
            {
                DataModeler parente = Parent as DataModeler;
                var innerLocation = parente != null ? parente.InnerLocation : Point.Empty;
                return ((innerLocation.X + Left) * (1 / ScaleValue)).ToInt();
            }
        }

        public int AbsoluteTop
        {
            get
            {
                DataModeler parente = Parent as DataModeler;
                var innerLocation = parente != null ? parente.InnerLocation : Point.Empty;
                return ((innerLocation.Y + Top) * (1 / ScaleValue)).ToInt();
            }
        }
        #endregion

        #region Load
        public NodeBase(DataModeler target): base(target)
        {
            LocationChanged += NodeBase_LocationChanged;
        }

        private void NodeBase_LocationChanged(object sender, EventArgs e)
        {
            if (GraphicMouseAction.IsMouseDown)
            {
                Point targetInnerLocation = Target.InnerLocation;
                Size targetInnerSize = Target.InnerSize;

                if (targetInnerLocation.X + Left < 0)
                {
                    Left = -targetInnerLocation.X;
                }
                else if (targetInnerSize.Width * Target.ScaleValue < targetInnerLocation.X + Right)
                {
                    Left = (targetInnerSize.Width * Target.ScaleValue - targetInnerLocation.X - Width).ToInt();
                }

                if (targetInnerLocation.Y + Top < 0)
                {
                    Top = -targetInnerLocation.Y;
                }
                else if (targetInnerSize.Height * Target.ScaleValue < targetInnerLocation.Y + Bottom)
                {
                    Top = (targetInnerSize.Height * Target.ScaleValue - targetInnerLocation.Y - Height).ToInt();
                }
            }
        }
        #endregion

        #region Function
        public virtual NodeModel GetNodeModel()
        {
            return new NodeModel
            {
                NODE_ID = ID,
                NODE_SEQ = SEQ,
                NODE_TYPE = NodeType.GetStringValue(),
                NODE_LEFT = AbsoluteLeft,
                NODE_TOP = AbsoluteTop,
                NODE_WIDTH = OriginalWidth,
                NODE_HEIGHT = OriginalHeight,
                NODE_Z_INDEX = ZIndex,
                NODE_OPTION = NodeOption,
                NODE_NOTE = NodeNote
            };
        }
        #endregion
    }
}