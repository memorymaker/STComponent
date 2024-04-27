using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static ST.Core.MainXML;

namespace ST.Main
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        public event FormClosingEventHandler UserFormClosing;

        // System Options
        private Size MainDefaultSize = new Size(1400, 800);
        private Size MainMinimumSize = new Size(400, 200);
        private string FormClosingMessage = "종료하시겠습니까?";
        private string FormClosingTitle = "프로그램 종료";

        // User Options
        public string CloseComfirmMessage = string.Empty;
        public string CloseComfirmCaption = string.Empty;

		// Ref
		private ToolStripMenuItem SelectedMenuItem;
        private Dictionary<ToolStripMenuItem, Control> MenuItemLink = new Dictionary<ToolStripMenuItem, Control>();

        // Control
        private MainMenuStrip MainMenu;

        new public bool KeyPreview
        {
            get
            {
                return base.KeyPreview;
            }
            set
            {
                base.KeyPreview = value;
            }
        }

        public MainForm()
        {
            
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            Instance = this;
            SetThis();
            SetMenu();
        }

        private void SetThis()
        {
            // Default
            this.StartPosition = FormStartPosition.Manual;

            // Layout
            this.MinimumSize = MainMinimumSize;

            // Size
            object width = MainXML.GetValue("MainWidth")?.Value;
            object height = MainXML.GetValue("MainHeight")?.Value;
            this.Size = new Size(width == null ? MainDefaultSize.Width : Math.Max(width.ToInt(), MinimumSize.Width)
                               , height == null ? MainDefaultSize.Height : Math.Max(height.ToInt(), MinimumSize.Height));

            // Location
            object left = MainXML.GetValue("MainLeft")?.Value;
            object top = MainXML.GetValue("MainTop")?.Value;
            if (left != null && (int)left > 0 && top != null && (int)top > 0)
            {
                this.Location = new Point((int)left, (int)top);
            }
            else
            {
                var mainScreenBounds = Screen.PrimaryScreen.Bounds;
                this.Location = new Point(
                    mainScreenBounds.X + mainScreenBounds.Width / 2 - this.Width / 2
                    , mainScreenBounds.Y + mainScreenBounds.Height / 2 - this.Height / 2
                );
            }

            // Events
            this.FormClosing += Main_FormClosing;
            this.FormClosed += Main_FormClosed;

            // panWrap
            PanelWarp.Dock = DockStyle.Fill;
            PanelWarp.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UserFormClosing != null)
            {
                UserFormClosing.Invoke(sender, e);
            }
            else
            {
                if (ModalMessageBox.Show(FormClosingMessage, FormClosingTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainXML.SetValue("MainWidth", Width);
            MainXML.SetValue("MainHeight", Height);
            MainXML.SetValue("MainLeft", Left);
            MainXML.SetValue("MainTop", Top);

            foreach(KeyValuePair<ToolStripMenuItem, Control> pair in MenuItemLink) {
                pair.Value.Dispose();
            }
        }

        private void SetMenu()
        {   
            MainMenu = new MainMenuStrip(this);

            // Set doc
            ToolStripMenuItem defaultMenuItem = null;
            var menuList = MainXML.GetMenuList();
            foreach(MainXML.MenuItem item in menuList)
            {
                // Add Menu Item
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Name = "menuId" + item.ID;
                // Todo : 추후 HotKey 적용?
                //menuItem.Text = hotkeyIndex != null
                //    ? name.Insert(int.Parse(hotkeyIndex), "&")
                //    : name;
                menuItem.Text = item.Name;
                MainMenu.Items.Add(menuItem);

                // Get instanceControl
                string _backslash = Application.StartupPath[Application.StartupPath.Length - 1] == '\\' ? "" : "\\";
                string _dllPath = Application.StartupPath + _backslash + item.DLL;
                Assembly _assembly = Assembly.LoadFile(_dllPath);
                object _instanceObject = _assembly.CreateInstance(item.Class);
                Control instanceControl = _instanceObject as Control;

                // Add Control
                instanceControl.Visible = false;
                this.PanelWarp.Controls.Add(instanceControl);
                instanceControl.Dock = DockStyle.Fill;
                instanceControl.Tag = item;

                // Add MenuItemLink
                MenuItemLink.Add(menuItem, instanceControl);

                // Item Events
                menuItem.Click += Item_Click;

                // Set Default
                if (item.Default == "true")
                {
                    defaultMenuItem = menuItem;
                }
            }

            if (defaultMenuItem != null)
            {
                defaultMenuItem.PerformClick();
            }

            MainMenu.Refresh();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            SelectedMenuItem = sender as ToolStripMenuItem;
            if (MenuItemLink.ContainsKey(SelectedMenuItem))
            {
                MenuItemLink[SelectedMenuItem].Visible = true;
                MenuItemLink[SelectedMenuItem].BringToFront();
                this.Text = ((MainXML.MenuItem)MenuItemLink[SelectedMenuItem].Tag).Name;
            }
        }
    }
}
