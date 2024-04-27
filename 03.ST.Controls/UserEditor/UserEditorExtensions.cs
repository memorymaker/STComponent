using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public static class UserEditorExtensions
    {
        public static int IndexOf(this StringBuilder sb, string text, int startIndex = 0, int count = 0)
        {
            int dataLength = sb.Length - startIndex;
            if (dataLength < 0 || count < 0)
            {
                return -1;
            }
            count++;

            char[] search = text.ToCharArray();
            int searchTextMaxIndex = search.Length - 1;
            
            for (int i = 0; i < dataLength; i++)
            {
                for (int k = 0; k <= searchTextMaxIndex; k++)
                {
                    try
                    {
                        if (sb[startIndex + i + k] == search[k])
                        {
                            if (k == searchTextMaxIndex)
                            {
                                count--;
                                if (count == 0)
                                {
                                    return i + startIndex;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return -1;
        }

        public static int LastIndexOf(this StringBuilder sb, string text, int? startIndex = null, int count = 0)
        {
            if (startIndex <= 0 || sb.Length < startIndex || count < 0)
            {
                return -1;
            }
            count++;

            if (startIndex == null)
            {
                startIndex = sb.Length;
            }
            
            char[] search = text.ToCharArray();
            int searchTextMaxIndex = search.Length - 1;

            for (int i = (int)startIndex - 1; i >= 0; i--)
            {
                for (int k = searchTextMaxIndex; k >= 0; k--)
                {
                    if (sb[i - (searchTextMaxIndex - k)] == search[k])
                    {
                        if (k == 0)
                        {
                            count--;
                            if (count == 0)
                            {
                                return i - searchTextMaxIndex;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return -1;
        }

        #region ETC BK Code
        //char[] search = text.ToCharArray();
        //int searchTextMaxIndex = search.Length - 1;

        //char[] data = new char[dataLength];
        //sb.CopyTo(startIndex, data, 0, dataLength);

        //for (int i = 0; i < dataLength; i++)
        //{
        //    for (int k = 0; k <= searchTextMaxIndex; k++)
        //    {
        //        if (data[i + k] == search[k])
        //        {
        //            if (k == searchTextMaxIndex)
        //            {
        //                return i + startIndex;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}
        //return -1;
        
        //if (startIndex < 0 || sb.Length <= startIndex)
        //{
        //    throw new Exception("wrong value for startIndex.");
        //}

        //if (startIndex == 0)
        //{
        //    startIndex = sb.Length - 1;
        //}

        //int dataLength = startIndex + 1;

        //char[] search = text.ToCharArray();
        //int searchTextMaxIndex = search.Length - 1;

        //char[] data = new char[dataLength];
        //sb.CopyTo(0, data, 0, dataLength);

        //for (int i = dataLength - 1; i >= 0; i--)
        //{
        //    for (int k = searchTextMaxIndex; k >= 0; k--)
        //    {
        //        if (data[i - (searchTextMaxIndex - k)] == search[k])
        //        {
        //            if (k == 0)
        //            {
        //                return i - searchTextMaxIndex;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}
        //return -1;
        #endregion
    }
}
