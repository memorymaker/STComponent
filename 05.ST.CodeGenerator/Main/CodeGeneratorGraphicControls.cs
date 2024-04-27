using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator
    {
        private List<GraphicButtonInfo> GraphicButtonInfoList;
        private Font GraphicButtonFont = new Font("맑은 고딕", 8f);

        public event EventHandler ExecuteButton_Click;
        public event EventHandler HistoryButton_Click;

        private bool NewButtonVisible
        {
            get => GetGraphicButtonInfo("New").Visible;
            set
            {
                var info = GetGraphicButtonInfo("New");
                info.Visible = value;
                GraphicControlsSetBounds();
            } 
        }

        private bool LoadButtonVisible
        {
            get => GetGraphicButtonInfo("Load").Visible;
            set
            {
                var info = GetGraphicButtonInfo("Load");
                info.Visible = value;
                GraphicControlsSetBounds();
            }
        }

        private bool SaveButtonVisible
        {
            get => GetGraphicButtonInfo("Save").Visible;
            set
            {
                var info = GetGraphicButtonInfo("Save");
                info.Visible = value;
                GraphicControlsSetBounds();
            }
        }

        private bool GenerateButtonVisible
        {
            get => GetGraphicButtonInfo("Generate").Visible;
            set
            {
                var info = GetGraphicButtonInfo("Generate");
                info.Visible = value;
                GraphicControlsSetBounds();
            }
        }

        private bool GenerateAllButtonVisible
        {
            get => GetGraphicButtonInfo("GenerateAll").Visible;
            set
            {
                var info = GetGraphicButtonInfo("GenerateAll");
                info.Visible = value;
                GraphicControlsSetBounds();
            }
        }

        private bool RenameButtonVisible
        {
            get => GetGraphicButtonInfo("Rename").Visible;
            set
            {
                var info = GetGraphicButtonInfo("Rename");
                info.Visible = value;
                GraphicControlsSetBounds();
            }
        }

        private GraphicButtonInfo GetGraphicButtonInfo(string name)
        {
            GraphicButtonInfo rs = null;
            foreach (var info in GraphicButtonInfoList)
            {
                if (info.Button.Name == name.Trim())
                {
                    rs = info;
                    break;
                }
            }
            return rs;
        }

        private void LoadGraphicControls()
        {
            GraphicButtonInfoList = new List<GraphicButtonInfo>()
            {
                  new GraphicButtonInfo(new GraphicControl(this, "New"        ), CodeGeneratorResource.Icon_File, true)
                , new GraphicButtonInfo(new GraphicControl(this, "Load"       ), CodeGeneratorResource.Icon_File, false)
                , new GraphicButtonInfo(new GraphicControl(this, "Save"       ), CodeGeneratorResource.Icon_File, false)
                , new GraphicButtonInfo(new GraphicControl(this, "Generate"   ), CodeGeneratorResource.Icon_File, true)
                , new GraphicButtonInfo(new GraphicControl(this, "GenerateAll"), CodeGeneratorResource.Icon_File, true)
                , new GraphicButtonInfo(new GraphicControl(this, "Rename"     ), CodeGeneratorResource.Icon_File, true)
            };
            
            for (int i = 0; i < GraphicButtonInfoList.Count; i++)
            {
                GraphicButtonInfo info = GraphicButtonInfoList[i];
                switch(info.Button.Name)
                {
                    case "New":
                        info.Button.Click += NewButton_Click;
                        break;
                    case "Load":info.Button.Click += LoadButton_Click;
                        break;
                    case "Save":
                        info.Button.Click += SaveButton_Click;
                        break;
                    case "Generate":
                        info.Button.Click += GenerateButton_Click;
                        break;
                    case "GenerateAll":
                        info.Button.Click += GenerateAllButton_Click;
                        break;
                    case "Rename":
                        info.Button.Click += RenameButton_Click;
                        break;
                }
            }

            GraphicControlsSetBounds();
        }

        private void GraphicControlsSetBounds()
        {
            int leftAppend = 0;
            using (var g = CreateGraphics())
            {
                for (int i = 0; i < GraphicButtonInfoList.Count; i++)
                {
                    GraphicButtonInfo info = GraphicButtonInfoList[i];
                    int _widthRevision = 12;
                    int width = g.MeasureString(info.Button.Name, GraphicButtonFont).Width.ToInt()
                        + info.Image.Width
                        + _widthRevision;

                    info.Button
                        .SetArea(new Rectangle(3 + leftAppend, 4, width, 23))
                        .SetDrawType(GraphicControl.DrawTypeEnum.ImageTextLeftRight)
                        .SetDrawFont(GraphicButtonFont)
                        .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo()
                        {
                              DrawBackColor = MenuBackColor
                            , DrawImage = CodeGeneratorResource.Icon_File
                            , DrawText = info.Button.Name
                            , DrawTextColor = Color.FromArgb(60, 60, 60)
                        })
                        .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo()
                        {
                              DrawBackColor = MenuBackColor.GetColor(0.05f)
                            , DrawBorderColor = Color.FromArgb(93, 107, 153)
                        })
                        .SetDrawInfo(GraphicControl.StateType.MouseDown, new GraphicControl.DrawInfo()
                        {
                              DrawBackColor = MenuBackColor.GetColor(0.1f)
                            , DrawBorderColor = Color.FromArgb(93, 107, 153)
                        });

                    if (info.Visible)
                    {
                        leftAppend += width;
                    }

                    info.Button.Enabled = info.Visible;
                }
            }
        }

        private void NewButton_Click(object sender, MouseEventArgs e)
        {
            this.BeginControlUpdate();
            AddNewTab();
            this.EndControlUpdate();
        }

        private void LoadButton_Click(object sender, MouseEventArgs e)
        {
            ModalMessageBox.Show("L");
        }

        private void SaveButton_Click(object sender, MouseEventArgs e)
        {
            ModalMessageBox.Show("S");
        }

        private void GenerateButton_Click(object sender, MouseEventArgs e)
        {
            Tab tab = GetActiveTab();
            if (tab != null)
            {
                tab.Generate();
            }
        }

        private void GenerateAllButton_Click(object sender, MouseEventArgs e)
        {
            GeneratorAll();
        }

        private void RenameButton_Click(object sender, MouseEventArgs e)
        {
            UserPanel panel = GetMainPanel();
            if (panel != null)
            {
                panel.ShowTitleEditor();
            }
        }

        #region Function
        public Tab AddNewTab()
        {
            return AddTab(AddNewTab_GetNewTabTitle(), AddNewTab_GetNewTabGUID());
        }

        private Tab AddTab(string title, string guid, string tamplateEditorText = null, string resultEditorText = null, string templateStyle = null, bool visible = true)
        {
            Tab parentTab = null;
            foreach (Control control in MainSplit.Panel2.Controls)
            {
                Tab tab = control as Tab;
                if (tab != null)
                {
                    parentTab = tab;
                }
            }

            Tab newTab = new Tab(title, guid);
            if (parentTab != null)
            {
                parentTab.AddPanel(newTab, parentTab.Controls.Count);
                newTab.SplitterDistance = newTab.Height / 2 - 8;
            }
            else
            {
                newTab.Visible = false;
                MainSplit.Panel2.Controls.Add(newTab);
                newTab.Dock = DockStyle.Fill;
                newTab.Top = MenuHeight;
                newTab.Width = Width;
                newTab.Height = Height - MenuHeight;
                newTab.SplitterDistance = newTab.Height / 2 - 20;
                newTab.SetEditorStyleList(EditorStyleList);

                newTab.Visible = visible;

                newTab.Focus();
            }

            newTab.TemplateEditor.Text = tamplateEditorText ?? string.Empty;
            newTab.ResultEditor.Text = resultEditorText ?? string.Empty;

            newTab.Shown += AddNewTab_Shown;
            //newTab.ExecuteButton_Click += Tab_ExecuteButton_Click;
            //newTab.HistoryButton_Click += NewTab_HistoryButton_Click;

            var editorStyle = GetEditorStyle(
                templateStyle != null
                    ? templateStyle
                    : DefqultEditorStyleName
            );
            if (editorStyle != null)
            {
                newTab.SetEditorStyle(editorStyle);
                SetButtonStyleText(editorStyle.Name);
            }
            
            return newTab;
        }

        private void AddNewTab_Shown(object sender, EventArgs e)
        {
            Tab tab = sender as Tab;
            SetButtonStyleText(tab.EditStyleName);
            Draw();
        }

        private string AddNewTab_GetNewTabTitle()
        {
            string titlePrefix = "NewTab";

            int maxNumber = 0;
            foreach(Control control in MainSplit.Panel2.Controls)
            {
                Tab tab = control as Tab;
                if (tab != null)
                {
                    foreach(var titleInfo in tab.TitleList)
                    {
                        if (titlePrefix.Length < titleInfo.Title.Length)
                        {
                            string numberNodeString = titleInfo.Title.Substring(titlePrefix.Length);
                            int numberNode = 0;
                            if (int.TryParse(numberNodeString, out numberNode))
                            {
                                if (maxNumber < numberNode)
                                {
                                    maxNumber = numberNode;
                                }
                            }
                        }
                    }
                }
            }

            string rs = $"{titlePrefix}{++maxNumber}";
            return rs;
        }

        private string AddNewTab_GetNewTabGUID()
        {
            int newGuid = BoundMaximumSEQ;
            foreach(Control control in MainSplit.Panel2.Controls)
            {
                Tab tab = control as Tab;
                if (tab != null)
                {
                    foreach(var titleInfo in tab.TitleList)
                    {
                        string guid = titleInfo.GUID;
                        int guidNumber = 0;
                        if (int.TryParse(guid, out guidNumber))
                        {
                            if (newGuid < guidNumber)
                            {
                                newGuid = guidNumber;
                            }
                        }
                    }
                }
            }

            string rs = $"{++newGuid}";
            return rs;
        }

        private void Tab_ExecuteButton_Click(object sender, EventArgs e)
        {
            ExecuteButton_Click?.Invoke(sender, e);
        }

        private void NewTab_HistoryButton_Click(object sender, EventArgs e)
        {
            HistoryButton_Click?.Invoke(sender, e);
        }

        private UserPanel GetMainPanel()
        {
            UserPanel rs = null;
            foreach(Control control in MainSplit.Panel2.Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    rs = panel;
                }
            }
            return rs;
        }

        private Tab GetActiveTab()
        {
            Tab rsTab = null;
            foreach(Control control in MainSplit.Panel2.Controls)
            {
                rsTab = control as Tab;
                if (rsTab != null)
                {
                    break;
                }
            }

            if (rsTab != null)
            {
                var selectedTitleInfo = (from titleInfo in rsTab.TitleList where titleInfo.Selected == true select titleInfo).ToList()[0];
                if (selectedTitleInfo.GUID != rsTab.GUID)
                {
                    foreach(Control control in rsTab.Controls)
                    {
                        Tab childTab = control as Tab;
                        if (childTab != null && childTab.GUID == selectedTitleInfo.GUID)
                        {
                            rsTab = childTab;
                        }
                    }
                }
            }

            return rsTab;
        }

        private List<Tab> GetAllTabs()
        {
            List<Tab> rsTabs = null;
            foreach (Control control in MainSplit.Panel2.Controls)
            {
                Tab tab = control as Tab;
                if (tab != null)
                {
                    rsTabs = new List<Tab>()
                    {
                        tab
                    };
                    break;
                }
            }

            if (rsTabs != null)
            {
                foreach (Control control in rsTabs[0].Controls)
                {
                    Tab childTab = control as Tab;
                    if (childTab != null)
                    {
                        rsTabs.Add(childTab);
                    }
                }
            }

            return rsTabs;
        }

        public void GeneratorAll()
        {
            List<Tab> tabs = GetAllTabs();
            if (tabs != null)
            {
                foreach (Tab tab in tabs)
                {
                    tab.Generate();
                }
            }
        }
        #endregion

        private class GraphicButtonInfo
        {
            public GraphicControl Button;
            public Bitmap Image;
            public bool Visible;

            public GraphicButtonInfo(GraphicControl _Button, Bitmap _Image, bool _Visible)
            {
                Button = _Button;
                Image = _Image;
                Visible = _Visible;
            }
        }
    }
}
