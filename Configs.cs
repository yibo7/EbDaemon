using System;
using System.Collections.Generic;
using System.Linq;
using XS.Core;

namespace EbDaemon
{
    public class Configs
    {
        public static readonly Configs Instance = new Configs();
        
         
        private IniParser iniParser;

        private Configs()
        {
            //D:\\web\\BeiMaiProject\\beimai5.0\\Web\\BeiMai.WebApp\\BeiMai.WebApp\\bin
            string sPath = AppDomain.CurrentDomain.BaseDirectory;
#if DEBUG
            if (sPath.EndsWith("\\bin"))
                sPath = sPath.Replace("\\bin", "");
#endif
            iniParser = new IniParser(string.Concat(sPath, @"\conf\conf.ini"));

            //app 
            ProcessList = iniParser.GetSetting("App", "ProcessList");
             

            TimeSpan = iniParser.GetSettingInt("App", "TimeSpan");
        }

        public void Save()
        {
            //app
            // iniParser.AddSetting("App", "LastTestIndex", LastTestIndex);
            iniParser.AddSetting("App", "TimeSpan", TimeSpan);
            iniParser.SaveSettings();

        }
        public int TimeSpan = 60; //秒 


        public string ProcessList { get; set; }

        public Dictionary<string,string> getProcess
        {
            get
            {
                Dictionary<string, string> rz = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(ProcessList))
                {
                    string[] aProc =  XsUtils.SplitString(ProcessList, "***");
                    foreach (string sP in aProc)
                    {
                        string[] OneP = XsUtils.SplitString(sP, ",");
                        if (OneP.Length == 2)
                        {
                            rz.Add(OneP[0], OneP[1]);
                        }
                    }
                    
                }
                return rz;
            }
        }

    }
}