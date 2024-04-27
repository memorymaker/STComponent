using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public class TemplateProcessorException : Exception
    {
        public int LineNumber { get; set; }

        public int CharNumber { get; set; }

        public int TextIndex { get; set; }

        public TemplateProcessorException() { }

        public TemplateProcessorException(string message) : base(message) { }
    }
}
