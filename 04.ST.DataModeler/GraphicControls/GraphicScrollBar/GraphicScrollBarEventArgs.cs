using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public class UserScrollBarEventArgs : EventArgs
    {
        public int Value
        {
            get
            {
                return _Value;
            }
        }
        private int _Value;

        public int OldValue
        {
            get
            {
                return _OldValue;
            }
        }
        private int _OldValue;

        public UserScrollBarEventArgs()
        {
        }

        public UserScrollBarEventArgs(int value, int oldValue)
        {
            _Value = value;
            _OldValue = oldValue;
        }
    }
}
