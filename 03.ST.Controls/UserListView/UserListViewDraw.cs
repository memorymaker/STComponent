using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ST.Controls
{
    public partial class UserListView
    {
        #region Options
        public Color SelectedItemBorderColor = Color.FromArgb(170, 218, 254);
        public Color SelectedItemBackColor = Color.FromArgb(229, 243, 254);

        public int ColumnHeight
        {
            get
            {
                return _ColumnHeight;
            }
            set
            {
                _ColumnHeight = value;
                Draw();
            }
        }
        public int _ColumnHeight = 26;

        public int ColumnHorizontalDistance
        {
            get
            {
                return _ColumnHorizontalDistance;
            }
            set
            {
                _ColumnHorizontalDistance = value;
                Draw();
            }
        }
        public int _ColumnHorizontalDistance = 0;

        public int ItemHeight
        {
            get
            {
                return _ItemHeight;
            }
            set
            {
                _ItemHeight = value;
                Draw();
            }
        }
        private int _ItemHeight = 20;

        public Padding ItemPadding
        {
            get
            {
                return _ItemPadding;
            }
            set
            {
                _ItemPadding = ItemPadding;
                Draw();
            }
        }
        private Padding _ItemPadding = new Padding(4);

        public int ItemVerticalDistance
        {
            get
            {
                return _ItemVerticalDistance;
            }
            set
            {
                _ItemVerticalDistance = value;
                Draw();
            }
        }
        private int _ItemVerticalDistance = 1;

        public Color ItemVerticalDistanceColor
        {
            get
            {
                return _ItemVerticalDistanceColor;
            }
            set
            {
                _ItemVerticalDistanceColor = value;
                Draw();
            }
        }
        private Color _ItemVerticalDistanceColor = Color.FromArgb(245, 245, 245);

        public Color DisableColor = Color.FromArgb(60, 0, 0, 0);

        public bool BlockDrawing = false;
        public bool _SuspendLayout = false;
        #endregion

        #region System Options
        public readonly int ItemRelativeLeft = 0;
        public readonly int ItemRelativeTop = 1;
        #endregion

        #region Reference
        public BufferedGraphics Grafx;
        public BufferedGraphicsContext Context = BufferedGraphicsManager.Current;
        public bool Binded = false;
        public Size FontPixelSize;
        #endregion

        public void Draw()
        {
            if (!BlockDrawing && !_SuspendLayout)
            {
                // Content
                DrawSetGrafx();
                DrawContent();
                DrawRenderGrafx();

                // Scroll
                ScrollBarVertical.Draw();
            }
        }

        private void DrawContent()
        {
            this.Grafx.Graphics.Clear(BackColor);
            Graphics g = this.Grafx.Graphics;

            DrawContentItems(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop, ItemHeight, ColumnHorizontalDistance, ItemVerticalDistance);
            DrawContentSelector(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop, ItemHeight, ColumnHorizontalDistance, ItemVerticalDistance);
            DrawContentDrag(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop);
            DrawContentHeader(g, Bounds, ScaleValue, ColumnHeight, ColumnHorizontalDistance);

            if (!Enabled)
            {
                g.FillRectangle(new SolidBrush(DisableColor), new Rectangle(0, 0, Width, Height));
            }
        }

        private void DrawContentHeader(Graphics g, Rectangle bounds, float ScaleValue, int columnHeight, int columnHorizontalDistance)
        {
            // Set targetColumns
            List<UserListViewColumn> targetColumns = Columns;
            if (Columns == null || Columns.Count == 0)
            {
                targetColumns = AutoColumns;
            }

            int drawLeft = 0;
            foreach (UserListViewColumn column in targetColumns)
            {
                // Scale
                int scaleColumnHeight = (int)Math.Round(columnHeight * ScaleValue);
                int scaleColumnWidth = (int)Math.Round(column.Width * ScaleValue);
                int scaleColumnHorizontalDistance = (int)Math.Round(columnHorizontalDistance * ScaleValue);

                if (scaleColumnHeight > 0 && scaleColumnWidth > 0)
                {
                    // Back
                    SolidBrush backBrush = new SolidBrush(column.BackColor);
                    Rectangle backRectangle = new Rectangle(drawLeft, 0, scaleColumnWidth, scaleColumnHeight);
                    g.FillRectangle(backBrush, backRectangle);

                    // Set nXStart, nYStart(Used in Draw String)
                    // Scale
                    Font scaleColumnFont = new Font(column.Font.FontFamily, column.Font.Size * ScaleValue, column.Font.Style);
                    SizeF scaleLinePixelSize = g.MeasureString(column.Text, scaleColumnFont);

                    // Scale
                    float scaleLinePixelSizeWidth = scaleLinePixelSize.Width;
                    float scaleLinePixelSizeHeight = scaleLinePixelSize.Height;
                    int nXStart = (int)Math.Round(drawLeft + scaleColumnWidth / 2 - scaleLinePixelSizeWidth / 2);
                    int nYStart = (int)Math.Round(0 + scaleColumnHeight / 2 - scaleLinePixelSizeHeight / 2);
                    
                    // Draw String
                    float minLeft = drawLeft + 3 * ScaleValue;
                    RectangleF stringRectangle = new RectangleF(
                          Math.Max(minLeft, nXStart), nYStart
                        , Math.Min(scaleColumnWidth, scaleLinePixelSizeWidth), scaleLinePixelSizeHeight);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                    SolidBrush stringBrush = new SolidBrush(column.ForeColor);
                    g.DrawString(column.Text, scaleColumnFont, stringBrush, stringRectangle, stringFormat);
                }

                drawLeft += scaleColumnWidth + scaleColumnHorizontalDistance;
            }
        }

        private void DrawContentItems(Graphics g, Rectangle bounds, float ScaleValue, int left, int top, int itemHeight, int columnHorizontalDistance, int itemVerticalDistance)
        {
            if (Data != null && Data.Rows.Count > 0)
            {
                // Set targetColumns
                List<UserListViewColumn> targetColumns = Columns;
                if (Columns == null || Columns.Count == 0)
                {
                    targetColumns = AutoColumns;
                }

                if (targetColumns?.Count > 0)
                {
                    // Scale
                    int scaleItemHeight = (int)Math.Round(itemHeight * ScaleValue);
                    int scaleColumnHorizontalDistance = (int)Math.Round(columnHorizontalDistance * ScaleValue);

                    // Set itemBoundsBase, dicMapping
                    Rectangle[] itemColumnsDrawBounds = new Rectangle[targetColumns.Count];
                    Dictionary<int, int> dicColumnsMapper = new Dictionary<int, int>();
                    int drawLeft = 0;
                    for (int i = 0; i < targetColumns.Count; i++)
                    {
                        // dicMapping
                        for (int k = 0; k < Data.Columns.Count; k++)
                        {
                            if (targetColumns[i].FieldName.Trim() == Data.Columns[k].ColumnName.Trim())
                            {
                                dicColumnsMapper.Add(i, k);
                                break;
                            }

                            // Not found
                            if (k == Data.Columns.Count - 1)
                            {
                                dicColumnsMapper.Add(i, -1);
                            }
                        }

                        // Scale
                        int scaleColumnsWidth = (int)Math.Round(targetColumns[i].Width * ScaleValue);

                        // itemBoundsBase
                        itemColumnsDrawBounds[i].X = drawLeft;
                        itemColumnsDrawBounds[i].Width = scaleColumnsWidth;
                        itemColumnsDrawBounds[i].Y = 0;
                        itemColumnsDrawBounds[i].Height = scaleItemHeight;

                        drawLeft += scaleColumnsWidth + scaleColumnHorizontalDistance;
                    }

                    // Scale
                    int scaleItemVerticalDistance = (int)Math.Round(itemVerticalDistance * ScaleValue);
                    int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);
                    int scaleTop = (int)Math.Round(top * ScaleValue);
                    // Draw
                    int iStart = Math.Max(Math.Min(ScrollTop / (scaleItemHeight + scaleItemVerticalDistance), Items.Count), 0);
                    int iEnd = Math.Min(((Height - scaleColumnHeight) / (scaleItemHeight + scaleItemVerticalDistance)) + iStart + 2, Items.Count);
                    int drawHeightRef = scaleItemHeight + scaleItemVerticalDistance;
                    for (int i = iStart; i < iEnd; i++)
                    {
                        UserListViewItem item = Items[i];

                        for (int k = 0; k < itemColumnsDrawBounds.Length; k++)
                        {
                            int subItemIndex = dicColumnsMapper[k];
                            UserListViewSubItem subItem = item.SubItems[subItemIndex];

                            // Get scaleItemFont
                            Font scaleItemFont = subItem.Font != null
                                ? new Font(subItem.Font.FontFamily, subItem.Font.Size * ScaleValue, subItem.Font.Style)
                                : new Font(item.Font.FontFamily, item.Font.Size * ScaleValue, item.Font.Style);

                            Rectangle itemBounds = itemColumnsDrawBounds[k];

                            if (dicColumnsMapper[k] >= 0)
                            {
                                // Block(Default or Selected)
                                SolidBrush backBrush;
                                if (SelectedItemIndexList.Contains(i))
                                {
                                    backBrush = new SolidBrush(SelectedItemBackColor);
                                }
                                else
                                {
                                    backBrush = subItem.BackColor != Color.Empty
                                        ? new SolidBrush(subItem.BackColor)
                                        : new SolidBrush(item.BackColor);
                                }
                                Rectangle itemNodeRectangle = new Rectangle(itemBounds.Left, (scaleColumnHeight + i * drawHeightRef) - ScrollTop + scaleTop, itemBounds.Width, scaleItemHeight);
                                g.FillRectangle(backBrush, itemNodeRectangle);

                                // Scale
                                int scaleItemPaddingLeft = (int)Math.Round(ItemPadding.Left * ScaleValue);
                                int scaleItemPaddingRight = (int)Math.Round(ItemPadding.Right * ScaleValue);
                                
                                // Get value, nXStart, nYStart(Used in next block // Draw String)
                                string value = item.Row[dicColumnsMapper[k]].ToString();
                                SizeF linePixelSize = g.MeasureString(value, scaleItemFont);
                                int nXStart;

                                // Get itemAlign
                                UserListAlignType itemAlign = subItem.Align != UserListAlignType.None
                                    ? subItem.Align
                                    : (item.Align != UserListAlignType.None
                                        ? item.Align : targetColumns[k].ItemAlign);
                                switch (itemAlign)
                                {
                                    case UserListAlignType.Left: nXStart = itemBounds.Left + scaleItemPaddingLeft; break;
                                    case UserListAlignType.Right: nXStart = (int)(itemBounds.Left + itemBounds.Width - linePixelSize.Width) - scaleItemPaddingRight; break;
                                    case UserListAlignType.Center: nXStart = (int)Math.Round(itemBounds.Left + itemBounds.Width / 2 - linePixelSize.Width / 2); break;
                                    default: nXStart = itemBounds.Left + scaleItemPaddingLeft; break;
                                }

                                int nYStart = (int)Math.Round((scaleColumnHeight + i * drawHeightRef + scaleItemHeight / 2 - linePixelSize.Height / 2) - ScrollTop);

                                // Draw String
                                float minLeft = 3 * ScaleValue;
                                float topRevision = 2 * ScaleValue;
                                RectangleF stringRectangle = new RectangleF(
                                      Math.Max(minLeft, nXStart), nYStart + scaleTop + topRevision
                                    , Math.Min(itemNodeRectangle.Width, linePixelSize.Width + 0.5f), linePixelSize.Height);
                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                                SolidBrush stringBrush = i == SelectedItemIndex
                                    ? new SolidBrush(subItem.ForeColor != Color.Empty ? subItem.ForeColor : item.ForeColor)
                                    : new SolidBrush(subItem.ForeColor != Color.Empty ? subItem.ForeColor : item.ForeColor);
                                g.DrawString(value, scaleItemFont, stringBrush, stringRectangle, stringFormat);
                            }
                        }

                        // Draw itemVerticalDistance
                        SolidBrush itemVerticalDistanceBrush = new SolidBrush(ItemVerticalDistanceColor);
                        Rectangle itemVerticalDistanceRectangle = new Rectangle(
                            0
                            , (scaleColumnHeight + i * drawHeightRef) - ScrollTop + scaleItemHeight + scaleTop
                            , itemColumnsDrawBounds[itemColumnsDrawBounds.Length - 1].Right
                            , scaleItemVerticalDistance
                        );
                        g.FillRectangle(itemVerticalDistanceBrush, itemVerticalDistanceRectangle);
                    }
                }
            }
        }

        private void DrawContentSelector(Graphics g, Rectangle bounds, float ScaleValue, int left, int top, int itemHeight, int columnHorizontalDistance, int itemVerticalDistance)
        {
            if (SelectedItemIndex >= 0)
            {
                // Scale
                int scaleTop = (int)Math.Round(top * ScaleValue);
                int scaleItemHeight = (int)Math.Round(itemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(itemVerticalDistance * ScaleValue);

                int drawHeightRef = scaleItemHeight + scaleItemVerticalDistance;
                int sumWidth = (from c in Columns
                             select c).Sum(t => (int)Math.Round(t.Width * ScaleValue));

                Pen selectedItemBorderPen = new Pen(SelectedItemBorderColor);
                Rectangle selectedItemRectangle = new Rectangle(
                    0
                    , ((int)Math.Round(ColumnHeight * ScaleValue) + SelectedItemIndex * drawHeightRef) - ScrollTop - 1 + scaleTop
                    , Width - ScrollBarVertical.Width - 1 // sumWidth
                    , scaleItemHeight + 1
                );
                g.DrawRectangle(selectedItemBorderPen, selectedItemRectangle);
            }
        }

        private void DrawContentDrag(Graphics g, Rectangle bounds, float ScaleValue, int left, int top)
        {

        }

        private void DrawSetGrafx()
        {
            Context.MaximumBuffer = new Size(Width, Height);
            Grafx = Context.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));
        }

        private void DrawRenderGrafx()
        {
            Grafx.Render(Graphics.FromHwnd(Handle));
        }

        new public void SuspendLayout()
        {
            BlockDrawing = true;
            _SuspendLayout = true;
        }

        new public void ResumeLayout()
        {
            BlockDrawing = false;
            _SuspendLayout = false;
        }
    }
}
