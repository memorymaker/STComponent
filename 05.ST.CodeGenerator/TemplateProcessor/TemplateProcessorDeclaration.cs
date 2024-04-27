using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public partial class TemplateProcessor
    {
        public static class COMMAND
        {
            /// <summary>
            /// o/
            /// </summary>
            public const string OPTION = "o/";

            /// <summary>
            /// s/
            /// </summary>
            public const string START = "s/";

            /// <summary>
            /// b/
            /// </summary>
            public const string BODY = "b/";

            /// <summary>
            /// e/
            /// </summary>
            public const string END = "e/";

            /// <summary>
            /// c/
            /// </summary>
            public const string CONDITION = "c/";

            /// <summary>
            /// d/
            /// </summary>
            public const string DECLARE = "d/";

            /// <summary>
            /// if/
            /// </summary>
            public const string IF = "if/";

            /// <summary>
            /// elseif/
            /// </summary>
            public const string ELSEIF = "elseif/";

            /// <summary>
            /// else/
            /// </summary>
            public const string ELSE = "else/";

            /// <summary>
            /// endif/
            /// </summary>
            public const string ENDIF = "endif/";
        }

        public static class KEYWORD
        {
            /// <summary>
            /// {from}
            /// </summary>
            public const string FROM = "{from}";
        }

        public static class OPERATOR
        {
            /// <summary>
            /// ==
            /// </summary>
            public const string EQUALS = "==";

            /// <summary>
            /// !=
            /// </summary>
            public const string NOT_EQUALS = "!=";
        }

        public class Result
        {
            public List<Range> NoneLoopValueRangeList = new List<Range>();
            public List<string> NoneLoopValueVariableList = new List<string>();

            public List<Range> LoopValueRangeList = new List<Range>();
            public List<int> LoopValueRowIndexList = new List<int>();
            public List<string> LoopValueVariableList = new List<string>();

            public List<Range> LoopTextRangeList = new List<Range>();

            public Dictionary<string, List<int>> LoopLineListDictionary = new Dictionary<string, List<int>>();
            public List<LoopLineInfo> LoopLineInfoDictionary = new List<LoopLineInfo>();

            public List<int> IndexMapper = new List<int>();

            public List<TemplateProcessorException> ExceptionList = new List<TemplateProcessorException>();
        }

        public class LoopLineInfo
        {
            public string LoopID;
            public DataRow LineDataRow;
            public int LineIndex;
            public List<Range> FixedBlocks;
            public List<CustomBlockInfo> CustomBlocks;
            public string RowLineString;
            public string ResultLineString;

            public LoopLineInfo(string loopID, DataRow lineDataRow, int lineIndex, List<Range> fixedBlocks, List<CustomBlockInfo> customBlocks, string rowLineString, string resultLineString)
            {
                LoopID           = loopID;
                LineDataRow      = lineDataRow;
                LineIndex        = lineIndex;
                FixedBlocks      = fixedBlocks;
                CustomBlocks     = customBlocks;
                RowLineString    = rowLineString;
                ResultLineString = resultLineString;
            }
        }

        public struct CustomBlockInfo
        {
            public int BlockIndex;
            public int BlockIndexSequence;
            public int BlockString;
        }

        public class NoneLoopAreaValueInfo
        {
            public string Variable;
            public Range Range;

            public NoneLoopAreaValueInfo(string variable, int sp, int ep)
            {
                Variable = variable;
                Range = new Range(sp, ep);
            }
        }

        public class LoopAreaLineInfo
        {
            public int Line;
            public DataRow Row;

            public LoopAreaLineInfo(int line, DataRow row)
            {
                Line = line;
                Row = row;
            }
        }

        public class LoopAreaValueInfo
        {
            public string NodeID;
            public string Variable;
            public Range Range;

            public LoopAreaValueInfo(string nodeID, string variable, int sp, int ep)
            {
                NodeID = nodeID;
                Variable = variable;
                Range = new Range(sp, ep);
            }
        }
    }
}
