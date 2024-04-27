using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace ST.Controls
{
    public partial class UserPanel
    {
        private void LoadOption()
        {
        }

        new public DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                if (value != base.Dock)
                {
                    if (value == DockStyle.Fill)
                    {
                        MouseAction.Enable = false;
                    }
                    else
                    {
                        MouseAction.Enable = true;
                    }
                    base.Dock = value;
                }
            }
        }

        public bool UsingMaximize { get; set; } = true;

        public bool UsingViewContextMenuButton {
            get
            {
                return _UsingViewContextMenuButton;
            }
            set
            {
                if (_UsingViewContextMenuButton != value)
                {
                    ViewContextMenuButton.Enabled = value;
                    _UsingViewContextMenuButton = value;
                }
            } 
        }
        private bool _UsingViewContextMenuButton = true;

        public bool UsingAwaysOnTopMenuButton
        {
            get
            {
                return _UsingAwaysOnTopMenuButton;
            }
            set
            {
                if (_UsingAwaysOnTopMenuButton != value)
                {
                    AwaysOnTopMenuButton.Enabled = value;
                    _UsingAwaysOnTopMenuButton = value;
                }
            }
        }
        private bool _UsingAwaysOnTopMenuButton = true;

        public bool UsingTitleSlider { get; set; } = false;

        public bool UsingTitleRename { get; set; } = false;

        public bool UsingPanelMerge { get; set; } = false;

        public int TitleStartIndex
        {
            get
            {
                return _TitleStartIndex;
            }
            set
            {
                if (_TitleStartIndex != value)
                {
                    int max = GetMaxTitleStartIndex();
                    _TitleStartIndex = Math.Min(Math.Max(0, value), max);
                }
            }
        }
        private int _TitleStartIndex = 0;

        private int GetMaxTitleStartIndex()
        {
            int addedWidth = 0;
            var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
            int index = sortedTitles.Count - 1;
            for (int i = sortedTitles.Count - 1; 0 <= i; i--)
            {
                UserPanelTitleInfo titleNode = sortedTitles[i];

                Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                string title = titleNode.Title.Trim();

                Rectangle TitleRect = new Rectangle(
                    TitlePadding.Left + addedWidth, TitlePadding.Top
                    , textSize.Width + TitleWidthRevision, TitleHeight - TitlePadding.Vertical
                );

                int totalWidth = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                addedWidth = totalWidth;
                if (Width - TitleSliderWidth < addedWidth)
                {
                    break;
                }
                index--;
            }

            return index + 1;
        }

        private void ResetTitleStartIndex()
        {
            int max = GetMaxTitleStartIndex();
            _TitleStartIndex = Math.Min(Math.Max(0, _TitleStartIndex), max);
        }
    }
}
