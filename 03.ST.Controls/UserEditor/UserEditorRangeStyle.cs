using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public partial class UserEditor
    {
        public Dictionary<string, UserEditorRangeStyleInfo> RangeStyles = new Dictionary<string, UserEditorRangeStyleInfo>();
    }

    public class UserEditorRangeStyleInfo
    {
        #region Conditions
        public List<Range> Ranges { get; set; }
        #endregion

        #region Line
        public Color LineColor { get; set; } = Color.Empty;
        #endregion

        #region Text
        public Color FontColor { get; set; } = Color.Empty;

        public Color BackColor { get; set; } = Color.Empty;

        public Color BorderColor { get; set; } = Color.Empty;

        public bool Bold { get; set; } = false;

        public bool Italic { get; set; } = false;

        public bool Underline { get; set; } = false;
        #endregion

        #region Etc
        public bool ReadOnly { get; set; } = false;
        #endregion
    }
}
