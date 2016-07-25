using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
namespace QCtest
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
            var root = doc.DocumentElement;
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("u", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
            var nodeList = root.SelectNodes("//u:UnitTestResult", nsmgr);

            foreach (XmlNode node in nodeList)
            {
                string outcome=node.Attributes["outcome"].Value;
                var caseid = node.InnerText;
                if (!string.IsNullOrEmpty(caseid) &&Regex.IsMatch(caseid, CaseIdPattern))
                {
                    var trx = new TrxObj(caseid, outcome);

                    list.Add(trx);
                }
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
