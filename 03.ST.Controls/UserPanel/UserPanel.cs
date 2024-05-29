using Newtonsoft.Json;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.Controls
{
    public partial class UserPanel : Panel, IMouseActionTarget
    {
        // Test
        int cnt = 0;

        // ------------ Option
        private int UserBorderWidth = 2;
        private Padding UserPadding = new Padding(2);
        private Color BorderSizingColor = Color.Red;
        // ------ Title
        // Position, Size, Location
        private Point TitleTextPosition = new Point(4, 7);
        private Padding TitlePadding = new Padding(0, 0, 0, 0);

        // System Options
        private int _TitleHeight = 23;
        private int TitleDistance = 1;
        private int TitleWidthRevision = 25;
        private Rectangle TitleCloseButtonRectangle = new Rectangle(-15, 7, 9, 9);
        private int TitleCloseButtonAreaRevision = 2;
        private int MergeRevisionY = -1;
        private int MergeRevisionX = -1;
        private decimal MregeRevisionTitleWidth = 0.5m;
        private int SplitRevisionY = 10;

        // ------ TitleArea
        private int TitleAreaRightPadding = 35;

        // ------------ Ref
        public string GUID
        {
            get { return _GUID; }
        }
        private string _GUID;
        private UserPanel ExitingPanel = null;
        private UserPanel BecomingParentPanel = null;
        private string TitleGUIDToClose = string.Empty;
        private UserPanelMouseDownArea MouseDownArea = UserPanelMouseDownArea.None;
        // Fill
        private bool IsFill = false;

        // ------------ Class
        public MouseAction MouseAction;
        private new UserPanelContextMenu ContextMenu;

        // ------------ UserPanelControl
        private GraphicControl ViewContextMenuButton = null;
        private GraphicControl AwaysOnTopMenuButton = null;
        private GraphicControl SlideLeftMenuButton = null;
        private GraphicControl SlideRightMenuButton = null;

        // EventHandler
        public event UserPanelClosingEventHandler Closing;
        public event UserPanelShownEventHandler Shown;

        #region Propertise
        // ------------ Public Values
        public UserPanelPositionInfo PositionInfo
        {
            get
            {
                return _PositionInfo;
            }
        }
        private UserPanelPositionInfo _PositionInfo = null;

        public Rectangle NBounds
        {
            get
            {
                return _NBounds;
            }
        }
        private Rectangle _NBounds;

        public bool AwaysOnTop
        {
            get
            {
                return _AwaysOnTop;
            }
            set
            {
                _AwaysOnTop = value;
                if (_AwaysOnTop)
                {
                    AwaysOnTopMenuButton.SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(245, 245, 245));
                }
                else
                {
                    AwaysOnTopMenuButton.SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(110, 132, 180));
                }
            }
        }
        private bool _AwaysOnTop = false;

        new public Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                if (value.Left != UserPadding.Left
                ||  value.Top != TitleHeight
                ||  value.Right != UserPadding.Right
                ||  value.Bottom != UserPadding.Bottom)
                {
                    UserPadding = value;
                    base.Padding = value;
                }
            }
        }

        public int TitleHeight
        {
            get { return _TitleHeight; }
            //set { _TitleHeight = value; }
        }

        public bool HasExitingPanel
        {
            get
            {
                return ExitingPanel != null;
            }
        }

        public bool HasBecomingParentPanel
        {
            get
            {
                return BecomingParentPanel != null;
            }
        }

        protected Padding _Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }
        #endregion

        [Localizable(true)]
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value != base.Text)
                {
                    _Title = value;
                    for (int i = 0; i < TitleList.Count; i++)
                    {
                        if (TitleList[i].GUID == GUID)
                        {
                            TitleList[i].Title = Title;
                            Draw();
                        }
                    }
                }
            }
        }
        private string _Title = string.Empty;

        [Localizable(true)]
        private bool TitleVisable
        {
            get
            {
                return _TitleVisable;
            }
            set
            {
                if (_TitleVisable != value)
                {
                    _TitleVisable = value;
                    if (_TitleVisable)
                    {
                        base.Padding = new Padding(UserPadding.Left, TitleHeight, UserPadding.Right, UserPadding.Bottom);
                        foreach (Control control in Controls)
                        {
                            if (!(control is Label))
                            {
                                control.Top += TitleHeight;
                            }
                        }
                        Draw();
                    }
                    else
                    {
                        //base.Padding = new Padding(UserPadding.Left, UserPadding.Top, UserPadding.Right, UserPadding.Bottom);
                        base.Padding = new Padding(0);
                        foreach (Control control in Controls)
                        {
                            if (!(control is Label))
                            {
                                control.Top -= TitleHeight;
                            }
                        }
                        CreateGraphics().Clear(BackColor);
                    }
                }
            }
        }
        private bool _TitleVisable = true;

        public bool P_TitleVisible
        {
            get
            {
                return _TitleVisable;
            }
        }

        public List<UserPanelTitleInfo> TitleList = new List<UserPanelTitleInfo>();

        public UserPanel()
        {
            InitializeComponent();
            //DoubleBuffered = true;
            //SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //UpdateStyles();
            _GUID = Guid.NewGuid().ToString();
            _Title = "New Panel";

            LoadThis();
            LoadGraphicControls();
            LoadInput();
            LoadDraw();
            LoadOption();
        }

        public UserPanel(string guid)
        {
            InitializeComponent();
            //DoubleBuffered = true;
            //SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //UpdateStyles();
            _GUID = guid;
            _Title = "New Panel";

            LoadThis();
            LoadGraphicControls();
            LoadInput();
            LoadDraw();
            LoadOption();
        }

        public UserPanel(UserPanelSaveInfo panelInfo)
        {
            InitializeComponent();
            //DoubleBuffered = true;
            //SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //UpdateStyles();
            _GUID = panelInfo.GUID;
            _Title = "New Panel";

            LoadThis();
            LoadGraphicControls(); 
            LoadInput();
            LoadDraw();
            LoadPanelInfo(panelInfo);
            LoadOption();
        }

        public void LoadPanelInfo(UserPanelSaveInfo panelInfo)
        {
            _GUID = panelInfo.GUID;

            Name = panelInfo.Name;
            Left = panelInfo.Left;
            Top = panelInfo.Top;
            Width = panelInfo.Width;
            Height = panelInfo.Height;
            Title = panelInfo.Title;
            TitleList = panelInfo.TitleList;
            TitleVisable = panelInfo.TitleVisable;
			AwaysOnTop = panelInfo.AwaysOnTop;
			ParentForShowNHideGUID = panelInfo.ParentForShowNHideGUID;

            string selectedTitleGuid = null;
            for(int i = 0; i < panelInfo.TitleList.Count; i++)
            {
                if (panelInfo.TitleList[i].Selected)
                {
                    selectedTitleGuid = panelInfo.TitleList[i].GUID;
                    break;
                }
            }
            
			if (panelInfo.Children != null)
            {
                foreach (UserPanelSaveInfo panelCildInfo in panelInfo.Children)
                {
                    Type type = Type.GetType(panelCildInfo.TypeFullName);
                    dynamic objType = Activator.CreateInstance(type);
                    UserPanel panelChild = objType as UserPanel;
                    panelChild.LoadPanelInfo(panelCildInfo);

                    Controls.Add(panelChild);
                    panelChild.Dock = DockStyle.Fill;

                    if (selectedTitleGuid == panelChild.GUID)
                    {
                        SelectTitle(panelChild.GUID);
                    }
                }
            }
        }

        public void LoadPanelInfoNoChildren(UserPanelSaveInfo panelInfo)
        {
            _GUID = panelInfo.GUID;

            Name = panelInfo.Name;
            Left = panelInfo.Left;
            Top = panelInfo.Top;
            Width = panelInfo.Width;
            Height = panelInfo.Height;
            Title = panelInfo.Title;
            TitleList = panelInfo.TitleList;
            TitleVisable = panelInfo.TitleVisable;
			AwaysOnTop = panelInfo.AwaysOnTop;
			ParentForShowNHideGUID = panelInfo.ParentForShowNHideGUID;
		}

        private void LoadThis()
        {
            SetDefault();
            SetEvents();
        }

        private void SetDefault()
        {
            TabStop = true;

            base.Padding = new Padding(UserPadding.Left, TitleHeight, UserPadding.Right, UserPadding.Bottom);
            BackColor = UserDefaultBackColor;

            MouseAction = new MouseAction(this);

            ContextMenu = new UserPanelContextMenu(this);
            ContextMenu.GotFocus += UserPanelOrChildren_GotFocus;
            ContextMenu.LostFocus += UserPanelOrChildren_LostFocus;
            ContextMenu.ItemClick += ContextMenu_ItemClick;
            ContextMenu.ItemCloseClick += ContextMenu_ItemCloseClick;

            TitleList = new List<UserPanelTitleInfo>()
            {
                new UserPanelTitleInfo(Title, 0, GUID, true, true)
            };

        }

        private void ContextMenu_ItemClick(object sender, UserPanelContextMenuItemEventArgs e)
        {
            if (e.SelectedIndex >= 0)
            {
                var guid = ((UserPanelTitleInfo)e.Data).GUID;
                SelectTitle(guid);
                Draw();
            }
        }

        private void ContextMenu_ItemCloseClick(object sender, UserPanelContextMenuItemEventArgs e)
        {
            if (e.SelectedIndex >= 0)
            {
                if (TitleList.Count == 1)
                {
                    e.DisposeContextMenu = true;
                }
                var guid = ((UserPanelTitleInfo)e.Data).GUID;
                Console.WriteLine(string.Format("1 : {0} - {1}", ((UserPanelTitleInfo)e.Data).Title, ((UserPanelTitleInfo)e.Data).GUID)); // asdf
                CloseTitle(guid);
                Draw();
            }
        }

        private void SetEvents()
        {
            GotFocus += UserPanelOrChildren_GotFocus;
            LostFocus += UserPanelOrChildren_LostFocus;
            ControlAdded += UserPanel_ControlAdded;
            ControlRemoved += UserPanel_ControlRemoved;
            SizeChanged += UserPanel_SizeChanged;
        }

        private void UserPanel_SizeChanged(object sender, EventArgs e)
        {
            var selectedTitleInfo = GetSelectedTitleInfo();

            if (UsingTitleSlider && GetTitleAreaWidth() < Width - TitleSliderWidth)
            {
                SlideLeftMenuButton.Enabled = false;
                SlideRightMenuButton.Enabled = false;
                TitleStartIndex = 0;
            }
            
            if (selectedTitleInfo != null)
            {
                if (!IsTitleDisplayedNotUsingTitleSlider(selectedTitleInfo.GUID))
                {
                    AddSelectedTitleSort(-1);
                }
                Draw();
            }

            foreach(Control control in Controls)
            {
                var panel = control as UserPanel;
                if (panel != null)
                {
                    panel.Size = new Size(Width - Padding.Horizontal, Height - Padding.Vertical);
                }
            }
        }

        private Rectangle GetSelectedTitleRectangle()
        {
            var rs = new Rectangle();
            var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
            int addedWidth = 0;
            for (int i = 0 + TitleStartIndex; i < sortedTitles.Count; i++)
            {
                UserPanelTitleInfo titleNode = sortedTitles[i];
                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                if (titleNode.Selected)
                {
                    rs = new Rectangle(addedWidth, 0, textSize.Width + TitleWidthRevision + TitleDistance, TitleHeight);
                    break;
                }
                addedWidth += textSize.Width + TitleWidthRevision + TitleDistance;
            }
            return rs;
        }

        private void AddSelectedTitleSort(int num)
        {
            var selectedTitle = GetSelectedTitleInfo();

            if (num > 0)
            {
                if (selectedTitle.Sort + num <= TitleList.Count)
                {
                    int sp = selectedTitle.Sort;
                    int ep = selectedTitle.Sort + num;

                    var sortedTitles = from title in TitleList orderby title.Sort select title;
                    foreach (UserPanelTitleInfo titleNode in sortedTitles)
                    {
                        if (sp < titleNode.Sort && titleNode.Sort <= ep)
                        {
                            titleNode.Sort -= 1;
                        }
                        else if (titleNode.GUID == selectedTitle.GUID)
                        {
                            titleNode.Sort += num;
                        }
                    }
                }
            }
            else
            {
                if (selectedTitle.Sort + num >= 0)
                {
                    int sp = selectedTitle.Sort + num;
                    int ep = selectedTitle.Sort;

                    var sortedTitles = from title in TitleList orderby title.Sort select title;
                    foreach (UserPanelTitleInfo titleNode in sortedTitles)
                    {
                        if (sp <= titleNode.Sort && titleNode.Sort < ep)
                        {
                            titleNode.Sort += 1;
                        }
                        else if (titleNode.GUID == selectedTitle.GUID)
                        {
                            titleNode.Sort += num;
                        }
                    }
                }
            }
        }

        private UserPanelTitleInfo GetSelectedTitleInfo()
        {
            UserPanelTitleInfo rs = null;
            for (int i = 0; i < TitleList.Count; i++)
            {
                if (TitleList[i].Selected)
                {
                    rs = TitleList[i];
                }
            }
            return rs;
        }

        private UserPanelTitleInfo GetTitleInfo(string guid)
        {
            UserPanelTitleInfo rs = null;
            for (int i = 0; i < TitleList.Count; i++)
            {
                if (TitleList[i].GUID == guid)
                {
                    rs = TitleList[i];
                }
            }
            return rs;
        }

        private int GetTitleAreaWidth()
        {
            int addedWidth = 0;
            for (int k = 0 + TitleStartIndex; k < TitleList.Count; k++)
            {
                Size textSize = TextRenderer.MeasureText(TitleList[0].Title, Font);
                int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;
                addedWidth = titleRight;
            }
            return addedWidth;
        }

        public void CloseTitle(string titleGUID)
        {
            UserPanel panel = GetUserPanel(titleGUID);

            Console.WriteLine(string.Format("2 : {0} - {1}", panel.Title, panel.GUID)); // asdf

            UserPanelClosingEventArgs e = new UserPanelClosingEventArgs();
            Closing?.Invoke(panel, e);
            if (!e.Cancel)
            {
                // This
                if (panel.Equals(this))
                {
                    // Exit N Close
                    if (HasChildrenPanel())
                    {
                        ExitPanelProcOwn(true);
                    }
                    // Close
                    else
                    {
                        panel.HIdePanel(); //panel.Dispose();
                    }
                }
                // Child
                else
                {
                    // New Select Title Proc #1 Start
                    int sort = GetTitleInfo(panel.GUID).Sort;

                    RemoveTitle(panel.GUID);
                    panel.HIdePanel(); //panel.Dispose();
                    ResetTitleStartIndex();

                    // New Select Title Proc #2 End
                    SelectTitle(sort);
                }
            }
        }

        public void SelectTitle(string guid)
        {
            UserPanelTitleInfo selectedTitleInfo = GetSelectedTitleInfo();
            if (selectedTitleInfo == null || GetSelectedTitleInfo().GUID != guid)
            {
                SelectTitleProcSetBody(guid);
            }
            SelectTitleProcSetSort(guid);
            SelectTitleProcSetTitle(guid);
        }

        public void SelectTitle(int sort)
        {
            sort = Math.Max(Math.Min(TitleList.Count - 1, sort), 0);
            string guid = (from title in TitleList where title.Sort == sort select title.GUID).ToList()[0];
            SelectTitle(guid);
        }

        private void SelectTitleProcSetSort(string guid)
        {
            if (UsingTitleSlider)
            {
                if (!IsTitleDisplayedUsingTitleSlider(guid))
                {
                    int sort = GetTitleInfo(guid).Sort;
                    if (sort < TitleStartIndex)
                    {
                        TitleStartIndex = sort;
                    }
                    else
                    {
                        int maxTitleIndex = GetMaxTitleStartIndex();
                        do
                        {
                            TitleStartIndex++;
                        }
                        while (!IsTitleDisplayedUsingTitleSlider(guid) && TitleStartIndex < maxTitleIndex);
                    }
                }
            }
            else
            {
                if (!IsTitleDisplayedNotUsingTitleSlider(guid))
                {
                    SetTitleSort(guid, 0);
                }
            }
        }

        private bool IsTitleDisplayedUsingTitleSlider(string guid)
        {
            bool rs = false;

            int addedWidth = 0;
            int index = 0;
            var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
            for (int k = 0 + TitleStartIndex; k < sortedTitles.Count; k++)
            {
                UserPanelTitleInfo titleNode = sortedTitles[k];

                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                string title = titleNode.Title.Trim();

                Rectangle titleRect = new Rectangle(
                    TitlePadding.Left + addedWidth, TitlePadding.Top
                    , textSize.Width + TitleWidthRevision, TitleHeight - TitlePadding.Vertical
                );

                int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                if (UsingTitleSlider)
                {
                    if (titleRight >= Width - TitleSliderWidth)
                    {
                        break;
                    }
                }

                if (titleNode.GUID == guid)
                {
                    rs = true;
                    break;
                }

                addedWidth = titleRight;
                index++;
            }

            return rs;
        }

        private bool IsTitleDisplayedNotUsingTitleSlider(string guid)
        {
            var rs = false;

            var sortedTitles = from title in TitleList orderby title.Sort select title;
            int addedWidth = 0;
            int index = 0;
            foreach (UserPanelTitleInfo titleNode in sortedTitles)
            {
                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                Rectangle TitleRect = new Rectangle(TitlePadding.Left + addedWidth, TitlePadding.Top
                    , textSize.Width + TitleWidthRevision, TitleHeight - TitlePadding.Vertical);

                if (titleRight + TitleAreaRightPadding > Width && index > 0)
                {
                    break;
                }

                if (titleNode.GUID == guid)
                {
                    rs = true;
                    break;
                }

                addedWidth = titleRight;
                index++;
            }
            
            return rs;
        }

        private void SelectTitleProcSetTitle(string guid)
        {
            bool refresh = false;
            foreach (UserPanelTitleInfo titleInfo in TitleList)
            {
                if (titleInfo.GUID == guid)
                {
                    if (!titleInfo.Selected)
                    {
                        titleInfo.Selected = true;
                        refresh = true;
                    }
                }
                else
                {
                    titleInfo.Selected = false;
                }
            }

            if (refresh)
            {
                Draw();
            }
        }

        private void SelectTitleProcSetBody(string guid)
        {
            SuspendLayout();

            bool isAllHidden = true;
            for (int i = 0; i < Controls.Count; i++)
            {
                var panel = Controls[i] as UserPanel;
                if (panel != null)
                {
                    if (panel.GUID == guid)
                    {
                        panel.SendToBack();
                        panel.Visible = true;
                        panel.BringToFrontCustom();
                        panel.OnShown();
                        isAllHidden = false;
                    }
                    else
                    {
                        // None
                    }
                }
            }

            // When this is parent
            if (isAllHidden)
            {
                for (int i = Controls.Count - 1; i >= 0; i--)
                {
                    var panel = Controls[i] as UserPanel;
                    if (panel != null)
                    {
                        panel.Visible = false;
                    }
                }
                OnShown();
            }

            ResumeLayout(false);
        }

        public virtual void OnShown()
        {
            Shown?.Invoke(this, new EventArgs());
        }

        private void UserPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            BindChildrenEvents(e.Control);
        }

        private void BindChildrenEvents(Control target)
        {
            target.GotFocus += UserPanelOrChildren_GotFocus;
            target.LostFocus += UserPanelOrChildren_LostFocus;
            target.MouseDown += Control_MouseDown;
            target.LocationChanged += Control_LocationChanged;
            foreach (Control targetChild in target.Controls)
            {
                BindChildrenEvents(targetChild);
            }
        }

        private void UserPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            UnbindChildrenEvents(e.Control);
        }

        private void UnbindChildrenEvents(Control target)
        {
            target.GotFocus -= UserPanelOrChildren_GotFocus;
            target.LostFocus -= UserPanelOrChildren_LostFocus;
            target.MouseDown -= Control_MouseDown;
            target.LocationChanged -= Control_LocationChanged;
            foreach (Control targetChild in target.Controls)
            {
                UnbindChildrenEvents(targetChild);
            }
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            Control _this = sender as Control;
            // 아래 삭제?
            if ((_this.Dock == DockStyle.None || _this.Dock == DockStyle.Bottom) && _this.Top < Padding.Top)
            {
                //_this.Top = Padding.Top;
            }
            //Console.WriteLine(String.Format("{0} {1} {2} {3}", MethodBase.GetCurrentMethod().Name, _this.Name, _this.Dock, cnt++));
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            var parent = Parent as UserPanel;
            if (parent != null)
            {
                parent.BringToFrontCustom();
                parent.Draw();
            }
            else
            {
                var _this = sender as Control;
                if (!_this.CanFocus) // if (!_this.CanFocus || !_this.TabStop)
                {
                    Focus();
                }
                BringToFrontCustom();
                Draw();
            }
        }

        private void UserPanelOrChildren_GotFocus(object sender, EventArgs e)
        {
            if (this is UserPanel)
            {
                var parent = Parent as UserPanel;
                if (parent != null)
                {
                    parent.BringToFrontCustom();
                }
                else
                {
                    BringToFrontCustom();
                }
                DrawThisNParentUserPanel();
            }
            else
            {
                UserPanel parent = GetParentPanel(sender as Control);
                UserPanel parentParent = parent.Parent as UserPanel;
                if (parentParent != null)
                {
                    parentParent.BringToFrontCustom();
                }
                else
                {
                    parent.BringToFrontCustom();
                }
                DrawThisNParentUserPanel();
            }
        }

        private UserPanel GetParentPanel(Control control)
        {
            if (control.Parent != null && control.Parent is UserPanel)
            {
                return control.Parent as UserPanel;
            }
            else if (control.Parent != null)
            {
                return GetParentPanel(control.Parent);
            }
            else
            {
                return null;
            }
        }

        private void UserPanelOrChildren_LostFocus(object sender, EventArgs e)
        {
            // ContextMenu 코드 추가
            if (!ContainsFocus && !ContextMenu.Focused)
            {
                DrawThisNParentUserPanel();
            }
            //Console.WriteLine(String.Format("{0} {1} {2}", MethodBase.GetCurrentMethod().Name, Name, cnt++));
        }

        public void AddPanel(UserPanel panel, int sort = 0)
        {
            // Remove, Title Visable
            panel.Visible = false;
            panel.BlockDrawing = true;
            Parent.Controls.Remove(panel);
            panel.TitleVisable = false;

            // Add, Dock, Bring To Front
            panel.Dock = DockStyle.Fill;
            panel.Top = TitleHeight;
            panel.Left = Padding.Left;
            //panel.Width = Width - Padding.Horizontal;
            //panel.Height = Height - TitleHeight - Padding.Bottom;
            Controls.Add(panel);
            panel.BringToFrontCustom();
            panel.BlockDrawing = false;

            // Add Title List
            sort = Math.Max(Math.Min(TitleList.Count, sort), 0);
            PushTitleSort(sort);
            TitleList.Add(new UserPanelTitleInfo(panel.Title, sort, panel.GUID, true));
            SelectTitle(panel.GUID);

            if (!HasExitingPanel)
            {
                panel.BecomingParentPanel = this;
                panel.MouseAction.Enable = false;

                if (MouseButtons == MouseButtons.Left && Bounds.Contains(Parent.PointToClient(Cursor.Position)))
                {
                    var titleRectangle = GetSelectedTitleRectangle();
                    panel.BecomingParentPanel.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 0, titleRectangle.X + titleRectangle.Width / 2, titleRectangle.Y + titleRectangle.Height / 2, 0));
                }
            }
            else
            {
                RemoveExitingPanel();
            }

            panel.Visible = true;

            Focus();
            Draw();
        }

        public void RemoveExitingPanel()
        {
            ExitingPanel = null;
        }

        private int GetTitleSortToBeAdded(int x, int addedPanelTitleHalfWidth)
        {
            int rs = -1;

            var sortedTitles = from title in TitleList orderby title.Sort select title;
            int addedWidth = 0;
            int maxSort = 0;
            foreach (UserPanelTitleInfo titleNode in sortedTitles)
            {
                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                int nextAddedWidth = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                if (addedWidth <= x && x <= nextAddedWidth / 2)
                {
                    rs = titleNode.Sort;
                }
                else if (nextAddedWidth / 2 <= x && x <= nextAddedWidth)
                {
                    rs = titleNode.Sort + 1;
                    break;
                }
                addedWidth = nextAddedWidth;
                maxSort = titleNode.Sort;
            }

            if (rs == -1 && addedWidth <= x && x <= addedWidth + addedPanelTitleHalfWidth)
            {
                rs = maxSort + 1;
            }

            return rs;
        }

        private void SetTitleSort(string guid, int newSort)
        {
            var titleInfo = GetTitleInfo(guid);
            if (titleInfo.Sort != newSort)
            {
                if (newSort < titleInfo.Sort)
                {
                    PushTitleSort(newSort, titleInfo.Sort - 1);
                }
                else
                {
                    PullTitleSort(titleInfo.Sort + 1, newSort);
                }
                titleInfo.Sort = newSort;
            }
        }

        private void PushTitleSort(int startSort, int endSort = -1)
        {
            ReviseTitleSort(startSort, endSort, 1);
        }

        private void PullTitleSort(int startSort, int endSort = -1)
        {
            ReviseTitleSort(startSort, endSort, -1);
        }

        private void ReviseTitleSort(int startSort, int endSort, int step)
        {
            int addedWidth = 0;
            foreach (UserPanelTitleInfo titleNode in TitleList)
            {
                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                if (startSort <= titleNode.Sort && (endSort < 0 || (0 <= endSort && titleNode.Sort <= endSort)))
                {
                    titleNode.Sort += step;
                }
                addedWidth += textSize.Width + TitleWidthRevision + TitleDistance;
            }
        }

        public void ExitPanel()
        {
            if (GetSelectedTitleInfo().Own)
            {
                ExitPanelProcOwn();
            }
            else
            {
                ExitPanelProcChild();
            }
        }

        private void ExitPanelProcOwn(bool closeOldOwnPanel = false)
        {
            UserPanel newOwnPanel = new UserPanel();
            for (int i = 0; i < TitleList.Count; i++)
            {
                if (TitleList[i].Own == false)
                {
                    newOwnPanel = GetUserPanel(TitleList[i].GUID);
                    break;
                }
            }

            if (newOwnPanel != null)
            {
                // New Select Title Proc #1 Start
                int sort = GetTitleInfo(GUID).Sort;

                // Remove
                Controls.Remove(newOwnPanel);

                // ------------ New Own
                // Dock, Title Visible
                newOwnPanel.Dock = Dock;
                newOwnPanel.TitleVisable = true;

                // Add, Location, Size
                Parent.Controls.Add(newOwnPanel);
                newOwnPanel.Location = Location;
                newOwnPanel.Size = Size;

                // TitleList
                newOwnPanel.TitleList = TitleList.ToList();
                newOwnPanel.RemoveTitle(GUID);
                for (int i = 0; i < newOwnPanel.TitleList.Count; i++)
                {
                    if (newOwnPanel.TitleList[i].GUID == newOwnPanel.GUID)
                    {
                        newOwnPanel.TitleList[i].Own = true;
                    }
                }

                // This, New Own / Move Children
                for (int i = Controls.Count - 1; i >= 0; i--)
                {
                    UserPanel panel = Controls[i] as UserPanel;
                    if (panel != null)
                    {
                        Controls.Remove(Controls[i]);
                        newOwnPanel.Controls.Add(panel);
                    }
                }

                // Move ContextMenu
                ContextMenu.GotFocus -= UserPanelOrChildren_GotFocus;
                ContextMenu.LostFocus -= UserPanelOrChildren_LostFocus;
                ContextMenu.ItemClick -= ContextMenu_ItemClick;
                ContextMenu.ItemCloseClick -= ContextMenu_ItemCloseClick;
                newOwnPanel.SetContextMenuFromOtherPanel(ContextMenu);

                // Options
                newOwnPanel.UsingMaximize = UsingMaximize;
                newOwnPanel.UsingViewContextMenuButton = UsingViewContextMenuButton;
                newOwnPanel.UsingAwaysOnTopMenuButton = UsingAwaysOnTopMenuButton;
                newOwnPanel.UsingTitleSlider = UsingTitleSlider;
                newOwnPanel.TitleStartIndex = TitleStartIndex;

                // Etc
                newOwnPanel._Padding = Padding;

                // Visible
                newOwnPanel.Visible = true;

                // New Select Title Proc #2 End
                newOwnPanel.SelectTitle(sort);

                // ------------ This
                if (closeOldOwnPanel)
                {
                    //Dispose();
                    HIdePanel(newOwnPanel);
                }
                else
                {
                    // TitleList
                    TitleList = new List<UserPanelTitleInfo>() { new UserPanelTitleInfo(Title, 0, GUID, true, true) };
                    Focus();

                    // Set Position
                    Point wrapPoint = Parent.PointToClient(Cursor.Position);
                    Rectangle titleRectangle = GetTitleRectangle();
                    Left = wrapPoint.X - (titleRectangle.Left + (titleRectangle.Width / 2));
                    Top = wrapPoint.Y - (titleRectangle.Top + (titleRectangle.Height / 2));

                    Point thisPoint = PointToClient(Cursor.Position);
                    MouseAction.Enable = true;
                    OnMouseDown(new MouseEventArgs(MouseButtons.Left, 0, titleRectangle.Left + (titleRectangle.Width / 2), titleRectangle.Top + (titleRectangle.Height / 2), 0));

                    // Paint
                    Draw();
                }
            }
        }

        public void SetContextMenuFromOtherPanel(UserPanelContextMenu newContextMenu)
        {
            ContextMenu.Dispose();
            ContextMenu = newContextMenu;
            ContextMenu.GotFocus += UserPanelOrChildren_GotFocus;
            ContextMenu.LostFocus += UserPanelOrChildren_LostFocus;
            ContextMenu.ItemClick += ContextMenu_ItemClick;
            ContextMenu.ItemCloseClick += ContextMenu_ItemCloseClick;
        }

        private void ExitPanelProcChild()
        {
            var targetPanel = GetSelectedUserPanel();

            // Remove, Set Title Selected
            Controls.Remove(targetPanel);
            RemoveTitle(targetPanel.GUID);
            SetSelectedTitleWithChildrenIndex();

            // Dock, Title Visible
            targetPanel.Dock = DockStyle.None;
            targetPanel.TitleVisable = true;

            // Add, Bring To Front
            Parent.Controls.Add(targetPanel);
            targetPanel.BringToFrontCustom();

            // Paint
            Draw();

            // Set Position
            Point wrapPoint = Parent.PointToClient(Cursor.Position);
            Rectangle targetTitleRectangle = targetPanel.GetTitleRectangle();
            targetPanel.Left = wrapPoint.X - (targetTitleRectangle.Left + (targetTitleRectangle.Width / 2));
            targetPanel.Top = wrapPoint.Y - (targetTitleRectangle.Top + (targetTitleRectangle.Height / 2));

            // Unbind Events
            targetPanel.GotFocus -= UserPanelOrChildren_GotFocus;
            targetPanel.LostFocus -= UserPanelOrChildren_LostFocus;
            targetPanel.MouseDown -= Control_MouseDown;
            targetPanel.LocationChanged -= Control_LocationChanged;

            // Set Focus, MouseDown, Sort
            targetPanel.TitleList[0].Sort = 0;
            targetPanel.Focus();
            if (!targetPanel.HasBecomingParentPanel)
            {
                ExitingPanel = targetPanel;
                Point targetPanelPoint = targetPanel.PointToClient(Cursor.Position);
                targetPanel.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 0, targetPanelPoint.X, targetPanelPoint.Y, 0));
            }
            else
            {
                targetPanel.RemoveBecomingParentPanel();
                targetPanel.MouseAction.Enable = true;
            }
        }

        public void RemoveBecomingParentPanel()
        {
            BecomingParentPanel = null;
        }

        private void SetSelectedTitleWithChildrenIndex()
        {
            int minIndex = -1;
            string panelMinGUID = string.Empty;
            foreach (Control control in Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null && panel.Visible)
                {
                    int panelIndex = Controls.GetChildIndex(panel);
                    if (minIndex == -1 || panelIndex < minIndex)
                    {
                        minIndex = Controls.GetChildIndex(panel);
                        panelMinGUID = panel.GUID;
                    }
                }
            }

            if (panelMinGUID != "")
            {
                SelectTitle(panelMinGUID);
            }
            else
            {
                SelectTitle(GUID);
            }
        }

        private UserPanel GetSelectedUserPanel()
        {
            UserPanel rs = null;
            string guid = GetSelectedTitleInfo().GUID;

            for (int i = 0; i < Controls.Count; i++)
            {
                var panel = Controls[i] as UserPanel;
                if (panel != null)
                {
                    if (panel.GUID == guid)
                    {
                        rs = panel;
                        break;
                    }
                }
            }
            return rs;
        }

        private UserPanel GetUserPanel(string guid)
        {
            UserPanel rs = null;
            if (this.GUID == guid)
            {
                rs = this;
            }
            else
            {
                foreach (Control control in Controls)
                {
                    UserPanel panel = control as UserPanel;
                    if (panel != null && panel.GUID == guid)
                    {
                        rs = panel;
                        break;
                    }
                }
            }
            return rs;
        }

        public void RemoveTitle(string guid)
        {
            int removedTitleSort = -1;
            for (int i = 0; i < TitleList.Count; i++)
            {
                if (TitleList[i].GUID == guid)
                {
                    removedTitleSort = TitleList[i].Sort;
                    TitleList.Remove(TitleList[i]);
                    break;
                }
            }

            PullTitleSort(removedTitleSort);
        }

        private bool HasChildrenPanel()
        {
            bool rs = false;
            foreach (Control control in Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }

        public Rectangle GetTitleRectangle()
        {
            Rectangle titleRectangle = new Rectangle();
            if (TitleVisable)
            {
                var sortedTitles = from title in TitleList orderby title.Sort select title;
                int addedWidth = 0;
                foreach (UserPanelTitleInfo titleNode in sortedTitles)
                {
                    Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                    addedWidth += textSize.Width + TitleWidthRevision + TitleDistance;
                }
                titleRectangle = new Rectangle(0, 0, addedWidth, TitleHeight);
            }
            return titleRectangle;
        }

        public static string GetSaveInfo(Control parent)
        {
            List<UserPanelSaveInfo> saveInfoList = new List<UserPanelSaveInfo>();
            foreach (Control control in parent.Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    saveInfoList.Add(new UserPanelSaveInfo(panel));
                }
            }

            UserWrapPanelForUserPanel wrapPanel = parent as UserWrapPanelForUserPanel;
            if (wrapPanel != null)
            {
                foreach(UserPanel panel in wrapPanel.HiddenPanels)
                {
					saveInfoList.Add(new UserPanelSaveInfo(panel));
				}
            }

            return JsonConvert.SerializeObject(saveInfoList);
        }

        public static List<UserPanelSaveInfo> GetSaveInfoList(Control parent)
        {
            List<UserPanelSaveInfo> saveInfoList = new List<UserPanelSaveInfo>();
            foreach (Control control in parent.Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    saveInfoList.Add(new UserPanelSaveInfo(panel));
                }
            }

            UserWrapPanelForUserPanel wrapPanel = parent as UserWrapPanelForUserPanel;
            if (wrapPanel != null)
            {
                foreach (UserPanel panel in wrapPanel.HiddenPanels)
                {
                    saveInfoList.Add(new UserPanelSaveInfo(panel));
                }
            }

            return saveInfoList;
        }

        public static void SetSaveInfo(Control parent, string saveInfo)
        {
            parent.Controls.Clear();
            var saveInfoList = JsonConvert.DeserializeObject<List<UserPanelSaveInfo>>(saveInfo);
            foreach (UserPanelSaveInfo panelInfo in saveInfoList)
            {
                Type type = Type.GetType(panelInfo.TypeFullName);
                dynamic objType = Activator.CreateInstance(type);
                UserPanel panel = objType as UserPanel;
                panel.LoadPanelInfo(panelInfo);
                parent.Controls.Add(panel);
            }
        }

        public void SetNboundsNPosotionInfo(Size wrapPanelSize, Control parent)
        {
            _NBounds = Bounds;
            UserPanelPositionType positionInfo = GetPositionInfo(wrapPanelSize, this);
            List<UserPanel> siblingLeft = GetPositionInfoSiblingLeft(parent, this);
            List<UserPanel> siblingTop = GetPosotionInfoSiblingTop(parent, this);
            _PositionInfo = new UserPanelPositionInfo(positionInfo, siblingLeft, siblingTop);
        }

        private UserPanelPositionType GetPositionInfo(Size wrapPanelSize, UserPanel panel)
        {
            string positionInfoStr = "9";
            positionInfoStr += panel.Top == 0 ? "1" : "0";
            positionInfoStr += panel.Right == wrapPanelSize.Width ? "1" : "0";
            positionInfoStr += panel.Bottom == wrapPanelSize.Height ? "1" : "0";
            positionInfoStr += panel.Left == 0 ? "1" : "0";
            return (UserPanelPositionType)Convert.ToInt32(positionInfoStr);
        }

        private List<UserPanel> GetPositionInfoSiblingLeft(Control parent, UserPanel panel)
        {
            List<UserPanel> leftSiblingPanelList = new List<UserPanel>();

            foreach (Control control in parent.Controls)
            {
                UserPanel leftSiblingPanel = control as UserPanel;

                if (leftSiblingPanel != null && !leftSiblingPanel.Equals(panel))
                {
                    if
                    (
                        (
                            leftSiblingPanel.Right == panel.Left
                        )
                        &&
                        (
                            (panel.Top <= leftSiblingPanel.Top && leftSiblingPanel.Top <= panel.Bottom)
                            ||
                            (panel.Top <= leftSiblingPanel.Bottom && leftSiblingPanel.Bottom <= panel.Bottom)
                            ||
                            (leftSiblingPanel.Top <= panel.Top && panel.Bottom <= leftSiblingPanel.Bottom)
                        )
                    )
                    {
                        leftSiblingPanelList.Add(leftSiblingPanel);
                    }
                }
            }

            return leftSiblingPanelList;
        }

        private List<UserPanel> GetPosotionInfoSiblingTop(Control parent, UserPanel panel)
        {
            List<UserPanel> leftSiblingPanelList = new List<UserPanel>();

            foreach (Control control in parent.Controls)
            {
                UserPanel leftSiblingPanel = control as UserPanel;

                if (leftSiblingPanel != null && !leftSiblingPanel.Equals(panel))
                {
                    if
                    (
                        (
                            leftSiblingPanel.Bottom == panel.Top
                        )
                        &&
                        (
                            (panel.Left <= leftSiblingPanel.Left && leftSiblingPanel.Left <= panel.Right)
                            ||
                            (panel.Left <= leftSiblingPanel.Right && leftSiblingPanel.Right <= panel.Right)
                            ||
                            (leftSiblingPanel.Left <= panel.Left && panel.Right <= leftSiblingPanel.Right)
                        )
                    )
                    {
                        leftSiblingPanelList.Add(leftSiblingPanel);
                    }
                }
            }

            return leftSiblingPanelList;
        }

        [Obsolete]
        public bool HasSiblingLeftZero()
        {
            bool rs = false;
            if (PositionInfo != null)
            {
                rs = HasSiblingLeftZeroProc(PositionInfo);
            }
            return rs;
        }

        [Obsolete]
        private bool HasSiblingLeftZeroProc(UserPanelPositionInfo positionInfo)
        {
            bool rs = false;
            string positionStr = ((int)positionInfo.Position).ToString();
            if (positionStr[4] == '1')
            {
                rs = true;
            }
            else
            {
                for (int i = 0; i < positionInfo.SiblingLeft.Count; i++)
                {
                    rs = HasSiblingLeftZeroProc(positionInfo.SiblingLeft[i].PositionInfo);
                    if (rs)
                    {
                        break;
                    }
                }
            }
            return rs;
        }

        [Obsolete]
        public bool HasSiblingLeftBroken()
        {
            bool rs = false;
            if (PositionInfo != null)
            {
                rs = HasSiblingLeftBrokenProc(this);
            }
            return rs;
        }

        [Obsolete]
        private bool HasSiblingLeftBrokenProc(UserPanel panel)
        {
            bool rs = false;
            for (int i = 0; i < panel.PositionInfo.SiblingLeft.Count; i++)
            {
                if (panel.Left != panel.PositionInfo.SiblingLeft[i].Right)
                {
                    rs = true;
                    break;
                }

                rs = HasSiblingLeftBrokenProc(panel.PositionInfo.SiblingLeft[i]);
                if (rs)
                {
                    break;
                }
            }
            return rs;
        }

        public void BringToFrontCustom()
        {
            if (Parent is UserPanel)
            {
                BringToFront();
            }
            else
            {
                if (AwaysOnTop)
                {
                    BringToFront();
                }
                else
                {
                    //var parent = Parent as UserWrapPanelForUserPanel;
                    //if (parent != null)
                    //{
                    Control parent = Parent as Control;
                    int maxAwaysOnTopIndex = -1;
                    foreach (Control control in parent.Controls)
                    {
                        UserPanel panel = control as UserPanel;
                        if (panel != null && !panel.Equals(this))
                        {
                            if (panel.AwaysOnTop && (maxAwaysOnTopIndex < 0 || maxAwaysOnTopIndex < parent.Controls.GetChildIndex(panel)))
                            {
                                maxAwaysOnTopIndex = parent.Controls.GetChildIndex(panel);
                            }
                        }
                    }
                    parent.Controls.SetChildIndex(this, maxAwaysOnTopIndex + 1);
                    //}
                    //else
                    //{
                    //    if (Parent != null)
                    //    {
                    //        BringToFront();
                    //    }
                    //}
                }
            }
        }

        public string ParentForShowNHideGUID;

        public UserWrapPanelForUserPanel WrapPanel = null;

		public void HIdePanel(UserPanel parentForShowNHide = null)
		{
            if (WrapPanel == null)
            {
                WrapPanel = GetWrapPanel();
            }

            if (WrapPanel != null)
            {
                WrapPanel.AddHiddenPanel(this);

                string guid = parentForShowNHide?.GUID;
                if (guid == null)
                {
                    UserPanel panel = Parent as UserPanel;
                    guid = panel != null ? panel.GUID : "";
                }

                ParentForShowNHideGUID = guid;
                Parent.Controls.Remove(this);
            }
            else
            {
                Dispose();
            }
		}

		public void ShowPanel()
		{
			// Focus
			if (Parent != null)
			{
				UserPanel parent = Parent as UserPanel;
				if (parent != null)
				{
					parent.BringToFront();
					parent.Focus();
					parent.SelectTitle(this.GUID);
				}
				else
				{
					BringToFront();
					Focus();
					if (HasChildrenPanel())
					{
						SelectTitle(this.GUID);
					}
				}
			}
			// Add Control
			else
			{
                Control control = ParentForShowNHideGUID == ""
                    ? WrapPanel
					: (Control)WrapPanel.GetUserPanel(ParentForShowNHideGUID);
				UserPanel parent = control as UserPanel;
                Control parentForShowNHide = control;
				while (true) {
					// UserPanel
					if (parent != null)
					{
						// parent.Parent is displayed
						if (parent.Parent != null)
						{
							UserPanel parentParent = parent.Parent as UserPanel;
							// Parent is Level 2
							if (parentParent != null)
							{
								parentParent.AddPanel(this);
                                BringToFront();
								Focus();
                                break;
							}
							// Parent is Level 1
							else
							{
								parent.AddPanel(this);
								BringToFront();
								Focus();
                                break;
							}
						}
						// parent.Parent is hidden
						else
						{
							Control _control = parent.ParentForShowNHideGUID == ""
					            ? WrapPanel
					            : (Control)WrapPanel.GetUserPanel(parent.ParentForShowNHideGUID);
							parent = _control as UserPanel;
							parentForShowNHide = _control;
							continue;
						}
					}
					// WrapPanel
					else
					{
						parentForShowNHide.Controls.Add(this);
						BringToFront();
						Focus();
						break;
					}
				}

                WrapPanel.RemoveHiddenPanel(this);
                ParentForShowNHideGUID = null;
			}
		}

        public UserWrapPanelForUserPanel GetWrapPanel() {
            UserWrapPanelForUserPanel rs = null;

            if (Parent != null)
            {
                if (Parent.GetType() == typeof(UserPanel))
                {
                    if (Parent.Parent != null && Parent.Parent.GetType() == typeof(UserWrapPanelForUserPanel))
                    {
                        rs = Parent.Parent as UserWrapPanelForUserPanel;
                    }
                }
                else if (Parent.GetType() == typeof(UserWrapPanelForUserPanel))
                {
                    rs = Parent as UserWrapPanelForUserPanel;
                }
            }

            //Control parent = Parent;
            //while(true)
            //{
            //    if (parent == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        if (parent.GetType() == typeof(UserWrapPanelForUserPanel))
            //        {
            //            rs = parent as UserWrapPanelForUserPanel;
            //            break;
            //        }
            //        else
            //        {
            //            parent = parent.Parent;
            //        }
            //    }
            //}

            return rs;
		}

        public UserPanel GetChildPanel(string guid)
        {
            UserPanel rs = null;
            foreach(Control control in Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null && panel.GUID == guid)
                {
                    rs = panel;
                    break;
                }
            }
            return rs;
        }
	}
}
