using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDAPIOLELib;

namespace AutoTest
{
    class Program
    {

        static void Main(string[] args)
        {


            QCConfig conf = ConfigManager.GetQCConfig();

            if (args.Length > 0)
            {
                var list = new TRXParser(args[0]).Parse();

                using (QCManager qc = new QCManager(conf))
                {
                    foreach (var node in list)
                    {
                        qc.UpdateTestResultToQC(node.Id, node.Result);
                    }
                }
            }



            Console.WriteLine("Finished...");
            Console.ReadLine();
        }


    }
}
