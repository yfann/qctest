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
            QCConfig conf = new QCConfig();
  
            HA();
            //using (QCManager qc = new QCManager(conf))
            //{
            //    //var list=qc.GetAllTestFromFolder(@"Root\yfann_test");

            //    //foreach(var a in list)
            //    //{
            //    //    qc.RunTest(a,"N/A");
            //    //}

            //    qc.UpdateTestResultToQC();

            //    //var test=qc.CreateOrGetTestSet("test03");
            //    //qc.AddTestCase(test,"16130");
            //}

            Console.ReadLine();
        }
        [TRXExtension("asdf")]
        static void HA()
        {

        }

    }
}
