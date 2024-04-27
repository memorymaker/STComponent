using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 1: DataModeler
            // 2: DataModeler
            // 3: UserPanel
            // 4: UserEditor
            // 5: CodeGenerator
            // 6: UserListView
            // 7: CodeGenerator Performance
            // 8: UserScrollBar
            // 9: UserSplitContainer
            Application.Run(new Form7()); // 7
        }
    }
}
