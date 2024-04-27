using ST.Controls;
using ST.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace ST.Main
{
    public class MainMenuStrip
    {
        // Init
        public Control Parent;
        public MainMenuStripItemCollection Items;

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                if (_SelectedIndex != value)
                {
                    if (_SelectedIndex >= 0)
                    {
                        Items.ItemControls[_SelectedIndex].State = GraphicControl.StateType.Default;
                    }

                    if (value >= 0)
                    {
                        Items.ItemControls[value].State = GraphicControl.StateType.Active;
                    }

                    _SelectedIndex = value;
                }
            }
        }
        private int _SelectedIndex = -1;

        // Options
        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
                MinimizedButton.SetDrawBackColor(GraphicControl.StateType.Default, BackColor);
                MaximizedButton.SetDrawBackColor(GraphicControl.StateType.Default, BackColor);
                CloseButton.SetDrawBackColor(GraphicControl.StateType.Default, BackColor);
            }
        }
        private Color _BackColor = Color.White;

        public int MarginTop
        {
            get
            {
                return _MarginTop;
            }
            set
            {
                _MarginTop = value;
            }
        }
        private int _MarginTop = 1;

        public int MarginLeft
        {
            get
            {
                return _MarginLeft;
            }
            set
            {
                _MarginLeft = value;
            }
        }
        private int _MarginLeft = 1;

        public int MarginRight
        {
            get
            {
                return _MarginRight;
            }
            set
            {
                _MarginRight = value;
            }
        }
        private int _MarginRight = 1;

        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }
        private int _Height = 24;

        public Padding ItemPadding
        {
            get
            {
                return _ItemPadding;
            }
            set
            {
                _ItemPadding = value;
            }
        }
        private Padding _ItemPadding = new Padding(6, 2, 6, 2);

        public int ItemDistance
        {
            get
            {
                return _ItemDistance;
            }
            set
            {
                _ItemDistance = value;
            }
        }
        private int _ItemDistance = 2;

        public Font ItemFont
        {
            get
            {
                return _ItemFont;
            }
            set
            {
                _ItemFont = value;
            }
        }
        private Font _ItemFont = new Font("맑은 고딕", 9F);

        public int ItemsLeftMargin
        {
            get
            {
                return _ItemsLeftMargin;
            }
            set
            {
                _ItemsLeftMargin = value;
            }
        }
        private int _ItemsLeftMargin = 2;

        // Controls
        public GraphicControl MinimizedButton;
        public GraphicControl MaximizedButton;
        public GraphicControl CloseButton;

        public MainMenuStrip(Control parent)
        {
            Parent = parent;
            Items = new MainMenuStripItemCollection(this);
            LoadThis();
        }

        private void LoadThis()
        {
            SetGraphicControls();
            SetEvents();
        }

        private void SetGraphicControls()
        {
            MinimizedButton = new GraphicControl(Parent, "Details");
            MinimizedButton
                .SetArea(new Rectangle(-108, 1, 35, 23))
                .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.37f, 0.5f), new PointF(0.63f, 0.5f)
                })
                .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo(
                      Color.FromArgb(60, 60, 60)
                    , BackColor
                ))
                .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo(
                      Color.FromArgb(60, 60, 60)
                    , Color.FromArgb(229, 229, 229)
                ))
                .SetAutoDraw(false)
                .SetSmoothingMode(SmoothingMode.None);

            MaximizedButton = new GraphicControl(Parent, "Details");
            MaximizedButton
                .SetArea(new Rectangle(-72, 1, 35, 23))
                .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                        new PointF(0.36f, 0.3f), new PointF(0.62f, 0.3f)
                      , new PointF(0.36f, 0.7f), new PointF(0.62f, 0.7f)
                      , new PointF(0.36f, 0.3f), new PointF(0.36f, 0.7f)
                      , new PointF(0.63f, 0.29f), new PointF(0.63f, 0.71f)
                })
                .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo(
                      Color.FromArgb(60, 60, 60)
                    , BackColor
                ))
                .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo(
                      Color.FromArgb(60, 60, 60)
                    , Color.FromArgb(229, 229, 229)
                ))
                .SetAutoDraw(false)
                .SetSmoothingMode(SmoothingMode.None);

            CloseButton = new GraphicControl(Parent, "Details");
            CloseButton
                .SetArea(new Rectangle(-36, 1, 35, 23))
                .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.37f, 0.3f), new PointF(0.63f, 0.7f)
                    , new PointF(0.37f, 0.7f), new PointF(0.63f, 0.3f)
                })
                .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo(
                      Color.FromArgb(0, 0, 0)
                    , BackColor
                ))
                .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo(
                      Color.FromArgb(255, 255, 255)
                    , Color.Red
                ))
                .SetAutoDraw(false)
                .SetSmoothingMode(SmoothingMode.HighQuality);
        }

        private void SetEvents()
        {
            Parent.MouseDown += ParentMenuStrip_MouseDown;
            Parent.MouseMove += ParentMenuStrip_MouseMove;
            Parent.MouseLeave += Parent_MouseLeave;
            Parent.Paint += Parent_Paint;
        }

        private void ParentMenuStrip_MouseDown(object sender, MouseEventArgs e)
        {
            if
            (
                !MinimizedButton.Bounds.Contains(e.Location)
                &&
                !MaximizedButton.Bounds.Contains(e.Location)
                &&
                !CloseButton.Bounds.Contains(e.Location)
                &&
                !Items.IsCursorHover(e.Location)
            )
            {
                Refresh();
            }
        }

        private void ParentMenuStrip_MouseMove(object sender, MouseEventArgs e)
        {
            Refresh();
        }

        private void Parent_MouseLeave(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            BufferedGraphicsContext currentContext = new BufferedGraphicsContext();
            using (BufferedGraphics buffer = currentContext.Allocate(e.Graphics, new Rectangle(0, 0, Parent.Width, Height)))
            {
                Graphics g = buffer.Graphics;

                // BackGround
                g.FillRectangle(new SolidBrush(this.BackColor), 1, 1, Parent.Width - 2, Height - 1);

                // Items
                Items.DrawItemControls(g);

                // Buttons
                MinimizedButton.Draw(g);
                MaximizedButton.Draw(g);
                CloseButton.Draw(g);

                // Draw
                buffer.Render(e.Graphics);
            }
        }

        public void MaximizedButtonToMaximizedButton()
        {
            MaximizedButton.SetDrawPositionPercent(new PointF[] {
                new PointF(0.36f, 0.3f), new PointF(0.62f, 0.3f)
                , new PointF(0.36f, 0.7f), new PointF(0.62f, 0.7f)
                , new PointF(0.36f, 0.3f), new PointF(0.36f, 0.7f)
                , new PointF(0.63f, 0.29f), new PointF(0.63f, 0.71f)
            });
        }

        public void MaximizedButtonToRestoredButton()
        {
            float reH = 0.06f;
            float reV = 0.07f;
            MaximizedButton.SetDrawPositionPercent(new PointF[] {
                // Font Rectangle
                new PointF(0.36f, 0.3f + reV), new PointF(0.62f - reH, 0.3f + reV)
                , new PointF(0.36f, 0.7f), new PointF(0.62f - reH, 0.7f)
                , new PointF(0.36f, 0.3f + reV), new PointF(0.36f, 0.7f)
                , new PointF(0.63f - reH, 0.3f + reV), new PointF(0.63f - reH, 0.71f)
                // Back Rectangle
                , new PointF(0.36f + reH, 0.3f), new PointF(0.62f, 0.3f)
                , new PointF(0.63f, 0.29f), new PointF(0.63f, 0.7f - reV)
                , new PointF(0.36f + reH, 0.3f), new PointF(0.36f + reH, 0.3f + 0.05f)
                , new PointF(0.63f, 0.7f - reV), new PointF(0.63f - 0.05f, 0.7f - reV)
            });
        }

        public void Refresh()
        {
            Parent_Paint(Parent, new PaintEventArgs(Parent.CreateGraphics(), Parent.Bounds));
        }

        public class MainMenuStripItemCollection
        {
            public MainMenuStrip Parent;
            public List<ToolStripItem> Items = new List<ToolStripItem>();
            public List<GraphicControl> ItemControls = new List<GraphicControl>();

            public MainMenuStripItemCollection(MainMenuStrip parent)
            {
                Parent = parent;
            }

            public ToolStripItem this[int index]
            {
                get
                {
                    return Items[index];
                }
                set
                {
                    Items[index] = value;
                    SetItemsControl();
                }
            }

            public int Count => Items.Count;

            public void Add(ToolStripItem item)
            {
                Items.Add(item);
                SetItemsControl();
            }

            public void Remove(ToolStripItem item)
            {
                Items.Remove(item);
                SetItemsControl();
            }

            public void Clear()
            {
                Items.Clear();
                SetItemsControl();
            }

            private void SetItemsControl()
            {
                for(int i = 0; i < ItemControls.Count; i++)
                {
                    ItemControls[i].Click -= ItemControl_Click;
                    ItemControls[i].Dispose();
                }
                ItemControls.Clear();

                using (var g = Parent.Parent.CreateGraphics())
                {
                    int leftRef = 0;
                    for (int i = 0; i < Items.Count; i++)
                    {
                        SizeF textSize = g.MeasureString(Items[i].Text, Parent.ItemFont);
                        int controlWidth = (int)Math.Round(textSize.Width) + Parent.ItemPadding.Horizontal;
                        int controlHeight = (int)Math.Round(textSize.Height) + Parent.ItemPadding.Vertical;

                        Rectangle controlRectangle = new Rectangle(
                              Parent.ItemsLeftMargin + leftRef
                            , Parent.Height / 2 - controlHeight / 2
                            , controlWidth
                            , controlHeight
                        );

                        GraphicControl itemControl = new GraphicControl(Parent.Parent, Items[i].Name);
                        itemControl
                            .SetArea(controlRectangle)
                            .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                            .SetDrawPositionPercent(new PointF[] {
                                    new PointF(0f, 0f), new PointF(1f, 0f)
                                  , new PointF(0f, 1f), new PointF(1f, 1f)
                                  , new PointF(0f, 0f), new PointF(0f, 1f)
                                  , new PointF(1f, 0f), new PointF(1f, 1f)
                            })
                            .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo(
                                  Parent.BackColor
                                , Parent.BackColor
                                , Items[i].Text
                                , Color.FromArgb(0, 0, 0)
                            ))
                            .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo(
                                  Color.FromArgb(0, 120, 215)
                                , Color.FromArgb(179, 215, 243)
                                , Items[i].Text
                                , Color.FromArgb(0, 0, 0)
                            ))
                            .SetDrawInfo(GraphicControl.StateType.Active, new GraphicControl.DrawInfo(
                                  Color.FromArgb(0, 120, 215)
                                , Color.FromArgb(204, 213, 240)
                                , Items[i].Text
                                , Color.FromArgb(0, 0, 0)
                            ))
                            .SetDrawFont(Parent.ItemFont)
                            .SetDrawTextAlign(GraphicControl.TextAlignType.Center)
                            .SetAutoDraw(false)
                            .SetSmoothingMode(SmoothingMode.None);

                        itemControl.Click += ItemControl_Click;
                        ItemControls.Add(itemControl);

                        Items[i].Click += MainMenuStripItemCollection_Click;

                        leftRef += controlWidth + Parent.ItemDistance;
                    }
                }
            }

            private void MainMenuStripItemCollection_Click(object sender, EventArgs e)
            {
                ToolStripItem _this = (ToolStripItem)sender;
                int index = Items.IndexOf(_this);
                Parent.SelectedIndex = index;
            }

            private void ItemControl_Click(object sender, MouseEventArgs e)
            {
                GraphicControl _this = (GraphicControl)sender;
                int index = ItemControls.IndexOf(_this);
                Items[index].PerformClick();
            }

            public void DrawItemControls(Graphics g)
            {
                foreach (GraphicControl graphicControl in ItemControls)
                {
                    graphicControl.Draw(g);
                }
            }

            public bool IsCursorHover(Point point)
            {
                bool isHover = false;
                foreach (GraphicControl graphicControl in ItemControls)
                {
                    if (graphicControl.Bounds.Contains(point))
                    {
                        isHover = true;
                        break;
                    }
                }
                return isHover;
            }
        }
    }
}
