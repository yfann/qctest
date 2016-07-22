using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDAPIOLELib;

namespace QCtest
{
    public class QCManager:IDisposable
    {
        const string PASSED = "Passed";
        const string FAILED = "Failed";
        const string NA = "N/A";
        const string NOTCOMPLETED = "Not Completed";

        private QCConfig _config;
        private TDConnection _conn;
        private TestSetTreeManager _setTree;
        private TestSetFolder _setFolder;
        public TestSetTreeManager SetTree
        {
            get
            {
                if(_setTree==null)
                {
                    _setTree = _conn.TestSetTreeManager as TestSetTreeManager;
                }
                return _setTree;
            }
        }
        public TestSetFolder SetFolder
        {
            get
            {
                if(_setFolder==null)
                {
                    _setFolder = SetTree.NodeByPath[_config.RootPath] as TestSetFolder;
                }
                return _setFolder;
            }
        }
        public QCManager(QCConfig config)
        {
            _config = config;
            ConnectToQC();
        }
        private bool FolderIsExist(string name)
        {
            try
            {
                SysTreeNode node=SetFolder.FindChildNode(name) as SysTreeNode;
                return node == null ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public void CreateFolder(string name)
        {
            try
            {
                if(!FolderIsExist(name))
                {
                    SysTreeNode node = SetFolder.AddNode(name) as SysTreeNode;
                    node.Post();
                }

            }
            catch(Exception e)
            {

            }
        }
        public TestSet CreateOrGetTestSet(string name)
        {
            var tsList = SetFolder.FindTestSets(name);
            TestSet setResult=null;
            if(tsList!=null&& tsList.Count>0)
            {
               foreach(var t in tsList)
                {
                    var ts=t as TestSet;
                    if(ts.Name==name)
                    {
                        setResult = ts;
                    }
                }
            }
            else
            {
                try {
                    var tsFact = SetFolder.TestSetFactory as TestSetFactory;
                    setResult = tsFact.AddItem(name) as TestSet;
                    setResult.Post();
                }
                catch(Exception e)
                {

                }

            }
            return setResult;
        }
        public List<TSTest> GetAllTestFromFolder(string path)
        {
            TestSetFolder folder;
            List<TSTest> result = new List<TSTest>();
            if (string.IsNullOrEmpty(path))
            {
                folder = SetFolder;
            }
            else
            {
                folder=SetTree.NodeByPath[path] as TestSetFolder;
            }
            TestSetFactory factory = folder.TestSetFactory as TestSetFactory;
            TDFilter filter = factory.Filter as TDFilter;
            List list = filter.NewList() as List;
            foreach(TestSet ts in list)
            {
                foreach(TSTest test in GetTestFromTestSet(ts))
                {
                    result.Add(test);
                }
            }

            return result;
        }
        public IEnumerable<TSTest> GetTestFromTestSet(TestSet ts)
        {
            TSTestFactory tsFactory = ts.TSTestFactory as TSTestFactory;
            var testList = tsFactory.NewList("") as List;
            foreach(TSTest test in testList)
            {
                yield return test;
            }
        }
        public TSTest AddTestCase(TestSet ts,string caseid)
        {
            TSTest result=null;
            try
            {
                var factory = ts.TSTestFactory as TSTestFactory;
                result = factory.AddItem(caseid) as TSTest;
            }
            catch (Exception e) { }
            return result;
        }
        public void RunTest(TSTest test, string result)
        {
            var runFactory=test.RunFactory as RunFactory;
            var now = DateTime.Now;
            var run = runFactory.AddItem("Run_"+now.ToShortDateString()+"_"+now.ToShortTimeString()) as Run;
            run.Status = result;
            run.Post();
            run.Refresh();
        }
        public void AddAndRunTest(TestSet ts,string caseid,string result)
        {
            var test = AddTestCase(ts,caseid);
            RunTest(test,result);
        }
        public void UpdateTestResultToQC()
        {
            var testSet = CreateOrGetTestSet("test");
            AddAndRunTest(testSet,"16125", PASSED);
        }
        public TDConnection GetConnection()
        {
            return _conn;
        }
        private void ConnectToQC()
        {
            _conn= new TDConnectionClass();
            try
            {
                _conn.InitConnectionEx(_config.QCAddress);
                _conn.Login(_config.UserName, _config.Pwd);
                _conn.Connect(_config.Domain, _config.Project);
            }
            catch(Exception e)
            {

            }
        }
        public void Dispose()
        {
            _conn.DisconnectProject();
            _conn.Logout();
            _conn.ReleaseConnection();
            _conn = null;
        }

    }


    public class QCConfig
    {
        public string QCAddress
        {
            get;set;
        }

        public string UserName
        {
            get;set;
        }
        public string Pwd
        {
            get;set;
        }
        public string Domain
        {
            get;set;
        }

        public string Project
        {
            get;set;
        }

        public string RootPath
        {
            get;set;
        }

    }
}
