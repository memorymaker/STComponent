using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public partial class UserScrollBar
    {
        public enum UserScrollBarType
        {
            Vertical, Horizontal
        }

        private enum MouseActionType
        {
            None, DecrementButton, EncrementButton, ScrollButton, DecrementArea, EncrementArea
        }
    }
}