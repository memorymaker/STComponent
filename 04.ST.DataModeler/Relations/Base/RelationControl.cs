using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class RelationControl
    {
        #region Propertise
        public DataModeler Target => _Target;
        private DataModeler _Target;

        public IRelationControlParent Parent { get; set; }

        public RelationModel Model => _Model;
        private RelationModel _Model;

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    Refresh();
                }
            }
        }
        private bool _Visible = true;

        public bool Focused
        {
            get
            {
                return _Focused;
            }
            set
            {
                if (_Focused != value)
                {
                    _Focused = value;
                    if (_Focused)
                    {
                        Event.CallEvent(this, GotFocus);
                    }
                    else
                    {
                        Event.CallEvent(this, LostFocus);
                    }
                    Refresh();
                }
            }
        }
        private bool _Focused = false;

        public bool CanFocus
        {
            get
            {
                return _CanFocus;
            }
            set
            {
                if (_CanFocus != value)
                {
                    _CanFocus = value;
                    if (!_CanFocus)
                    {
                        Focused = false;
                    }
                }
            }
        }
        private bool _CanFocus = true;

        public Keys ModifierKeys => Control.ModifierKeys;
        #endregion

        #region EventHandler
        // Key
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        // Mouse
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event EventHandler MouseLeave;
        public event MouseEventHandler Click;

        // Etc
        public event EventHandler GotFocus;
        public event EventHandler LostFocus;
        public event PaintEventHandler Paint;
        #endregion

        #region Load
        public RelationControl(DataModeler target, RelationModel model) // : base(target)
        {
            _Target = target;
            _Model = model;

            LoadRelationControlInput();
            LoadRelationControlDraw();
        }

        public RelationControl(DataModeler target, DataRow modelRow) // : base(target)
        {
            _Target = target;

            var model = new RelationModel() 
            {
                  RELATION_TYPE      = modelRow[DataModeler.RELATION.RELATION_TYPE].ToString()
                , RELATION_OPERATOR  = modelRow[DataModeler.RELATION.RELATION_OPERATOR].ToString()
                , RELATION_VALUE     = modelRow[DataModeler.RELATION.RELATION_VALUE].ToString()
                , RELATION_NOTE      = modelRow[DataModeler.RELATION.RELATION_NOTE].ToString()
                , NODE_ID1           = modelRow[DataModeler.RELATION.NODE_ID1].ToString()
                , NODE_SEQ1          = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_SEQ1])
                , NODE_DETAIL_ID1    = modelRow[DataModeler.RELATION.NODE_DETAIL_ID1].ToString()
                , NODE_DETAIL_SEQ1   = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_DETAIL_SEQ1])
                , NODE_DETAIL_ORDER1 = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_DETAIL_ORDER1])
                , NODE_ID2           = modelRow[DataModeler.RELATION.NODE_ID2].ToString()
                , NODE_SEQ2          = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_SEQ2])
                , NODE_DETAIL_ID2    = modelRow[DataModeler.RELATION.NODE_DETAIL_ID2].ToString()
                , NODE_DETAIL_SEQ2   = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_DETAIL_SEQ2])
                , NODE_DETAIL_ORDER2 = Convert.ToInt32(modelRow[DataModeler.RELATION.NODE_DETAIL_ORDER2])
            };
            _Model = model;

            LoadRelationControlInput();
            LoadRelationControlDraw();
        }
        #endregion

        #region Function
        public RelationModel GetRelationModel()
        {
            return new RelationModel
            {
                  RELATION_TYPE      = Model.RELATION_TYPE
                , RELATION_OPERATOR  = Model.RELATION_OPERATOR
                , RELATION_VALUE     = Model.RELATION_VALUE
                , RELATION_NOTE      = Model.RELATION_NOTE
                , NODE_ID1           = Model.NODE_ID1
                , NODE_SEQ1          = Model.NODE_SEQ1
                , NODE_DETAIL_ID1    = Model.NODE_DETAIL_ID1
                , NODE_DETAIL_SEQ1   = Model.NODE_DETAIL_SEQ1
                , NODE_DETAIL_ORDER1 = Model.NODE_DETAIL_ORDER1
                , NODE_ID2           = Model.NODE_ID2
                , NODE_SEQ2          = Model.NODE_SEQ2
                , NODE_DETAIL_ID2    = Model.NODE_DETAIL_ID2
                , NODE_DETAIL_SEQ2   = Model.NODE_DETAIL_SEQ2
                , NODE_DETAIL_ORDER2 = Model.NODE_DETAIL_ORDER2
			};
        }

        public bool EqualsData(RelationControl relationControl)
        {
            bool rs = false;
            if (Model.RELATION_TYPE      == relationControl.Model.RELATION_TYPE
            &&  Model.RELATION_OPERATOR  == relationControl.Model.RELATION_OPERATOR
            &&  Model.RELATION_VALUE     == relationControl.Model.RELATION_VALUE
            &&  Model.RELATION_NOTE      == relationControl.Model.RELATION_NOTE
            &&  Model.NODE_ID1           == relationControl.Model.NODE_ID1
            &&  Model.NODE_SEQ1          == relationControl.Model.NODE_SEQ1
            &&  Model.NODE_DETAIL_ID1    == relationControl.Model.NODE_DETAIL_ID1
            &&  Model.NODE_DETAIL_SEQ1   == relationControl.Model.NODE_DETAIL_SEQ1
            &&  Model.NODE_DETAIL_ORDER1 == relationControl.Model.NODE_DETAIL_ORDER1
            &&  Model.NODE_ID2           == relationControl.Model.NODE_ID2
            &&  Model.NODE_SEQ2          == relationControl.Model.NODE_SEQ2
            &&  Model.NODE_DETAIL_ID2    == relationControl.Model.NODE_DETAIL_ID2
            &&  Model.NODE_DETAIL_SEQ2   == relationControl.Model.NODE_DETAIL_SEQ2
            &&  Model.NODE_DETAIL_ORDER2 == relationControl.Model.NODE_DETAIL_ORDER2)
            {
                rs = true;
            }
            return rs;
        }
        #endregion

        #region OnEvent Method
        public void OnKeyDown(KeyEventArgs e)
        {
            Event.CallEvent(this, KeyDown, e);
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            Event.CallEvent(this, KeyUp, e);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseDown, e);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseMove, e);
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseUp, e);
        }

        public void OnMouseLeave()
        {
            Event.CallEvent(this, MouseLeave);
        }

        public void OnClick(MouseEventArgs e)
        {
            Event.CallEvent(this, Click, e);
        }

        public void OnPaint(PaintEventArgs e)
        {
            Event.CallEvent(this, Paint, e);
        }
        #endregion
    }
}
