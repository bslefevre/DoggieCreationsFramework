using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoggieCreationsWindowsFormsApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += GetResourcedAssembly;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static Assembly GetResourcedAssembly(object sender, ResolveEventArgs arg)
        {
            if (arg.Name.StartsWith("DoggieCreationsFramework")) return Assembly.Load(Properties.Resources.DoggieCreationsFramework);
            else if (arg.Name.StartsWith("HtmlAgilityPack")) return Assembly.Load(Properties.Resources.HtmlAgilityPack);
            return null;
        }
    }
}
