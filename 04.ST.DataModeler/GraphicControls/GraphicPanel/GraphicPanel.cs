using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicPanel : GraphicControl, IGraphicMouseActionTarget
    {
        #region Values
        #region EventHandler
        public event UserEventHandler SizeLocationChanged;
        public event UserEventHandler BtDeleteMouseDown;
        public event UserEventHandler BtContextMenuMouseDown;
        public event UserEventHandler BtContextMenuClick;
        public event TitleChangingEventHandeler TitleChanging;
        #endregion

        #region Classes
        protected SimpleGraphicControl BtDelete;
        protected SimpleGraphicControl BtContextMenu;
        public GraphicMouseAction GraphicMouseAction;
        #endregion

        #region Options
        public bool EnableCaptionEdit = true;
        public int MinWidth = 50;
        public int MinHeight = 50;
        #endregion

        #region Controls
        protected TextBox TitleEditor;
        #endregion
        #endregion

        #region Propertise
        new public Padding Padding
        {
            get
            {
                return _Padding;
            }
            set
            {
                if (_Padding != value)
                {
                    _Padding = value;
                    base.Padding = new Padding(_Padding.Left, _Padding.Top + _TitleHeight, _Padding.Right, _Padding.Bottom);
                    Refresh();
                }
            }
        }
        private Padding _Padding = new Padding(3, 3, 3, 3);

        virtual public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                }
            }
        }
        protected private string _ID;

        virtual public int SEQ
        {
            get
            {
                return _SEQ;
            }
            set
            {
                if (_SEQ != value)
                {
                    _SEQ = value;
                }
            }
        }
        protected int _SEQ;

        public int TitleHeight
        {
            get
            {
                return _TitleHeight;
            }
            set
            {
                if (_TitleHeight != value)
                {
                    _TitleHeight = value;
                    base.Padding = new Padding(_Padding.Left, _Padding.Top + value, _Padding.Right, _Padding.Bottom);
                    Refresh();
                }
            }
        }
        private int _TitleHeight = 25;

        public bool TitleBold
        {
            get
            {
                return _TitleBold;
            }
            set
            {
                if (_TitleBold != value)
                {
                    _TitleBold = value;
                    Refresh();
                }
            }
        }
        private bool _TitleBold = true;

        public bool TitleVisible
        {
            get
            {
                return _TitleVisible;
            }
            set
            {
                if (_TitleVisible != value)
                {
                    _TitleVisible = value;
                    Refresh();
                }
            }
        }
        private bool _TitleVisible = false;

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    Refresh();
                }
            }
        }
        private string _Title = string.Empty;

        public bool ShowBtContextMenu
        {
            get
            {
                return _ShowBtContextMenu;
            }
            set
            {
                _ShowBtContextMenu = value;
            }
        }
        private bool _ShowBtContextMenu = false;
        #endregion

        #region Load
        public GraphicPanel(DataModeler target) : base(target)
        {
            LoadThis();
            LoadInput();
            LoadDraw();
            SetSimpleGraphicControls();
        }

        private void LoadThis()
        {
            this.SetDefault();
            this.SetEvent();
        }

        private void SetDefault()
        {
            // Base
            base.Padding = new Padding(
                Padding.Left
                , Padding.Top + TitleHeight
                , Padding.Right
                , Padding.Bottom
            );

            // This
            ForeColor = Color.White;
            
            // GraphicMouseAction
            GraphicMouseAction = new GraphicMouseAction(this);
            GraphicMouseAction.UseLocationCorrection = false;

            // TitleEditor
            TitleEditor = new TextBox();
            TitleEditor.BorderStyle = BorderStyle.None;
            TitleEditor.Name = "BaseTextBoxTitle";
            TitleEditor.Visible = false;
            TitleEditor.Width = Width;
            Target.BaseControls.Add(TitleEditor);
            TitleEditor.VisibleChanged += (object sender, EventArgs e) =>
            {
                BtDelete.Enabled = !TitleEditor.Visible;
                Refresh();
            };
        }

        private void SetSimpleGraphicControls()
        {
            // UserPanelControl
            int marginTotal = (4 * ScaleValue).ToInt();
            int areaSideLength = TitleHeight + Padding.Top - marginTotal;

            BtDelete = new SimpleGraphicControl(this, "Delete");
            BtDelete
                .SetArea(new Rectangle(-(areaSideLength + (marginTotal / 2)), (marginTotal / 2), areaSideLength, areaSideLength))
                .SetDrawType(SimpleGraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.25f, 0.25f), new PointF(0.75f, 0.75f) // /
                    , new PointF(0.75f, 0.25f), new PointF(0.25f, 0.75f) // \
                })
                .SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(220, 231, 241))
                .SetDrawColor(SimpleGraphicControl.StateType.Over, Color.FromArgb(255, 255, 255))
                .SetDrawWeight(1.6f);
            BtDelete.MouseDown += (object sender, MouseEventArgs e) =>
            {
                Event.CallEvent(this, BtDeleteMouseDown
                    , new string[] { "ID", "SEQ", "e" }
                    , new object[] { this.ID, this.SEQ, e }
                );

                if (Parent == null)
                {
                    Target.BaseControls.Remove(TitleEditor);
                }
            };
            BtDelete.Enabled = false;

            Rectangle btContextMenuRectagle = new Rectangle(
                BtDelete.Area.Left - Padding.Right - 10, TitleHeight / 2 + Padding.Top - 8
                , 16, 16
            );
            BtContextMenu = new SimpleGraphicControl(this, "Details");
            BtContextMenu
                .SetArea(btContextMenuRectagle)
                .SetDrawType(SimpleGraphicControl.DrawTypeEnum.Multi)
                .SetDrawItems(
                    new SimpleGraphicControl.DrawItem[]
                    {
                        new SimpleGraphicControl.DrawItem(SimpleGraphicControl.DrawTypeEnum.DrawLines, new PointF[]{
                            new PointF(0.25f, 0.20f)  , new PointF(0.75f, 0.20f)
                        })
                        , new SimpleGraphicControl.DrawItem(SimpleGraphicControl.DrawTypeEnum.FillPolygon, new PointF[]{
                            new PointF(0.2f, 0.4f) , new PointF(0.8f, 0.4f), new PointF(0.5f , 0.65f )
                        })
                    }
                )
                .SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(220, 231, 241))
                .SetDrawColor(SimpleGraphicControl.StateType.Over, Color.FromArgb(255, 255, 255));
            BtContextMenu.MouseDown += (object sender, MouseEventArgs e) =>
            {
                Event.CallEvent(this, BtContextMenuMouseDown
                    , new string[] { "ID", "SEQ", "e" }
                    , new object[] { this.ID, this.SEQ, e }
                );

                if (Parent == null)
                {
                    Target.BaseControls.Remove(TitleEditor);
                }
            };
            BtContextMenu.Click += (object sender, MouseEventArgs e) =>
            {
                Event.CallEvent(this, BtContextMenuClick
                    , new string[] { "ID", "SEQ", "e" }
                    , new object[] { this.ID, this.SEQ, e }
                );

                if (Parent == null)
                {
                    Target.BaseControls.Remove(TitleEditor);
                }
            };

            BtContextMenu.Enabled = false;
        }
        #endregion

        #region Events
        private void SetEvent()
        {
            LocationChanged += GraphicPanel_LocationChanged;
            SizeChanged += GraphicPanel_SizeChanged;
        }

        private void GraphicPanel_LocationChanged(object sender, EventArgs e)
        {
            SetTitleEditorBoundsNFont();
        }

        private void GraphicPanel_SizeChanged(object sender, EventArgs e)
        {
            SetTitleEditorBoundsNFont();
        }
        #endregion
    }
}