using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static ST.CodeGenerator.TemplateProcessor;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator : Panel
    {
        #region Class, Value
        // Options
        private Color MenuBackColor = Color.FromArgb(204, 213, 240);
        private int MenuHeight = 30;

        // Ref
        private int BoundMaximumSEQ = -1;
        private Dictionary<string, string> CommonVariables = new Dictionary<string, string>();
        private string CommonVariablesAbstact = string.Empty;

        // Class
        TemplateProcessor Processor = new TemplateProcessor();
        #endregion

        #region Propertise
        new public Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
            }
        }

        public Dictionary<string, DataTable> NodeData { get; set; } = new Dictionary<string, DataTable>();

        public DataTable RelationData { get; set; }

        new public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                this.BeginControlUpdate();

                _Enabled = value;
                foreach (Control control in MainSplit.Panel2.Controls)
                {
                    var tab = control as Tab;
                    if (tab != null)
                    {
                        tab.Enabled = _Enabled;
                    }
                }
                CommonVariablesEditor.Enabled = _Enabled;
                Draw(CreateGraphics());

                this.EndControlUpdate();
            }
        }
        private bool _Enabled = true;

        public bool ReadOnly { get; set; } = false;
        #endregion

        #region Load
        public CodeGenerator()
        {
            LoadThis();
            LoadControls();
            LoadGraphicControls();
            LoadDraw();
            LoadInput();
            LoadEditorStyle();
        }

        private void LoadThis()
        {
            base.Padding = new Padding(0, MenuHeight, 0, 0);

            SizeChanged += CodeGenerator_SizeChanged;
        }

        private void CodeGenerator_SizeChanged(object sender, EventArgs e)
        {
            Draw();
        }
        #endregion

        #region Function
        public List<TabModel> GetData()
        {
            List<TabModel> data = new List<TabModel>();

            // Tabs
            Tab parentTab = GetParentTab();
            if (parentTab != null)
            {
                var titleInfoList = from info in parentTab.TitleList
                                    orderby info.Sort
                                    select info;
                foreach(var titleInfo in titleInfoList)
                {
                    Tab targetTab;
                    if (parentTab.GUID == titleInfo.GUID)
                    {
                        targetTab = parentTab;
                    }
                    else
                    {
                        targetTab = parentTab.GetChildPanel(titleInfo.GUID) as Tab;
                    }

                    TabModel tabModel = new TabModel()
                    {
                        TEMPLATE_SEQ = Convert.ToInt32(titleInfo.GUID),
                        TEMPLATE_TITLE = titleInfo.Title,
                        TEMPLATE_CONTENT = targetTab.TemplateEditor.Text,
                        TEMPLATE_RESULT = targetTab.ResultEditor.Text,
                        TEMPLATE_STYLE = targetTab.EditStyleName,
                        TEMPLATE_SORT = titleInfo.Sort,
                        TEMPLATE_NOTE = string.Empty,
                        TEMPLATE_SELECTED = titleInfo.Selected
                    };
                    data.Add(tabModel);
                }
            }

            // Common Variables
            data.Insert(0, new TabModel()
            {
                TEMPLATE_SEQ = -1,
                TEMPLATE_TITLE = null,
                TEMPLATE_CONTENT = CommonVariablesEditor.Text,
                TEMPLATE_RESULT = null,
                TEMPLATE_STYLE = null,
                TEMPLATE_SORT = 0,
                TEMPLATE_NOTE = null,
                TEMPLATE_SELECTED = false
            });

            return data;
        }

        public DataTable GetDataTable()
        {
            return GetData().ToDataTable();
        }

        public void SetData(List<TabModel> data)
        {
            this.BeginControlUpdate();
            Clear();
            
            Tab parentTab = null;
            string selectedTabGuid = null;
            var sortedData = (from tab in data
                              orderby tab.TEMPLATE_SORT
                              select tab).ToList();
            for(int i = 0; i < sortedData.Count; i++)
            {
                TabModel node = sortedData[i];

                // CommonVariables
                if (node.TEMPLATE_SEQ == -1)
                {
                    CommonVariablesEditor.Text = node.TEMPLATE_CONTENT;
                }
                // Tabs
                else
                {
                    Tab newTab = AddTab(node.TEMPLATE_TITLE
                                      , node.TEMPLATE_SEQ.ToString()
                                      , node.TEMPLATE_CONTENT
                                      , node.TEMPLATE_RESULT
                                      , node.TEMPLATE_STYLE
                                      , i != 0);

                    if (node.TEMPLATE_SELECTED)
                    {
                        selectedTabGuid = node.TEMPLATE_SEQ.ToString();
                    }

                    if (parentTab == null)
                    {
                        parentTab = newTab;
                    }
                }
            }

            if (selectedTabGuid != null)
            {
                GetParentTab().SelectTitle(selectedTabGuid);
            }

            parentTab.Visible = true;
            this.EndControlUpdate(false);
        }

        public void SetDataTable(DataTable data)
        {
            this.BeginControlUpdate();
            Clear();

            Tab parentTab = null;
            string selectedTabGuid = null;
            int boundMaximumSeq = -1;
            data.DefaultView.Sort = TEMPLATE.TEMPLATE_SORT;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                // CommonVariables
                if (row[TEMPLATE.TEMPLATE_SEQ].ToInt() == -1)
                {
                    CommonVariablesEditor.Text = row[TEMPLATE.TEMPLATE_CONTENT].ToString();
                }
                // Tabs
                else
                {
                    Tab newTab = AddTab(row[TEMPLATE.TEMPLATE_TITLE].ToString()
                                      , row[TEMPLATE.TEMPLATE_SEQ].ToString()
                                      , row[TEMPLATE.TEMPLATE_CONTENT].ToString()
                                      , row[TEMPLATE.TEMPLATE_RESULT].ToString()
                                      , row[TEMPLATE.TEMPLATE_STYLE].ToString()
                                      , i != 0);

                    if (Convert.ToBoolean(row[TEMPLATE.TEMPLATE_SELECTED]))
                    {
                        selectedTabGuid = row[TEMPLATE.TEMPLATE_SEQ].ToString();
                    }

                    if (parentTab == null)
                    {
                        parentTab = newTab;
                    }

                    if (boundMaximumSeq < Convert.ToInt32(row[TEMPLATE.TEMPLATE_SEQ]))
                    {
                        boundMaximumSeq = Convert.ToInt32(row[TEMPLATE.TEMPLATE_SEQ]);
                    }
                }
            }

            if (selectedTabGuid != null)
            {
                GetParentTab().SelectTitle(selectedTabGuid);
            }

            if (parentTab != null)
            {
                parentTab.Visible = true;
            }
            this.EndControlUpdate();
        }

        public void Clear()
        {
            CommonVariablesEditor.Text = string.Empty;

            for (int i = MainSplit.Panel2.Controls.Count - 1; 0 <= i; i--)
            {
                Tab tab = MainSplit.Panel2.Controls[i] as Tab;
                //tab.BeginControlUpdate();
                MainSplit.Panel2.Controls.Remove(tab);

                //Controls.Clear();
                //tab.Dispose();
            }
        }

        private Tab GetParentTab()
        {
            Tab rs = null;
            foreach (Control control in MainSplit.Panel2.Controls)
            {
                Tab parentTab = control as Tab;
                if (parentTab != null)
                {
                    rs = parentTab;
                    break;
                }
            }
            return rs;
        }

        public Dictionary<string, string> GetCommonVariables()
        {
            List<TemplateProcessorException> exceptionList;
            Dictionary<string, string> rs = Processor.GetDictionaryValuesWithoutCommand(CommonVariablesEditor.Text, out exceptionList);
            CommonVariables = rs;
            CommonVariablesAbstact = string.Join(", ", rs.Keys.ToArray()).Trim();

            // Set errorData
            if (exceptionList.Count > 0)
            {
                DataTable errorData = new DataTable();
                errorData.AddColumns("{s}DESCRIPTON {s}LINE {s}COLUMN");
                foreach (TemplateProcessorException exceptionNode in exceptionList)
                {
                    errorData.Rows.Add(new object[] { exceptionNode.Message, $"{exceptionNode.LineNumber}", $"{exceptionNode.CharNumber}" });
                }

                // Bind ErrorList
                ErrorList.SuspendLayout();
                ErrorList.Bind(errorData);
                foreach (UserListViewItem item in ErrorList.Items)
                {
                    item.ForeColor = Color.Red;
                    item.BackColor = Color.FromArgb(249, 249, 249);
                }
                ErrorList.ResumeLayout();
                ErrorList.Draw();

                if (!ErrorListWrapPanel.Visible)
                {
                    MainSplit.Panel1.BeginControlUpdate();
                    ErrorListWrapPanel.Visible = true;
                    if (MainSplit.SplitterDistance < MainSplitPanel1WithErrorListMinSize)
                    {
                        MainSplit.SplitterDistance = MainSplitPanel1WithErrorListMinSize;
                    }
                    CommonVariablesEditor.SetScrollToCursor();
                    MainSplit.Panel1.EndControlUpdate();
                }
            }
            else
            {
                if (ErrorListWrapPanel.Visible)
                {
                    MainSplit.Panel1.BeginControlUpdate();
                    ErrorListWrapPanel.Visible = false;
                    ErrorList.Clear();
                    MainSplit.Panel1.EndControlUpdate();
                }
            }

            return rs;
        }
        #endregion
    }
}
