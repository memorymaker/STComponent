using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public class TabModel
    {
        public int TEMPLATE_SEQ { get; set; }
        
        public string TEMPLATE_TITLE { get; set; }

        public string TEMPLATE_CONTENT { get; set; }

        public string TEMPLATE_RESULT { get; set; }

        public string TEMPLATE_STYLE { get; set; }

        public int TEMPLATE_SORT { get; set; }

        public string TEMPLATE_NOTE { get; set; }

        public bool TEMPLATE_SELECTED { get; set; }
    }
}
