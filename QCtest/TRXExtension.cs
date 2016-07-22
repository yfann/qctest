using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCtest
{
    [AttributeUsage(AttributeTargets.Method)]
    class TRXExtension:Attribute
    {
        public TRXExtension(string str)
        {
            Console.WriteLine(str);
        }
    }
}
