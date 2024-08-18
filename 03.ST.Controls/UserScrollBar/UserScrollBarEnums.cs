using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public enum UserScrollBarType
    {
        Vertical, Horizontal
    }

    public partial class UserScrollBar
    {
        private enum MouseActionType
        {
            None, DecrementButton, EncrementButton, ScrollButton, DecrementArea, EncrementArea
        }
    }
}