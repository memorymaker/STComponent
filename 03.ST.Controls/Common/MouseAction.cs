using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Automation;
using System.Collections;

namespace ST.Controls
{
    public class MouseAction
    {
        // Etc
        public bool Enable { get; set; } = true;

        // User Option
        public bool UseLocationCorrection = true;
        public bool UseSizing = true;

        // System Option
        public const int AutoMovePixelInSide = 10;
        public const int SizeArea = 5;
        public const int TopSizeArea = 5;
        public const int MinWidth = 80;
        public const int MinHeight = 80;
        public MouseActionType ActionType = MouseActionType.None;
        private System.Windows.Input.Key NoneAutoMoveKey = System.Windows.Input.Key.LeftCtrl;
        private System.Windows.Input.Key GroupMoveKey = System.Windows.Input.Key.LeftShift;

        // Referece Mouse
        private Rectangle MouseDownRectangle = new Rectangle();
        private int MouseDownX = 0;
        private int MouseDownY = 0;
        private bool MouseDown = false;

        public bool IsMouseDown => MouseDown;

        // Referece Panel
        private List<IMouseActionTarget> PanelGroup = new List<IMouseActionTarget>();
        private Dictionary<MouseActionDirectionType, List<IMouseActionTarget>> PanelGroupSomeDirection = new Dictionary<MouseActionDirectionType, List<IMouseActionTarget>>();
        private Dictionary<MouseActionDirectionType, List<IMouseActionTarget>> PanelGroupAttachedTargetSide = new Dictionary<MouseActionDirectionType, List<IMouseActionTarget>>();
        private Dictionary<MouseActionDirectionType, bool> HasSomeDirectionPanelsAttachedParentSide = new Dictionary<MouseActionDirectionType, bool>();

        // Default Refernce
        private IMouseActionTarget Target = null;

        public MouseAction(IMouseActionTarget target)
        {
            Target = target;
            SetEvents();
        }

        private void SetEvents()
        {
            Target.MouseDown += Target_MouseDown;
            Target.MouseMove += Target_MouseMove;
            Target.MouseUp += Target_MouseUp;
            Target.MouseLeave += Target_MouseLeave;
        }

        private void Target_MouseDown(object sender, MouseEventArgs e)
        {
            if (Enable)
            {
                SetMouseTypeNMouseCursor(e.X, e.Y);

                SetPanelGroup();
                SetPanelGroupSomeDirectionFromPanelGroup();
                SetPanelGroupAttachedTargetSideFromPanelGroupSomeDirection();
                SetHasSomeDirectionPanelsAttachedParentSide();

                MouseDownRectangle = Target.Bounds;
                MouseDownX = e.X;
                MouseDownY = e.Y;
                MouseDown = true;
            }
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (Enable)
            {
                //Console.WriteLine(string.Format("{0} move {1} {2}", Target.Name, e.Button, MouseActionType));
                if (e.Button == MouseButtons.Left && MouseDown)
                {
                    switch (ActionType)
                    {
                        case MouseActionType.Move:
                            var targetNewLocation = GetLocation(e.X, e.Y);
                            if (IsGroupMove)
                            {
                                targetNewLocation = SetPanelGroupLocationNReturnTargetNewLocation(targetNewLocation);
                            }
                            else if (!targetNewLocation.Equals(Target.Location))
                            {
                                PanelGroup.Clear();
                            }
                            Target.Location = targetNewLocation;
                            break;
                        case MouseActionType.SizeTop:
                        case MouseActionType.SizeTopRight:
                        case MouseActionType.SizeRight:
                        case MouseActionType.SizeBottomRight:
                        case MouseActionType.SizeBottom:
                        case MouseActionType.SizeBottomLeft:
                        case MouseActionType.SizeLeft:
                        case MouseActionType.SizeTopLeft:
                            //this.SuspendLayout(); //this.ResumeLayout();

                            var targetNewBounds = GetBounds(ActionType, e.X, e.Y);
                            if (IsAutoMove)
                            {
                                targetNewBounds = SetPanelGroupLocationOrSizeNReturnTargetNewBounds(Target.Bounds, targetNewBounds);
                            }

                            //if (IsGroupMove)
                            //{
                            //    targetNewBounds = SetPanelGroupLocationNReturnTargetNewBounds(Target.Bounds, targetNewBounds);
                            //}
                            //else if (!targetNewBounds.Equals(Target.Bounds))
                            //{
                            //    PanelGroup.Clear();
                            //}
                            Target.Bounds = targetNewBounds;
                            break;
                    }
                }
                else if (e.Button == MouseButtons.None)
                {
                    SetMouseTypeNMouseCursor(e.X, e.Y);
                    MouseDown = false;
                }
            }
        }

        private void Target_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = true;
        }

        private void SetMouseTypeNMouseCursor(int eX, int eY)
        {
            if ((TopSizeArea < eY && eY < Target.TitleHeight) || !UseSizing)
            {
                Target.Cursor = Cursors.Default;
                ActionType = MouseActionType.Move;
            }
            else
            {
                SetMouseTypeNMouseCursorNotTitle(eX, eY);
            }
        }

        private void SetMouseTypeNMouseCursorNotTitle(int eX, int eY)
        {
            ActionType = GetMouseActionType(eX, eY);
            switch (ActionType)
            {
                case MouseActionType.SizeTopRight: Target.Cursor = Cursors.SizeNESW; break;
                case MouseActionType.SizeBottomLeft: Target.Cursor = Cursors.SizeNESW; break;
                case MouseActionType.SizeTopLeft: Target.Cursor = Cursors.SizeNWSE; break;
                case MouseActionType.SizeBottomRight: Target.Cursor = Cursors.SizeNWSE; break;
                case MouseActionType.SizeTop: Target.Cursor = Cursors.SizeNS; break;
                case MouseActionType.SizeBottom: Target.Cursor = Cursors.SizeNS; break;
                case MouseActionType.SizeRight: Target.Cursor = Cursors.SizeWE; break;
                case MouseActionType.SizeLeft: Target.Cursor = Cursors.SizeWE; break;
                case MouseActionType.Move: Target.Cursor = Cursors.Default; break;
                default: Target.Cursor = Cursors.Default; break;
            }
        }

        private void SetPanelGroup()
        {
            PanelGroup.Clear();
            GetPanelGroup(Target, ref PanelGroup);
        }

        private void SetPanelGroupSomeDirectionFromPanelGroup()
        {
            // Top
            List<IMouseActionTarget> topPanels = new List<IMouseActionTarget>();
            PutSomeDirectionPanelsFromPanelGroup(Target.Bounds, MouseActionDirectionType.Top, ref topPanels);

            // Right
            List<IMouseActionTarget> rightPanels = new List<IMouseActionTarget>();
            PutSomeDirectionPanelsFromPanelGroup(Target.Bounds, MouseActionDirectionType.Right, ref rightPanels);

            // Bottom
            List<IMouseActionTarget> bottomPanels = new List<IMouseActionTarget>();
            PutSomeDirectionPanelsFromPanelGroup(Target.Bounds, MouseActionDirectionType.Bottom, ref bottomPanels);

            // Left
            List<IMouseActionTarget> leftPanels = new List<IMouseActionTarget>();
            PutSomeDirectionPanelsFromPanelGroup(Target.Bounds, MouseActionDirectionType.Left, ref leftPanels);

            PanelGroupSomeDirection.Clear();
            PanelGroupSomeDirection.Add(MouseActionDirectionType.Top, topPanels);
            PanelGroupSomeDirection.Add(MouseActionDirectionType.Right, rightPanels);
            PanelGroupSomeDirection.Add(MouseActionDirectionType.Bottom, bottomPanels);
            PanelGroupSomeDirection.Add(MouseActionDirectionType.Left, leftPanels);
        }

        private void SetPanelGroupAttachedTargetSideFromPanelGroupSomeDirection()
        {
            PanelGroupAttachedTargetSide.Clear();
            PanelGroupAttachedTargetSide.Add(MouseActionDirectionType.Top, new List<IMouseActionTarget>());
            PanelGroupAttachedTargetSide.Add(MouseActionDirectionType.Right, new List<IMouseActionTarget>());
            PanelGroupAttachedTargetSide.Add(MouseActionDirectionType.Bottom, new List<IMouseActionTarget>());
            PanelGroupAttachedTargetSide.Add(MouseActionDirectionType.Left, new List<IMouseActionTarget>());

            foreach (MouseActionDirectionType userPanelDirectionType in Enum.GetValues(typeof(MouseActionDirectionType)))
            {
                switch (userPanelDirectionType)
                {
                    case MouseActionDirectionType.Top:
                        for (int i = 0; i < PanelGroupSomeDirection[userPanelDirectionType].Count; i++)
                        {
                            if (Target.Bounds.Top == PanelGroupSomeDirection[userPanelDirectionType][i].Bounds.Bottom)
                            {
                                PanelGroupAttachedTargetSide[userPanelDirectionType].Add(PanelGroupSomeDirection[userPanelDirectionType][i]);
                            }
                        }
                        break;
                    case MouseActionDirectionType.Right:
                        for (int i = 0; i < PanelGroupSomeDirection[userPanelDirectionType].Count; i++)
                        {
                            if (Target.Bounds.Right == PanelGroupSomeDirection[userPanelDirectionType][i].Bounds.Left)
                            {
                                PanelGroupAttachedTargetSide[userPanelDirectionType].Add(PanelGroupSomeDirection[userPanelDirectionType][i]);
                            }
                        }
                        break;
                    case MouseActionDirectionType.Bottom:
                        for (int i = 0; i < PanelGroupSomeDirection[userPanelDirectionType].Count; i++)
                        {
                            if (Target.Bounds.Bottom == PanelGroupSomeDirection[userPanelDirectionType][i].Bounds.Top)
                            {
                                PanelGroupAttachedTargetSide[userPanelDirectionType].Add(PanelGroupSomeDirection[userPanelDirectionType][i]);
                            }
                        }
                        break;
                    case MouseActionDirectionType.Left:
                        for (int i = 0; i < PanelGroupSomeDirection[userPanelDirectionType].Count; i++)
                        {
                            if (Target.Bounds.Left == PanelGroupSomeDirection[userPanelDirectionType][i].Bounds.Right)
                            {
                                PanelGroupAttachedTargetSide[userPanelDirectionType].Add(PanelGroupSomeDirection[userPanelDirectionType][i]);
                            }
                        }
                        break;
                }
            }
        }

        private void SetHasSomeDirectionPanelsAttachedParentSide()
        {
            HasSomeDirectionPanelsAttachedParentSide.Clear();
            foreach (MouseActionDirectionType userPanelDirectionType in Enum.GetValues(typeof(MouseActionDirectionType)))
            {
                HasSomeDirectionPanelsAttachedParentSide.Add(
                    userPanelDirectionType,
                    IsSomeDirectionPanelsAttachedParentSide(userPanelDirectionType, PanelGroupSomeDirection[userPanelDirectionType])
                );
            }
        }

        private Point SetPanelGroupLocationNReturnTargetNewLocation(Point targetNewLocation)
        {
            Point[] panelGroupNewLocations = new Point[PanelGroup.Count];
            Point revisionPoint = new Point(targetNewLocation.X - Target.Location.X, targetNewLocation.Y - Target.Location.Y);
            int[] minMaxPoint = new int[4]; // 0: Top Min / 1: Right Max / 2: Bottom Max / 3: Left Min
            int[] minMaxIndex = new int[4]; // 0: Top Min / 1: Right Max / 2: Bottom Max / 3: Left Min
            for (int i = 0; i < PanelGroup.Count; i++)
            {
                panelGroupNewLocations[i].X = PanelGroup[i].Left + revisionPoint.X;
                panelGroupNewLocations[i].Y = PanelGroup[i].Top + revisionPoint.Y;
                if (minMaxPoint[0] > panelGroupNewLocations[i].Y)
                {
                    minMaxPoint[0] = panelGroupNewLocations[i].Y;
                    minMaxIndex[0] = i;
                }

                if (minMaxPoint[1] < panelGroupNewLocations[i].X + PanelGroup[i].Width)
                {
                    minMaxPoint[1] = panelGroupNewLocations[i].X + PanelGroup[i].Width;
                    minMaxIndex[1] = i;
                }

                if (minMaxPoint[2] < panelGroupNewLocations[i].Y + PanelGroup[i].Height)
                {
                    minMaxPoint[2] = panelGroupNewLocations[i].Y + PanelGroup[i].Height;
                    minMaxIndex[2] = i;
                }

                if (minMaxPoint[3] > panelGroupNewLocations[i].X)
                {
                    minMaxPoint[3] = panelGroupNewLocations[i].X;
                    minMaxIndex[3] = i;
                }
            }

            // ------------ groupRevisionTop
            int? groupRevisionTop = null;
            if (minMaxPoint[0] < 0)
            {
                groupRevisionTop = -minMaxPoint[0];
            }
            else if (minMaxPoint[2] > Target.Parent.Height)
            {
                groupRevisionTop = Target.Parent.Height - minMaxPoint[2];
            }

            if (groupRevisionTop != null)
            {
                for (int i = 0; i < panelGroupNewLocations.Length; i++)
                {
                    panelGroupNewLocations[i].Y += (int)groupRevisionTop;
                }
                targetNewLocation.Y += (int)groupRevisionTop;
            }

            // ------------ groupRevisionLeft
            int? groupRevisionLeft = null;
            if (minMaxPoint[3] < 0)
            {
                groupRevisionLeft = -minMaxPoint[3];
            }
            else if (minMaxPoint[1] > Target.Parent.Width)
            {
                groupRevisionLeft = Target.Parent.Width - minMaxPoint[1];
            }

            if (groupRevisionLeft != null)
            {
                for (int i = 0; i < panelGroupNewLocations.Length; i++)
                {
                    panelGroupNewLocations[i].X += (int)groupRevisionLeft;
                }
                targetNewLocation.X += (int)groupRevisionLeft;
            }

            // Set Panel Group Location
            for (int i = 0; i < PanelGroup.Count; i++)
            {
                PanelGroup[i].Location = panelGroupNewLocations[i];
            }

            return targetNewLocation;
        }

        private void PutSomeDirectionPanelsFromPanelGroup(Rectangle baseBounds, MouseActionDirectionType direction, ref List<IMouseActionTarget> directonPanels)
        {
            for (int i = 0; i < PanelGroup.Count; i++)
            {
                if
                (
                    (direction == MouseActionDirectionType.Top && baseBounds.Top == PanelGroup[i].Bottom)
                    ||
                    (direction == MouseActionDirectionType.Right && baseBounds.Right == PanelGroup[i].Left)
                    ||
                    (direction == MouseActionDirectionType.Bottom && baseBounds.Bottom == PanelGroup[i].Top)
                    ||
                    (direction == MouseActionDirectionType.Left && baseBounds.Left == PanelGroup[i].Right)
                )
                {
                    if (!directonPanels.Contains(PanelGroup[i]))
                    {
                        directonPanels.Add(PanelGroup[i]);
                        PutSomeDirectionPanelsFromPanelGroup(PanelGroup[i].Bounds, direction, ref directonPanels);
                    }
                }
            }
        }

        private Rectangle SetPanelGroupLocationOrSizeNReturnTargetNewBounds(Rectangle targetBouds, Rectangle targetNewBounds)
        {
            if (!targetBouds.Equals(targetNewBounds))
            {
                // Horizontal
                if (targetBouds.Width != targetNewBounds.Width)
                {
                    // Right
                    if (targetBouds.X == targetNewBounds.X)
                    {
                        List<IMouseActionTarget> rightPanels = PanelGroupSomeDirection[MouseActionDirectionType.Right];
                        List<IMouseActionTarget> attachedRightPanels = PanelGroupAttachedTargetSide[MouseActionDirectionType.Right];

                        if (HasSomeDirectionPanelsAttachedParentSide[MouseActionDirectionType.Right])
                        {
                            // Get minAttachedPalensWidth
                            int minAttachedPalensWidth = -1;
                            for (int i = 0; i < attachedRightPanels.Count; i++)
                            {
                                if (minAttachedPalensWidth < 0 || rightPanels[i].Width < minAttachedPalensWidth)
                                {
                                    minAttachedPalensWidth = rightPanels[i].Width;
                                }
                            }

                            // Get revisionSize
                            int revisionSize = 0;
                            if (minAttachedPalensWidth - (targetNewBounds.Width - targetBouds.Width) < MinWidth)
                            {
                                revisionSize = minAttachedPalensWidth - (MinWidth + (targetNewBounds.Width - targetBouds.Width));
                            }

                            // Set Attached Panels
                            for (int i = 0; i < attachedRightPanels.Count; i++)
                            {
                                Rectangle newBounds = attachedRightPanels[i].Bounds;
                                newBounds.X = targetNewBounds.Right + revisionSize;
                                newBounds.Width += attachedRightPanels[i].Bounds.X - newBounds.X;

                                attachedRightPanels[i].Bounds = newBounds;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Width += revisionSize;
                        }
                        else
                        {
                            // Get revisionLeft
                            int maxRightIndex = -1;
                            int[] horizontalPanelsNewLeft = new int[rightPanels.Count];
                            for (int i = 0; i < rightPanels.Count; i++)
                            {
                                horizontalPanelsNewLeft[i] = rightPanels[i].Bounds.X + targetNewBounds.Right - targetBouds.Right;
                                if ((maxRightIndex < 0 && Target.Parent.Width < horizontalPanelsNewLeft[i] + rightPanels[i].Width) || (0 <= maxRightIndex && horizontalPanelsNewLeft[maxRightIndex] < horizontalPanelsNewLeft[i]))
                                {
                                    maxRightIndex = i;
                                }
                            }
                            int revisionLeft = maxRightIndex >= 0 ? Target.Parent.Width - (horizontalPanelsNewLeft[maxRightIndex] + rightPanels[maxRightIndex].Width) : 0;

                            // Set Right Panels
                            for (int i = 0; i < rightPanels.Count; i++)
                            {
                                rightPanels[i].Left = horizontalPanelsNewLeft[i] + revisionLeft;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Width += revisionLeft;
                        }
                    }
                    // Left
                    else
                    {
                        List<IMouseActionTarget> leftPanels = PanelGroupSomeDirection[MouseActionDirectionType.Left];
                        List<IMouseActionTarget> attachedLeftPanels = PanelGroupAttachedTargetSide[MouseActionDirectionType.Left];

                        if (HasSomeDirectionPanelsAttachedParentSide[MouseActionDirectionType.Left])
                        {
                            // Get minAttachedPalensWidth
                            int minAttachedPalensWidth = -1;
                            for (int i = 0; i < attachedLeftPanels.Count; i++)
                            {
                                if (minAttachedPalensWidth < 0 || leftPanels[i].Width < minAttachedPalensWidth)
                                {
                                    minAttachedPalensWidth = leftPanels[i].Width;
                                }
                            }

                            // Get revisionSize
                            int revisionSize = 0;
                            if (minAttachedPalensWidth - (targetNewBounds.Width - targetBouds.Width) < MinWidth)
                            {
                                revisionSize = minAttachedPalensWidth - (MinWidth + (targetNewBounds.Width - targetBouds.Width));
                            }

                            // Set Attached Panels
                            for (int i = 0; i < attachedLeftPanels.Count; i++)
                            {
                                Rectangle newBounds = attachedLeftPanels[i].Bounds;
                                newBounds.Width += targetNewBounds.Left - attachedLeftPanels[i].Bounds.Right - revisionSize;

                                attachedLeftPanels[i].Bounds = newBounds;
                            }

                            // Set Target New Bounds
                            targetNewBounds.X -= revisionSize;
                            targetNewBounds.Width += revisionSize;
                        }
                        else
                        {
                            // Get revisionLeft
                            int minLeftIndex = -1;
                            int[] horizontalPanelsNewLeft = new int[leftPanels.Count];
                            for (int i = 0; i < leftPanels.Count; i++)
                            {
                                horizontalPanelsNewLeft[i] = leftPanels[i].Bounds.X + targetNewBounds.X - targetBouds.X;
                                if ((minLeftIndex < 0 && horizontalPanelsNewLeft[i] < 0) || (minLeftIndex >= 0 && horizontalPanelsNewLeft[i] < horizontalPanelsNewLeft[minLeftIndex]))
                                {
                                    minLeftIndex = i;
                                }
                            }
                            int revisionLeft = minLeftIndex >= 0 ? 0 - horizontalPanelsNewLeft[minLeftIndex] : 0;

                            // Set Left Panels
                            for (int i = 0; i < leftPanels.Count; i++)
                            {
                                leftPanels[i].Left = horizontalPanelsNewLeft[i] + revisionLeft;
                            }

                            // Set Target New Bounds
                            targetNewBounds.X += revisionLeft;
                            targetNewBounds.Width -= revisionLeft;
                        }
                    }
                }

                // Vertical
                if (targetBouds.Height != targetNewBounds.Height)
                {
                    // Bottom
                    if (targetBouds.Y == targetNewBounds.Y)
                    {
                        List<IMouseActionTarget> bottomPanels = PanelGroupSomeDirection[MouseActionDirectionType.Bottom];
                        List<IMouseActionTarget> attachedBottomPanels = PanelGroupAttachedTargetSide[MouseActionDirectionType.Bottom];

                        if (HasSomeDirectionPanelsAttachedParentSide[MouseActionDirectionType.Bottom])
                        {
                            // Get minAttachedPalensHeight
                            int minAttachedPalensHeight = -1;
                            for (int i = 0; i < attachedBottomPanels.Count; i++)
                            {
                                if (minAttachedPalensHeight < 0 || bottomPanels[i].Height < minAttachedPalensHeight)
                                {
                                    minAttachedPalensHeight = bottomPanels[i].Height;
                                }
                            }

                            // Get revisionSize
                            int revisionSize = 0;
                            if (minAttachedPalensHeight - (targetNewBounds.Height - targetBouds.Height) < MinHeight)
                            {
                                revisionSize = minAttachedPalensHeight - (MinHeight + (targetNewBounds.Height - targetBouds.Height));
                            }

                            // Set Attached Panels
                            for (int i = 0; i < attachedBottomPanels.Count; i++)
                            {
                                Rectangle newBounds = attachedBottomPanels[i].Bounds;
                                newBounds.Y = targetNewBounds.Bottom + revisionSize;
                                newBounds.Height += attachedBottomPanels[i].Bounds.Y - newBounds.Y;

                                attachedBottomPanels[i].Bounds = newBounds;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Height += revisionSize;
                        }
                        else
                        {
                            // Set Rivision Top
                            int maxBottomIndex = -1;
                            int[] horizontalPanelsNewTop = new int[bottomPanels.Count];
                            for (int i = 0; i < bottomPanels.Count; i++)
                            {
                                horizontalPanelsNewTop[i] = bottomPanels[i].Bounds.Y + targetNewBounds.Bottom - targetBouds.Bottom;
                                if ((maxBottomIndex < 0 && Target.Parent.Height < horizontalPanelsNewTop[i] + bottomPanels[i].Height) || (0 <= maxBottomIndex && horizontalPanelsNewTop[maxBottomIndex] < horizontalPanelsNewTop[i]))
                                {
                                    maxBottomIndex = i;
                                }
                            }
                            int revisionTop = maxBottomIndex >= 0 ? Target.Parent.Height - (horizontalPanelsNewTop[maxBottomIndex] + bottomPanels[maxBottomIndex].Height) : 0;

                            // Set Bottom Panels
                            for (int i = 0; i < bottomPanels.Count; i++)
                            {
                                bottomPanels[i].Top = horizontalPanelsNewTop[i] + revisionTop;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Height += revisionTop;
                        }
                    }
                    // Top
                    else
                    {
                        List<IMouseActionTarget> topPanels = PanelGroupSomeDirection[MouseActionDirectionType.Top];
                        List<IMouseActionTarget> attachedTopPanels = PanelGroupAttachedTargetSide[MouseActionDirectionType.Top];

                        if (HasSomeDirectionPanelsAttachedParentSide[MouseActionDirectionType.Top])
                        {
                            // Get minAttachedPalensHeight
                            int minAttachedPalensHeight = -1;
                            for (int i = 0; i < attachedTopPanels.Count; i++)
                            {
                                if (minAttachedPalensHeight < 0 || topPanels[i].Height < minAttachedPalensHeight)
                                {
                                    minAttachedPalensHeight = topPanels[i].Height;
                                }
                            }

                            // Get revisionSize
                            int revisionSize = 0;
                            if (minAttachedPalensHeight - (targetNewBounds.Height - targetBouds.Height) < MinHeight)
                            {
                                revisionSize = minAttachedPalensHeight - (MinHeight + (targetNewBounds.Height - targetBouds.Height));
                            }

                            // Set Attached Panels
                            for (int i = 0; i < attachedTopPanels.Count; i++)
                            {
                                Rectangle newBounds = attachedTopPanels[i].Bounds;
                                newBounds.Height += targetNewBounds.Top - attachedTopPanels[i].Bounds.Bottom - revisionSize;

                                attachedTopPanels[i].Bounds = newBounds;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Y -= revisionSize;
                            targetNewBounds.Height += revisionSize;
                        }
                        else
                        {
                            // Set Rivision Top
                            int minTopIndex = -1;
                            int[] horizontalPanelsNewTop = new int[topPanels.Count];
                            for (int i = 0; i < topPanels.Count; i++)
                            {
                                horizontalPanelsNewTop[i] = topPanels[i].Bounds.Y + targetNewBounds.Y - targetBouds.Y;
                                if ((minTopIndex < 0 && horizontalPanelsNewTop[i] < 0) || (minTopIndex >= 0 && horizontalPanelsNewTop[i] < horizontalPanelsNewTop[minTopIndex]))
                                {
                                    minTopIndex = i;
                                }
                            }
                            int revisionTop = minTopIndex >= 0 ? 0 - horizontalPanelsNewTop[minTopIndex] : 0;

                            // Set Top Panels
                            for (int i = 0; i < topPanels.Count; i++)
                            {
                                topPanels[i].Top = horizontalPanelsNewTop[i] + revisionTop;
                            }

                            // Set Target New Bounds
                            targetNewBounds.Y += revisionTop;
                            targetNewBounds.Height -= revisionTop;
                        }
                    }
                }

            }

            return targetNewBounds;
        }

        private bool IsSomeDirectionPanelsAttachedParentSide(MouseActionDirectionType direction, List<IMouseActionTarget> directonPanels)
        {
            switch (direction)
            {
                case MouseActionDirectionType.Top:
                    for (int i = directonPanels.Count - 1; 0 <= i; i--)
                    {
                        if (directonPanels[i].Top == 0)
                        {
                            return true;
                        }
                    }
                    break;
                case MouseActionDirectionType.Right:
                    for (int i = directonPanels.Count - 1; 0 <= i; i--)
                    {
                        if (directonPanels[i].Right == Target.Parent.Width)
                        {
                            return true;
                        }
                    }
                    break;
                case MouseActionDirectionType.Bottom:
                    for (int i = directonPanels.Count - 1; 0 <= i; i--)
                    {
                        if (directonPanels[i].Bottom == Target.Parent.Height)
                        {
                            return true;
                        }
                    }
                    break;
                case MouseActionDirectionType.Left:
                    for (int i = directonPanels.Count - 1; 0 <= i; i--)
                    {
                        if (directonPanels[i].Left == 0)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        private void GetPanelGroup(IMouseActionTarget target, ref List<IMouseActionTarget> rs)
        {
            //if (UsePalenGroup)
            {
                foreach (Control control in target.Parent.Controls)
                {
                    IMouseActionTarget panel = control as IMouseActionTarget;
                    if (panel != null && panel.Visible && !panel.Equals(target) && !panel.Equals(this.Target))
                    {
                        if
                        (
                            (
                                (/*target.Left == panel.Left || */target.Left == panel.Right || target.Right == panel.Left/* || target.Right == panel.Right*/)
                                &&
                                (
                                    (target.Top <= panel.Top && panel.Top <= target.Bottom)
                                    ||
                                    (target.Top <= panel.Bottom && panel.Bottom <= target.Bottom)
                                    ||
                                    (target.Top >= panel.Top && panel.Bottom >= target.Bottom)
                                )
                            )
                            ||
                            (
                                (/*target.Top == panel.Top || */target.Top == panel.Bottom || target.Bottom == panel.Top/* || target.Bottom == panel.Bottom*/)
                                &&
                                (
                                    (target.Left <= panel.Left && panel.Left <= target.Right)
                                    ||
                                    (target.Left <= panel.Right && panel.Right <= target.Right)
                                    ||
                                    (target.Left >= panel.Left && panel.Right >= target.Right)
                                )
                            )
                        )
                        {
                            if (!rs.Contains(panel))
                            {
                                rs.Add(panel);
                                GetPanelGroup(panel, ref rs);
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    rs = new List<IUserPanelMouseActionTarget>();
            //}
        }

        private void Target_MouseLeave(object sender, EventArgs e)
        {
            if (Enable)
            {
            }
        }

        private Rectangle GetBounds(MouseActionType mouseActionType, int eX, int eY)
        {
            Rectangle rs = Target.Bounds;

            // Top
            if (mouseActionType == MouseActionType.SizeTop || mouseActionType == MouseActionType.SizeTopLeft || mouseActionType == MouseActionType.SizeTopRight)
            {
                rs.Y = Target.Top + (eY - MouseDownY);

                // Parent Side - Top
                if (rs.Y <= AutoMovePixelInSide)
                {
                    rs.Y = 0;
                }

                // Other Panel Side - Top, Bottom
                if (IsAutoMove)
                {
                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroupSomeDirection[MouseActionDirectionType.Top].Contains(panel))
                        {
                            // Top
                            if (panel.Bounds.Top - AutoMovePixelInSide <= rs.Y && rs.Y <= panel.Bounds.Top + AutoMovePixelInSide)
                            {
                                rs.Y = panel.Top;
                            }
                            // Bottom
                            else if (panel.Bounds.Bottom - AutoMovePixelInSide <= rs.Y && rs.Y <= panel.Bounds.Bottom + AutoMovePixelInSide)
                            {
                                rs.Y = panel.Bounds.Bottom;
                            }
                        }
                    }
                }

                rs.Height = MouseDownRectangle.Top + MouseDownRectangle.Height - rs.Y;

                // Min Height
                if (rs.Height < MinHeight)
                {
                    rs.Y -= MinHeight - rs.Height;
                    rs.Height = MinHeight;
                }
            }
            // Bottom
            else if (mouseActionType == MouseActionType.SizeBottom || mouseActionType == MouseActionType.SizeBottomLeft || mouseActionType == MouseActionType.SizeBottomRight)
            {
                rs.Height = MouseDownRectangle.Height + (eY - MouseDownY);

                // Parent Side - Bottom
                if (Target.Parent.Height - AutoMovePixelInSide <= rs.Y + rs.Height)
                {
                    rs.Height = Target.Parent.Height - Target.Top;
                }

                // Other Panel Side - Top, Bottom
                if (IsAutoMove)
                {
                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroupSomeDirection[MouseActionDirectionType.Bottom].Contains(panel))
                        {
                            // Top
                            if (panel.Bounds.Top - AutoMovePixelInSide <= rs.Bottom && rs.Bottom <= panel.Bounds.Top + AutoMovePixelInSide)
                            {
                                rs.Height = panel.Bounds.Top - rs.Top;
                            }
                            // Bottom
                            else if (panel.Bounds.Bottom - AutoMovePixelInSide <= rs.Bottom && rs.Bottom <= panel.Bounds.Bottom + AutoMovePixelInSide)
                            {
                                rs.Height = panel.Bounds.Bottom - rs.Top;
                            }
                        }
                    }
                }

                // Min Height
                if (rs.Height < MinHeight)
                {
                    rs.Height = MinHeight;
                }
            }

            // Left
            if (mouseActionType == MouseActionType.SizeLeft || mouseActionType == MouseActionType.SizeTopLeft || mouseActionType == MouseActionType.SizeBottomLeft)
            {
                rs.X = Target.Left + (eX - MouseDownX);

                // Parent Side - Left
                if (rs.X <= AutoMovePixelInSide)
                {
                    rs.X = 0;
                }

                // Other Panel Side - Left, Right
                if (IsAutoMove)
                {
                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroupSomeDirection[MouseActionDirectionType.Left].Contains(panel))
                        {
                            // Left
                            if (panel.Bounds.Left - AutoMovePixelInSide <= rs.X && rs.X <= panel.Bounds.Left + AutoMovePixelInSide)
                            {
                                rs.X = panel.Left;
                            }
                            // Right
                            else if (panel.Bounds.Right - AutoMovePixelInSide <= rs.X && rs.X <= panel.Bounds.Right + AutoMovePixelInSide)
                            {
                                rs.X = panel.Bounds.Right;
                            }
                        }
                    }
                }

                rs.Width = MouseDownRectangle.Left + MouseDownRectangle.Width - rs.X;

                // Min Width
                if (rs.Width < MinWidth)
                {
                    rs.X -= MinWidth - rs.Width;
                    rs.Width = MinWidth;
                }
            }
            // Right
            else if (mouseActionType == MouseActionType.SizeRight || mouseActionType == MouseActionType.SizeTopRight || mouseActionType == MouseActionType.SizeBottomRight)
            {
                rs.Width = MouseDownRectangle.Width + (eX - MouseDownX);

                // Parent Side - Right
                if (Target.Parent.Width - AutoMovePixelInSide <= rs.X + rs.Width)
                {
                    rs.Width = Target.Parent.Width - Target.Left;
                }

                // Other Panel Side - Top, Right
                if (IsAutoMove)
                {
                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroupSomeDirection[MouseActionDirectionType.Right].Contains(panel))
                        {
                            // Left
                            if (panel.Bounds.Left - AutoMovePixelInSide <= rs.Right && rs.Right <= panel.Bounds.Left + AutoMovePixelInSide)
                            {
                                rs.Width = panel.Bounds.Left - rs.Left;
                            }
                            // Right
                            else if (panel.Bounds.Right - AutoMovePixelInSide <= rs.Right && rs.Right <= panel.Bounds.Right + AutoMovePixelInSide)
                            {
                                rs.Width = panel.Bounds.Right - rs.Left;
                            }
                        }
                    }
                }

                // Min Width
                if (rs.Width < MinWidth)
                {
                    rs.Width = MinWidth;
                }
            }

            return rs;
        }

        private Point GetLocation(int eX, int eY)
        {
            return new Point(GetLocation_GetLeft(eX), GetLocation_GetTop(eY));
        }

        private int GetLocation_GetLeft(int eX)
        {
            int left = Target.Left + eX - MouseDownX;

            if (UseLocationCorrection)
            {
                if (left < 0)
                {
                    left = 0;
                }
                else if (Target.Width + left >= Target.Parent.Width)
                {
                    left = Target.Parent.Width - Target.Width;
                }

                // Other Panel Side - Left, Right
                if (IsAutoMove)
                {
                    // Parent Side
                    if (left <= AutoMovePixelInSide || left < 0)
                    {
                        left = 0;
                        return left;
                    }
                    else if (Target.Width + left >= Target.Parent.Width - AutoMovePixelInSide)
                    {
                        left = Target.Parent.Width - Target.Width;
                        return left;
                    }

                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroup.Contains(panel))
                        {
                            // 제한 옵션으로? / 가상의 선?
                            //if
                            //(
                            //    (Target.Top < panel.Top && panel.Top < Target.Bottom)
                            //    ||
                            //    (Target.Top < panel.Bottom && panel.Bottom < Target.Bottom)
                            //)
                            {
                                // Target : Left / Panel : Left
                                if (panel.Left - AutoMovePixelInSide <= left && left <= panel.Left + AutoMovePixelInSide)
                                {
                                    left = panel.Left;
                                    return left;
                                }
                                // Target : Left / Panel : Right
                                else if (panel.Right - AutoMovePixelInSide <= left && left <= panel.Right + AutoMovePixelInSide)
                                {
                                    left = panel.Right;
                                    return left;
                                }
                                // Target : Right / Panel : Left
                                else if (panel.Left - AutoMovePixelInSide <= left + Target.Width && left + Target.Width <= panel.Left + AutoMovePixelInSide)
                                {
                                    left = panel.Left - Target.Width;
                                    return left;
                                }
                                // Target : Right / Panel : Right
                                else if (panel.Right - AutoMovePixelInSide <= left + Target.Width && left + Target.Width <= panel.Right + AutoMovePixelInSide)
                                {
                                    left = panel.Right - Target.Width;
                                    return left;
                                }
                            }
                        }
                    }
                }
            }

            return left;
        }

        private int GetLocation_GetTop(int eY)
        {
            int top = Target.Top + eY - MouseDownY;
            if (UseLocationCorrection)
            {
                if (top < 0)
                {
                    top = 0;
                }
                else if (Target.Height + top >= Target.Parent.Height)
                {
                    top = Target.Parent.Height - Target.Height;
                }

                // Other Panel Side - Top, Botton
                if (IsAutoMove)
                {
                    // Parent Edge
                    if (top <= AutoMovePixelInSide || top < 0)
                    {
                        top = 0;
                        return top;
                    }
                    else if (Target.Height + top >= Target.Parent.Height - AutoMovePixelInSide)
                    {
                        top = Target.Parent.Height - Target.Height;
                        return top;
                    }

                    foreach (Control control in Target.Parent.Controls)
                    {
                        IMouseActionTarget panel = control as IMouseActionTarget;
                        if (panel != null && panel.Visible && !panel.Equals(Target) && !PanelGroup.Contains(panel))
                        {
                            // 제한 옵션으로?
                            //if
                            //(
                            //    (Target.Left < panel.Left && panel.Left < Target.Right)
                            //    ||
                            //    (Target.Left < panel.Right && panel.Right < Target.Right)
                            //)
                            {
                                // Target : Top / Panel : Top
                                if (panel.Top - AutoMovePixelInSide <= top && top <= panel.Top + AutoMovePixelInSide)
                                {
                                    top = panel.Top;
                                    return top;
                                }
                                // Target : Top / Panel : Bottom
                                else if (panel.Bottom - AutoMovePixelInSide <= top && top <= panel.Bottom + AutoMovePixelInSide)
                                {
                                    top = panel.Bottom;
                                    return top;
                                }
                                // Target : Bottom / Panel : Top
                                else if (panel.Top - AutoMovePixelInSide <= top + Target.Height && top + Target.Height <= panel.Top + AutoMovePixelInSide)
                                {
                                    top = panel.Top - Target.Height;
                                    return top;
                                }
                                // Target : Bottom / Panel : Bottom
                                else if (panel.Bottom - AutoMovePixelInSide <= top + Target.Height && top + Target.Height <= panel.Bottom + AutoMovePixelInSide)
                                {
                                    top = panel.Bottom - Target.Height;
                                    return top;
                                }
                            }
                        }
                    }
                }
            }

            return top;
        }

        private MouseActionType GetMouseActionType(int x, int y)
        {
            MouseActionType rs = MouseActionType.Move;

            // Left
            if (x <= SizeArea)
            {
                rs = MouseActionType.SizeLeft;
            }
            // Right
            else if (Target.Width - SizeArea <= x)
            {
                rs = MouseActionType.SizeRight;
            }

            // Top
            if (y <= SizeArea)
            {
                switch (rs)
                {
                    case MouseActionType.SizeRight: rs = MouseActionType.SizeTopRight; break;
                    case MouseActionType.SizeLeft: rs = MouseActionType.SizeTopLeft; break;
                    default: rs = MouseActionType.SizeTop; break;
                }
            }
            // Bottom
            else if (Target.Height - SizeArea <= y)
            {
                switch (rs)
                {
                    case MouseActionType.SizeRight: rs = MouseActionType.SizeBottomRight; break;
                    case MouseActionType.SizeLeft: rs = MouseActionType.SizeBottomLeft; break;
                    default: rs = MouseActionType.SizeBottom; break;
                }
            }

            return rs;
        }

        private bool IsAutoMove
        {
            get
            {
                return !System.Windows.Input.Keyboard.IsKeyDown(NoneAutoMoveKey);
            }
        }

        private bool IsGroupMove
        {
            get
            {
                return System.Windows.Input.Keyboard.IsKeyDown(GroupMoveKey);
            }
        }
    }

    public enum MouseActionType
    {
        None = 0,
        Move = 1,
        SizeTop,
        SizeTopRight,
        SizeRight,
        SizeBottomRight,
        SizeBottom,
        SizeBottomLeft,
        SizeLeft,
        SizeTopLeft,
    }

    public enum MouseActionDirectionType
    {
        Top = 1,
        Right,
        Bottom,
        Left
    }
}
