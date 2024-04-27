using Newtonsoft.Json;
using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace ST.CodeGenerator
{

    public partial class CodeGenerator
    {
        private const string StyleFolderName = "Styles";
        private List<EditorStyle> EditorStyleList = new List<EditorStyle>();

        private GraphicControl ButtonStyle;
        private ContextMenuStrip ButtonStyleMenuStrip = new ContextMenuStrip();

        private string DefqultEditorStyleName = "SQL";

        private void LoadEditorStyle()
        {
            SetEditorStyleList();
            SetButtonStyle();
        }

        private void SetEditorStyleList()
        {
            string stylesPath = $@"{Directory.GetCurrentDirectory()}\{StyleFolderName}";
            if (Directory.Exists(stylesPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(stylesPath);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    if (fileInfo.Extension.ToLower() == ".json")
                    {
                        string text = File.ReadAllText(fileInfo.FullName);
                        try
                        {
                            var style = JsonConvert.DeserializeObject<EditorStyle>(text);
                            SetUserEditorStyleInfo(style);

                            int insertIndex = -1;
                            for (int i = 0; i < EditorStyleList.Count; i++)
                            {
                                if (style.Sort < EditorStyleList[i].Sort)
                                {
                                    insertIndex = i;
                                    break;
                                }
                            }

                            if (insertIndex < 0)
                            {
                                EditorStyleList.Add(style);
                            }
                            else
                            {
                                EditorStyleList.Insert(insertIndex, style);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            EditorStyle none = new EditorStyle()
            {
                Name = "None"
            };
            none.Content = new List<EditorStyleContent>();
            EditorStyleList.Add(none);
        }

        private void SetUserEditorStyleInfo(EditorStyle style)
        {
            for(int i = 0; i < style.Content.Count; i++)
            {
                var content = style.Content[i];
                content.UserEditorStyleInfo = new UserEditorStyleInfo()
                {
                      Name                  = content.Name
                    , Regex                 = content.Regex
                    , Text                  = content.Text
                    , CaseSensitive         = content.CaseSensitive ?? false
                    , MultiLineStartingText = content.MultiLineStartingText
                    , MultiLineEndText      = content.MultiLineEndText
                    , SingleLineText        = content.SingleLineText
                    , MultiLine             = content.MultiLine ?? false
                    , LineColor             = content.LineColor == null ? Color.Empty : GetColor(content.LineColor)
                    , FontColor             = content.FontColor == null ? Color.Empty : GetColor(content.FontColor)
                    , BackColor             = content.BackColor == null ? Color.Empty : GetColor(content.BackColor)
                    , Bold                  = content.Bold ?? false
                    , Italic                = content.Italic ?? false
                    , Underline             = content.Underline ?? false
                };
            }
        }

        private Color GetColor(string colorString)
        {
            Color rs = Color.Empty;
            string[] colorArr = colorString.Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (colorArr.Length == 3)
            {
                int r, g, b;
                if (int.TryParse(colorArr[0], out r)
                    && int.TryParse(colorArr[1], out g)
                    && int.TryParse(colorArr[2], out b)
                    && 0 <= r && r <= 255
                    && 0 <= g && g <= 255
                    && 0 <= b && b <= 255)
                {
                    rs = Color.FromArgb(r, g, b);
                }
            }
            return rs;
        }

        private void SetButtonStyle()
        {
            // Set ButtonStyleMenuStrip
            foreach(EditorStyle style in EditorStyleList)
            {
                ButtonStyleMenuStrip.Items.Add(style.Name, null, ButtonStyleMenuStrip_Click);
                ButtonStyleMenuStrip.Items[ButtonStyleMenuStrip.Items.Count - 1].Name = style.Name;
            }

            // Set ButtonStyle
            int areaLeft = GraphicButtonInfoList[GraphicButtonInfoList.Count - 1].Button.Area.Right;
            ButtonStyle = new GraphicControl(this, "ButtonStyle");
            ButtonStyle
                .SetArea(new Rectangle(areaLeft, 4, 73, 23))
                .SetDrawType(GraphicControl.DrawTypeEnum.ImageTextLeftRight)
                .SetDrawFont(new Font("맑은 고딕", 8f))
                .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo()
                {
                      DrawBackColor = MenuBackColor
                    , DrawImage = CodeGeneratorResource.Icon_File
                    , DrawText = "None"
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
            ButtonStyle.Click += ButtonStyle_Click;
        }

        private void ButtonStyleMenuStrip_Click(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Name;

            EditorStyle style = GetEditorStyle(name);
            Tab tab = GetActiveTab();
            if (tab != null)
            {
                tab.SetEditorStyle(style);
            }

            SetButtonStyleText(name);
            Draw();
        }

        private void SetButtonStyleText(string styleName)
        {
            ButtonStyle.SetDrawText(GraphicControl.StateType.Default, styleName);
            ButtonStyle.SetDrawText(GraphicControl.StateType.Over, styleName);
            ButtonStyle.SetDrawText(GraphicControl.StateType.MouseDown, styleName);
        }

        private EditorStyle GetEditorStyle(string name)
        {
            EditorStyle rs = null;
            foreach (EditorStyle style in EditorStyleList)
            {
                if (style.Name == name)
                {
                    rs = style;
                    break;
                }
            }
            return rs;
        }

        private void ButtonStyle_Click(object sender, MouseEventArgs e)
        {
            ButtonStyleMenuStrip.Show(Cursor.Position);
        }
    }
}