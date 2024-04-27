using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ST.Core
{
    public static class Event
    {
        public static void CallEvent(object sender, EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, null);
            }
        }

        public static void CallEvent(object sender, PreviewKeyDownEventHandler eventHandler, PreviewKeyDownEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, KeyEventHandler eventHandler, KeyEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, MouseEventHandler eventHandler, MouseEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, PaintEventHandler eventHandler, PaintEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, DragEventHandler eventHandler, DragEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, UserEventHandler eventHandler, UserEventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, UserEventHandler eventHandler, string dataName, object data)
        {
            if (eventHandler != null)
            {
                UserEventArgs e = new UserEventArgs(new Dictionary<string, object> {
                    { dataName, data }
                });
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, UserEventHandler eventHandler, string[] dataName, object[] data)
        {
            if (eventHandler != null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                for (int i = 0; i < dataName.Length; i++)
                {
                    dic.Add(dataName[i], data[i]);
                }
                UserEventArgs e = new UserEventArgs(dic);
                eventHandler(sender, e);
            }
        }

        public static void CallEvent(object sender, UserEventHandler eventHandler, Dictionary<string, object> data)
        {
            if (eventHandler != null)
            {
                UserEventArgs e = new UserEventArgs(data);
                eventHandler(sender, e);
            }
        }
    }

    public delegate void UserEventHandler(object sender, UserEventArgs e);

    public class UserEventArgs : EventArgs
    {
        public Dictionary<string, object> Data;
        public UserEventArgs(Dictionary<string, object> data)
        {
            Data = data;
        }
    }

    public class UserEventArgsString : EventArgs
    {
        public string Data;
        public UserEventArgsString(string data)
        {
            Data = data;
        }
    }
}
