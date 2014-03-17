using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace DoggieCreationsConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serb = new DoggieCreationsServiceReference.DoggieCreationsServiceSoapClient())
            {
                var line = Console.ReadLine();
                Console.WriteLine(serb.VertaalAllesMaar(line));
                Thread.Sleep(new TimeSpan(0, 0, 10));
            }
        }
    }
}
