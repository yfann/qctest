using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QCtest
{
    class ConfigManager
    {
        public static QCConfig GetQCConfig()
        {
            var setting = ConfigurationManager.AppSettings;
            var con = new QCConfig();
            con.QCAddress = setting["QCAddress"];
            con.UserName = setting["QCUser"];
            con.Pwd = setting["QCPwd"];
            con.Domain = setting["QCDomain"];
            con.Project = setting["QCProject"];
            con.RootPath = setting["QCAutoTestFolder"];


            return con;
        }
    }
}
