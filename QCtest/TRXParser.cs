using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
namespace AutoTest
{
    public class TRXParser
    {
        private XmlDocument doc;
        public const string CaseIdPattern=@"^\d+$";
        public TRXParser(string path)
        {
            doc = new XmlDocument();
            doc.Load(path);
        }

        public List<TrxObj> Parse()
        {
            List<TrxObj> list = new List<TrxObj>();
            try
            {
                var root = doc.DocumentElement;
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("u", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
                var nodeList = root.SelectNodes("//u:UnitTestResult", nsmgr);

                foreach (XmlNode node in nodeList)
                {
                    string outcome = node.Attributes["outcome"].Value;
                    var stdout = node.SelectSingleNode(".//u:StdOut", nsmgr);

                    if (stdout != null && !string.IsNullOrEmpty(stdout.InnerText) && Regex.IsMatch(stdout.InnerText, CaseIdPattern))
                    {
                        var trx = new TrxObj(stdout.InnerText, outcome);

                        list.Add(trx);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return list;
        }
    }


    public class TrxObj
    {
        public TrxObj(string id,string result)
        {
            Id = id;
            Result = result;
        }
        public string Id { get; set; }
        public string Result { get; set; }
    }
}
