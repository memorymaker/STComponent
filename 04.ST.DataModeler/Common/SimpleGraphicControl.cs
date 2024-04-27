using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public class SimpleGraphicControl
    {
        // ---- Base
        public string Name = string.Empty;
        public GraphicControl Parent;
        
        public StateType State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }
        private StateType _State = StateType.Default;

        // ---- Option
        public bool Enabled = true;

        // ---- Draw Info
        public Rectangle Area;
        public DrawTypeEnum DrawType;
        public PointF[] DrawPositionPercent;
        public float DrawWeight = 1f;
        public bool AutoDraw = true;
        public SmoothingMode SmoothingMode = SmoothingMode.HighQuality;
        public DrawItem[] DrawItems;

        // Text
        public TextAlignType DrawTextAlign = TextAlignType.Center;
        public Font DrawTextFont;

        // DrawInfoDic Private
        private Dictionary<StateType, DrawInfo> DrawInfoDic = new Dictionary<StateType, DrawInfo>();

        // ---- Event
        public event MouseEventHandler Click;
        public event MouseEventHandler MouseDown;

        // ---- Ref
        private bool ThisAreaMouseDown = false;

        public SimpleGraphicControl(GraphicControl parent, string name)
        {
            Parent = parent;
            Parent.MouseDown += Parent_MouseDown;
            Parent.MouseUp += Parent_MouseUp;
            Name = name;
        }

        public Rectangle Bounds
        {
            get
            {
                IScaleControl scaleControl = Parent as IScaleControl;
                float scaleValue = 1f;
                if (scaleControl != null)
                {
                    scaleValue = scaleControl.ScaleValue;
                }

                //return new Rectangle(Area.X < 0 ? Parent.Width + Area.X : Area.X, Area.Y < 0 ? Parent.Height + Area.Y : Area.Y, Area.Width, Area.Height);
                var x = (Area.X * scaleValue).ToInt();
                var y = (Area.Y * scaleValue).ToInt();
                var width = (Area.Width * scaleValue).ToInt();
                var height = (Area.Height * scaleValue).ToInt();

                return new Rectangle(
                      x < 0 ? Parent.Width + x : x
                    , y < 0 ? Parent.Height + y : y
                    , width
                    , height
                );
            }
        }

        public SimpleGraphicControl SetArea(Rectangle area)
        {
            Area = area;
            return this;
        }

        public SimpleGraphicControl SetDrawType(DrawTypeEnum drawType)
        {
            DrawType = drawType;
            return this;
        }

        public SimpleGraphicControl SetDrawPositionPercent(PointF[] drawPositionPercent)
        {
            DrawPositionPercent = drawPositionPercent;
            return this;
        }

        public SimpleGraphicControl SetDrawItems(DrawItem[] drawItems)
        {
            DrawItems = drawItems;
            return this;
        }

        public SimpleGraphicControl SetDrawWeight(float drawWeight)
        {
            DrawWeight = drawWeight;
            return this;
        }

        public SimpleGraphicControl SetAutoDraw(bool autoDraw)
        {
            AutoDraw = autoDraw;
            return this;
        }

        public SimpleGraphicControl SetSmoothingMode(SmoothingMode smoothingMode)
        {
            SmoothingMode = smoothingMode;
            return this;
        }

        public SimpleGraphicControl SetDrawTextAlign(TextAlignType drawTextAlign)
        {
            DrawTextAlign = drawTextAlign;
            return this;
        }

        public SimpleGraphicControl SetDrawFont(Font drawFont)
        {
            DrawTextFont = drawFont;
            return this;
        }

        public SimpleGraphicControl SetDrawInfo(StateType state, DrawInfo drawInfo)
        {
            if (DrawInfoDic.ContainsKey(state))
            {
                DrawInfoDic[state] = drawInfo;
            }
            else
            {
                DrawInfoDic.Add(state, drawInfo);
            }

            return this;
        }

        public SimpleGraphicControl SetDrawColor(StateType stateType, Color drawColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawColor = drawColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(drawColor, Color.Empty));
                // throw new Exception(string.Format("there is no state type [{0}] in DrawInfoDic. GraphicControl.SetDrawColor", stateType.ToString()));
            }

            return this;
        }

        public SimpleGraphicControl SetDrawBackColor(StateType stateType, Color drawBackColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawBackColor = drawBackColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, drawBackColor));
                // throw new Exception(string.Format("there is no state type [{0}] in DrawInfoDic. GraphicControl.SetDrawBackColor", stateType.ToString()));
            }

            return this;
        }

        public SimpleGraphicControl SetDrawFontColor(StateType stateType, Color drawFontColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawTextColor = drawFontColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, Color.Empty, null, drawFontColor));
                // throw new Exception(string.Format("there is no state type [{0}] in DrawInfoDic. GraphicControl.SetDrawFontColor", stateType.ToString()));
            }

            return this;
        }

        public SimpleGraphicControl SetDrawText(StateType stateType, string drawText)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawText = drawText;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, Color.Empty, drawText, Color.Empty));
                // throw new Exception(string.Format("there is no state type [{0}] in DrawInfoDic. GraphicControl.SetDrawText", stateType.ToString()));
            }

            return this;
        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            if (Enabled)
            {
                var _cursorPoint = Parent.PointToClient(Cursor.Position);
                if (Bounds.Contains(_cursorPoint))
                {
                    MouseDown?.Invoke(this, e);
                    ThisAreaMouseDown = true;
                }
                else
                {
                    ThisAreaMouseDown = false;
                }
            }
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            if (Enabled)
            {
                var _cursorPoint = Parent.PointToClient(Cursor.Position);
                if (Bounds.Contains(_cursorPoint))
                {
                    if (ThisAreaMouseDown)
                    {
                        Click?.Invoke(this, e);
                    }
                }
                ThisAreaMouseDown = false;
            }
        }

        public void Draw(Graphics g)
        {
            if (DrawType != DrawTypeEnum.None && Enabled && Bounds.Width > 0 && Bounds.Height > 0)
            {
                //IScaleControl scaleControl = Parent as IScaleControl;
                //float scaleValue = 1f;
                //if (scaleControl != null)
                //{
                //    scaleValue = scaleControl.ScaleValue;
                //}

                var realArea = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width - 1, Bounds.Height - 1);
                var _cursorPoint = Parent.PointToClient(Cursor.Position);
                var _isOver = realArea.Contains(_cursorPoint);
                var drawState = _isOver ? StateType.Over : State;

                if (DrawInfoDic.ContainsKey(drawState))
                {
                    var _drawColor = DrawInfoDic[drawState].DrawColor;
                    var brush = new SolidBrush(_drawColor);
                    var pen = new Pen(_drawColor, DrawWeight);
                    // pen.Width = 1f;

                    var _drawBackColor = DrawInfoDic[drawState].DrawBackColor;
                    g.FillRectangle(new SolidBrush(_drawBackColor), new Rectangle(new Point(0, 0), realArea.Size));

                    if (DrawType == DrawTypeEnum.Multi)
                    {
                        foreach(DrawItem item in DrawItems)
                        {
                            DrawProc(g, realArea, item.DrawType, item.DrawPositionPercent, brush, pen, drawState);
                        }
                    }
                    else
                    {
                        DrawProc(g, realArea, DrawType, DrawPositionPercent, brush, pen, drawState);
                    }
                }
            }
        }

        public void DrawProc(Graphics g, Rectangle realArea, DrawTypeEnum drawType, PointF[] drawPositionPercent, Brush brush, Pen pen, StateType drawState)
        {
            switch (drawType)
            {
                case DrawTypeEnum.FillRectangle:
                    break;
                case DrawTypeEnum.FillPolygon:
                    {
                        SmoothingMode oldSmoothingMode = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode;
                        {
                            var realDrawPosition = new List<PointF>();
                            var startPoint = PointF.Empty;
                            for (int i = 0; i < drawPositionPercent.Length; i++)
                            {
                                PointF node = new PointF(
                                      realArea.Left + realArea.Width * drawPositionPercent[i].X
                                    , realArea.Top + realArea.Height * drawPositionPercent[i].Y
                                );

                                if (startPoint == PointF.Empty)
                                {
                                    startPoint = node;
                                    realDrawPosition.Add(node);
                                }
                                else if (startPoint == node)
                                {
                                    g.FillPolygon(brush, realDrawPosition.ToArray());
                                    startPoint = PointF.Empty;
                                    realDrawPosition.Clear();
                                }
                                else
                                {
                                    realDrawPosition.Add(node);
                                }
                            }
                            g.FillPolygon(brush, realDrawPosition.ToArray());
                        }
                        g.SmoothingMode = oldSmoothingMode;
                    }
                    break;
                case DrawTypeEnum.DrawLines:
                    {
                        var realDrawPosition = new PointF[drawPositionPercent.Length];
                        for (int i = 0; i < drawPositionPercent.Length; i++)
                        {
                            realDrawPosition[i] = new PointF(
                                  realArea.Left + realArea.Width * drawPositionPercent[i].X
                                , realArea.Top + realArea.Height * drawPositionPercent[i].Y
                            );
                        }

                        SmoothingMode oldSmoothingMode = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode;
                        {
                            for (int i = 0; i < realDrawPosition.Length; i = i + 2)
                            {
                                g.DrawLine(pen, realDrawPosition[i], realDrawPosition[i + 1]);
                            }
                        }
                        g.SmoothingMode = oldSmoothingMode;
                    }
                    break;
            }

            // Draw String
            if (!string.IsNullOrWhiteSpace(DrawInfoDic[drawState].DrawText))
            {
                string text = DrawInfoDic[drawState].DrawText.Trim();
                Point drawStringPoint = Point.Empty;
                if (DrawTextAlign == TextAlignType.Center)
                {
                    SizeF textSize = g.MeasureString(text, DrawTextFont);
                    drawStringPoint = new Point(
                          (int)Math.Round(realArea.Width / 2 - textSize.Width / 2)
                        , (int)Math.Round(realArea.Height / 2 - textSize.Height / 2)
                    );
                }

                g.DrawString(
                    text
                    , DrawTextFont
                    , new SolidBrush(DrawInfoDic[drawState].DrawTextColor)
                    , drawStringPoint
                );
            }
        }

        public void Dispose()
        {
            Parent.MouseDown -= Parent_MouseDown;
            Parent.MouseUp -= Parent_MouseUp;
        }

        public enum DrawTypeEnum
        {
            None = 0,
            FillRectangle,
            FillPolygon,
            DrawLines,
            CustomDrawX,
            Multi
        }

        public enum StateType
        {
            Default = 1,
            Over,
            Active,
            Disable
        }

        public enum TextAlignType
        {
            None = 0,
            Left,
            Center,
            Right
        }

        public class DrawInfo
        {
            public Color DrawColor;
            public Color DrawBackColor;
            public string DrawText;
            public Color DrawTextColor;

            public DrawInfo(Color _DrawColor, Color _DrawBackColor, string _DrawText = null, Color? _DrawFontColor = null)
            {
                DrawColor = _DrawColor;
                DrawBackColor = _DrawBackColor;
                DrawText = _DrawText;
                DrawTextColor = _DrawFontColor ?? Color.FromArgb(0, 0, 0);
            }
        }

        public class DrawItem
        {
            public DrawTypeEnum DrawType;
            public PointF[] DrawPositionPercent;

            public DrawItem(DrawTypeEnum drawType, PointF[] positionPercent)
            {
                DrawType = drawType;
                DrawPositionPercent = positionPercent;
            }
        }
    }
}
