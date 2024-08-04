using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using ST.Controls;
using ST.Core;
using ST.Core.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static ST.CodeGenerator.TemplateProcessor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ST.CodeGenerator
{
    public partial class TemplateProcessor
    {
        #region Description
        /*
        New
            Loop
                {row}
            Not Loop
                {variable}
                {from id:list}
                {id:key, 0, TABLE_ID}
                {id:key, 0, 0}
        Old
            Loop
                Wrapper: b/ s/ e/ o/
                Option Ex: o/ row: 0-2,4,6-7,10 | if: {6} == Y, {3} != DATE and {7} != N
                           o/ list:1
                {row}
            Not Loop
                ${0}: Fixed Value
                {0,0}: Column Value
                {!from}: from query write
                {!where}: ? 범위 지정?
            
                #valueName#: Dictionary Value
                if/ #valueName# == name
                    content...
                endif/
            
                d/ id: 123, name: 가나다라
                d/ id2: 123, name2: 가나다라
                {id} {name}

                {where id:test1}:
            숫자와 컬럼명 혼용으로 사용?
        */
        #endregion

        #region Values
        private bool UsingIndexMapper = false;
        #endregion

        #region Propertise(NODE, RELATION)
        public _NODE NODE = new _NODE();
        public _RELATION RELATION = new _RELATION();

        public class _NODE
        {
            public string NODE_ID_REF { get; set; } = null;

            public string NODE_SEQ_REF { get; set; } = null;

            public string NODE_DETAIL_TABLE_ALIAS { get; set; } = null;

            public string NODE_DETAIL_ID { get; set; } = null;
        }

        public class _RELATION
        {
            /// <summary>
            /// Destination Table
            /// </summary>
            public string NODE_ID2 { get; set; } = null;

            public string NODE_SEQ2 { get; set; } = null;

            public string NODE_DETAIL_TABLE_ALIAS2 { get; set; } = null;

            public string NODE_DETAIL_ID2 { get; set; } = null;

            /// <summary>
            /// Origin Table
            /// </summary>
            public string NODE_ID1 { get; set; } = null;

            public string NODE_SEQ1 { get; set; } = null;

            public string NODE_DETAIL_TABLE_ALIAS1 { get; set; } = null;

            public string NODE_DETAIL_ID1 { get; set; } = null;

            public string NODE_DETAIL_ORDER1 { get; set; } = null;

            public string RELATION_TYPE { get; set; } = null;

            public string RELATION_OPERATOR { get; set; } = null;

            public string RELATION_VALUE { get; set; } = null;
        }
        #endregion

        #region Public
        public string CreateDocument(string template, Dictionary<string, DataTable> columnTableDic, DataTable relationTable, ref Result result, Dictionary<string, string> customValueDic = null)
        {
            // Apply the code below when performance issues occur
            // StringBuilder templateSB = new StringBuilder(template);
            var valueDic = customValueDic ?? new Dictionary<string, string>();
            var loopDic = new Dictionary<string, Dictionary<string, List<string>>>()
            {
                  { COMMAND.OPTION   , new Dictionary<string, List<string>>() { { string.Empty, new List<string>() } } }
                , { COMMAND.START    , new Dictionary<string, List<string>>() { { string.Empty, new List<string>() } } }
                , { COMMAND.BODY     , new Dictionary<string, List<string>>() { { string.Empty, new List<string>() } } }
                , { COMMAND.END      , new Dictionary<string, List<string>>() { { string.Empty, new List<string>() } } }
            };
            List<string> commands = loopDic.Keys.ToList();
            commands.Add(COMMAND.CONDITION);

            // IndexMapper Proc #1 - Set Default
            if (UsingIndexMapper)
            {
                result.IndexMapper = new List<int>(Enumerable.Range(0, template.Length));
            }

            // Appaly if command
            // ref result: Append result.ExceptionList
            // Set removedLineDic
            Dictionary<int, int> removedLIneDic;
            List<string> templateLines = AppalyIfCommandNAppendValueDictionaryFromDeclarationLine(template, ref valueDic, ref result, out removedLIneDic);

            string currentCondition = string.Empty;
            bool currentConditionValidateRs = true;
            int currentLineStartCharIndex = 0;
            int currentLineIndex = 0;
            for (int templateLineIndex = 0; templateLineIndex < templateLines.Count; templateLineIndex++)
            {
                int lineLength = templateLines[templateLineIndex].Length;

                // Remove Line(Comments)
                if (templateLines[templateLineIndex].Length > 0 && templateLines[templateLineIndex].Substring(0, 1) == "!")
                {
                    templateLines.RemoveAt(templateLineIndex);
                    RemovedLineDicAppend(ref removedLIneDic, templateLineIndex);
                    templateLineIndex--;

                    // IndexMapper Proc #3 - Remove Comment Lines
                    if (UsingIndexMapper)
                    {
                        result.IndexMapper.RemoveRange(currentLineStartCharIndex, lineLength + 2);
                    }
                    continue;
                }
                // Process Lines
                else if (templateLines[templateLineIndex].Length >= 2)
                {
                    // Get currentCommand
                    string currentCommand = templateLines[templateLineIndex].Substring(0, 2);
                    if (!commands.Contains(currentCommand))
                    {
                        currentCommand = string.Empty;
                    }

                    // None Loop Line
                    if (currentCommand == string.Empty)
                    {
                        List<Range> noneLoopValueRangeList = new List<Range>();
                        List<string> noneLoopValueVariableList = new List<string>();

                        // Replace Commands - #1
                        if (relationTable != null)
                        {
                            string errorMessage; int errorCharNumber;
                            templateLines[templateLineIndex] = TranslateCommands(templateLines[templateLineIndex], columnTableDic, relationTable, currentLineIndex, ref removedLIneDic, out errorMessage, out errorCharNumber);
                            if (!string.IsNullOrWhiteSpace(errorMessage))
                            {
                                int errorLine = GetRawLineIndex(removedLIneDic, templateLineIndex) + 1;
                                result.ExceptionList.Add(GetException(errorMessage, errorLine, errorCharNumber));
                            }
                            else
                            {
                                // Append line count of from result
                                currentLineIndex += templateLines[templateLineIndex].GetContainsCount("\r\n");
                            }
                        }

                        // Variable Proc #2 - Replace none loop line
                        int oldRangeListCount = noneLoopValueRangeList.Count;
                        templateLines[templateLineIndex] = templateLines[templateLineIndex].ReplaceByKey(valueDic, "{", "}"
                            , ref noneLoopValueRangeList
                            , ref noneLoopValueVariableList
                            , currentLineStartCharIndex
                        );

                        // Replace Hard coded loop values  {id:key, 0, TABLE_ID}
                        templateLines[templateLineIndex] = TranslateHardCodedLoopValues(templateLines[templateLineIndex]
                            , columnTableDic
                            , ref noneLoopValueRangeList
                            , ref noneLoopValueVariableList
                            , templateLineIndex
                            , currentLineStartCharIndex
                            , noneLoopValueRangeList.Count - oldRangeListCount
                            , ref removedLIneDic
                            , ref result
                        );

                        result.NoneLoopValueRangeList.AddRange(noneLoopValueRangeList);
                        result.NoneLoopValueVariableList.AddRange(noneLoopValueVariableList);

                        // IndexMapper Proc #4 - Replace Variable
                        if (UsingIndexMapper)
                        {
                            CreateDocument_ReplaceIndexMapperByVariable(
                                  ref result.IndexMapper
                                , noneLoopValueRangeList
                                , noneLoopValueVariableList
                            );
                        }
                    }
                    // Loop Line
                    else
                    {
                        // Get nextCommand
                        string _nextCommand = (templateLineIndex + 1 == templateLines.Count || templateLines[templateLineIndex + 1].Length < 2)
                            ? string.Empty : templateLines[templateLineIndex + 1].Substring(0, 2);
                        string nextCommand = (_nextCommand == string.Empty || !commands.Contains(_nextCommand))
                            ? string.Empty : _nextCommand;

                        switch (currentCommand)
                        {
                            // Clear condition values
                            case COMMAND.OPTION:
                                currentCondition = string.Empty;
                                currentConditionValidateRs = true;
                                break;
                            // Condition Proc
                            case COMMAND.CONDITION:
                                currentCondition = templateLines[templateLineIndex].Substring(2, templateLines[templateLineIndex].Length - 2);

                                string errorMessage; int errorCharNumber;
                                currentConditionValidateRs = ValidateCondition(currentCommand, currentCondition, out errorMessage, out errorCharNumber);
                                if (!currentConditionValidateRs)
                                {
                                    int errorLine = GetRawLineIndex(removedLIneDic, templateLineIndex) + 1;
                                    result.ExceptionList.Add(GetException(errorMessage, errorLine, errorCharNumber));
                                }

                                // When the current condition line is the last line
                                if (nextCommand == COMMAND.START || nextCommand == string.Empty)
                                {
                                    templateLines[templateLineIndex] = string.Empty;
                                    currentConditionValidateRs = true;
                                }
                                // Not Last Line
                                else
                                {
                                    currentCondition = currentCondition.Trim();
                                    templateLines.RemoveAt(templateLineIndex);
                                    RemovedLineDicAppend(ref removedLIneDic, templateLineIndex);
                                    templateLineIndex--;
                                    continue;
                                }
                                break;
                        }

                        // Condition string error and not last line Proc #1
                        if (!currentConditionValidateRs && nextCommand != COMMAND.OPTION && nextCommand != string.Empty)
                        {
                            templateLines.RemoveAt(templateLineIndex);
                            RemovedLineDicAppend(ref removedLIneDic, templateLineIndex);
                            templateLineIndex--;
                            continue;
                        }
                        // Etc
                        else
                        {
                            // Condition string error and not last line Proc #2
                            if (!currentConditionValidateRs)
                            {
                                templateLines[templateLineIndex] = string.Empty;
                            }

                            // When the current condition line is not the last line and currentConditionValidateRs is true
                            if (!(currentCommand == COMMAND.CONDITION && (nextCommand == COMMAND.START || nextCommand == string.Empty)) && currentConditionValidateRs)
                            {
                                // Replace Commands - #1
                                if (relationTable != null)
                                {
                                    string errorMessage; int errorCharNumber;
                                    templateLines[templateLineIndex] = TranslateCommands(templateLines[templateLineIndex], columnTableDic, relationTable, currentLineIndex, ref removedLIneDic, out errorMessage, out errorCharNumber);
                                    if (!string.IsNullOrWhiteSpace(errorMessage))
                                    {
                                        int errorLine = GetRawLineIndex(removedLIneDic, templateLineIndex) + 1;
                                        result.ExceptionList.Add(GetException(errorMessage, errorLine, errorCharNumber));
                                    }
                                    else
                                    {
                                        // Append line count of from result
                                        currentLineIndex += templateLines[templateLineIndex].GetContainsCount("\r\n");
                                    }
                                }

                                // Replace Hard coded loop values  {id:key, 0, TABLE_ID}
                                templateLines[templateLineIndex] = TranslateHardCodedLoopValues(templateLines[templateLineIndex], columnTableDic);

                                // Loop - Add Node Content
                                if (!loopDic[currentCommand].ContainsKey(currentCondition))
                                {
                                    loopDic[currentCommand].Add(currentCondition, new List<string>());
                                }
                                loopDic[currentCommand][currentCondition].Add(templateLines[templateLineIndex].Substring(2, templateLines[templateLineIndex].Length - 2).TrimEnd());
                            }

                            // Loop - Continue(new line or middle line)
                            if ((currentCommand == COMMAND.OPTION && nextCommand != string.Empty)
                            || (currentCommand != string.Empty && nextCommand != COMMAND.OPTION && nextCommand != string.Empty))
                            {
                                templateLines.RemoveAt(templateLineIndex);
                                RemovedLineDicAppend(ref removedLIneDic, templateLineIndex);
                                templateLineIndex--;

                                // IndexMapper Proc #5 - Remove Loop Lines
                                if (UsingIndexMapper)
                                {
                                    result.IndexMapper.RemoveRange(currentLineStartCharIndex, lineLength + 2);
                                }
                                continue;
                            }
                            // Loop - Last
                            else
                            {
                                // Variavble Proc #3 - Replace loop line
                                string optionID;
                                string loopRs = this.GetBody(
                                      loopDic
                                    , valueDic
                                    , columnTableDic
                                    , currentLineIndex
                                    , currentLineStartCharIndex
                                    , ref result
                                    , out optionID
                                ).TrimEnd();

                                // result Proc - Set result.LoopLineDictionary
                                int loopRsLineCount = loopRs.GetContainsCount("\r\n") + 1;
                                if (optionID != string.Empty && loopRs != string.Empty)
                                {
                                    optionID = optionID + "(0)";
                                    if (result.LoopLineListDictionary.ContainsKey(optionID))
                                    {
                                        int idCount = 1;
                                        while(result.LoopLineListDictionary.ContainsKey(optionID + $"({idCount})"))
                                        {
                                            idCount++;
                                        }
                                        optionID = optionID + $"({idCount})";
                                    }

                                    result.LoopLineListDictionary.Add(optionID, new List<int>());
                                    for (int i = 0; i < loopRsLineCount; i++)
                                    {
                                        result.LoopLineListDictionary[optionID].Add(currentLineIndex + i);
                                    }
                                }

                                // Set templateLines
                                templateLines[templateLineIndex] = loopRs;

                                // Append line count of loop result
                                currentLineIndex += loopRsLineCount - 1;

                                // Clear loopDic
                                foreach (string command in commands)
                                {
                                    if (loopDic.ContainsKey(command))
                                    {
                                        loopDic[command].Clear();
                                        loopDic[command].Add(string.Empty, new List<string>());
                                    }
                                }

                                // IndexMapper Proc #6 - Replace Loop Lines
                                if (UsingIndexMapper)
                                {
                                    result.IndexMapper.RemoveRange(currentLineStartCharIndex, lineLength);
                                    result.IndexMapper.InsertRange(currentLineStartCharIndex, Enumerable.Repeat(-1, loopRs.Length));
                                }
                            }
                        }
                    }
                }

                currentLineStartCharIndex += templateLines[templateLineIndex].Length + 2;
                currentLineIndex++;
            }

            string rs = string.Join("\r\n", templateLines);
            return rs;
        }

        public Dictionary<string, string> GetDictionaryValues(string template)
        {
            Dictionary<string, string> rsValues = new Dictionary<string, string>();
            List<string> templateLines = template.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
            List<TemplateProcessorException> exceptionList = new List<TemplateProcessorException>();

            for (int i = 0; i < templateLines.Count; i++)
            {
                if (templateLines[i].Length >= 2 && templateLines[i].Substring(0, 2) == COMMAND.DECLARE)
                {
                    AppendValueDictionaryFromDeclarationLine(templateLines[i], i, ref rsValues, ref exceptionList);
                }
            }
            
            return rsValues;
        }

        public Dictionary<string, string> GetDictionaryValuesWithoutCommand(string template, out List<TemplateProcessorException>  exceptionList)
        {
            Dictionary<string, string> rsValues = new Dictionary<string, string>();
            List<string> templateLines = template.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
            exceptionList = new List<TemplateProcessorException>();

            for (int i = 0; i < templateLines.Count; i++)
            {
                AppendValueDictionaryFromDeclarationLine(templateLines[i], i, ref rsValues, ref exceptionList);
            }

            return rsValues;
        }
        #endregion

        #region AppalyIfCommandAppendValueDictionaryFromDeclarationLine
        private List<string> AppalyIfCommandNAppendValueDictionaryFromDeclarationLine(string template, ref Dictionary<string, string> valueDic, ref Result result, out Dictionary<int, int> removedLineDic)
        {
            removedLineDic = new Dictionary<int, int>();
            List<string> templateLines = template.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();

            bool ifAreaStart = false;
            bool elseifStart = false;
            bool hasTrue = false;
            bool prevTrue = false;

            int ifLength = COMMAND.IF.Length;
            int elseifLength = COMMAND.ELSEIF.Length;
            int elseLength = COMMAND.ELSE.Length;
            int endifLength = COMMAND.ENDIF.Length;

            int removeLinesCount = 0;
            for (int i = 0; i < templateLines.Count; i++)
            {
                bool lineRemoved = true;
                int templateLineLength = templateLines[i].Length;

                // if/
                if (!ifAreaStart && !elseifStart && templateLineLength >= ifLength && templateLines[i].Substring(0, ifLength) == COMMAND.IF)
                {
                    ifAreaStart = true;
                    if (AppalyIfCommand_GetConditionResult(COMMAND.IF, i + removeLinesCount, templateLines[i].Substring(ifLength, templateLineLength - ifLength), valueDic, ref result))
                    {
                        hasTrue = true;
                        prevTrue = true;
                    }

                    templateLines.RemoveAt(i);
                    i--;
                    removeLinesCount++;
                }
                // elseif/
                else if (ifAreaStart && templateLineLength >= elseifLength && templateLines[i].Substring(0, elseifLength) == COMMAND.ELSEIF)
                {
                    elseifStart = true;
                    if (hasTrue)
                    {
                        prevTrue = false;
                    }
                    else
                    {
                        if (AppalyIfCommand_GetConditionResult(COMMAND.IF, i + removeLinesCount, templateLines[i].Substring(elseifLength, templateLineLength - elseifLength), valueDic, ref result))
                        {
                            hasTrue = true;
                            prevTrue = true;
                        }
                    }

                    templateLines.RemoveAt(i);
                    i--;
                    removeLinesCount++;
                }
                // else/
                else if (ifAreaStart && templateLineLength >= elseLength && templateLines[i].Substring(0, elseLength) == COMMAND.ELSE)
                {
                    prevTrue = !hasTrue;

                    templateLines.RemoveAt(i);
                    i--;
                    removeLinesCount++;
                }
                // end/
                else if (ifAreaStart && templateLines[i].Length >= endifLength && templateLines[i].Substring(0, endifLength) == COMMAND.ENDIF)
                {
                    ifAreaStart = false;
                    elseifStart = false;
                    hasTrue = false;
                    prevTrue = false;

                    templateLines.RemoveAt(i);
                    i--;
                    removeLinesCount++;
                }
                else
                {
                    if ((ifAreaStart) && !prevTrue)
                    {
                        templateLines.RemoveAt(i);
                        i--;
                        removeLinesCount++;
                    }
                    else
                    {
                        // Declare LInes
                        if (templateLines[i].Length >= 2 && templateLines[i].Substring(0, 2) == COMMAND.DECLARE)
                        {
                            AppendValueDictionaryFromDeclarationLine(templateLines[i], i + removeLinesCount, ref valueDic, ref result.ExceptionList);

                            templateLines.RemoveAt(i);
                            i--;
                            removeLinesCount++;
                        }
                        else
                        {
                            lineRemoved = false;
                        }
                    }
                }

                if (lineRemoved)
                {
                    RemovedLineDicAppend(ref removedLineDic, i + 1);
                }
            }

            return templateLines;
        }

        private bool AppalyIfCommand_GetConditionResult(string commandText, int lineIndex, string condition, Dictionary<string, string> valueDic, ref Result result)
        {
            string errorMessage;
            int errorCharNumber;
            bool rs = GetConditionResult(commandText, condition, out errorMessage, out errorCharNumber, valueDic);

            if (errorMessage != string.Empty)
            {
                result.ExceptionList.Add(GetException(errorMessage, lineIndex + 1, errorCharNumber));
            }
            
            return rs;
        }

        private void AppendValueDictionaryFromDeclarationLine(string lineText, int lineIndex, ref Dictionary<string, string> valueDic, ref List<TemplateProcessorException> exceptionList)
        {
            string key = string.Empty;
            string value;

            bool isKey = true;
            StringBuilder sb = new StringBuilder();

            string declaraionText;
            int lineCharIndexRevision;
            if (lineText.Length >= 2 && lineText.Substring(0, 2) == COMMAND.DECLARE)
            {
                declaraionText = lineText.Substring(2).Trim();
                lineCharIndexRevision = 2 + lineText.Substring(2).Length - lineText.Substring(2).TrimStart().Length;
            }
            else
            {
                declaraionText = lineText.Trim();
                lineCharIndexRevision = lineText.Length - lineText.TrimStart().Length;
            }

            if (declaraionText.Length > 0)
            {
                for (int i = 0; i < declaraionText.Length; i++)
                {
                    int asc = declaraionText[i];

                    // 0 ~ 9 // A ~ Z // a ~ z
                    if ((48 <= asc && asc <= 57)
                        || (65 <= asc && asc <= 90)
                        || (97 <= asc && asc <= 122))
                    {
                        if (isKey && key.Trim().Length > 0)
                        {
                            int keyLastChar = key[key.Length - 1];
                            // Space Characters
                            if ((9 <= keyLastChar && keyLastChar <= 13) || keyLastChar == 32)
                            {
                                exceptionList.Add(GetException(
                                    $"사용자 정의 변수에 공백 문자가 포함돼 있습니다.",
                                    lineIndex + 1, i + 1 + lineCharIndexRevision
                                ));
                            }
                        }
                        sb.Append((char)asc);
                    }
                    // Space Characters
                    else if ((9 <= asc && asc <= 13) || asc == 32)
                    {
                        sb.Append((char)asc);
                    }
                    // ,
                    else if (asc == 44)
                    {
                        if (isKey)
                        {
                            exceptionList.Add(GetException(
                                $"사용자 정의 변수의 값이 존재하지 않습니다. (변수명: {key})",
                                lineIndex + 1, i + 1 + lineCharIndexRevision
                            ));
                        }
                        else
                        {
                            value = sb.ToString().Trim();
                            if (value.Length == 0)
                            {
                                exceptionList.Add(GetException(
                                    $"사용자 정의 변수의 값이 존재하지 않습니다. (변수명: {key})",
                                    lineIndex + 1, i + 1 + lineCharIndexRevision
                                ));
                            }
                            else
                            {
                                if (valueDic.ContainsKey(key))
                                {
                                    exceptionList.Add(GetException(
                                        $"사용자 정의 변수가 중복되었습니다. (중복 변수명: {key})",
                                        lineIndex + 1, i + 1 + lineCharIndexRevision
                                    ));
                                }
                                else
                                {
                                    valueDic.Add(key, value);
                                    key = string.Empty;

                                    sb.Clear();
                                    isKey = true;
                                }
                            }
                        }
                    }
                    // :(58) =(61)
                    else if (asc == 58)
                    {
                        if (isKey)
                        {
                            key = sb.ToString().Trim();
                            if (key.Length == 0)
                            {
                                exceptionList.Add(GetException(
                                    "사용자 정의 변수의 키가 존재하지 않습니다.",
                                    lineIndex + 1, i + 1 + lineCharIndexRevision
                                ));
                            }
                            else
                            {
                                sb.Clear();
                                isKey = false;
                            }
                        }
                        else
                        {
                            sb.Append((char)asc);
                        }
                    }
                    // "
                    else if (asc == 34)
                    {
                        if (isKey)
                        {
                            exceptionList.Add(GetException(
                                $"사용자 정의 변수는 [\"] 문자를 사용할 수 없습니다.",
                                lineIndex + 1, i + 1 + lineCharIndexRevision
                            ));
                        }
                        else
                        {
                            sb.Append((char)asc);
                        }
                    }
                    // '
                    else if (asc == 39)
                    {
                        if (isKey)
                        {
                            exceptionList.Add(GetException(
                                $"사용자 정의 변수는 ['] 문자를 사용할 수 없습니다.",
                                lineIndex + 1, i + 1 + lineCharIndexRevision
                            ));
                        }
                        else
                        {
                            sb.Append((char)asc);
                        }
                    }
                    // Special Characters
                    else if ((33 <= asc && asc <= 47) || (58 <= asc && asc <= 64) || (91 <= asc && asc <= 96) || (123 <= asc && asc <= 126))
                    {
                        if (isKey)
                        {
                            exceptionList.Add(GetException(
                                $"사용자 정의 변수는 특수문자를 사용할 수 없습니다.",
                                lineIndex + 1, i + 1 + lineCharIndexRevision
                            ));
                        }
                        else
                        {
                            sb.Append((char)asc);
                        }
                    }
                    // Control Characters
                    else
                    {
                        sb.Append((char)asc);
                    }
                }

                // Last
                if (isKey)
                {
                    if (sb.ToString().Trim().Length == 0)
                    {
                        exceptionList.Add(GetException(
                            "사용자 정의 변수 영역이 올바지 않습니다.",
                            lineIndex + 1, declaraionText.Length + lineCharIndexRevision + 1
                        ));
                    }
                    else
                    {
                        exceptionList.Add(GetException(
                            $"사용자 정의 변수의 값이 존재하지 않습니다. (변수명: {key})",
                            lineIndex + 1, declaraionText.Length + lineCharIndexRevision + 1 // 1
                        ));
                    }
                }
                else
                {
                    value = sb.ToString().Trim();
                    if (value.Length == 0)
                    {
                        exceptionList.Add(GetException(
                            $"사용자 정의 변수의 값이 존재하지 않습니다. (변수명: {key})",
                            lineIndex + 1, declaraionText.Length + lineCharIndexRevision + 1
                        ));
                    }
                    else
                    {
                        if (valueDic.ContainsKey(key))
                        {
                            exceptionList.Add(GetException(
                                $"사용자 정의 변수가 중복되었습니다. (중복 변수명: {key})",
                                lineIndex + 1, declaraionText.Length + lineCharIndexRevision
                            ));
                        }
                        else
                        {
                            valueDic.Add(key, value);
                            sb.Clear();
                        }
                    }
                }
            }
        }

        private void RemovedLineDicAppend(ref Dictionary<int, int> removedLineDic, int lineIndex, int count = 1)
        {
            if (!removedLineDic.ContainsKey(lineIndex))
            {
                removedLineDic.Add(lineIndex, count);
            }
            else
            {
                removedLineDic[lineIndex] += count;
            }
        }

        private int GetRawLineIndex(Dictionary<int, int> removedLineDic, int lineIndex)
        {
            int rs = lineIndex;
            foreach(KeyValuePair<int, int> pair in removedLineDic)
            {
                if (pair.Key <= lineIndex)
                {
                    rs += pair.Value;
                }
            }
            return rs;
        }
        #endregion

        #region IndexMapper
        private void CreateDocument_ReplaceIndexMapperByVariable(ref List<int> indexMapper, List<Range> noneLoopValueRangeList, List<string> noneLoopValueVariableList)
        {
            for (int i = 0; i < noneLoopValueRangeList.Count; i++)
            {
                int varLength = noneLoopValueVariableList[i].Length;
                Range range = noneLoopValueRangeList[i];
                if (varLength < range.Interval + 1)
                {
                    indexMapper.InsertRange(range.StartingValue + 1, new int[range.Interval + 1 - varLength]);
                }
                else if (varLength > range.Interval + 1)
                {
                    // When the value length is shorter than variable length
                    int removeLength = varLength - (range.Interval + 1);
                    if (range.Interval == 0)
                    {
                        int rawFrontIndex = indexMapper[range.StartingValue];
                        int rawRearIndex = indexMapper[range.StartingValue + varLength - 1];

                        indexMapper.RemoveRange(range.StartingValue + 1, removeLength);

                        // When the value position is the last
                        if (range.StartingValue + 1 == indexMapper.Count)
                        {
                            indexMapper.Add(rawRearIndex + 1);
                        }
                    }
                    else
                    {
                        indexMapper.RemoveRange(range.StartingValue + 1, removeLength);
                    }
                }
            }
        }
        #endregion

        #region GetBody
        private string GetBody(Dictionary<string, Dictionary<string, List<string>>> loopDic, Dictionary<string, string> valueDic, Dictionary<string, DataTable> columnTableDic, int startLineIndex, int startLineCharIndex, ref Result result, out string optionID)
        {
            // Add result.LoopAreaLineInfoList
            // Add result.LoopAreaLineRangeList
            // Add result.LoopAreaValueInfoList
            // Add result.LoopAreaValueRangeList

            string rs = string.Empty;
            optionID = string.Empty;

            if (columnTableDic != null)
            {
                Dictionary<string, string> loopDefaultTextDic = new Dictionary<string, string>()
                {
                      { COMMAND.START, string.Join("\r\n", loopDic[COMMAND.START][string.Empty]) }
                    , { COMMAND.BODY,  string.Join("\r\n", loopDic[COMMAND.BODY][string.Empty]) }
                    , { COMMAND.END,   string.Join("\r\n", loopDic[COMMAND.END][string.Empty]) }
                };

                // Options
                Dictionary<string, object> options = this.GetOption(loopDic[COMMAND.OPTION][string.Empty]);
                List<int> optionRows = options.ContainsKey("rows") ? (List<int>)options["rows"] : null;
                List<object[]> optionIf = options.ContainsKey("if") ? (List<object[]>)options["if"] : null;
                optionID = options.ContainsKey("id") ? options["id"].ToString() : "";

                if (optionID.Length > 0 && columnTableDic.ContainsKey(optionID))
                {
                    // Options List Set dtColumn
                    DataTable columnDt = columnTableDic[optionID];

                    // Option rows, if
                    // Get targetRowIndexes
                    List<int> targetIndexes = new List<int>();
                    for (int m = 0; m < columnDt.Rows.Count; m++)
                    {
                        if (optionRows == null || (optionRows != null && optionRows.Contains(m)))
                        {
                            DataRow dr = columnDt.Rows[m];
                            if (this.CheckOptionIf(dr, optionIf))
                            {
                                targetIndexes.Add(m);
                            }
                        }
                    }

                    // Get loopNodeList, crow
                    Dictionary<int, string> loopNodeDic = new Dictionary<int, string>();
                    Dictionary<int, int> crowDic = new Dictionary<int, int>();
                    Dictionary<string, int> _conditionStringCountDic = new Dictionary<string, int>();
                    foreach (int rowIndex in targetIndexes)
                    {
                        DataRow dr = columnDt.Rows[rowIndex];

                        // Get loopNode
                        string conditionString;
                        string loopNode;
                        if (rowIndex == targetIndexes[0] && HasLoopDicData(loopDic[COMMAND.START]))
                        {
                            loopNode = GetLoopNode(COMMAND.START, loopDic[COMMAND.START], valueDic, dr, out conditionString);
                            if (loopNode == string.Empty) { loopNode = loopDefaultTextDic[COMMAND.START]; }
                            if (loopNode == string.Empty) { loopNode = loopDefaultTextDic[COMMAND.BODY]; }
                        }
                        else if (rowIndex == targetIndexes[targetIndexes.Count - 1] && HasLoopDicData(loopDic[COMMAND.END]))
                        {
                            loopNode = GetLoopNode(COMMAND.END, loopDic[COMMAND.END], valueDic, dr, out conditionString);
                            if (loopNode == string.Empty) { loopNode = loopDefaultTextDic[COMMAND.END]; }
                            if (loopNode == string.Empty) { loopNode = loopDefaultTextDic[COMMAND.BODY]; }
                        }
                        else
                        {
                            loopNode = GetLoopNode(COMMAND.BODY, loopDic[COMMAND.BODY], valueDic, dr, out conditionString);
                            if (loopNode == string.Empty) { loopNode = loopDefaultTextDic[COMMAND.BODY]; }
                        }

                        // Add loopNode
                        loopNodeDic.Add(rowIndex, loopNode);

                        // Set conditionStringCount
                        if (_conditionStringCountDic.ContainsKey(conditionString))
                        {
                            _conditionStringCountDic[conditionString]++;
                            crowDic.Add(rowIndex, _conditionStringCountDic[conditionString]);
                        }
                        else
                        {
                            _conditionStringCountDic.Add(conditionString, 1);
                            crowDic.Add(rowIndex, 1);
                        }
                    }

                    // Main Proc
                    int lineIndex = startLineIndex;
                    int lineCharIndex = startLineCharIndex;
                    for (int i = 0; i < targetIndexes.Count; i++)
                    {
                        string currentLoopNode = loopNodeDic[targetIndexes[i]];

                        // Get groupNodeEp
                        int groupNodeEp = i;
                        for(int k = i + 1; k < targetIndexes.Count; k++)
                        {
                            if (currentLoopNode.Replace(new char[] { ',', ';' }, ' ').TrimEnd() == loopNodeDic[targetIndexes[k]].Replace(new char[] { ',', ';' }, ' ').TrimEnd())
                            {
                                groupNodeEp = k;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Replace {[ColumnName]}, {row}
                        Dictionary<int, int> maxColumnsTextByteCount = GetMaxColumnsTextByteCount(targetIndexes, i, groupNodeEp, columnDt);
                        for(int k = i; k <= groupNodeEp; k++)
                        {
                            currentLoopNode = loopNodeDic[targetIndexes[k]];

                            int rowIndex = targetIndexes[k];
                            DataRow dr = columnDt.Rows[rowIndex];

                            // Get loopNode, Replace {row}
                            string loopNode = currentLoopNode.Replace("{row}", (rowIndex + 1).ToString());
                            loopNode = loopNode.Replace("{crow}", crowDic[targetIndexes[k]].ToString());

                            // Replace {[ColumnName]}
                            string[] currentLinesArr = loopNode.Split(new string[] { "\r\n" } , StringSplitOptions.None);
                            if (currentLinesArr.Length == 2 && string.IsNullOrWhiteSpace(currentLinesArr[1]))
                            {
                                for (int m = 0; m < columnDt.Columns.Count; m++)
                                {
                                    string columnText = dr[m].ToString().Trim();

                                    // {[ColumnName]}
                                    string toBeReplaced = "{" + columnDt.Columns[m].ColumnName + "}";
                                    int columnNameSp = loopNode.IndexOf(toBeReplaced);
                                    if (columnNameSp >= 0)
                                    {
                                        do
                                        {
                                            int columnNameEp = columnNameSp + toBeReplaced.Length;
                                            if (columnNameSp >= 0)
                                            {
                                                for (int inserPoint = columnNameEp; inserPoint < loopNode.Length; inserPoint++)
                                                {
                                                    if (loopNode[inserPoint] == ' '
                                                    || loopNode[inserPoint] == '\t'
                                                    || loopNode[inserPoint] == ','
                                                    || loopNode[inserPoint] == ';')
                                                    {
                                                        int revisionCount = maxColumnsTextByteCount[m] - columnText.GetByteCount();
                                                        loopNode = loopNode.Insert(inserPoint, new string(' ', revisionCount));
                                                        break;
                                                    }
                                                    else if (loopNode[inserPoint] == '\r' || loopNode[inserPoint] == '\n')
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            columnNameSp = loopNode.IndexOf(toBeReplaced, columnNameEp);
                                        }
                                        while (columnNameSp >= 0);
                                    }
                                }
                            }

                            // Replace & result Proc 1
                            loopNode = loopNode.ReplaceByKey(valueDic, dr, "{", "}"
                                , ref result.NoneLoopValueRangeList
                                , ref result.NoneLoopValueVariableList
                                , ref result.LoopValueRangeList
                                , ref result.LoopValueVariableList
                                , lineCharIndex
                            );

                            // result Proc 2 - Add rowIndex
                            result.LoopValueRowIndexList.Add(rowIndex);

                            lineIndex++;
                            lineCharIndex += loopNode.Length;
                            rs += loopNode;
                        }

                        i = groupNodeEp;
                    }
                }
            }

            return rs;
        }

        private bool CheckOptionIf(DataRow dr, List<object[]> optionIf)
        {
            bool rs = true;
            if (optionIf != null)
            {
                foreach (object[] node in optionIf)
                {
                    if (Convert.ToInt32(node[0]) < dr.Table.Columns.Count)
                    {
                        if (((string)node[2] == "equal" && dr[Convert.ToInt32(node[0])].ToString() != (string)node[1])
                        || ((string)node[2] == "notEqual" && dr[Convert.ToInt32(node[0])].ToString() == (string)node[1]))
                        {
                            rs = false;
                            break;
                        }
                    }
                }
            }
            return rs;
        }

        private Dictionary<string, object> GetOption(List<string> mapperO)
        {
            Dictionary<string, object> rs = new Dictionary<string, object>();

            string[] options = string.Join("|", mapperO.ToArray()).Split('|');
            foreach (string optionNode in options)
            {
                string[] arrOptionNode = optionNode.Split(':');
                if (arrOptionNode.Length > 1 && arrOptionNode[1].Trim().Length > 0)
                {
                    arrOptionNode[0] = arrOptionNode[0].Trim();
                    switch (arrOptionNode[0])
                    {
                        case "rows":
                            rs[arrOptionNode[0]] = this.GetOptionRows(arrOptionNode[1]);
                            break;
                        case "if":
                            rs[arrOptionNode[0]] = this.GetOptionIf(arrOptionNode[1]);
                            break;
                        case "id":
                            rs[arrOptionNode[0]] = this.GetOptionID(arrOptionNode[1]);
                            break;
                    }
                }
            }
            return rs;
        }

        private List<int> GetOptionRows(string value)
        {
            List<int> rs = new List<int>();

            string[] optionValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string optionValue in optionValues)
            {
                string[] arrOptionValue = optionValue.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrOptionValue.Length == 1)
                {
                    if (this.IsParseNumber(arrOptionValue[0]))
                    {
                        rs.Add(Int32.Parse(arrOptionValue[0]));
                    }
                }
                else
                {
                    if (this.IsParseNumber(arrOptionValue[0]) && this.IsParseNumber(arrOptionValue[arrOptionValue.Length - 1]))
                    {
                        for (int i = Int32.Parse(arrOptionValue[0]); i <= Int32.Parse(arrOptionValue[arrOptionValue.Length - 1]); i++)
                        {
                            rs.Add(i);
                        }
                    }
                }
            }
            return rs;
        }

        private List<object[]> GetOptionIf(string value)
        {
            List<object[]> rs = new List<object[]>();

            string[] optionValues = value.Split(new string[] { ",", "and" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string optionValue in optionValues)
            {
                string[] arrOptionValueEqual = optionValue.Split(new string[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrOptionValueEqual.Length > 1)
                {
                    arrOptionValueEqual[0] = arrOptionValueEqual[0].Replace("{", "").Replace("}", "");
                    if (IsParseNumber(arrOptionValueEqual[0]))
                    {
                        rs.Add(new object[] { Int32.Parse(arrOptionValueEqual[0]), arrOptionValueEqual[1].Trim(), "equal" });
                    }
                }
                else
                {
                    string[] arrOptionValueNotEqual = optionValue.Split(new string[] { "!=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrOptionValueNotEqual.Length > 1)
                    {
                        arrOptionValueNotEqual[0] = arrOptionValueNotEqual[0].Replace("{", "").Replace("}", "");
                        if (IsParseNumber(arrOptionValueNotEqual[0]))
                        {
                            rs.Add(new object[] { Int32.Parse(arrOptionValueNotEqual[0]), arrOptionValueNotEqual[1].Trim(), "notEqual" });

                        }
                    }
                }
            }
            return rs;
        }

        private string GetOptionID(string value)
        {
            return value.Trim();
        }

        private string GetLoopNode(string commandString, Dictionary<string, List<string>> loopDic, Dictionary<string, string> valueDic, DataRow columnRow, out string conditionString)
        {
            // loopDicNode Key : Conditions, Value : Lines
            conditionString = string.Empty;
            string rs;
            StringBuilder sb = new StringBuilder();

            // Set sb(Conditions)
            foreach(KeyValuePair<string, List<string>> pair in loopDic)
            {
                if (pair.Key != string.Empty)
                {
                    bool conditionsRs = GetConditionResult(commandString, pair.Key, valueDic, columnRow);
                    if (conditionsRs)
                    {
                        conditionString = pair.Key;

                        if (sb.Length > 0)
                        {
                            sb.Append("\r\n");
                        }
                        sb.Append(string.Join("\r\n", pair.Value));
                        break;
                    }
                }
            }

            rs = sb.ToString();
            return rs;
        }

        private bool HasLoopDicData(Dictionary<string, List<string>> loopDicNode)
        {
            bool rs = false;
            foreach(KeyValuePair<string, List<string>> pair in loopDicNode)
            {
                if (pair.Value.Count > 0)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }

        private Dictionary<int, int> GetMaxColumnsTextByteCount(List<int> targetIndexes, int sp, int ep, DataTable columnDt)
        {
            Dictionary<int, int> maxColumnsTextByteCount = new Dictionary<int, int>();
            for(int i = sp; i <= ep; i++)
            {
                for (int k = 0; k < columnDt.Columns.Count; k++)
                {
                    string columnText = columnDt.Rows[targetIndexes[i]][k].ToString().Trim();
                    int refNumber = columnText.GetByteCount();

                    if (!maxColumnsTextByteCount.ContainsKey(k))
                    {
                        maxColumnsTextByteCount.Add(k, refNumber);
                    }
                    else if (maxColumnsTextByteCount[k] < refNumber)
                    {
                        maxColumnsTextByteCount[k] = refNumber;
                    }
                }
            }
            return maxColumnsTextByteCount;
        }

        private bool IsParseNumber(object obj)
        {
            int parse;
            try
            {
                parse = int.Parse(obj.ToString());
                return true;
            }
            catch
            {
                int.TryParse(obj.ToString(), out parse);
                return parse == -1 || parse == 0 ? false : true;
            }
        }
        #endregion

        #region TranslateCommands - From
        private string TranslateCommands(string content, Dictionary<string, DataTable> columnTableDic, DataTable relationTable, int lineIndex, ref Dictionary<int, int> removedLIneDic, out string errorMessage, out int errorCharNumber)
        {
            errorMessage = string.Empty;
            errorCharNumber = -1;
            DataTable rsRelationData = relationTable.Clone();

            // from Proc
            Regex regx = new Regex("{ *from *id *: *([A-Za-z0-9_]*) *}");
            Match match = regx.Match(content);
            do
            {
                int rsFromStringLength = 0;
                if (match.Success)
                {
                    string rsFromString = string.Empty;

                    string id = match.Groups[1].Value;
                    if (columnTableDic.ContainsKey(id))
                    {
                        DataRow[] tablesRows = columnTableDic[id].DefaultView.ToTable(true, new string[] { NODE.NODE_ID_REF, NODE.NODE_SEQ_REF, NODE.NODE_DETAIL_TABLE_ALIAS }).Select($"{NODE.NODE_ID_REF} <> '' AND {NODE.NODE_ID_REF} IS NOT NULL");

                        // None Table
                        if (tablesRows.Length == 0)
                        {
                            // None
                        }
                        // Single Table
                        else if (tablesRows.Length == 1)
                        {
                            rsFromString = $"FROM {tablesRows[0][NODE.NODE_ID_REF].ToString().Trim()}";
                        }
                        // Multi Tables
                        else
                        {
                            bool success = false;
                            string nodeErrorMessage = string.Empty;
                            rsRelationData.Clear();

                            // Set rsRelationData
                            for (int i = 0; i < tablesRows.Length; i++)
                            {
                                DataRow originRow = tablesRows[i];
                                for (int k = 0; k < tablesRows.Length; k++)
                                {
                                    bool pathFound = false;
                                    if (i != k)
                                    {
                                        DataRow destinationRow = tablesRows[k];
                                        TranslateCommands_GetPath(originRow, destinationRow, relationTable, ref rsRelationData, out pathFound, out nodeErrorMessage);
                                    }

                                    if (pathFound)
                                    {
                                        success = true;
                                    }
                                }
                            }

                            if (!success)
                            {
                                errorMessage = $"from 문(id: {id})의 경로를 찾을 수 없습니다."; //  경로: {errorMessage}
                                errorCharNumber = match.Index + 1;
                                break;
                            }

                            int leftSpace = content.LastIndexOf("\r\n", match.Index);
                            leftSpace = leftSpace == -1 ? match.Index : match.Index - leftSpace - 2;
                            rsFromString = TranslateCommands_GetFromString(rsRelationData, tablesRows, leftSpace);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            errorMessage = $"from 문의 아이디({id})를 찾을 수 없습니다.";
                            errorCharNumber = match.Groups[1].Index + match.Groups[1].Length + 1;
                        }
                    }

                    // Replace N Set rsFromStringLength
                    rsFromStringLength = rsFromString.Length;
                    if (rsFromStringLength > 0)
                    {
                        content = content.Remove(match.Index, match.Length)
                                         .Insert(match.Index, rsFromString);
                    }
                }

                match = regx.Match(content, match.Index + (rsFromStringLength == 0 ? match.Length : rsFromStringLength));
            }
            while (match.Success);

            return content;
        }

        private void TranslateCommands_GetPath(DataRow originRow, DataRow destinationRow, DataTable relationTable, ref DataTable rsRelationData, out bool success, out string errorMessage)
        {
            string originTable = originRow[NODE.NODE_ID_REF].ToString();
            string originTableSeq = originRow[NODE.NODE_SEQ_REF].ToString();
            string destinationTable = destinationRow[NODE.NODE_ID_REF].ToString();
            string destinationTableSeq = destinationRow[NODE.NODE_SEQ_REF].ToString();
            success = TranslateCommands_GetPathProc(originTable, originTableSeq, destinationTable, destinationTableSeq, relationTable, ref rsRelationData, out errorMessage);
        }

        private bool TranslateCommands_GetPathProc(string originTable, string originTableSeq, string destinationTable, string destinationTableSeq, DataTable relationTable, ref DataTable rsRelationData, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool rs = false;

            // Get path
            DataRow[] path = relationTable.Select($"{RELATION.NODE_ID2} = '{originTable}' AND {RELATION.NODE_SEQ2} = '{originTableSeq}'");

            // Get destinationTables
            // 0: Table, 1: TableAlias
            List<string[]> destinationTables = new List<string[]>();
            foreach (DataRow row in path)
            {
                string table = row[RELATION.NODE_ID1].ToString();
                string tableSeq = row[RELATION.NODE_SEQ1].ToString();

                if (!TranslateCommands_Fnc_ContainsTableInfo(destinationTables, table, tableSeq))
                {
                    destinationTables.Add(new string[] { table, tableSeq });
                }
            }

            if (destinationTables.Count > 0)
            {
                // Success
                if (TranslateCommands_Fnc_ContainsTableInfo(destinationTables, destinationTable, destinationTableSeq))
                {
                    DataRow[] rsPaths = relationTable.Select($"{RELATION.NODE_ID2} = '{originTable}' AND {RELATION.NODE_SEQ2} = {originTableSeq} AND {RELATION.NODE_ID1} = '{destinationTable}' AND {RELATION.NODE_SEQ1} = {destinationTableSeq}");
                    foreach(DataRow rsPath in rsPaths)
                    {
                        rsRelationData.Rows.Add(rsPath.ItemArray);
                    }
                    rs = true;
                }
                // Search Path
                else
                {
                    for (int i = 0; i < destinationTables.Count; i++)
                    {
                        DataTable rsRelationDataNode = rsRelationData.Clone();
                        bool found = TranslateCommands_GetPathProc(destinationTables[i][0], destinationTables[i][1], destinationTable, destinationTableSeq, relationTable, ref rsRelationDataNode, out errorMessage);
                        if (found)
                        {
                            DataRow[] rsPathsPrev = relationTable.Select($"{RELATION.NODE_ID2} = '{originTable}' AND {RELATION.NODE_SEQ2} = '{originTableSeq}' AND {RELATION.NODE_ID1} = '{destinationTables[i][0]}' AND {RELATION.NODE_SEQ1} = '{destinationTables[i][1]}'");
                            foreach (DataRow rsPath in rsPathsPrev)
                            {
                                rsRelationData.Rows.Add(rsPath.ItemArray);
                            }

                            foreach (DataRow rowPath in rsRelationDataNode.Rows)
                            {
                                rsRelationData.Rows.Add(rowPath.ItemArray);
                            }

                            rs = true;
                        }
                    }

                    /*
                    DataTable rsRelationDataNode = rsRelationData.Clone();
                    int iResult = -1;

                    for (int i = 0; i < destinationTables.Count; i++)
                    {
                        DataTable rsRelationDataNodeTemp = rsRelationDataNode.Clone();
                        bool found = TranslateCommands_GetPathProc(destinationTables[i][0], destinationTables[i][1], destinationTable, destinationTableSeq, relationTable, ref rsRelationDataNodeTemp);
                        if (found)
                        {
                            if (rsRelationDataNode.Rows.Count == 0)
                            {
                                rsRelationDataNode = rsRelationDataNodeTemp;
                                iResult = i;
                            }
                            else
                            {
                                if (rsRelationDataNodeTemp.Rows.Count < rsRelationDataNode.Rows.Count)
                                {
                                    rsRelationDataNode = rsRelationDataNodeTemp;
                                    iResult = i;
                                }
                            }
                        }
                    }

                    if (iResult >= 0)
                    {
                        DataRow[] rsPathsPrev = relationTable.Select($"{RelationDataTableFieldName1} = '{originTable}' AND {RelationDataTableSeqFieldName1} = '{originTableSeq}' AND {RelationDataTableFieldName2} = '{destinationTables[iResult][0]}' AND {RelationDataTableSeqFieldName2} = '{destinationTables[iResult][1]}'");
                        foreach (DataRow rsPath in rsPathsPrev)
                        {
                            rsRelationData.Rows.Add(rsPath.ItemArray);
                        }

                        foreach(DataRow rowPath in rsRelationDataNode.Rows)
                        {
                            rsRelationData.Rows.Add(rowPath.ItemArray);
                        }

                        rs = true;
                    }
                    */
                }
            }
            // Not found
            else
            {
                errorMessage = $"{destinationTable}({destinationTableSeq}) <- {originTable}({originTableSeq})";
                rs = false;
            }

            return rs;
        }

        private string TranslateCommands_GetFromString(DataTable relationData, DataRow[] tablesRows, int leftSpaceCount)
        {
            string rs = string.Empty;
            string leftSpace = new string(' ', leftSpaceCount);
            StringBuilder sb = new StringBuilder();
            bool fail = false;

            // Sort relationData
            relationData.DefaultView.Sort = $"{RELATION.NODE_ID2}, {RELATION.NODE_SEQ2}, {RELATION.NODE_ID1}, {RELATION.NODE_SEQ1}, {RELATION.NODE_DETAIL_ORDER1}";
            relationData = relationData.DefaultView.ToTable(true, new string[] { RELATION.NODE_ID2, RELATION.NODE_SEQ2, RELATION.NODE_DETAIL_TABLE_ALIAS2, RELATION.NODE_DETAIL_ID2, RELATION.NODE_ID1, RELATION.NODE_SEQ1, RELATION.NODE_DETAIL_TABLE_ALIAS1, RELATION.NODE_DETAIL_ID1, RELATION.NODE_DETAIL_ORDER1, RELATION.RELATION_TYPE, RELATION.RELATION_OPERATOR, RELATION.RELATION_VALUE });

            // Get mainTableInfo
            // 0: Table, 1: TableSeq
            string[] mainTableInfo;
            List<string[]> _originTables = new List<string[]>();
            foreach(DataRow relationRow in relationData.Rows)
            {
                string[] originTableInfo = new string[] { relationRow[RELATION.NODE_ID2].ToString(), relationRow[RELATION.NODE_SEQ2].ToString() };
                if (!TranslateCommands_Fnc_ContainsTableInfo(_originTables, originTableInfo[0], originTableInfo[1]))
                {
                    _originTables.Add(new string[] { originTableInfo[0], originTableInfo[1] } );
                }
            }
            foreach (DataRow relationRow in relationData.Rows)
            {
                string[] destinationTableInfo = new string[] { relationRow[RELATION.NODE_ID1].ToString(), relationRow[RELATION.NODE_SEQ1].ToString() };
                string[] outKey;
                if (TranslateCommands_Fnc_ContainsTableInfo(_originTables, destinationTableInfo[0], destinationTableInfo[1], out outKey))
                {
                    _originTables.Remove(outKey);
                }
            }
            mainTableInfo = _originTables.Count == 1 ? _originTables[0] : null;

            // Set sb
            if (mainTableInfo != null)
            {
                int relationDataCount = relationData.Rows.Count;

                // Result Proc #1
                sb.Append($"FROM {mainTableInfo[0]} AS {TranslateCommands_GetFromString_GetTableAlias(tablesRows, relationData, mainTableInfo[0], mainTableInfo[1])}\r\n");

                List<string[]> originTableInfoList = new List<string[]>();
                originTableInfoList.Add(new string[] { mainTableInfo[0], mainTableInfo[1] });

                for(int i = 0; i < originTableInfoList.Count; i++)
                {
                    string[] originTableInfo = originTableInfoList[i];
                    DataRow[] nodeRows = relationData.Select($"{RELATION.NODE_ID2} = '{originTableInfo[0]}' AND {RELATION.NODE_SEQ2} = '{originTableInfo[1]}'");

                    // Get nodeDestinationTablesCount
                    Dictionary<string[], int> nodeDestinationTablesCount = new Dictionary<string[], int>();
                    foreach(DataRow nodeRow in nodeRows)
                    {
                        string[] nodeDestinationTableInfo = new string[] { nodeRow[RELATION.NODE_ID1].ToString(), nodeRow[RELATION.NODE_SEQ1].ToString() };
                        string[] outKey = null;
                        if (!TranslateCommands_Fnc_ContainsTableInfo(nodeDestinationTablesCount, nodeDestinationTableInfo[0], nodeDestinationTableInfo[1], out outKey))
                        {
                            nodeDestinationTablesCount.Add(outKey, 1);
                        }
                        else
                        {
                            nodeDestinationTablesCount[outKey]++;
                        }
                    }

                    // Append sb N Remove relationData rows
                    foreach (KeyValuePair<string[], int> pair in nodeDestinationTablesCount)
                    {
                        DataRow[] targetRelationRows = relationData.Select($"{RELATION.NODE_ID1} = '{pair.Key[0]}' AND {RELATION.NODE_SEQ1} = '{pair.Key[1]}'");

                        // Append sb N Remove relationData rows
                        if (targetRelationRows.Length == pair.Value)
                        {
                            TranslateCommands_GetFromString_AppendSbNRemoveRelationData(ref sb, ref relationData, tablesRows, targetRelationRows, originTableInfo, leftSpace, pair);
                            originTableInfoList.Add(pair.Key);
                        }
                        // Reservation Or Append N Remove Rows
                        else
                        {
                            // Block overflow
                            if (relationDataCount < i)
                            {
                                fail = true;
                                break;
                            }
                            else
                            {
                                bool isIncludedOtherTables = true;

                                // Get isIncludedOtherTables
                                string oldTargetTableNode = string.Empty;
                                foreach(DataRow targetRow in targetRelationRows)
                                {
                                    string targetTableNode = targetRow[RELATION.NODE_ID2].ToString();
                                    if (targetTableNode != oldTargetTableNode)
                                    {
                                        if (sb.IndexOf($" {targetTableNode} ") < 0)
                                        {
                                            isIncludedOtherTables = false;
                                            break;
                                        }
                                    }
                                }

                                if (isIncludedOtherTables)
                                {
                                    TranslateCommands_GetFromString_AppendSbNRemoveRelationData(ref sb, ref relationData, tablesRows, targetRelationRows, originTableInfo, leftSpace, pair);
                                }

                                originTableInfoList.Add(pair.Key);
                            }
                        }
                    }

                    if (fail)
                    {
                        break;
                    }
                }
            }
            else
            {
                fail = true;
            }

            if (!fail)
            {
                rs = sb.ToString().TrimEnd();
            }

            return rs;
        }

        private void TranslateCommands_GetFromString_AppendSbNRemoveRelationData(ref StringBuilder sb, ref DataTable relationData, DataRow[] tablesRows, DataRow[] targetRelationRows, string[] originTableInfo, string leftSpace, KeyValuePair<string[], int> pair)
        {
            string originAlias = TranslateCommands_GetFromString_GetTableAlias(tablesRows, relationData, originTableInfo[0], originTableInfo[1]);
            string targetAlias = TranslateCommands_GetFromString_GetTableAlias(tablesRows, relationData, pair.Key[0], pair.Key[1]);

            // Append join string to sb
            sb.Append($"{leftSpace}{TranslateCommands_GetJoinType(targetRelationRows[0][RELATION.RELATION_TYPE].ToString())} {targetRelationRows[0][RELATION.NODE_ID1]} AS {targetAlias}\r\n");

            // Add N Remove
            for (int k = 0; k < targetRelationRows.Length; k++)
            {
                // Append condition string to sb
                DataRow targetRow = targetRelationRows[k];

                // Get originAliasNode
                string originAliasNode = originAlias;
                string[] targetRelationRowOriginInfo = new string[] { targetRow[RELATION.NODE_ID2].ToString(), targetRow[RELATION.NODE_SEQ2].ToString() };
                if (!(originTableInfo[0] == targetRelationRowOriginInfo[0]
                &&    originTableInfo[1] == targetRelationRowOriginInfo[1]))
                {
                    originAliasNode = TranslateCommands_GetFromString_GetTableAlias(tablesRows, relationData, targetRelationRowOriginInfo[0], targetRelationRowOriginInfo[1]);
                }

                // Append to sb
                string joinOnOrAnd = k == 0 ? "ON" : "AND";
                string joinOperator = targetRow[RELATION.RELATION_OPERATOR] == null || string.IsNullOrWhiteSpace(targetRow[RELATION.RELATION_OPERATOR].ToString())
                        ? "=" : targetRow[RELATION.RELATION_OPERATOR].ToString();
                if (string.IsNullOrWhiteSpace(targetRow[RELATION.NODE_DETAIL_ID2].ToString()))
                {
                    // Using {targetRow[RelationDataValueFieldName]}
                    sb.Append($"{leftSpace}    {joinOnOrAnd} {targetAlias}.{targetRow[RELATION.NODE_DETAIL_ID1]} {joinOperator} {targetRow[RELATION.RELATION_VALUE]}\r\n");
                }
                else
                {
                    // Using {originAliasNode}.{targetRow[RelationDataColumnFieldName1]}
                    sb.Append($"{leftSpace}    {joinOnOrAnd} {targetAlias}.{targetRow[RELATION.NODE_DETAIL_ID1]} {joinOperator} {originAliasNode}.{targetRow[RELATION.NODE_DETAIL_ID2]}\r\n");
                }

                // Remove targetRow in targetRow
                relationData.Rows.Remove(targetRow);
            }
        }

        private string TranslateCommands_GetFromString_GetTableAlias(DataRow[] tablesRows, DataTable relationData, string table, string tableSeq)
        {
            string rsAlias = string.Empty;

            foreach(DataRow tableRow in tablesRows)
            {
                if (tableRow[NODE.NODE_ID_REF].ToString() == table
                &&  tableRow[NODE.NODE_SEQ_REF].ToString() == tableSeq)
                {
                    rsAlias = tableRow[NODE.NODE_DETAIL_TABLE_ALIAS].ToString();
                    break;
                }
            }

            if (rsAlias == string.Empty)
            {
                DataRow[] rsRows2 = relationData.Select($"{RELATION.NODE_ID1} = '{table}' AND {RELATION.NODE_SEQ1} = {tableSeq}");
                if (rsRows2.Length > 0)
                {
                    rsAlias = rsRows2[0][RELATION.NODE_DETAIL_TABLE_ALIAS1].ToString();
                }
                else
                {
                    DataRow[] rsRows1 = relationData.Select($"{RELATION.NODE_ID2} = '{table}' AND {RELATION.NODE_SEQ2} = {tableSeq}");
                    if (rsRows1.Length > 0)
                    {
                        rsAlias = rsRows1[0][RELATION.NODE_DETAIL_TABLE_ALIAS2].ToString();
                    }
                    else
                    {
                        throw new Exception($"Error - TranslateCommands_GetFromString_GetTableAlias - table: {table}, tableSeq: {tableSeq}");
                    }
                }
            }

            return rsAlias;
        }
        
        private bool TranslateCommands_Fnc_ContainsTableInfo(List<string[]> tables, string table, string tableSeq)
        {
            bool rsContains = false;

            foreach (string[] node in tables)
            {
                if (node[0] == table && node[1] == tableSeq)
                {
                    rsContains = true;
                    break;
                }
            }

            return rsContains;
        }

        private bool TranslateCommands_Fnc_ContainsTableInfo(List<string[]> tables, string table, string tableSeq, out string[] outKey)
        {
            outKey = null;
            bool rsContains = false;

            foreach (string[] node in tables)
            {
                if (node[0] == table && node[1] == tableSeq)
                {
                    outKey = node;
                    rsContains = true;
                    break;
                }
            }

            return rsContains;
        }

        private bool TranslateCommands_Fnc_ContainsTableInfo(Dictionary<string[], int> tables, string table, string tableSeq, out string[] containsKey)
        {
            containsKey = null;
            bool rsContains = false;

            foreach (KeyValuePair<string[], int> node in tables)
            {
                if (node.Key[0] == table && node.Key[1] == tableSeq)
                {
                    rsContains = true;
                    containsKey = node.Key;
                    break;
                }
            }

            if (containsKey == null)
            {
                containsKey = new string[] { table, tableSeq };
            }

            return rsContains;
        }

        private string TranslateCommands_GetJoinType(string type)
        {
            string rs;
            switch (type)
            {
                case "I": rs = "JOIN"; break;
                case "L": rs = "LEFT JOIN"; break;
                case "F": rs = "FULL JOIN"; break;
                default:
                    throw new Exception($"TemplateProcessor - TranslateCommandsProcGetJoinType(string type) - invaild [type] value : {type}");
            }
            return rs;
        }
        #endregion

        #region TranslateCommands - Etc
        private string TranslateHardCodedLoopValues(string text, Dictionary<string, DataTable> columnTableDic)
        {
            List<Range> noneLoopValueRangeList = null;
            List<string> noneLoopValueVariableList = null;
            int lineIndex = -1;
            int lineStartIndex = -1;
            int checkingRangeListCount = -1;
            Dictionary<int, int> removedLIneDic = null;
            Result result = null;
            return TranslateHardCodedLoopValues(text, columnTableDic, ref noneLoopValueRangeList, ref noneLoopValueVariableList, lineIndex, lineStartIndex, checkingRangeListCount, ref removedLIneDic, ref result);
        }

        private string TranslateHardCodedLoopValues(string text, Dictionary<string, DataTable> columnTableDic, ref List<Range> noneLoopValueRangeList, ref List<string> noneLoopValueVariableList, int lineIndex, int lineStartIndex, int checkingRangeListCount, ref Dictionary<int, int> removedLIneDic, ref Result result)
        {
            if (columnTableDic != null)
            {
                Regex regx = new Regex("{ *id *: *([A-Za-z0-9_]*) *, *([0-9]*) *, *([A-Za-z0-9_]*) *}");
                Match match = regx.Match(text);
                do
                {
                    int rsStringLength = 0;
                    if (match.Success)
                    {
                        string id = match.Groups[1].Value;
                        string columnName = match.Groups[3].Value;
                        int columnIndex = -1;

                        if (!columnTableDic.ContainsKey(id))
                        {
                            if (result != null)
                            {
                                int errorLine = GetRawLineIndex(removedLIneDic, lineIndex) + 1;
                                result.ExceptionList.Add(GetException($"ID를 찾을 수 없습니다. (입력된 ID: {id})", errorLine, match.Groups[1].Index + match.Groups[1].Value.Length + 1));
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(match.Groups[2].Value))
                        {
                            if (result != null)
                            {
                                int errorLine = GetRawLineIndex(removedLIneDic, lineIndex) + 1;
                                result.ExceptionList.Add(GetException("Row Index 값이 없습니다.", errorLine, match.Groups[2].Index + 1));
                            }
                        }
                        else if (!(match.Groups[2].Value.ToInt() < columnTableDic[id].Rows.Count))
                        {
                            if (result != null)
                            {
                                int errorLine = GetRawLineIndex(removedLIneDic, lineIndex) + 1;
                                result.ExceptionList.Add(GetException($"Row Index 값이 범위를 벗어났습니다. (최대값: {columnTableDic[id].Rows.Count - 1})", errorLine, match.Groups[2].Index + match.Groups[2].Value.Length + 1));
                            }
                        }
                        else if (!columnTableDic[id].Columns.Contains(columnName)
                             && (!(int.TryParse(columnName, out columnIndex) && columnIndex < columnTableDic[id].Columns.Count)))
                        {
                            if (result != null)
                            {
                                int errorLine = GetRawLineIndex(removedLIneDic, lineIndex) + 1;
                                if (string.IsNullOrWhiteSpace(match.Groups[3].Value))
                                {
                                    result.ExceptionList.Add(GetException($"컬럼명이 입력되지 않았습니다.", errorLine, match.Groups[3].Index + 1));
                                }
                                else if (columnName.IsInt())
                                {
                                    result.ExceptionList.Add(GetException($"컬럼 Index 값이 범위를 벗어났습니다. (최대값: {columnTableDic[id].Columns.Count - 1})", errorLine, match.Groups[3].Index + 1));
                                }
                                else
                                {
                                    result.ExceptionList.Add(GetException($"컬럼명이 존재하지 않습니다. (입력된 컬럼명: {columnName})", errorLine, match.Groups[3].Index + match.Groups[3].Value.Length + 1));
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = match.Groups[2].Value.ToInt();
                            string rsString = columnIndex >= 0
                                ? columnTableDic[id].Rows[rowIndex][columnIndex].ToString()
                                : columnTableDic[id].Rows[rowIndex][columnName].ToString();
                            rsStringLength = rsString.Length;

                            // Replace N Set rsStringLength
                            text = text.Remove(match.Index, match.Length)
                                        .Insert(match.Index, rsString);

                            int insertIndex = noneLoopValueRangeList.Count;

                            if (noneLoopValueRangeList != null)
                            {
                                int difference = rsStringLength - match.Length;
                                int noneLoopValueRangeListCount = noneLoopValueRangeList.Count;
                                for (int i = noneLoopValueRangeListCount - 1; noneLoopValueRangeListCount - checkingRangeListCount <= i; i--)
                                {
                                    if (match.Index < noneLoopValueRangeList[i].StartingValue)
                                    {
                                        noneLoopValueRangeList[i].StartingValue += difference;
                                        noneLoopValueRangeList[i].EndValue += difference;
                                    }
                                    else
                                    {
                                        insertIndex = i + 1;
                                        break;
                                    }
                                }
                            }

                            noneLoopValueRangeList.Insert(insertIndex, new Range(match.Index + lineStartIndex, match.Index + rsStringLength + lineStartIndex - 1));
                            noneLoopValueVariableList.Insert(insertIndex, match.Value);
                        }
                    }

                    int nextSp = match.Index + (rsStringLength == 0 ? match.Length : rsStringLength);
                    if (nextSp < text.Length)
                    {
                        match = regx.Match(text, nextSp);
                    }
                    else
                    {
                        break;
                    }
                }
                while (match.Success);
            }

            return text;
        }
        #endregion

        #region Function
        private TemplateProcessorException GetException(string message, int lineNumber, int charNumber)
        {
            return new TemplateProcessorException($"{message}")
            {
                LineNumber = lineNumber,
                CharNumber = charNumber
            };
        }

        private void OffsetResultNoneLoopValueRangeList(int rowStartIndex, ref Result result, int offset)
        {
            if (result.NoneLoopValueRangeList != null)
            {
                foreach (var node in result.NoneLoopValueRangeList)
                {
                    if (rowStartIndex <= node.StartingValue)
                    {
                        node.Offset(offset);
                    }
                }
            }
        }

        private List<Range> GetBody_GetLoopLineBlockNSetLoopTextRangeList(ref List<Range> loopTextRangeList, ref List<Range> noneLoopValueRangeList, ref List<Range> loopValueRangeList, ref string loopNode, int lineCharIndex)
        {
            List<Range> rsLoopLineBlock = new List<Range>();

            // Set rsLoopLineBlock - from LoopValueRangeList
            for (int i = loopValueRangeList.Count - 1; i >= 0; i--)
            {
                if (lineCharIndex <= loopValueRangeList[i].StartingValue)
                {
                    rsLoopLineBlock.Insert(0, loopValueRangeList[i]);
                }
                else
                {
                    break;
                }
            }

            // Set rsLoopLineBlock - from NoneLoopValueRangeList
            for (int i = noneLoopValueRangeList.Count - 1; i >= 0; i--)
            {
                Range targetRange = noneLoopValueRangeList[i];
                if (lineCharIndex <= targetRange.StartingValue)
                {
                    for (int k = 0; k < rsLoopLineBlock.Count; k++)
                    {
                        // First
                        if (k == 0 && targetRange.EndValue < rsLoopLineBlock[k].StartingValue)
                        {
                            rsLoopLineBlock.Insert(k, targetRange);
                            break;
                        }
                        // Last
                        else if (k == rsLoopLineBlock.Count - 1 && targetRange.EndValue < rsLoopLineBlock[k].StartingValue)
                        {
                            rsLoopLineBlock.Add(targetRange);
                            break;
                        }
                        // Middle
                        else
                        {
                            if (rsLoopLineBlock[k].EndValue < targetRange.StartingValue
                            && noneLoopValueRangeList[i].EndValue < rsLoopLineBlock[k + 1].StartingValue)
                            {
                                rsLoopLineBlock.Insert(k + 1, noneLoopValueRangeList[i]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            // Set rsLoopLineBlock - from nomal text
            // Set loopTextRangeList - from nomal text
            for (int i = 0; i < rsLoopLineBlock.Count; i++)
            {
                Range node = rsLoopLineBlock[i];

                // First
                if (i == 0 && node.StartingValue != lineCharIndex)
                {
                    Range addedRange = new Range(lineCharIndex, node.StartingValue - 1);
                    rsLoopLineBlock.Insert(0, addedRange);
                    i++;
                    node = rsLoopLineBlock[i];
                    loopTextRangeList.Add(addedRange);
                }

                // Last
                if (i == rsLoopLineBlock.Count - 1)
                {
                    if (node.EndValue != lineCharIndex + loopNode.Length - 1 - 2)
                    {
                        Range addedRange = new Range(node.EndValue + 1, lineCharIndex + loopNode.Length - 1);
                        rsLoopLineBlock.Add(addedRange);
                        i++;
                        loopTextRangeList.Add(addedRange);
                    }
                }
                // Not last
                else
                {
                    if (node.EndValue + 1 != rsLoopLineBlock[i + 1].StartingValue)
                    {
                        Range addedRange = new Range(node.EndValue + 1, rsLoopLineBlock[i + 1].StartingValue - 1);
                        rsLoopLineBlock.Insert(i + 1, addedRange);
                        i++;
                        loopTextRangeList.Add(addedRange);
                    }
                }
            }

            return rsLoopLineBlock;
        }

        private bool GetConditionResult(string commandText, string condition, Dictionary<string, string> valueDic = null, DataRow columnRow = null)
        {
            return GetConditionResult(commandText, condition, out _, out _, valueDic, columnRow);
        }

        private bool GetConditionResult(string commandText, string condition, out string errorMessage, out int errorCharNumber, Dictionary<string, string> valueDic = null, DataRow columnRow = null)
        {
            bool rs = false;
            string comparisonText, leftText, rightText;

            bool validateRs = ValidateCondition(commandText, condition, out errorMessage, out errorCharNumber, out comparisonText, out leftText, out rightText);
            if (validateRs)
            {
                if (valueDic != null)
                {
                    leftText = leftText.ReplaceByKey(valueDic, "{", "}");
                    rightText = rightText.ReplaceByKey(valueDic, "{", "}");
                }

                if (columnRow != null)
                {
                    foreach (DataColumn column in columnRow.Table.Columns)
                    {
                        leftText = leftText.Replace($"{{{column.ColumnName}}}", columnRow[column.ColumnName].ToString()).Trim();
                        rightText = rightText.Replace($"{{{column.ColumnName}}}", columnRow[column.ColumnName].ToString()).Trim();
                    }
                }

                if ((comparisonText == OPERATOR.EQUALS && leftText.Trim() == rightText.Trim())
                || (comparisonText == OPERATOR.NOT_EQUALS && leftText.Trim() != rightText.Trim()))
                {
                    rs = true;
                }
            }

            return rs;
        }

        private bool ValidateCondition(string commandText, string condition, out string errorMessage, out int errorCharNumber)
        {
            return ValidateCondition(commandText, condition, out errorMessage, out errorCharNumber, out _, out _, out _);
        }

        private bool ValidateCondition(string commandText, string condition, out string errorMessage, out int errorCharNumber, out string comparisonText, out string leftText, out string rightText)
        {
            bool rs = false;
            errorMessage = string.Empty;
            errorCharNumber = 0;
            comparisonText = string.Empty;
            leftText = string.Empty;
            rightText = string.Empty;

            int equalsTextSp = condition.IndexOf(OPERATOR.EQUALS);
            int notEqualsTextSp = condition.IndexOf(OPERATOR.NOT_EQUALS);
            if (equalsTextSp >= 0 || notEqualsTextSp >= 0)
            {
                int comparisonTextSp = -1;

                // Equals Proc
                if ((equalsTextSp >= 0 && notEqualsTextSp >= 0 && equalsTextSp < notEqualsTextSp)
                || (equalsTextSp >= 0 && notEqualsTextSp < 0))
                {
                    comparisonText = OPERATOR.EQUALS;
                    comparisonTextSp = equalsTextSp;
                }
                // Not Equals Proc
                else if ((equalsTextSp >= 0 && notEqualsTextSp >= 0 && notEqualsTextSp < equalsTextSp)
                || (notEqualsTextSp >= 0 && equalsTextSp < 0))
                {
                    comparisonText = OPERATOR.NOT_EQUALS;
                    comparisonTextSp = notEqualsTextSp;
                }

                if (comparisonTextSp >= 0)
                {
                    leftText = condition.Substring(0, comparisonTextSp);
                    rightText = condition.Substring(comparisonTextSp + 2, condition.Length - (comparisonTextSp + 2));
                    if (!string.IsNullOrWhiteSpace(leftText) && !string.IsNullOrWhiteSpace(rightText))
                    {
                        rs = true;
                    }
                    else
                    {
                        errorCharNumber = 1;
                        if (string.IsNullOrWhiteSpace(leftText))
                        {
                            errorCharNumber = commandText.Length + comparisonTextSp + 1 - leftText.Length;
                        }
                        else if (string.IsNullOrWhiteSpace(rightText))
                        {
                            errorCharNumber = commandText.Length + leftText.Length + comparisonText.Length + rightText.Length + 1;
                        }

                        errorMessage = $"{commandText} 문의 비교 조건이 올바르지 않습니다.";
                    }
                }
                else
                {
                    errorCharNumber = 3;
                    errorMessage = $"{commandText} 문이 올바르지 않습니다.";
                }
            }
            else
            {
                errorCharNumber = 3;
                errorMessage = $"{commandText} 문에 비교 연산자가 존재하지 않습니다.";
            }

            return rs;
        }
        #endregion
    }
}