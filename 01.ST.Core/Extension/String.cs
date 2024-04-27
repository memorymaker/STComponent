using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ST.Core.Extension
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 문자열의 바이트 수를 반환합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetByteCount(this string text)
        {
            return Encoding.Default.GetByteCount(text);
        }

        /// <summary>
        /// 이 문자열 내에 searchText가 발견된 수를 반환합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static int GetContainsCount(this string text, string searchText)
        {
            int rs = 0;
            int searchTextLength = searchText.Length;
            if (searchTextLength > 0)
            {
                int sp = 0;
                do
                {
                    sp = text.IndexOf(searchText, sp);
                    if (sp >= 0)
                    {
                        sp += searchTextLength;
                        rs++;
                    }
                }
                while (sp >= 0);
            }
            return rs;
        }

        /// <summary>
        /// 현재 인스턴스의 지정된 유니코드 문자 배열이 지정된 다른 유니코드 문자로 모두 바뀌는 새 문자열을 반환합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="oldChars"></param>
        /// <param name="newChar"></param>
        /// <returns></returns>
        public static string Replace(this string text, char[] oldChars, char newChar)
        {
            StringBuilder rs = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char charNode = text[i];
                bool found = false;
                for (int k = 0; k < oldChars.Length; k++)
                {
                    if (charNode == oldChars[k])
                    {
                        rs.Append(newChar);
                        found = true;
                    }
                }

                if (!found)
                {
                    rs.Append(charNode);
                }
            }
            return rs.ToString();
        }

        #region ReplaceByKey
        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자가 Value 문자로 모두 바뀌는 새 문자열을 반환합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary)
        {
            List<Range> replacedRanges = null;
            List<string> replacedKeys = null;
            return ReplaceByKeyProc(text, replaceDictionary, string.Empty, string.Empty, ref replacedRanges, ref replacedKeys);
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자가 Value 문자로 모두 바뀌는 새 문자열을 반환합니다. replacedRanges 파라미터에 바뀐 문자열들의 시작 Index와 마지막 Index를 저장합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="replacedRanges"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, ref List<Range> replacedRanges)
        {
            List<string> replacedKeys = null;
            return ReplaceByKeyProc(text, replaceDictionary, string.Empty, string.Empty, ref replacedRanges, ref replacedKeys);
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자에 앞 뒤로 keyFrontText, keyRearText를 추가한 후 Value 문자로 모두 바뀌는 새 문자열을 반환합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="replacedRanges"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, string keyFrontText, string keyRearText)
        {
            List<Range> replacedRanges = null;
            List<string> replacedKeys = null;
            return ReplaceByKeyProc(text, replaceDictionary, keyFrontText ?? string.Empty, keyRearText ?? string.Empty, ref replacedRanges, ref replacedKeys);
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자에 앞 뒤로 keyFrontText, keyRearText를 추가한 후 Value 문자로 모두 바뀌는 새 문자열을 반환합니다. replacedRanges 파라미터에 바뀐 문자열들의 시작 Index와 마지막 Index를 저장합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="replacedRanges"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, string keyFrontText, string keyRearText, ref List<Range> replacedRanges)
        {
            List<string> replacedKeys = null;
            return ReplaceByKeyProc(text, replaceDictionary, keyFrontText ?? string.Empty, keyRearText ?? string.Empty, ref replacedRanges, ref replacedKeys);
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자에 앞 뒤로 keyFrontText, keyRearText를 추가한 후 Value 문자로 모두 바뀌는 새 문자열을 반환합니다. replacedRanges 파라미터에 변경된 문자열들의 시작 Index와 마지막 Index를 저장합니다. replacedKeys 파라미터에 replacedRanges와 매핑 가등한 변경된 문자열들의 변수 명을 저장합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="keyFrontText"></param>
        /// <param name="keyRearText"></param>
        /// <param name="replacedRanges"></param>
        /// <param name="replacedKeys"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, string keyFrontText, string keyRearText, ref List<Range> replacedRanges, ref List<string> replacedKeys)
        {
            return ReplaceByKeyProc(text, replaceDictionary, keyFrontText ?? string.Empty, keyRearText ?? string.Empty, ref replacedRanges, ref replacedKeys);
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key 문자에 앞 뒤로 keyFrontText, keyRearText를 추가한 후 Value 문자로 모두 바뀌는 새 문자열을 반환합니다. replacedRanges 파라미터에 변경된 문자열들의 시작 Index와 마지막 Index를 저장합니다. replacedKeys 파라미터에 replacedRanges와 매핑 가등한 변경된 문자열들의 변수 명을 저장합니다. resultIndexOffset 파라미터로 replacedRanges의 값을 보정합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="keyFrontText"></param>
        /// <param name="keyRearText"></param>
        /// <param name="replacedRanges"></param>
        /// <param name="replacedKeys"></param>
        /// <param name="resultIndexOffset"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, string keyFrontText, string keyRearText, ref List<Range> replacedRanges, ref List<string> replacedKeys, int resultIndexOffset)
        {
            return ReplaceByKeyProc(text, replaceDictionary, keyFrontText ?? string.Empty, keyRearText ?? string.Empty, ref replacedRanges, ref replacedKeys, resultIndexOffset);
        }

        private static string ReplaceByKeyProc(string text, Dictionary<string, string> replaceDictionary, string keyFrontText, string keyRearText, ref List<Range> replacedRanges, ref List<string> replacedKeys, int resultIndexOffset = 0)
        {
            StringBuilder sb = new StringBuilder(text);

            Dictionary<string, string> _replaceDictionary;
            if (!string.IsNullOrEmpty(keyFrontText) || !string.IsNullOrEmpty(keyRearText))
            {
                _replaceDictionary = new Dictionary<string, string>();
                foreach(var pair in replaceDictionary)
                {
                    _replaceDictionary.Add($"{keyFrontText}{pair.Key}{keyRearText}", pair.Value);
                }
            }
            else
            {
                _replaceDictionary = replaceDictionary;
            }

            List<string> keys = _replaceDictionary.Keys.ToList();
            if (keys.Contains(string.Empty))
            {
                keys.Remove(string.Empty);
            }
            int keysLength = keys.Count;
            int[] matchedLength = new int[keysLength];

            for (int i = 0; i < sb.Length; i++)
            {
                for (int k = 0; k < keysLength; k++)
                {
                    if (sb[i] == keys[k][matchedLength[k]])
                    {
                        matchedLength[k]++;
                        if (keys[k].Length == matchedLength[k])
                        {
                            // Repace Node
                            int sp = i - (matchedLength[k] - 1);
                            sb.Remove(sp, matchedLength[k]);
                            sb.Insert(sp, _replaceDictionary[keys[k]]);
                            
                            // Set replacedRanges
                            if (replacedRanges != null)
                            {
                                replacedRanges.Add(new Range(sp + resultIndexOffset, sp + _replaceDictionary[keys[k]].Length - 1 + resultIndexOffset));
                            }

                            // Set replacedKeys
                            if (replacedKeys != null)
                            {
                                replacedKeys.Add(keys[k]);
                            }

                            // Set i
                            int lengthDifference = _replaceDictionary[keys[k]].Length - matchedLength[k];
                            if (lengthDifference != 0)
                            {
                                i = i + lengthDifference;
                            }

                            // Clear
                            for(int m = 0; m < keysLength; m++)
                            {
                                matchedLength[m] = 0;
                            }
                        }
                    }
                    else
                    {
                        matchedLength[k] = 0;
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 현재 지정된 인스턴스의 Dictionary Key, DataRow Column 문자에 앞 뒤로 keyFrontText, keyRearText를 추가한 후 Value 문자로 모두 바뀌는 새 문자열을 반환합니다. replacedDictionaryRanges, replacedRowRanges 파라미터에 변경된 문자열들의 시작 Index와 마지막 Index를 저장합니다. replacedDictionaryKeys, replacedRowKeys 파라미터에 replacedDictionaryRanges, replacedRowRanges와 매핑 가등한 변경된 문자열들의 변수 명을 저장합니다. resultIndexOffset 파라미터로 replacedDictionaryRanges, replacedRowRanges의 값을 보정합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replaceDictionary"></param>
        /// <param name="replaceRow"></param>
        /// <param name="keyFrontText"></param>
        /// <param name="keyRearText"></param>
        /// <param name="replacedDictionaryRanges"></param>
        /// <param name="replacedDictionaryKeys"></param>
        /// <param name="replacedRowRanges"></param>
        /// <param name="replacedRowKeys"></param>
        /// <param name="resultIndexOffset"></param>
        /// <returns></returns>
        public static string ReplaceByKey(this string text, Dictionary<string, string> replaceDictionary, DataRow replaceRow, string keyFrontText, string keyRearText, ref List<Range> replacedDictionaryRanges, ref List<string> replacedDictionaryKeys, ref List<Range> replacedRowRanges, ref List<string> replacedRowKeys, int resultIndexOffset = 0)
        {
            StringBuilder sb = new StringBuilder(text);

            // Set dictionaryKeysLength, rowKeysLength, replaceDictionaryNRow
            int dictionaryKeysLength = 0;
            int rowKeysLength = 0;
            Dictionary<string, string> replaceDictionaryNRow;
            if (!string.IsNullOrEmpty(keyFrontText) || !string.IsNullOrEmpty(keyRearText))
            {
                replaceDictionaryNRow = new Dictionary<string, string>();
                foreach (var pair in replaceDictionary)
                {
                    if (pair.Key != string.Empty)
                    {
                        dictionaryKeysLength++;
                        replaceDictionaryNRow.Add($"{keyFrontText}{pair.Key}{keyRearText}", pair.Value);
                    }
                }

                foreach (DataColumn column in replaceRow.Table.Columns)
                {
                    string key = $"{keyFrontText}{column.ColumnName}{keyRearText}";
                    if (replaceDictionaryNRow.ContainsKey(key))
                    {
                        dictionaryKeysLength--;
                        replaceDictionaryNRow[key] = replaceRow[column.ColumnName].ToString();
                    }
                    else
                    {
                        rowKeysLength++;
                        replaceDictionaryNRow.Add(key, replaceRow[column.ColumnName].ToString());
                    }
                }
            }
            else
            {
                replaceDictionaryNRow = replaceDictionary;
            }

            // Set keys, keysLength, matchedLength
            List<string> keys = replaceDictionaryNRow.Keys.ToList();
            int keysLength = dictionaryKeysLength + rowKeysLength;
            int[] matchedLength = new int[keysLength];

            // Proc
            for (int i = 0; i < sb.Length; i++)
            {
                for (int k = 0; k < keysLength; k++)
                {
                    if (sb[i] == keys[k][matchedLength[k]])
                    {
                        matchedLength[k]++;
                        if (keys[k].Length == matchedLength[k])
                        {
                            // Replace Node
                            int sp = i - (matchedLength[k] - 1);
                            sb.Remove(sp, matchedLength[k]);
                            sb.Insert(sp, replaceDictionaryNRow[keys[k]]);

                            // Add replacedDictionaryRanges, replacedDictionaryKeys
                            if (k < dictionaryKeysLength)
                            {
                                // Set replacedRanges
                                if (replacedDictionaryRanges != null)
                                {
                                    replacedDictionaryRanges.Add(new Range(sp + resultIndexOffset, sp + replaceDictionaryNRow[keys[k]].Length - 1 + resultIndexOffset));
                                }

                                // Set replacedKeys
                                if (replacedDictionaryKeys != null)
                                {
                                    replacedDictionaryKeys.Add(keys[k]);
                                }
                            }
                            // Add replacedRowRanges, replacedRowKeys
                            else
                            {
                                // Set replacedRanges
                                if (replacedRowRanges != null)
                                {
                                    replacedRowRanges.Add(new Range(sp + resultIndexOffset, sp + replaceDictionaryNRow[keys[k]].Length - 1 + resultIndexOffset));
                                }

                                // Set replacedKeys
                                if (replacedRowKeys != null)
                                {
                                    replacedRowKeys.Add(keys[k]);
                                }
                            }

                            // Set i
                            int lengthDifference = replaceDictionaryNRow[keys[k]].Length - matchedLength[k];
                            if (lengthDifference != 0)
                            {
                                i = i + lengthDifference;
                            }

                            // Clear
                            for (int m = 0; m < keysLength; m++)
                            {
                                matchedLength[m] = 0;
                            }
                        }
                    }
                    else
                    {
                        matchedLength[k] = 0;
                    }
                }
            }

            return sb.ToString();
        }
        #endregion

        #region ToAlias
        /// <summary>
        /// 이 문자열을 쿼리문에서 테이블의 Alias 형태로 반환합니다.(예> "TableName": "TN", "TABLE_NAME": "TN")
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string ToAlias(this string tableName)
        {
            return ToAlias(tableName, null, -1, -1);
        }

        /// <summary>
        /// 이 문자열을 쿼리문에서 테이블의 Alias 형태로 반환합니다.(예> "TableName": "TN", "TABLE_NAME": "TN") refData에 전에 만들어진 Table과 Alias의 값을 전달하면 중복되지 않는 결과를 반환합니다. tableNameIndex와 aliasIndex는 refData의 Table과 Alias의 인덱스입니다. currentIndex는 현재 Table의 refData 인덱스입니다.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string ToAlias(this string tableName, List<object[]> refData, int tableNameIndex, int aliasIndex, int currentIndex = -1)
        {
            string rsAlias;

            // Get nodes
            List<string> nodes = new List<string>();
            if (tableName.IndexOf("_") >= 0)
            {
                nodes = tableName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                StringBuilder node = new StringBuilder();
                for (int i = 0; i < tableName.Length; i++)
                {
                    int a = tableName[i];
                    if (65 <= a && a <= 90)
                    {
                        if (node.Length > 0)
                        {
                            nodes.Add(node.ToString());
                            node.Clear();
                        }
                        else
                        {
                            node.Append((char)a);
                        }
                    }
                    else
                    {
                        node.Append((char)a);
                    }
                }

                if (node.Length > 0)
                {
                    nodes.Add(node.ToString());
                }
            }

            bool isDuplicated;
            int appendCharCount = 0;
            int nodeTotalLength = 0;
            string defaultRsAlias = string.Empty;
            string maxRsAlias = string.Join("", nodes).ToUpper();

            // Set defaultRsAlias, nodeTotalLength
            for (int i = 0; i < nodes.Count; i++)
            {
                defaultRsAlias += nodes[i].Substring(0, 1).ToUpper();
                nodeTotalLength += nodes[i].Length;
            }
            rsAlias = defaultRsAlias;

            // Append Characters
            if (refData != null)
            {
                do
                {
                    rsAlias = defaultRsAlias;
                    isDuplicated = false;

                    if (appendCharCount > 0)
                    {
                        // Append characters to rsAlias
                        if (appendCharCount < nodeTotalLength)
                        {
                            // Get appendStringList
                            string[] appendStringList = new string[nodes.Count];
                            int loopCount = 1;
                            for (int i = 0; i < appendCharCount; i++)
                            {
                                for (int k = nodes.Count - 1; 0 <= k; k--)
                                {
                                    if (nodes[k].Length > loopCount)
                                    {
                                        appendStringList[k] = appendStringList[k] == null
                                            ? nodes[k].Substring(nodes[k].Length - loopCount, 1)
                                            : nodes[k].Substring(nodes[k].Length - loopCount, 1) + appendStringList[k];

                                        if (appendCharCount <= i)
                                        {
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                loopCount++;
                            }

                            // rsAlias + appendStringList
                            for (int i = appendStringList.Length - 1; 0 <= i; i--)
                            {
                                if (appendStringList[i] != null)
                                {
                                    rsAlias = rsAlias.Insert(i + 1, appendStringList[i].ToUpper());
                                }
                            }
                        }
                        else
                        {
                            rsAlias = maxRsAlias + (appendCharCount - nodeTotalLength == 0
                                ? "" : (appendCharCount - nodeTotalLength).ToString());
                        }
                    }

                    // Set isDuplicated
                    int iMaxCount = 0 <= currentIndex ? currentIndex : refData.Count;
                    for (int i = 0; i < iMaxCount; i++)
                    {
                        if (tableName != refData[i][tableNameIndex].ToString()
                            && refData[i][aliasIndex] != null && rsAlias == refData[i][aliasIndex].ToString())
                        {
                            appendCharCount++;
                            isDuplicated = true;
                            break;
                        }
                    }
                } while (isDuplicated);
            }

            return rsAlias;
        }
        #endregion
    }
}
