using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using ST.Core;
using ST.Controls;

namespace Common
{
    public partial class UserNodeBase : UserControl, IMouseActionTarget
    {
        #region Values
        #region EventHandler
        new public event MouseEventHandler MouseUp;
        public event UserEventHandler SizeLocationChanged;
        public event UserEventHandler IDChanging;
        public event UserEventHandler BtDeleteMouseDown;
        #endregion

        #region Classes
        protected GraphicControl BtDelete;
        public MouseAction MouseAction;
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
                    Draw();
                }
            }
        }
        private Padding _Padding = new Padding(3, 3, 3, 3);

        public string ID
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
                    Title = value;
                }
            }
        }
        private string _ID;

        public int SEQ { get; set; }

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
                    Draw();
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
                    Draw();
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
                    Draw();
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
                    Draw();
                }
            }
        }
        private string _Title = string.Empty;
        #endregion

        #region Load
        public UserNodeBase()
        {
            InitializeComponent();
            this.LoadThis();
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

            // MouseAction
            MouseAction = new MouseAction(this);
            MouseAction.UseLocationCorrection = false;

            // TitleEditor
            TitleEditor = new TextBox();
            TitleEditor.BorderStyle = BorderStyle.None;
            TitleEditor.Name = "BaseTextBoxTitle";
            TitleEditor.Visible = false;
            TitleEditor.Width = Width;
            Controls.Add(TitleEditor);
            
            // UserPanelControl
            int marginTotal = (4 * ScaleValue).ToInt();
            int areaSideLength = TitleHeight + Padding.Top - marginTotal;
            BtDelete = new GraphicControl(this, "Delete");
            BtDelete
                .SetArea(new Rectangle(-(areaSideLength + (marginTotal / 2)), (marginTotal / 2), areaSideLength, areaSideLength))
                .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.25f, 0.25f), new PointF(0.75f, 0.75f) // /
                    , new PointF(0.75f, 0.25f), new PointF(0.25f, 0.75f) // \
                })
                //.SetDrawColorNomal(Color.FromArgb(110, 132, 180))
                .SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(220, 231, 241))
                .SetDrawColor(GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255))
                .SetDrawWeight(1.6f);
            BtDelete.MouseDown += (object sender, MouseEventArgs e) =>
            {
                Event.CallEvent(this, BtDeleteMouseDown
                    , new string[] { "ID", "SEQ" }
                    , new object[] { this.ID, this.SEQ }
                );
            };
            BtDelete.Enabled = true;

            this.OnSizeChanged(null);
        }
        #endregion

        #region Static 
        public static UserTableNode GetUserTableNode(Control parent, string TABLE_ID, int SEQ)
        {
            foreach (UserTableNode uTable in parent.Controls)
            {
                if (uTable.TABLE_ID == TABLE_ID && uTable.SEQ == SEQ)
                {
                    return uTable;
                }
            }
            return null;
        }

        public static UserTableNode GetUserTableNode(ArrayList arr, string TABLE_ID, int SEQ)
        {
            foreach (UserTableNode uTable in arr)
            {
                if (uTable.TABLE_ID == TABLE_ID && uTable.SEQ == SEQ)
                {
                    return uTable;
                }
            }
            return null;
        }

        public static bool HasUserTalbe(Control parent, string TABLE_ID, int SEQ)
        {
            foreach (Control node in parent.Controls)
            {
                if (node.GetType() == typeof(UserTableNode))
                {
                    UserTableNode userTableNode = (UserTableNode)node;
                    if (userTableNode.TABLE_ID == TABLE_ID && userTableNode.SEQ == SEQ)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static UserTableNode FindTableNode(Control prent, string TABLE_ID, int SEQ)
        {
            for (int i = 0; i < prent.Controls.Count; i++)
            {
                UserTableNode userTableNode = (UserTableNode)prent.Controls[i];
                if (userTableNode.TABLE_ID == TABLE_ID && userTableNode.SEQ == SEQ)
                {
                    return (UserTableNode)prent.Controls[i];
                }
            }
            return null;
        }
        #endregion

        #region Events
        private void SetEvent()
        {
            this.PreviewKeyDown += UserNodeBase_PreviewKeyDown;
            this.MouseDown += UserNodeBase_MouseDown;
            this.MouseMove += UserNodeBase_MouseMove;
            this.MouseUp += UserNodeBase_MouseUp;
            this.MouseLeave += UserNodeBase_MouseLeave;
            this.Click += UserNodeBase_Click;
            this.SizeChanged += UserNodeBase_SizeChanged;
            this.Paint += UserNodeBase_Paint;

            TitleEditor.KeyDown += Txt_KeyDown;
            TitleEditor.LostFocus += Txt_LostFocus;
        }

        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TitleEditor.Visible = false;
                Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //this.ID = txt.Text;
                TitleEditor.Visible = false;
                Event.CallEvent(this, this.IDChanging
                    , new string[] { "NEW_ID", "ID", "SEQ" }
                    , new object[] { TitleEditor.Text, this.ID, this.SEQ }
                );
            }
        }

        private void Txt_LostFocus(object sender, EventArgs e)
        {
            TitleEditor.Visible = false;
            //if (txt.Visible)
            //{
            //    this.Txt_KeyDown(sender, new KeyEventArgs(Keys.Enter));
            //}
        }

        private void UserNodeBase_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Label _this = (Label)sender;
            // Ctrn + C
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                Clipboard.SetText(this.ID);
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (this.EnableCaptionEdit)
                {
                    TitleEditor.Font = Font;
                    TitleEditor.ForeColor = ForeColor;
                    TitleEditor.BackColor = BackColor;
                    TitleEditor.Left = 7;
                    TitleEditor.Top = 5;
                    TitleEditor.Width = Width - 14;
                    TitleEditor.Text = this.Title;
                    TitleEditor.Visible = true;
                    TitleEditor.Focus();
                    TitleEditor.SelectionStart = TitleEditor.Text.Length;
                }
            }
        }

        private void UserNodeBase_MouseDown(object sender, MouseEventArgs e)
        {
            UserScaleControlWarpPanel parent = Parent as UserScaleControlWarpPanel;
            if (parent != null)
            {
                parent.BringToFrontChild(this);
            }
            else
            {
                BringToFront();
            }
        }

        private void UserNodeBase_MouseMove(object sender, MouseEventArgs e)
        {
            BtDelete.Enabled = e.Y <= TitleHeight;
            Draw();
        }

        private void UserNodeBase_MouseUp(object sender, MouseEventArgs e)
        {
            Event.CallEvent(this, this.SizeLocationChanged
                , new string[] { "Left", "Top", "Width", "Height" }
                , new object[] { this.Left, this.Top, this.Width, this.Height }
            );

            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }

        }

        private void UserNodeBase_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            BtDelete.Enabled = false;
            Draw();
        }

        private void UserNodeBase_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        private void UserNodeBase_SizeChanged(object sender, EventArgs e)
        {
            int marginTotal = (4 * ScaleValue).ToInt();
            int areaSideLength = TitleHeight + Padding.Top - marginTotal;
            BtDelete.SetArea(new Rectangle(-(areaSideLength + (marginTotal / 2)), (marginTotal / 2), areaSideLength, areaSideLength));
            BtDelete.SetDrawWeight(1.6f * ScaleValue);
        }

        private void UserNodeBase_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }
        #endregion

        #region Functions
        private void Draw(Graphics g = null)
        {
            if (BtDelete != null)
            {
                if (g == null)
                {
                    g = CreateGraphics();
                }

                // Set bitmap
                var bitmap = new Bitmap(Width, Height);
                Graphics bitmapGraphics = Graphics.FromImage(bitmap);
                bitmapGraphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width, Height));

                // Draw Title
                int titleHeightAPaddingTop = TitleHeight + Padding.Top;
                Font scaleFont = new Font(Font.FontFamily, Font.Size * ScaleValue
                    , (TitleBold ? FontStyle.Bold : FontStyle.Regular)
                );
                var scaleFontSize = bitmapGraphics.MeasureString(Title, scaleFont);
                float leftNtop = (titleHeightAPaddingTop / 2) - (scaleFontSize.Height / 2);
                bitmapGraphics.DrawString(Title, scaleFont, new SolidBrush(ForeColor), leftNtop, leftNtop);

                // Draw DeleteButton
                bitmapGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //int marginTotal = (4 * ScaleValue).ToInt();
                //int scaleAreaSideLength = TitleHeight + Padding.Top - marginTotal;
                //BtDelete.SetArea(new Rectangle(-(scaleAreaSideLength + (marginTotal / 2)), (marginTotal / 2), scaleAreaSideLength, scaleAreaSideLength));
                
                BtDelete.Draw(bitmapGraphics);
                bitmapGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                // ------------ Draw
                g.DrawImage(bitmap, 0, 0);
                bitmapGraphics.Dispose();
            }
        }
        #endregion
    }
}