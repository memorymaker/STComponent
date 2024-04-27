using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Ink;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.Controls
{
    public class GraphicControl
    {
        // ---- Base
        public string Name = string.Empty;
        public Control Parent;
        
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
        public bool Visible = true;
        public Color DisableColor = Color.FromArgb(60, 0, 0, 0);
        
        // ---- Draw Info
        public Rectangle Area;
        public DrawTypeEnum DrawType;
        public PointF[] DrawPositionPercent;
        public float DrawWeight = 1f;
        public bool AutoDraw = true;
        public SmoothingMode SmoothingMode = SmoothingMode.HighQuality;

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

        public GraphicControl(Control parent, string name)
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
                return new Rectangle(Area.X < 0 ? Parent.Width + Area.X : Area.X, Area.Y < 0 ? Parent.Height + Area.Y : Area.Y, Area.Width, Area.Height);
            }
        }

        public GraphicControl SetArea(Rectangle area)
        {
            Area = area;
            return this;
        }

        public GraphicControl SetDrawType(DrawTypeEnum drawType)
        {
            DrawType = drawType;
            return this;
        }

        public GraphicControl SetDrawPositionPercent(PointF[] drawPositionPercent)
        {
            DrawPositionPercent = drawPositionPercent;
            return this;
        }

        public GraphicControl SetDrawWeight(float drawWeight)
        {
            DrawWeight = drawWeight;
            return this;
        }

        public GraphicControl SetAutoDraw(bool autoDraw)
        {
            AutoDraw = autoDraw;
            return this;
        }

        public GraphicControl SetSmoothingMode(SmoothingMode smoothingMode)
        {
            SmoothingMode = smoothingMode;
            return this;
        }

        public GraphicControl SetDrawTextAlign(TextAlignType drawTextAlign)
        {
            DrawTextAlign = drawTextAlign;
            return this;
        }

        public GraphicControl SetDrawFont(Font drawFont)
        {
            DrawTextFont = drawFont;
            return this;
        }

        public GraphicControl SetDrawInfo(StateType state, DrawInfo drawInfo)
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

        public GraphicControl SetDrawColor(StateType stateType, Color drawColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawColor = drawColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(drawColor, Color.Empty));
            }

            return this;
        }

        public GraphicControl SetDrawBackColor(StateType stateType, Color drawBackColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawBackColor = drawBackColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, drawBackColor));
            }

            return this;
        }

        public GraphicControl SetDrawFontColor(StateType stateType, Color drawFontColor)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawTextColor = drawFontColor;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, Color.Empty, null, drawFontColor));
            }

            return this;
        }

        public GraphicControl SetDrawText(StateType stateType, string drawText)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawText = drawText;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, Color.Empty, drawText, Color.Empty));
            }

            return this;
        }

        public GraphicControl SetDrawImage(StateType stateType, Bitmap drawImage)
        {
            if (DrawInfoDic.ContainsKey(stateType))
            {
                DrawInfoDic[stateType].DrawImage = drawImage;
            }
            else
            {
                DrawInfoDic.Add(stateType, new DrawInfo(Color.Empty, Color.Empty, "", Color.Empty, drawImage));
            }

            return this;
        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            if (Enabled && Visible)
            {
                if (Bounds.Contains(e.Location))
                {
                    MouseDown?.Invoke(this, e);
                    ThisAreaMouseDown = true;
                    ParentRefresh();
                }
            }
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            if (Enabled && Visible)
            {
                var _cursorPoint = Parent.PointToClient(Cursor.Position);
                if (Bounds.Contains(_cursorPoint))
                {
                    if (ThisAreaMouseDown)
                    {
                        Click?.Invoke(this, e);
                        ThisAreaMouseDown = false;
                        ParentRefresh();
                    }
                }
                else
                {
                    if (ThisAreaMouseDown)
                    {
                        ThisAreaMouseDown = false;
                        ParentRefresh();
                    }
                }
            }
        }

        public void Draw(Graphics g, StateType _stateType = StateType.None)
        {
            if (DrawType != DrawTypeEnum.None && Visible && Bounds.Width > 0 && Bounds.Height > 0)
            {
                //IScaleControl scaleControl = Parent as IScaleControl;
                //float scaleValue = 1f;
                //if (scaleControl != null)
                //{
                //    scaleValue = scaleControl.ScaleValue;
                //}

                var realArea = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
                var _cursorPoint = Parent.PointToClient(Cursor.Position);
                var _isOver = realArea.Contains(_cursorPoint);
                StateType drawState;
                if (_stateType != StateType.None)
                {
                    drawState = _stateType;
                }
                else
                {
                    if (ThisAreaMouseDown && DrawInfoDic.ContainsKey(StateType.MouseDown))
                    {
                        drawState = StateType.MouseDown;
                    }
                    else if (_isOver)
                    {
                        drawState = StateType.Over;
                    }
                    else
                    {
                        if (State == StateType.Active)
                        {
                            drawState = StateType.Active;
                        }
                        else
                        {
                            drawState = StateType.Default;
                        }
                    }
                }

                if (DrawInfoDic.ContainsKey(drawState))
                {
                    var _drawColor = DrawInfoDic[drawState].DrawColor;
                    var brush = new SolidBrush(_drawColor);
                    var pen = new Pen(_drawColor, DrawWeight);
                    var leftRevision = DrawInfoDic[drawState].LeftRevision;
                    var topRevision = DrawInfoDic[drawState].TopRevision;
                    var textLeftRevision = DrawInfoDic[drawState].TextLeftRevision;
                    var textTopRevision = DrawInfoDic[drawState].TextTopRevision;
                    // pen.Width = 1f;

                    var bitmap = new Bitmap(Bounds.Width, Bounds.Height);
                    Graphics bitmapGraphics = Graphics.FromImage(bitmap);

                    var _drawBackColor = DrawInfoDic[drawState].DrawBackColor == null && drawState != StateType.Default
                        ? DrawInfoDic[StateType.Default].DrawBackColor : DrawInfoDic[drawState].DrawBackColor;
                    if (_drawBackColor == null)
                    {
                        _drawBackColor = Parent.BackColor;
                    }

                    bitmapGraphics.FillRectangle(new SolidBrush((Color)_drawBackColor), new Rectangle(new Point(0, 0), realArea.Size));

                    switch (DrawType)
                    {
                        case DrawTypeEnum.FillRectangle:
                            break;
                        case DrawTypeEnum.FillPolygon:
                        case DrawTypeEnum.DrawLines:
                        case DrawTypeEnum.Text:
                            if (DrawType == DrawTypeEnum.FillPolygon)
                            {
                                var realDrawPosition = new PointF[DrawPositionPercent.Length];
                                for (int i = 0; i < DrawPositionPercent.Length; i++)
                                {
                                    realDrawPosition[i] = new PointF(
                                          0 + realArea.Width * DrawPositionPercent[i].X
                                        , 0 + realArea.Height * DrawPositionPercent[i].Y
                                    );
                                }

                                SmoothingMode oldSmoothingMode = bitmapGraphics.SmoothingMode;
                                bitmapGraphics.SmoothingMode = SmoothingMode;
                                {
                                    bitmapGraphics.FillPolygon(brush, realDrawPosition);
                                }
                                bitmapGraphics.SmoothingMode = oldSmoothingMode;
                            }
                            else if (DrawType == DrawTypeEnum.DrawLines)
                            {
                                var realDrawPosition = new PointF[DrawPositionPercent.Length];
                                for (int i = 0; i < DrawPositionPercent.Length; i++)
                                {
                                    realDrawPosition[i] = new PointF(
                                          0 + realArea.Width * DrawPositionPercent[i].X
                                        , 0 + realArea.Height * DrawPositionPercent[i].Y
                                    );
                                }

                                SmoothingMode oldSmoothingMode = bitmapGraphics.SmoothingMode;
                                bitmapGraphics.SmoothingMode = SmoothingMode;
                                {
                                    for (int i = 0; i < realDrawPosition.Length; i = i + 2)
                                    {
                                        bitmapGraphics.DrawLine(pen, realDrawPosition[i], realDrawPosition[i + 1]);
                                    }
                                }
                                bitmapGraphics.SmoothingMode = oldSmoothingMode;
                            }

                            // DrawTypeEnum.FillPolygon, DrawTypeEnum.DrawLines, DrawTypeEnum.Text
                            {
                                // Draw String
                                if (!string.IsNullOrWhiteSpace(DrawInfoDic[drawState].DrawText))
                                {
                                    string text = DrawInfoDic[drawState].DrawText.Trim();
                                    Point drawStringPoint = Point.Empty;
                                    if (DrawTextAlign == TextAlignType.Center)
                                    {
                                        SizeF textSize = bitmapGraphics.MeasureString(text, DrawTextFont);
                                        drawStringPoint = new Point(
                                            (int)Math.Round(realArea.Width / 2 - textSize.Width / 2)
                                            , (int)Math.Round(realArea.Height / 2 - textSize.Height / 2)
                                        );
                                    }

                                    bitmapGraphics.DrawString(
                                        text
                                        , DrawTextFont
                                        , new SolidBrush(DrawInfoDic[drawState].DrawTextColor)
                                        , drawStringPoint
                                    );
                                }
                            }
                            break;
                        case DrawTypeEnum.Image:
                            {
                                Bitmap image = DrawInfoDic[drawState].DrawImage == null && drawState != StateType.Default
                                    ? DrawInfoDic[StateType.Default].DrawImage : DrawInfoDic[drawState].DrawImage;

                                if (image != null)
                                {
                                    bitmapGraphics.DrawImage(image,
                                        (realArea.Width - image.Width) / 2,
                                        (realArea.Height - image.Height) / 2,
                                        image.Width,
                                        image.Height);
                                }
                            }
                            break;
                        case DrawTypeEnum.ImageTextLeftRight:
                            {
                                Bitmap image = DrawInfoDic[drawState].DrawImage == null && drawState != StateType.Default
                                    ? DrawInfoDic[StateType.Default].DrawImage : DrawInfoDic[drawState].DrawImage;

                                if (image != null)
                                {
                                    int distance = 4;
                                    string text = DrawInfoDic[drawState].DrawText == null && drawState != StateType.Default
                                        ? DrawInfoDic[StateType.Default].DrawText : DrawInfoDic[drawState].DrawText;
                                    SizeF textSize = bitmapGraphics.MeasureString(text, DrawTextFont);

                                    int imageTop = (realArea.Height - image.Height) / 2;
                                    int imageLeft = string.IsNullOrWhiteSpace(text)
                                        ? (realArea.Width - image.Width) / 2
                                        : (realArea.Width - (image.Width + distance + textSize.Width.ToInt())) / 2;
                                    bitmapGraphics.DrawImage(image, imageLeft + leftRevision, imageTop + topRevision, image.Width, image.Height);

                                    if (!string.IsNullOrWhiteSpace(text))
                                    {
                                        text = text.Trim();
                                        Color textColor = DrawInfoDic[drawState].DrawTextColor == Color.Empty && drawState != StateType.Default
                                            ? DrawInfoDic[StateType.Default].DrawTextColor : DrawInfoDic[drawState].DrawTextColor;
                                        
                                        bitmapGraphics.DrawString(
                                            text
                                            , DrawTextFont
                                            , new SolidBrush(textColor)
                                            , new Point(imageLeft + image.Width + distance + textLeftRevision + leftRevision, ((realArea.Height - textSize.Height) / 2).ToInt() + textTopRevision + topRevision)
                                        );
                                    }
                                }
                            }
                            break;
                        case DrawTypeEnum.ImageTextTopBottom:
                            {
                                Bitmap image = DrawInfoDic[drawState].DrawImage == null && drawState != StateType.Default
                                    ? DrawInfoDic[StateType.Default].DrawImage : DrawInfoDic[drawState].DrawImage;

                                if (image != null)
                                {
                                    int distance = 4;
                                    string text = DrawInfoDic[drawState].DrawText == null && drawState != StateType.Default
                                        ? DrawInfoDic[StateType.Default].DrawText : DrawInfoDic[drawState].DrawText;
                                    SizeF textSize = bitmapGraphics.MeasureString(text, DrawTextFont);

                                    int imageLeft = (realArea.Width - image.Width) / 2;
                                    int imageTop = string.IsNullOrWhiteSpace(text)
                                        ? (realArea.Height - image.Height) / 2
                                        : (realArea.Height - (image.Height + distance + textSize.Height.ToInt())) / 2;
                                    bitmapGraphics.DrawImage(image, imageLeft + leftRevision, imageTop + topRevision, image.Width, image.Height);

                                    if (!string.IsNullOrWhiteSpace(text))
                                    {
                                        Color textColor = DrawInfoDic[drawState].DrawTextColor == Color.Empty && drawState != StateType.Default
                                            ? DrawInfoDic[StateType.Default].DrawTextColor : DrawInfoDic[drawState].DrawTextColor;

                                        bitmapGraphics.DrawString(
                                            text
                                            , DrawTextFont
                                            , new SolidBrush(textColor)
                                            , new Point(((realArea.Width - textSize.Width) / 2).ToInt() + textLeftRevision + leftRevision, imageTop + image.Height + distance + textTopRevision + topRevision)
                                        );
                                    }
                                }
                            }
                            break;
                    }

                    // Border
                    Color? borderColor = DrawInfoDic[drawState].DrawBorderColor == null && drawState != StateType.Default
                        ? DrawInfoDic[StateType.Default].DrawBorderColor : DrawInfoDic[drawState].DrawBorderColor;

                    if (borderColor != null)
                    {
                        bitmapGraphics.DrawRectangle(new Pen((Color)borderColor), new Rectangle(
                            0, 0, realArea.Width - 1, realArea.Height - 1));
                    }

                    if (!Enabled)
                    {
                        bitmapGraphics.FillRectangle(new SolidBrush(DisableColor), 0, 0, Bounds.Width, Bounds.Height);
                    }

                    g.DrawImage(bitmap, realArea.Location);
                    bitmapGraphics.Dispose();
                }
            }
        }

        private void ParentRefresh()
        {
            IGraphicControlParent parent = Parent as IGraphicControlParent;
            if (parent != null)
            {
                parent.Redraw();
            }
            else
            {
                using(var g = Parent.CreateGraphics())
                {
                    Rectangle controlBounds = Bounds;
                    Rectangle srcRect = new Rectangle(0, 0, controlBounds.Width, controlBounds.Height);

                    // Clip #1 Start
                    g.Clip = new Region(controlBounds);

                    // GraphicsContainer #1 Start
                    GraphicsContainer containerState = g.BeginContainer(
                        srcRect
                        , srcRect
                        , GraphicsUnit.Pixel
                    );

                    // Call Draw
                    Draw(g);

                    // GraphicsContainer #2 End
                    g.EndContainer(containerState);

                    // Clip #2 End
                    g.ResetClip();
                }
            }
        }

        public void Dispose()
        {
            Parent.MouseDown -= Parent_MouseDown;
            Parent.MouseUp -= Parent_MouseUp;
        }

        public void OnClick()
        {
            Click?.Invoke(this, null);
        }

        public enum DrawTypeEnum
        {
            None = 0,
            FillRectangle,
            FillPolygon,
            DrawLines,
            CustomDrawX,
            Text,
            Image,
            ImageTextTopBottom,
            ImageTextLeftRight,
        }

        public enum StateType
        {
            None = 0,
            Default,
            Over,
            Active,
            MouseDown,
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
            public Color? DrawBackColor;
            public Color DrawColor;
            public string DrawText;
            public Color DrawTextColor;
            public Bitmap DrawImage;
            public Color? DrawBorderColor;
            public int LeftRevision;
            public int TopRevision;
            public int TextLeftRevision;
            public int TextTopRevision;

            public DrawInfo()
            {
            }

            public DrawInfo(Color? _DrawColor = null, Color? _DrawBackColor = null, string _DrawText = null, Color? _DrawFontColor = null, Bitmap _DrawImage = null, Color? _DrawBorderColor = null, int _LeftRevision = 0, int _TopRevision = 0, int _TextLeftRevision = 0, int _TextTopRevision = 0)
            {
                DrawColor = _DrawColor ?? Color.FromArgb(0, 0, 0);
                DrawBackColor = _DrawBackColor;
                DrawText = _DrawText;
                DrawTextColor = _DrawFontColor ?? Color.FromArgb(0, 0, 0);
                DrawImage = _DrawImage;
                DrawBorderColor = _DrawBorderColor;
                LeftRevision = _LeftRevision;
                TopRevision = _TopRevision;
                TextLeftRevision = _TextLeftRevision;
                TextTopRevision = _TextTopRevision;
            }
        }
    }
}