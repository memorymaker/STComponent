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
    public partial class RelationControl
    {
        #region Value & Propertise - DrawInfo
        public RelationHorizontalDirectionType HorizontalDirectionType = RelationHorizontalDirectionType.None;
        public RelationVerticalDirectionType VerticalDirectionType = RelationVerticalDirectionType.None;
        public Point StartPoint = Point.Empty;
        public Point EndPoint = Point.Empty;
        public int StartStraightLength = 10;

        public RelationControlStatus Status {
            get
            {
                return _Status;
            }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    Refresh();
                }
            }
        }
        private RelationControlStatus _Status = RelationControlStatus.None;

        public Color Color => Colors[Status];
        private Dictionary<RelationControlStatus, Color> Colors = new Dictionary<RelationControlStatus, Color>()
        {
              { RelationControlStatus.None, Color.Black }
            , { RelationControlStatus.MouseOver, Color.Red }
            , { RelationControlStatus.Selected, Color.Red }
        };

        public int Width => Widths[Status];
        private Dictionary<RelationControlStatus, int> Widths = new Dictionary<RelationControlStatus, int>()
        {
              { RelationControlStatus.None, 1 }
            , { RelationControlStatus.MouseOver, 1 }
            , { RelationControlStatus.Selected, 2 }
        };

        public List<Rectangle> Area = null;
        public Point[] Points = null;

        // Default
        private DashStyle DashStyle = DashStyle.Dash;
        private float[] DashPattern = new float[4] { 9, 1, 9, 1 };
        
        // System Option
        private int DefaultStartStraightLength = 10;
        private int StartStraightLengthDistant = 12;
        private int TopStartStraightLengthAppend = 6;
        private int ArrowSize = 5;

        private Font Font = new Font("맑은 고딕", 9F);
        private PointF DrawTextPositionRevision = new PointF(4F, 2F);
        #endregion

        #region Load
        private void LoadRelationControlDraw()
        {
            Paint += RelationControl_Paint;
        }

        private void RelationControl_Paint(object sender, PaintEventArgs e)
        {
            if (HorizontalDirectionType != RelationHorizontalDirectionType.None)
            {
                SetDrawInfo(Target.ScaleValue);
                Pen pen = new Pen(Color, Width);
                pen.DashStyle = DashStyle;
                pen.DashPattern = new float[] {
                      Math.Max(DashPattern[0] * Target.ScaleValue, 1) / Width, Math.Max(DashPattern[1] * Target.ScaleValue, 1) / Width
                    , Math.Max(DashPattern[2] * Target.ScaleValue, 1) / Width, Math.Max(DashPattern[3] * Target.ScaleValue, 1) / Width 
                };
                e.Graphics.DrawLines(pen, Points);

                if (!string.IsNullOrEmpty(Model.RELATION_VALUE))
                {
                    PointF drawTextPositionRevision = new PointF(
                          DrawTextPositionRevision.X * Target.ScaleValue
                        , DrawTextPositionRevision.Y * Target.ScaleValue
                    );

                    string drawText = $"{Model.RELATION_VALUE}({Model.RELATION_OPERATOR})";
                    Font scaleFont = new Font(Font.FontFamily, Font.Size * Target.ScaleValue);
                    SizeF textSize = e.Graphics.MeasureString(drawText, scaleFont);

                    if (Points[0].X <= Points[Points.Length - 1].X)
                    {
                        e.Graphics.DrawString(drawText, scaleFont, new SolidBrush(Color)
                            , Points[Points.Length - 1].X - textSize.Width - drawTextPositionRevision.X
                            , Points[Points.Length - 1].Y - textSize.Height - drawTextPositionRevision.Y
                        );
                    }
                    else
                    {
                        e.Graphics.DrawString(drawText, scaleFont, new SolidBrush(Color)
                            , Points[Points.Length - 1].X + drawTextPositionRevision.X
                            , Points[Points.Length - 1].Y - textSize.Height - drawTextPositionRevision.Y
                        );
                    }
                }
            }
        }
        #endregion

        #region Function
        public void SetDrawInfo(float scaleValue)
        {
            SetDrawStartPointNEndPointNDirectionType();
            SetDrawStartStraightLength(scaleValue);
            SetDrawPointsNArea(scaleValue);
        }

        public void SetDrawStartPointNEndPointNDirectionType()
        {
            Rectangle fromRectangle = GetFromRectangle();
            Rectangle toRectangle = GetToRectangle();

            if (fromRectangle.Width > 0 && toRectangle.Width > 0)
            {
                // Set points1(Origin)
                int relationCountInColumn1;
                int relationIndexInColumn1 = GetRelationIndexInOriginColumn(out relationCountInColumn1);
                float positionUnit1 = (float)fromRectangle.Height / (relationCountInColumn1 + 1);
                int yPosition1 = ((relationIndexInColumn1 + 1) * positionUnit1).ToInt();
                Point[] points1 = {
                      new Point(fromRectangle.Left , fromRectangle.Top + yPosition1)
                    , new Point(fromRectangle.Right, fromRectangle.Top + yPosition1)
                };

                // Set points2(Destination)
                int relationCountInColumn2;
                int relationIndexInColumn2 = GetRelationIndexInDestinationColumn(out relationCountInColumn2);
                float positionUnit2 = (float)toRectangle.Height / (relationCountInColumn2 + 1);
                int yPosition2 = ((relationIndexInColumn2 + 1) * positionUnit2).ToInt();
                Point[] points2 = {
                      new Point(toRectangle.Left , toRectangle.Top + yPosition2)
                    , new Point(toRectangle.Right, toRectangle.Top + yPosition2)
                };

                // Left To Left
                if (points1[0].X <= points2[0].X && points2[0].X <= points1[1].X + (StartStraightLength * 1.5))
                {
                    HorizontalDirectionType = RelationHorizontalDirectionType.LeftToLeft;
                    StartPoint = points1[0];
                    EndPoint = points2[0];
                }
                // Right To Right
                else if (points1[0].X - (StartStraightLength * 1.5) <= points2[1].X && points2[1].X <= points1[1].X)
                {
                    HorizontalDirectionType = RelationHorizontalDirectionType.RightToRight;
                    StartPoint = points1[1];
                    EndPoint = points2[1];
                }
                // Right To Left
                else if (points1[1].X <= points2[0].X - (StartStraightLength * 1.5))
                {
                    HorizontalDirectionType = RelationHorizontalDirectionType.RightToLeft;
                    StartPoint = points1[1];
                    EndPoint = points2[0];
                }
                // Left To Right
                else if (points1[0].X >= points2[1].X + (StartStraightLength * 1.5))
                {
                    HorizontalDirectionType = RelationHorizontalDirectionType.LeftToRight;
                    StartPoint = points1[0];
                    EndPoint = points2[1];
                }

                // Top To Down
                if (StartPoint.Y < EndPoint.Y)
                {
                    VerticalDirectionType = RelationVerticalDirectionType.TopToDown;
                }
                // Down To Top
                else if (StartPoint.Y > EndPoint.Y)
                {
                    VerticalDirectionType = RelationVerticalDirectionType.DownToTop;
                }
                // Equals
                else
                {
                    VerticalDirectionType = RelationVerticalDirectionType.Equals;
                }
            }
        }

        private int GetRelationIndexInOriginColumn(out int relationCountInOriginColumn)
        {
            int rsSort = 0;
            int rsCount = 1;
            int index = Target.Relations.IndexOf(this);
            for(int i = index + 1; i < Target.Relations.Count; i++)
            {
                if (Model.NODE_ID1 == Target.Relations[i].Model.NODE_ID1
                && Model.NODE_SEQ1 == Target.Relations[i].Model.NODE_SEQ1
				&& Model.NODE_DETAIL_ID1 == Target.Relations[i].Model.NODE_DETAIL_ID1
				&& Model.NODE_DETAIL_SEQ1 == Target.Relations[i].Model.NODE_DETAIL_SEQ1)
                {
                    rsCount++;
                }
                else
                {
                    break;
                }
            }

            for (int i = index - 1; i >= 0; i--)
            {
                if (Model.NODE_ID1 == Target.Relations[i].Model.NODE_ID1
                && Model.NODE_SEQ1 == Target.Relations[i].Model.NODE_SEQ1
				&& Model.NODE_DETAIL_ID1 == Target.Relations[i].Model.NODE_DETAIL_ID1
				&& Model.NODE_DETAIL_SEQ1 == Target.Relations[i].Model.NODE_DETAIL_SEQ1)
                {
                    rsCount++;
                    rsSort++;
                }
                else
                {
                    break;
                }
            }

            relationCountInOriginColumn = rsCount;
            return rsSort;
        }

        private int GetRelationIndexInDestinationColumn(out int relationCountInDestinationColumn)
        {
            int rsSort = 0;
            int rsCount = 1;
            string sortingString = Model.GetSortingString();

            for(int i = 0; i < Target.Relations.Count; i++)
            {
                if (!Model.Equals(Target.Relations[i].Model)
                && Model.NODE_ID2 == Target.Relations[i].Model.NODE_ID2
                && Model.NODE_SEQ2 == Target.Relations[i].Model.NODE_SEQ2
				&& Model.NODE_DETAIL_ID2 == Target.Relations[i].Model.NODE_DETAIL_ID2
				&& Model.NODE_DETAIL_SEQ2 == Target.Relations[i].Model.NODE_DETAIL_SEQ2)
                {
                    if (sortingString.CompareTo(Target.Relations[i].Model.GetSortingString()) > 0)
                    {
                        rsSort++;
                    }
                    rsCount++;
                }
            }

            relationCountInDestinationColumn = rsCount;
            return rsSort;
        }

        private Rectangle GetFromRectangle()
        {
            Rectangle rs = Rectangle.Empty;

            TableNode tableNode = GetTableNode(Model.NODE_ID1, Model.NODE_SEQ1);
            if (tableNode != null)
            {
                GraphicListViewItem item = tableNode.GetItemFromRowValue(
                      new string[] { DataModeler.NODE.NODE_DETAIL_ID, DataModeler.NODE.NODE_DETAIL_SEQ }
                    , new object[] { Model.NODE_DETAIL_ID1, Model.NODE_DETAIL_SEQ1 } 
                );
                if (item != null)
                {
                    Rectangle rectangle = tableNode.GetItemRectangle(item);
                    rectangle.X -= tableNode.Padding.Left;
                    rectangle.Width += tableNode.Padding.Horizontal;
                    rs = rectangle;
                }
            }

            return rs;
        }

        private Rectangle GetToRectangle()
        {
            Rectangle rs = Rectangle.Empty;

            TableNode tableNode = GetTableNode(Model.NODE_ID2, Model.NODE_SEQ2);
            if (tableNode != null)
            {
                // Column To Column
                if (Model.NODE_DETAIL_ID2 != string.Empty)
                {
                    GraphicListViewItem item = tableNode.GetItemFromRowValue(
                        new string[] { DataModeler.NODE.NODE_DETAIL_ID , DataModeler.NODE.NODE_DETAIL_SEQ }
					    , new object[] { Model.NODE_DETAIL_ID2, Model.NODE_DETAIL_SEQ2 }
				    );

                    if (item != null)
                    {
                        Rectangle rectangle = tableNode.GetItemRectangle(item);
                        rectangle.X -= tableNode.Padding.Left;
                        rectangle.Width += tableNode.Padding.Horizontal;
                        rs = rectangle;
                    }
                }
                // Column To Table
                else
                {
                    Rectangle rectangle = new Rectangle(
                        tableNode.Left, tableNode.Top, tableNode.Width, tableNode.TitleHeight + tableNode.Padding.Top
                    );
                    rs = rectangle;
                }
            }
            
            return rs;
        }

        private TableNode GetTableNode(string id, int seq)
        {
            TableNode rs = null;
            foreach (GraphicControl control in Target.Controls)
            {
                TableNode node = control as TableNode;
                if (node != null)
                {
                    if (node.ID == id && node.SEQ == seq)
                    {
                        rs = node;
                        break;
                    }
                }
            }
            return rs;
        }

        private void SetDrawStartStraightLength(float scaleValue)
        {
            int idx = Target.Relations.GetChildIndex(this);

            // DownToTop
            if (VerticalDirectionType == RelationVerticalDirectionType.DownToTop)
            {
                int count = 0;
                for(int i = idx; 1 <= i; i--)
                {
                    if (Target.Relations[i].Model.NODE_ID1 != Target.Relations[i - 1].Model.NODE_ID1)
                    {
                        break;
                    }
                    count++;
                }

                StartStraightLength = ((DefaultStartStraightLength + (StartStraightLengthDistant * count)) * scaleValue).ToInt();
            }
            // TopToDown
            else if (VerticalDirectionType == RelationVerticalDirectionType.TopToDown)
            {
                int count = 0;
                for (int i = idx; i < Target.Relations.Count - 1; i++)
                {
                    if (Target.Relations[i].Model.NODE_ID1 != Target.Relations[i + 1].Model.NODE_ID1)
                    {
                        break;
                    }
                    count++;
                }

                StartStraightLength = ((DefaultStartStraightLength + (StartStraightLengthDistant * count) + TopStartStraightLengthAppend) * scaleValue).ToInt();
            }

        }

        public void SetDrawPointsNArea(float scaleValue)
        {
            if (HorizontalDirectionType != RelationHorizontalDirectionType.None)
            {
                Point[] points = GetDrawPoints(
                      HorizontalDirectionType
                    , StartPoint
                    , EndPoint
                    , StartStraightLength
                    , scaleValue
                );

                Points = points;
                Area = GetDrawArea(points);
            }
        }

        private Point[] GetDrawPoints(RelationHorizontalDirectionType relationDrawType, Point sp, Point ep, int distant = 10, float scaleValue = 1f)
        {
            Point[] poi = null;
            int arrowSize = (ArrowSize * scaleValue).ToInt(); 

            switch (relationDrawType)
            {
                case RelationHorizontalDirectionType.LeftToLeft:
                    poi = new Point[] {
                        new Point(sp.X, sp.Y)
                        ,new Point(sp.X - distant, sp.Y)
                        ,new Point(sp.X - distant, ep.Y)
                        ,new Point(ep.X, ep.Y)

                        ,new Point(ep.X - arrowSize, ep.Y - arrowSize)
                        ,new Point(ep.X - arrowSize, ep.Y + arrowSize)
                        ,new Point(ep.X, ep.Y)
                    };
                    break;
                case RelationHorizontalDirectionType.RightToRight:
                    poi = new Point[] {
                        new Point(sp.X, sp.Y)
                        ,new Point(sp.X + distant, sp.Y)
                        ,new Point(sp.X + distant, ep.Y)
                        ,new Point(ep.X, ep.Y)

                        ,new Point(ep.X + arrowSize, ep.Y - arrowSize)
                        ,new Point(ep.X + arrowSize, ep.Y + arrowSize)
                        ,new Point(ep.X, ep.Y)
                    };
                    break;
                case RelationHorizontalDirectionType.RightToLeft:
                    poi = new Point[] {
                        new Point(sp.X, sp.Y)
                        ,new Point(sp.X + distant, sp.Y)
                        ,new Point(sp.X + distant, ep.Y)
                        ,new Point(ep.X, ep.Y)

                        ,new Point(ep.X - arrowSize, ep.Y - arrowSize)
                        ,new Point(ep.X - arrowSize, ep.Y + arrowSize)
                        ,new Point(ep.X, ep.Y)
                    };
                    break;
                case RelationHorizontalDirectionType.LeftToRight:
                    poi = new Point[] {
                        new Point(sp.X, sp.Y)
                        ,new Point(sp.X - distant, sp.Y)
                        ,new Point(sp.X - distant, ep.Y)
                        ,new Point(ep.X, ep.Y)

                        ,new Point(ep.X + arrowSize, ep.Y - arrowSize)
                        ,new Point(ep.X + arrowSize, ep.Y + arrowSize)
                        ,new Point(ep.X, ep.Y)
                    };
                    break;
            }

            return poi;
        }

        private List<Rectangle> GetDrawArea(Point[] points)
        {
            List<Rectangle> rs = new List<Rectangle>();
            int rectangleThickness = 8;

            for (int i = 1; i < points.Length; i++)
            {
                // Horizontal
                if (points[i - 1].Y == points[i].Y)
                {
                    Rectangle node = new Rectangle(
                        Math.Min(points[i - 1].X, points[i].X)
                        , Math.Min(points[i - 1].Y, points[i].Y) - rectangleThickness / 2
                        , Math.Abs(points[i - 1].X - points[i].X)
                        , Math.Abs(points[i - 1].Y - points[i].Y) + rectangleThickness
                    );
                    rs.Add(node);
                }
                // Vertical
                else if (points[i - 1].X == points[i].X)
                {
                    Rectangle node = new Rectangle(
                        Math.Min(points[i - 1].X, points[i].X) - rectangleThickness / 2
                        , Math.Min(points[i - 1].Y, points[i].Y)
                        , Math.Abs(points[i - 1].X - points[i].X) + rectangleThickness
                        , Math.Abs(points[i - 1].Y - points[i].Y)
                    );
                    rs.Add(node);
                }
                else
                {
                    // Passing arrow 
                }
            }

            return rs;
        }

        public void Refresh()
        {
            if (Visible)
            {
                Target.RequestRefresh(this, RefreshType.This);
            }
        }

        public void RefreshAll()
        {
            if (Visible)
            {
                Target.RequestRefresh(this, RefreshType.All);
            }
        }
        #endregion
    }
}
