using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassportTester
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Tester());
        }
    }
}
