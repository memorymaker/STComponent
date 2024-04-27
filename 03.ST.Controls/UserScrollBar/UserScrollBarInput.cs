using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserScrollBar
    {
        // Options
        public bool UseWheel = false;

        // ---- Using
        // DecrementButtonRectangle
        // EncrementButtonRectangle
        // ScrollButtonRectangle
        // SetScrollButtonRectangle()
        // Draw()

        // ---- Set
        // Value

        // ---- Options
        private readonly int TimerInterval = 50;
        private readonly int TimerStartDelay = 250;

        // ---- Ref
        // Mouse
        private bool IsMouseDown = false;
        private Point MouseDownPoint = Point.Empty;
        private Rectangle MouseDownScrollButtonRectangle = Rectangle.Empty;
        private MouseActionType ActionType = MouseActionType.None;
        // Timer
        private System.Timers.Timer Timer;
        private delegate void TimerEventFiredDelegate();

        private void LoadInput()
        {
            // Controls
            Timer = new System.Timers.Timer(TimerStartDelay);
            Timer.Enabled = false;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);

            // Events
            MouseDown += UserScrollBar_MouseDown;
            MouseMove += UserScrollBar_MouseMove;
            MouseUp += UserScrollBar_MouseUp;
            MouseLeave += UserScrollBar_MouseLeave;
            MouseWheel += UserScrollBar_MouseWheel;
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BeginInvoke(new TimerEventFiredDelegate(TimerWork));
        }

        private void TimerWork()
        {
            Point point = PointToClient(Cursor.Position);
            if (GetActionType(point) == ActionType)
            {
                switch (ActionType)
                {
                    case MouseActionType.DecrementButton: Value -= SmallChange; break;
                    case MouseActionType.EncrementButton: Value += SmallChange; break;
                    case MouseActionType.DecrementArea: Value -= LargeChange; break;
                    case MouseActionType.EncrementArea: Value += LargeChange; break;
                }
            }

            if (Timer.Interval == TimerStartDelay)
            {
                Timer.Interval = TimerInterval;
            }
        }

        private void UserScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownPoint = e.Location;
            IsMouseDown = true;

            ActionType = GetActionType(e.Location);
            switch (ActionType)
            {
                case MouseActionType.DecrementButton:
                    Value -= SmallChange;
                    Timer.Enabled = true;
                    break;
                case MouseActionType.EncrementButton:
                    Value += SmallChange;
                    Timer.Enabled = true;
                    break;
                case MouseActionType.DecrementArea:
                    Value -= LargeChange;
                    Timer.Enabled = true;
                    break;
                case MouseActionType.EncrementArea:
                    Value += LargeChange;
                    Timer.Enabled = true;
                    break;
                case MouseActionType.ScrollButton:
                    MouseDownScrollButtonRectangle = ScrollButtonRectangle;
                    break;
            }
            Draw();
        }

        private MouseActionType GetActionType(Point point)
        {
            MouseActionType rs;

            if (DecrementButtonRectangle.Contains(point))
            {
                rs = MouseActionType.DecrementButton;
            }
            else if (EncrementButtonRectangle.Contains(point))
            {
                rs = MouseActionType.EncrementButton;
            }
            else if (ScrollButtonRectangle.Contains(point))
            {
                rs = MouseActionType.ScrollButton;
            }
            else if (point.Y < ScrollButtonRectangle.Top)
            {
                rs = MouseActionType.DecrementArea;
            }
            else if (ScrollButtonRectangle.Bottom < point.Y)
            {
                rs = MouseActionType.EncrementArea;
            }
            else
            {
                rs = MouseActionType.None;
            }

            return rs;
        }

        private void UserScrollBar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsMouseDown && e.Button == MouseButtons.Left)
            {
                switch(ActionType)
                {
                    case MouseActionType.DecrementArea:
                        if (GetActionType(e.Location) == MouseActionType.DecrementArea)
                        {
                            Value -= LargeChange;
                        }
                        break;
                    case MouseActionType.EncrementArea:
                        if (GetActionType(e.Location) == MouseActionType.EncrementArea)
                        {
                            Value += LargeChange;
                        }
                        break;
                    case MouseActionType.ScrollButton:
                        if (Type == UserScrollBarType.Vertical)
                        {
                            int newScrollButtonTop = MouseDownScrollButtonRectangle.Y + e.Y - MouseDownPoint.Y;
                            newScrollButtonTop = Math.Max(Math.Min(newScrollButtonTop, EncrementButtonRectangle.Top - ScrollButtonRectangle.Height), DecrementButtonRectangle.Bottom);

                            decimal areaHeight = EncrementButtonRectangle.Top - DecrementButtonRectangle.Bottom;
                            // ((top / (height[0~NNN] - scrollSize)) * (max - min)) + min = val
                            int value = (int)Math.Round((((newScrollButtonTop - DecrementButtonRectangle.Bottom) / (areaHeight - ScrollButtonRectangle.Height)) * (Maximum - Minimum)) + Minimum);

                            int oldValue = _Value;
                            _Value = value;
                            if (_Value != oldValue)
                            {
                                ValueChanged?.Invoke(this, new UserScrollBarEventArgs(_Value, oldValue));
                            }

                            ScrollButtonRectangle.Y = newScrollButtonTop;
                        }
                        else if (Type == UserScrollBarType.Horizontal)
                        {
                            int newScrollButtonLeft = MouseDownScrollButtonRectangle.X + e.X - MouseDownPoint.X;
                            newScrollButtonLeft = Math.Max(Math.Min(newScrollButtonLeft, EncrementButtonRectangle.Left - ScrollButtonRectangle.Width), DecrementButtonRectangle.Right);

                            decimal areaWidth = EncrementButtonRectangle.Left - DecrementButtonRectangle.Right;
                            // ((top / (height[0~NNN] - scrollSize)) * (max - min)) + min = val
                            int value = (int)Math.Round((((newScrollButtonLeft - DecrementButtonRectangle.Right) / (areaWidth - ScrollButtonRectangle.Width)) * (Maximum - Minimum)) + Minimum);

                            int oldValue = _Value;
                            _Value = value;
                            if (_Value != oldValue)
                            {
                                ValueChanged?.Invoke(this, new UserScrollBarEventArgs(_Value, oldValue));
                            }

                            ScrollButtonRectangle.X = newScrollButtonLeft;
                        }
                        break;
                }
            }
            Draw();
        }

        private void UserScrollBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Timer.Enabled = false;
            Timer.Interval = TimerStartDelay;
            IsMouseDown = false;
            ActionType = MouseActionType.None;
            SetScrollButtonRectangle();
            Draw();
        }

        private void UserScrollBar_MouseLeave(object sender, EventArgs e)
        {
            ActionType = MouseActionType.None;
            Draw(ScrollButtonColor);
        }

        private void UserScrollBar_MouseWheel(object sender, MouseEventArgs e)
        {
            if (UseWheel)
            {
                var point = -(e.Delta * 3 / 120) * SmallChange;
                Value = Value + point;
            }
        }

        private enum MouseActionType
        {
            None, DecrementButton, EncrementButton, ScrollButton, DecrementArea, EncrementArea
        }
    }
}
