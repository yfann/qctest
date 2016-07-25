using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDAPIOLELib;

namespace QCtest
{
    class Program
    {

        static void Main(string[] args)
        {
            QCConfig conf = ConfigManager.GetQCConfig();

            var trxpath = @"source\yfann.trx";


            var list = new TRXParser(trxpath).Parse();

            using (QCManager qc = new QCManager(conf))
            {
                foreach(var node in list)
                {
                    qc.UpdateTestResultToQC(node.Id,node.Result);
                }
            }


            Console.WriteLine("Finished...");
            Console.ReadLine();
        }


    }
}
