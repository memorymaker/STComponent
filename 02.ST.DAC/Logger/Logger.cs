using System;
using System.Text;

namespace ST.DAC
{
    public static class Logger
    {
        public static void WriteLog(string _path, string category1, string category2, string _message) 
        {
            try
            {
                DateTime nowTime = DateTime.Now;
                string subFolder = nowTime.ToString("yyyyMM");

                string path = _path.Substring(_path.Length - 1, 1) == "\\"
                    ? _path + subFolder
                    : _path + "\\" + subFolder;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                string message = string.Format("[{0}][{1}][{2}]{3} \r\n", nowTime.ToString("yyyy-MM-dd HH:mm:ss"), category1, category2, _message);

                string fullPath = path + nowTime.ToString("yyyy-MM-dd") + ".log";
                System.IO.File.AppendAllText(fullPath, message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
