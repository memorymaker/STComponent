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

namespace ST.DataModeler
{
    public partial class GraphicListView
    {
        #region User Attribute
        // Insert Line
        public int InsertLineIndex
        {
            get
            {
                return _InsertLineIndex;
            }
            set
            {
                if (_InsertLineIndex != value)
                {
                    _InsertLineIndex = value;
                    Refresh();
                }
            }
        }
        private int _InsertLineIndex = -1;
        // Insert Line Options
        private int InsertLineWidth = 1;
        private Color InsertLineColor = Color.Red;
        #endregion

        #region Options
        public Color SelectedItemBorderColor = Color.FromArgb(170, 218, 254);
        public Color SelectedItemBackColor = Color.FromArgb(229, 243, 254);
        public Color SelectedItemUnfucusedBackColor = Color.FromArgb(239, 248, 254);

        public int ColumnHeight
        {
            get
            {
                return _ColumnHeight;
            }
            set
            {
                _ColumnHeight = value;
                Refresh();
            }
        }
        private int _ColumnHeight = 26;

        public int ColumnHorizontalDistance
        {
            get
            {
                return _ColumnHorizontalDistance;
            }
            set
            {
                _ColumnHorizontalDistance = value;
                Refresh();
            }
        }
        private int _ColumnHorizontalDistance = 0;

        public int ItemHeight
        {
            get
            {
                return _ItemHeight;
            }
            set
            {
                _ItemHeight = value;
                Refresh();
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
                Refresh();
            }
        }
        private Padding _ItemPadding = new Padding(4);

        /// <summary>
        /// 리스트 항목들의 간격입니다. 해당 값은 ScaleValue에 따라 변하지 않습니다.
        /// </summary>
        public int ItemVerticalDistance
        {
            get
            {
                return _ItemVerticalDistance;
            }
            set
            {
                _ItemVerticalDistance = value;
                Refresh();
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
                Refresh();
            }
        }
        private Color _ItemVerticalDistanceColor = Color.FromArgb(245, 245, 245);
        #endregion

        #region System Options
        public readonly int ItemRelativeLeft = 0;
        public readonly int ItemRelativeTop = 1;
        #endregion

        #region Reference
        //public BufferedGraphics Grafx;
        //public BufferedGraphicsContext Context = BufferedGraphicsManager.Current;
        public bool Binded = false;
        public Size FontPixelSize;
        #endregion

        private void LoadDraw()
        {
            Paint += UserListView_Paint;
        }

        private void UserListView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            DrawContentItems(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop, ItemHeight, ColumnHorizontalDistance, ItemVerticalDistance);
            DrawContentSelector(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop, ItemHeight, ColumnHorizontalDistance, ItemVerticalDistance);
            DrawContentInsertLine(g, Bounds, ScaleValue, ItemRelativeLeft, ItemRelativeTop, ItemHeight, ColumnHorizontalDistance, ItemVerticalDistance);
            DrawContentHeader(g, Bounds, ScaleValue, ColumnHeight, ColumnHorizontalDistance);
        }

        private void DrawContentHeader(Graphics g, Rectangle bounds, float ScaleValue, int columnHeight, int columnHorizontalDistance)
        {
            // Set targetColumns
            List<GraphicListViewColumn> targetColumns = Columns;
            if (Columns == null || Columns.Count == 0)
            {
                targetColumns = AutoColumns;
            }

            // 차후 수정 필요
            if (ColumnHeight > 0)
            {
                int drawLeft = 0;
                foreach (GraphicListViewColumn column in targetColumns)
                {
                    // Scale
                    int scaleColumnHeight = (int)Math.Round(columnHeight * ScaleValue);
                    int scaleColumnWidth = (int)Math.Round(column.Width * ScaleValue);
                    int scaleColumnHorizontalDistance = (int)Math.Round(columnHorizontalDistance * ScaleValue);

                    // Back
                    SolidBrush backBrush = new SolidBrush(column.BackColor);
                    Rectangle backRectangle = new Rectangle(drawLeft, 0, scaleColumnWidth, scaleColumnHeight);
                    g.FillRectangle(backBrush, backRectangle);

                    // Set nXStart, nYStart(Used in Draw String)
                    // Scale
                    Font scaleColumnFont = new Font(column.Font.FontFamily, column.Font.Size * ScaleValue);
                    SizeF scaleLinePixelSize = g.MeasureString(column.Text, scaleColumnFont, scaleColumnWidth);
                    // Scale
                    float scaleLinePixelSizeWidth = scaleLinePixelSize.Width;
                    float scaleLinePixelSizeHeight = scaleLinePixelSize.Height;
                    int nXStart = (int)Math.Round(drawLeft + scaleColumnWidth / 2 - scaleLinePixelSizeWidth / 2);
                    int nYStart = (int)Math.Round(0 + scaleColumnHeight / 2 - scaleLinePixelSizeHeight / 2);

                    // Draw String
                    SolidBrush stringBrush = new SolidBrush(column.ForeColor);
                    g.DrawString(column.Text, scaleColumnFont, stringBrush, nXStart, nYStart);

                    drawLeft += scaleColumnWidth + scaleColumnHorizontalDistance;
                }
            }
        }

        private void DrawContentItems(Graphics g, Rectangle bounds, float ScaleValue, int left, int top, int itemHeight, int columnHorizontalDistance, int itemVerticalDistance)
        {
            if (Data != null && Data.Rows.Count > 0)
            {
                // Set targetColumns
                List<GraphicListViewColumn> targetColumns = Columns;
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
                        GraphicListViewItem item = Items[i];

                        for (int k = 0; k < itemColumnsDrawBounds.Length; k++)
                        {
                            Font scaleItemFont = new Font(item.Font.FontFamily, item.Font.Size * ScaleValue);

                            Rectangle itemBounds = itemColumnsDrawBounds[k];

                            if (dicColumnsMapper[k] >= 0)
                            {
                                //SolidBrush backBrush = SelectedIndexes.Contains(i)
                                //    ? new SolidBrush(SelectedItemBackColor)
                                //    : new SolidBrush(item.BackColor);
                                SolidBrush backBrush;

                                // Block(Default or Selected)
                                if (Focused)
                                {
                                    backBrush = SelectedIndexes.Contains(i)
                                        ? new SolidBrush(SelectedItemBackColor)
                                        : new SolidBrush(item.BackColor);
                                }
                                else
                                {
                                    backBrush = SelectedIndexes.Contains(i)
                                        ? new SolidBrush(SelectedItemUnfucusedBackColor)
                                        : new SolidBrush(item.BackColor);
                                }
                                Rectangle itemNodeRectangle = new Rectangle(itemBounds.Left, (scaleColumnHeight + i * drawHeightRef) - ScrollTop + scaleTop, itemBounds.Width, scaleItemHeight);
                                g.FillRectangle(backBrush, itemNodeRectangle);

                                // Scale
                                int scaleItemPaddingLeft = (int)Math.Round(ItemPadding.Left * ScaleValue);
                                int scaleItemPaddingRight = (int)Math.Round(ItemPadding.Right * ScaleValue);
                                // Set value
                                string value = item.Row[dicColumnsMapper[k]].ToString();
                                SizeF linePixelSize = g.MeasureString(value, scaleItemFont);
                                if (itemBounds.Width < linePixelSize.Width)
                                {
                                    value.Substring(0, value.Length - 1);
                                    do
                                    {
                                        value = value.Substring(0, value.Length - 1);
                                        linePixelSize = g.MeasureString(value + ".....", scaleItemFont);
                                    } while (itemBounds.Width < linePixelSize.Width && 1 < value.Length);
                                    value = value + "...";
                                    if (!item.TextOverFlowList.Contains(k))
                                    {
                                        item.TextOverFlowList.Add(k);
                                    }
                                }
                                else
                                {
                                    if (item.TextOverFlowList.Contains(k))
                                    {
                                        item.TextOverFlowList.Remove(k);
                                    }
                                }
                                // Set nXStart, nYStart(Used in next block // Draw String)
                                int nXStart;
                                var itemAlign = item.Align == GraphicListAlignType.None
                                    ? targetColumns[k].ItemAlign
                                    : item.Align;
                                switch (itemAlign)
                                {
                                    case GraphicListAlignType.Left: nXStart = itemBounds.Left + scaleItemPaddingLeft; break;
                                    case GraphicListAlignType.Right: nXStart = (int)(itemBounds.Left + itemBounds.Width - linePixelSize.Width) - scaleItemPaddingRight; break;
                                    case GraphicListAlignType.Center: nXStart = (int)Math.Round(itemBounds.Left + itemBounds.Width / 2 - linePixelSize.Width / 2); break;
                                    default: nXStart = itemBounds.Left + scaleItemPaddingLeft; break;
                                }
                                int nYStart = (int)Math.Round((scaleColumnHeight + i * drawHeightRef + scaleItemHeight / 2 - linePixelSize.Height / 2) - ScrollTop);

                                // Draw String
                                SolidBrush stringBrush = i == FocusedIndexOnly
                                    ? new SolidBrush(item.ForeColor)
                                    : new SolidBrush(item.ForeColor);
                                g.DrawString(value, scaleItemFont, stringBrush, nXStart, nYStart + scaleTop);
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
            if (FocusedIndexOnly >= 0)
            {
                // Scale
                int scaleTop = (int)Math.Round(top * ScaleValue);
                int scaleItemHeight = (int)Math.Round(itemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(itemVerticalDistance * ScaleValue);

                int drawHeightRef = scaleItemHeight + scaleItemVerticalDistance;
                int sumWidth = (from c in Columns
                             select c).Sum(t => (int)Math.Round(t.Width * ScaleValue));

                Pen selectedItemBorderPen = new Pen(SelectedItemBorderColor);
                    selectedItemBorderPen.Width = scaleItemVerticalDistance;
                Rectangle selectedItemRectangle = new Rectangle(
                    (int)Math.Floor(scaleItemVerticalDistance / 2.0f)
                    , ((int)Math.Round(ColumnHeight * ScaleValue) + FocusedIndexOnly * drawHeightRef) - ScrollTop - 1 + scaleTop
                    , sumWidth - scaleItemVerticalDistance
                    , scaleItemHeight + scaleItemVerticalDistance
                );
                g.DrawRectangle(selectedItemBorderPen, selectedItemRectangle);
            }
        }

        private void DrawContentInsertLine(Graphics g, Rectangle bounds, float ScaleValue, int left, int top, int itemHeight, int columnHorizontalDistance, int itemVerticalDistance)
        {
            if (InsertLineIndex >= 0)
            {
                // Scale
                int scaleTop = (int)Math.Round(top * ScaleValue);
                int scaleItemHeight = (int)Math.Round(itemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(itemVerticalDistance * ScaleValue);

                int drawHeightRef = scaleItemHeight + scaleItemVerticalDistance;
                int sumWidth = (from c in Columns
                                select c).Sum(t => (int)Math.Round(t.Width * ScaleValue));

                Pen insertLineBorderPen = new Pen(InsertLineColor, InsertLineWidth);
                    insertLineBorderPen.Width = scaleItemVerticalDistance;
                Rectangle insertLineRectangle = new Rectangle(
                    0
                    , ((int)Math.Round(ColumnHeight * ScaleValue) + InsertLineIndex * drawHeightRef) - ScrollTop - 1 + scaleTop
                    , sumWidth
                    , 1
                );
                g.DrawLine(insertLineBorderPen, insertLineRectangle.X, insertLineRectangle.Y, insertLineRectangle.Right, insertLineRectangle.Y);
            }
        }
    }
}
