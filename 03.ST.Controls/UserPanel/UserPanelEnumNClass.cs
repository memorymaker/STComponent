using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public enum UserPanelTitleStateType
    {
        None = 1,
        Over,
        Selected,
        FocusedSelected
    }

    public enum UserPanelMouseDownArea
    {
        None = 0,
        Title = 1,
        TitleClose,
        Body
    }

    public class UserPanelTitleInfo
    {
        public string Title;
        public int Sort;
        public string GUID;
        public bool Selected;
        public bool Own;
        public UserPanelTitleInfo(string Title, int Sort, string GUID, bool Selected = false, bool Own = false)
        {
            this.Title = Title;
            this.Sort = Sort;
            this.GUID = GUID;
            this.Selected = Selected;
            this.Own = Own;
        }
    }

    public class UserPanelSaveInfo
    {
        public string GUID;
        public string Name;
        public int Left;
        public int Top;
        public int Width;
        public int Height;
        public string Title;
        public bool TitleVisable;
        public string TypeFullName;
        public bool AwaysOnTop;
        public int ZOrder;
        public bool Visible;
        public string ParentForShowNHideGUID;

        public List<UserPanelTitleInfo> TitleList;
        public List<UserPanelSaveInfo> Children;

        public UserPanelSaveInfo() { }

        public UserPanelSaveInfo(UserPanel userPanel)
        {
            GUID = userPanel.GUID;
            Name = userPanel.Name;
            Left = userPanel.Left;
            Top = userPanel.Top;
            Width = userPanel.Width;
            Height = userPanel.Height;
            Title = userPanel.Title;
            TitleList = userPanel.TitleList;
            TitleVisable = userPanel.P_TitleVisible;
            TypeFullName = userPanel.GetType().FullName;
            AwaysOnTop = userPanel.AwaysOnTop;
            ZOrder = userPanel.Parent == null
                ? 0 : userPanel.Parent.Controls.GetChildIndex(userPanel);
            Visible = userPanel.Visible;
            ParentForShowNHideGUID = userPanel.ParentForShowNHideGUID;

            foreach (Control control in userPanel.Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    if (Children == null)
                    {
                        Children = new List<UserPanelSaveInfo>();
                    }
                    Children.Add(new UserPanelSaveInfo(panel));
                }
            }
        }
    }

    public class UserPanelPositionInfo
    {
        public UserPanelPositionType Position;
        public List<UserPanel> SiblingLeft;
        public List<UserPanel> SiblingTop;
        public UserPanelPositionInfo(UserPanelPositionType Position, List<UserPanel> SiblingLeft, List<UserPanel> SiblingTop)
        {
            this.Position = Position;
            this.SiblingLeft = SiblingLeft;
            this.SiblingTop = SiblingTop;
        }
    }

    public enum UserPanelPositionType
    {
        None = 90000,
        Top = 91000,
        Right = 90100,
        Bottom = 90010,
        Left = 90001,
        TopRight = 91100,
        TopBottom = 91010,
        TopLeft = 91001,
        RightBottom = 90110,
        RightLeft = 90101,
        BottomLeft = 90011,
        TopRightBottom = 91110, // Right Bar
        TopRightLeft = 91101, // Top Bar
        TopBottomLeft = 91011, // Left Bar
        RightBottomLeft = 90111, // Bottom Bar
        TopRightBottomLeft = 91111  // Fill
    }
}
